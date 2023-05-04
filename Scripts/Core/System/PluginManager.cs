using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SofaUnityAPI;
using System.IO;


namespace SofaUnity
{
    /// <summary>
    /// Data class refering to a plugin, to store it's name, and the options
    /// </summary>
    public class Plugin
    {
        public Plugin(string _name, bool available)
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
        private List<Plugin> m_plugins = new List<Plugin>();

        /// List of dll library in directory
        private string[] m_dllList = null;

        // Static singleton property
        public static PluginManager Instance
        {
            // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
            // otherwise we assign instance to a new component and return that
            get { return instance ?? (instance = new PluginManager()); }
        }

        public int GetNbrPlugins()
        {
            return m_plugins.Count;
        }


        public List<Plugin> GetPluginList()
        {
            return m_plugins;
        }

        public Plugin GetPluginByName(string pluginName)
        {
            for (int id = 0; id < m_plugins.Count; id++)
            {
                if (m_plugins[id].Name == pluginName)
                {
                    return m_plugins[id];
                }
            }

            return null;
        }


        public bool CheckPlugin(string pluginName)
        {
            if (m_dllList == null)
            {
                string dllDirPath = Application.dataPath + "/SofaUnity/Plugins/Native/x64/";
                m_dllList = Directory.GetFiles(dllDirPath, "*.dll");

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


        public void AddPlugin(string pluginName)
        {
            // first check if plugin is in dll list
            bool exist = CheckPlugin(pluginName);

            bool found = false;
            int id = 0;
            for (id = 0; id < m_plugins.Count; id++)
            {
                if (m_plugins[id].Name == pluginName)
                {
                    found = true;
                    break;
                }
            }

            if (found) // already registered, nothing to do.
            {
                m_plugins[id].IsAvailable = exist;
            }
            else
            {
                m_plugins.Add(new Plugin(pluginName, exist));
            }
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
        protected List<string> m_savedPlugins = null;


        ////////////////////////////////////////////
        //////     PluginManager accessors     /////
        ////////////////////////////////////////////

        /// Default constructor taking a SofaContext as argument
        public PluginManagerInterface(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            if (m_savedPlugins == null)
                m_savedPlugins = new List<string>();

            SaveEnabledPlugins();
        }

        /// Method to set the SofaContextAPI to be used by this manager
        public void SetSofaContextAPI(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
#if UNITY_EDITOR
            RegisterDefaultPlugin();
#endif
        }

        /// Method to update the list of save plugin to load from plugins enable status
        public void SaveEnabledPlugins()
        {
            m_savedPlugins.Clear();
            List<Plugin> plugins = PluginManager.Instance.GetPluginList();
            foreach (Plugin plugin in plugins)
            {
                if (plugin.IsEnable)
                    m_savedPlugins.Add(plugin.Name);
            }
        }


        /// Method to manually clear all saved plugins
        public void ClearSavedPlugin()
        {
            m_savedPlugins.Clear();
        }


        public string getPluginFullPrefixPath()
        {
            string pluginPath = "";
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Plugins/Native/x64/";
            else
#if UNITY_ANDROID
                pluginPath = "/Plugins/Android/";
#else
                pluginPath = "/Plugins/x86_64/";
#endif
            return Application.dataPath + pluginPath + "/";
        }

        public string getPluginFullName(string pluginName)
        {
            string pluginFullPath = "";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            pluginFullPath = pluginName + ".dll";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
            pluginFullPath = "lib" + pluginName + ".so";
#endif
            return pluginFullPath;
        }

        /// Method to load the plugins one by one from the list of enable plugins
        public void LoadPlugins()
        {
            string fullPrefixPath = getPluginFullPrefixPath();

            // Internally load all default plugins from core and module
            m_sofaAPI.loadDefaultPlugins(fullPrefixPath);

            foreach (string pluginName in m_savedPlugins)
            {
                string fullPluginPath = getPluginFullPrefixPath() + getPluginFullName(pluginName);
#if UNITY_EDITOR
                Plugin plug = PluginManager.Instance.GetPluginByName(pluginName);
                if (plug == null || plug.IsAvailable == false)
                {
                    Debug.LogError("Plugin " + pluginName + " is requested but can't be found.");
                    continue;
                }
                else
                {
                    plug.IsEnable = true;
                    m_sofaAPI.loadPlugin(fullPluginPath);
                }
#else
                m_sofaAPI.loadPlugin(fullPluginPath);
#endif
            }
        }

        public void LoadPlugin(string pluginName)
        {
            string fullPluginPath = getPluginFullPrefixPath() + getPluginFullName(pluginName);

            m_sofaAPI.loadPlugin(fullPluginPath);
        }


        ////////////////////////////////////////////
        //////    PluginManager public API     /////
        ////////////////////////////////////////////

        /// method to get the number of plugin registered in the PluginManager
        public int GetNbrPlugins()
        {
            return PluginManager.Instance.GetNbrPlugins();
        }

        /// method to get access of the list of plugin registered
        public List<Plugin> GetPluginList()
        {
            return PluginManager.Instance.GetPluginList();
        }


        ////////////////////////////////////////////
        //////    PluginManager private API    /////
        ////////////////////////////////////////////

        /// Default onlload method to register default SOFA plugin
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void RegisterDefaultPlugin()
        {
            PluginManager.Instance.AddPlugin("InfinyToolkit");
            PluginManager.Instance.AddPlugin("MeshRefinement");
            PluginManager.Instance.AddPlugin("SofaCarving");
            PluginManager.Instance.AddPlugin("SofaSphFluid");
            //PluginManager.Instance.AddPlugin("MultiCoreGPU");
        }
#endif

    }
}
