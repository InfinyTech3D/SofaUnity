using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SofaUnityAPI;
using System.IO;
using System;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.VersionControl;
#endif



namespace SofaUnity
{
    /// <summary>
    /// Data class refering to a plugin, to store it's name, and the options
    /// </summary>
    [System.Serializable]
    public class PluginInfo
    {
        public PluginInfo(string _name, bool available)
        {
            Name = _name;
            IsAvailable = available;
            IsEnable = false;
        }

        public string Name = "None";
        public bool IsAvailable = false;
        public bool IsEnable = false;
    }


    /// <summary>
    /// 
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    class PluginManager
    {
        // Static singleton instance
        private static PluginManager instance;

        /// List of plugin dll library names to load
        private List<PluginInfo> m_registeredPlugins = new List<PluginInfo>();

        /// List of dll library in directory
        private string[] m_dllList = null;

        // Static singleton property
        public static PluginManager Instance
        {
            // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
            // otherwise we assign instance to a new component and return that
            get { return instance ?? (instance = new PluginManager()); }
        }

        public int GetNbrRegisteredPlugins()
        {
            return m_registeredPlugins.Count;
        }


        public List<PluginInfo> GetRegisteredPluginList()
        {
            return m_registeredPlugins;
        }


        public PluginInfo GetRegisteredPluginByName(string pluginName)
        {
            for (int id = 0; id < m_registeredPlugins.Count; id++)
            {
                if (m_registeredPlugins[id].Name == pluginName)
                {
                    return m_registeredPlugins[id];
                }
            }

            return null;
        }

        public PluginInfo RegisterPlugin(string pluginName)
        {
            // First check if plugin is in dll list
            bool exist = CheckPluginExists(pluginName);

            // Check if plugin not already registered
            for (int id = 0; id < m_registeredPlugins.Count; id++)
            {
                if (m_registeredPlugins[id].Name == pluginName)
                {
                    // if found just update the availability status
                    m_registeredPlugins[id].IsAvailable = exist;
                    return m_registeredPlugins[id];
                }
            }

            // If not found, add it to the list of registered plugin
            PluginInfo pluginInfo = new PluginInfo(pluginName, exist);
            m_registeredPlugins.Add(pluginInfo);

            return pluginInfo;
        }


        private bool CheckPluginExists(string pluginName)
        {
            if (m_dllList == null)
            {
                
                string dllDirPath = SofaUtils.GetPluginFullPrefixPath();// Application.dataPath + "/SofaUnity/Core/Plugins/Native/x64/";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                m_dllList = Directory.GetFiles(dllDirPath, "*.dll");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                m_dllList = Directory.GetFiles(dllDirPath, "*.dylib");
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
                m_dllList = Directory.GetFiles(dllDirPath, "*.so");
#elif UNITY_ANDROID
                m_dllList = Directory.GetFiles(dllDirPath, "*.so");
#else
                m_dllList = null;
#endif
                
                if (m_dllList == null)
                {
                    Debug.LogError("PluginManager: Can't find any plugin in directory " + dllDirPath);
                    return false;
                }

                for (int i = 0; i < m_dllList.Length; i++)
                {
                    m_dllList[i] = Path.GetFileName(m_dllList[i]);
                }
            }

            for (int i = 0; i < m_dllList.Length; i++)
            {
                if (m_dllList[i].Contains(pluginName))
                {
                    return true;
                }
            }

            return false;
        }
    }




    /// <summary>
    /// Class to manage the list of Sofa plugin dll to load
    /// </summary>
    [System.Serializable]
    public class PluginManagerInterface
    {
        ////////////////////////////////////////////
        //////      PluginManager members      /////
        ////////////////////////////////////////////

        /// Pointer to the SofaContext
        protected SofaContextAPI m_sofaAPI = null;

        /// List of plugin dll library names to load for this simulation
        [SerializeField]
        protected List<PluginInfo> m_savedPlugins = null;

        ////////////////////////////////////////////
        //////     PluginManager accessors     /////
        ////////////////////////////////////////////

        /// Default constructor taking a SofaContext as argument
        public PluginManagerInterface(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            if (m_savedPlugins == null)
            {
                m_savedPlugins = new List<PluginInfo>();
                List<PluginInfo> plugins = PluginManager.Instance.GetRegisteredPluginList();
                foreach (PluginInfo plugin in plugins)
                {
                    m_savedPlugins.Add(new PluginInfo(plugin.Name, plugin.IsAvailable));
                }
            }
        }

        /// Method to set the SofaContextAPI to be used by this manager
        public void SetSofaContextAPI(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            UpdateEnabledPlugins();
        }

        /// Method to update the list of save plugin to load from plugins enable status
        public void UpdateEnabledPlugins()
        {
            Debug.Log(GetNbrPlugins() + " plugins to load");
            foreach (PluginInfo plugin in m_savedPlugins)
            {
                PluginInfo availabledPlug = PluginManager.Instance.GetRegisteredPluginByName(plugin.Name);
                if (availabledPlug != null)
                {
                    plugin.IsAvailable = availabledPlug.IsAvailable;
                }
                else
                    plugin.IsAvailable = false;
            }
        }


        /// Method to manually clear all saved plugins
        public void ClearSavedPlugin()
        {
            m_savedPlugins.Clear();
            List<PluginInfo> plugins = PluginManager.Instance.GetRegisteredPluginList();
            foreach (PluginInfo plugin in plugins)
            {
                m_savedPlugins.Add(new PluginInfo(plugin.Name, plugin.IsAvailable));
            }
        }


        /// Method to load the plugins one by one from default plugin (Core of SOFA) + the list of saved plugins
        public void LoadPlugins()
        {
            string fullPrefixPath = SofaUtils.GetPluginFullPrefixPath();

            // Internally load all default plugins from core and module
            Debug.Log("m_sofaAPI.loadDefaultPlugins using path: " + fullPrefixPath);
            m_sofaAPI.loadDefaultPlugins(fullPrefixPath);

            Debug.Log("# LoadPlugins: m_savedPlugins Count: " + m_savedPlugins.Count);
            foreach (PluginInfo plugin in m_savedPlugins)
            {
                if (!plugin.IsEnable)
                    continue;

                Debug.Log("  ## plugin enable, loading: " + plugin.Name);
                string fullPluginPath = SofaUtils.GetPluginFullPrefixPath() + SofaUtils.GetPluginFullName(plugin.Name);
#if UNITY_EDITOR
                PluginInfo plug = GetPluginByName(plugin.Name);
                if (plug == null || plug.IsAvailable == false)
                {
                    Debug.LogError("Plugin " + plugin.Name + " is requested but can't be found. Disable it for the moment but you should investigate the issue.");
                    plug.IsEnable = false;
                    continue;
                }
                else
                {                    
                    m_sofaAPI.loadPlugin(fullPluginPath);
                }
#else
                m_sofaAPI.loadPlugin(fullPluginPath);
#endif
            }
        }

        // Direct method to load a plugin by name, without checking if it is in the saved list
        public void LoadPlugin(string pluginName)
        {
            string fullPluginPath = SofaUtils.GetPluginFullPrefixPath() + SofaUtils.GetPluginFullName(pluginName);

            m_sofaAPI.loadPlugin(fullPluginPath);
        }

        ////////////////////////////////////////////
        //////    PluginManager public API     /////
        ////////////////////////////////////////////

        /// method to get the number of plugin registered in the PluginManager
        public int GetNbrPlugins()
        {
            return m_savedPlugins.Count;
        }

        /// method to get access of the list of saved plugin status upon the list of available plugins
        public List<PluginInfo> GetPluginList()
        {
            return m_savedPlugins;
        }

        /// method to get a plugin by name from the list of saved plugins
        public PluginInfo GetPluginByName(string pluginName)
        {
            for (int id = 0; id < m_savedPlugins.Count; id++)
            {
                if (m_savedPlugins[id].Name == pluginName)
                {
                    return m_savedPlugins[id];
                }
            }

            return null;
        }

        /// Method to add a plugin to the list of saved plugins to load, if it is available in the list of registered plugins
        public bool AddPluginToLoad(string pluginName)
        {
            // first check if already in the list of saved plugin
            PluginInfo plugin = GetPluginByName(pluginName);

            // if doesn't exist in the list of saved plugin, get or add it from the list of registered plugins
            if (plugin == null)
            {
                plugin = PluginManager.Instance.RegisterPlugin(pluginName);
            }

            // if not available, return false
            if (plugin.IsAvailable == false)
            {
                Debug.LogError("Plugin " + pluginName + " is requested but can't be found. Disable it for the moment but you should investigate the issue.");
                plugin.IsEnable = false;
                return false;
            }
            else
            {
                plugin.IsEnable = true;
                return true;
            }
        }


        public void printAvailablePlugin()
        {
            foreach (PluginInfo pi in m_savedPlugins)
            {
                Debug.Log("Loaded plugin " + pi.Name);
            }
        }

        ////////////////////////////////////////////
        //////    PluginManager private API    /////
        ////////////////////////////////////////////

    }

    /// Default onlload method to register default SOFA plugin
    class InitDefaultPlugin
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void RegisterDefaultPlugin()
        {
            PluginManager.Instance.RegisterPlugin("MeshRefinement");
            PluginManager.Instance.RegisterPlugin("SofaCarving");
            PluginManager.Instance.RegisterPlugin("SofaSphFluid");
            PluginManager.Instance.RegisterPlugin("SofaCUDA");
            PluginManager.Instance.RegisterPlugin("SofaPython3");
            PluginManager.Instance.RegisterPlugin("Tearing");
            PluginManager.Instance.RegisterPlugin("MultiThreading");
        }
#endif
    }
}
