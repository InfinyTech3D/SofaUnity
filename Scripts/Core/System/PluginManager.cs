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
            for (id=0; id<m_plugins.Count; id++)
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
            Debug.Log("construct PluginManagerInterface");
            m_sofaAPI = sofaAPI;
            if (m_savedPlugins == null)
                m_savedPlugins = new List<string>();

            InitDefaultPlugins();
        }

        /// Method to set the SofaContextAPI to be used by this manager
        public void SetSofaContextAPI(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            RegisterDefaultPlugin();
            InitDefaultPlugins();
        }

        /// Method to update the list of save plugin to load from plugins enable status
        public void UpdateEnabledPlugins()
        {
            Debug.Log("UpdateEnabledPlugins");
            m_savedPlugins.Clear();
            List<Plugin> plugins = PluginManager.Instance.GetPluginList();
            foreach (Plugin plugin in plugins)
            {
                if (plugin.IsEnable)
                    m_savedPlugins.Add(plugin.Name);
            }
        }


        /// Method to load the plugins one by one from the list of enable plugins
        public void LoadPlugins()
        {
            string pluginPath = "";
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Plugins/Native/x64/";
            else
                pluginPath = "/Plugins/";

            foreach (string pluginName in m_savedPlugins)
            {
#if UNITY_EDITOR
                Plugin plug = PluginManager.Instance.GetPluginByName(pluginName);
                if (plug.IsAvailable == false)
                {
                    Debug.LogError("Plugin " + plug.Name + " is requested but can't be found.");
                }
                else
                {
                    plug.IsEnable = true;
                    m_sofaAPI.loadPlugin(Application.dataPath + pluginPath + pluginName + ".dll");
                }
#else
                m_sofaAPI.loadPlugin(Application.dataPath + pluginPath + pluginName + ".dll");
#endif
            }
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
            PluginManager.Instance.AddPlugin("SofaOpenglVisual");
            PluginManager.Instance.AddPlugin("SofaImplicitOdeSolver");
            PluginManager.Instance.AddPlugin("SofaMiscForceField");
            PluginManager.Instance.AddPlugin("SofaEngine");
            PluginManager.Instance.AddPlugin("SofaTopologyMapping");
            PluginManager.Instance.AddPlugin("SofaMiscCollision");
            PluginManager.Instance.AddPlugin("SofaGeneralLoader");
            PluginManager.Instance.AddPlugin("SofaConstraint");
            PluginManager.Instance.AddPlugin("SofaNonUniformFem");
            PluginManager.Instance.AddPlugin("SofaLoader");
            PluginManager.Instance.AddPlugin("SofaGeneralLoader");
            PluginManager.Instance.AddPlugin("SofaGeneralTopology");

            PluginManager.Instance.AddPlugin("SofaSparseSolver");
            PluginManager.Instance.AddPlugin("InteractionTools");
            PluginManager.Instance.AddPlugin("SofaCarving");
            PluginManager.Instance.AddPlugin("SofaSphFluid");
            //PluginManager.Instance.AddPlugin("MultiCoreGPU");
        }
#endif

        /// Init method of the pluginManager Interface to enable default plugin.
        private void InitDefaultPlugins()
        {
            PluginManager.Instance.GetPluginByName("SofaOpenglVisual").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaImplicitOdeSolver").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaMiscForceField").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaEngine").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaTopologyMapping").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaMiscCollision").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaGeneralLoader").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaConstraint").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaNonUniformFem").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaLoader").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaGeneralLoader").IsEnable = true;
            PluginManager.Instance.GetPluginByName("SofaGeneralTopology").IsEnable = true;

            UpdateEnabledPlugins();
        }

    }
}
