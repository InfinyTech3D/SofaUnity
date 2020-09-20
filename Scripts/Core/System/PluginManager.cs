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
    class Plugin
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
    [InitializeOnLoad]
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

        /// List of plugin dll library names to load
        [SerializeField]
        protected List<string> m_savedPlugins = null;

        /// Pointer to the pluginManager singleton
        //public PluginManager m_pluginImpl = null;


        ////////////////////////////////////////////
        //////     PluginManager accessors     /////
        ////////////////////////////////////////////

        /// Default constructor taking a SofaContext as argument
        public PluginManagerInterface(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            InitDefaultPlugins();

            //m_pluginImpl = SofaUnity.PluginManager.Instance;
        }

        /// Method to set the SofaContextAPI to be used by this manager
        public void SetSofaContextAPI(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
        }

        /// Method to load the plugins one by one from the list
        public void LoadPlugins()
        {
            string pluginPath = "";
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Plugins/Native/x64/";
            else
                pluginPath = "/Plugins/";

            //List<Plugin> plugins = m_pluginImpl.GetPluginList();
            //foreach (Plugin plugin in plugins)
            //{
            //    if (plugin.IsAvailable)
            //        m_sofaAPI.loadPlugin(Application.dataPath + pluginPath + plugin.Name + ".dll");
            //}
        }


        ////////////////////////////////////////////
        //////    PluginManager public API     /////
        ////////////////////////////////////////////

        /// method to add a plugin name into the list
        //public void SetPluginName(int id, string value)
        //{
        //    if (id < m_plugins.Count)
        //        m_plugins[id] = value;
        //}


        ///// method to get the plugin name from an id of the list
        //public string GetPluginName(int id)
        //{
        //    if (id < m_plugins.Count)
        //        return m_plugins[id];
        //    else
        //        return "";
        //}

        public void EnablePlugin(int id, bool value)
        {
            //m_pluginImpl.m_
        }


        /// method to get the number of plugin name of the list
        //public int GetNbrPlugins()
        //{
        //    return m_pluginImpl.GetNbrPlugins();
        //}

        //public List<Plugin> GetPluginList()
        //{
        //    return m_pluginImpl.GetPluginList();
        //}


        /// Internal method to set a default list of plugin to be loaded.
        protected void InitDefaultPlugins()
        {
            PluginManager.Instance.AddPlugin("SofaOpenglVisual");
            PluginManager.Instance.AddPlugin("SofaMiscCollision");
            PluginManager.Instance.AddPlugin("SofaSparseSolver");
            PluginManager.Instance.AddPlugin("SofaSphFluid");
            PluginManager.Instance.AddPlugin("MultiCoreGPU");

            if (m_savedPlugins == null)
                m_savedPlugins = new List<string>();
            //            m_plugins = new List<string>
            //            {
            //                "SofaOpenglVisual",
            //                "SofaMiscCollision"
            ////                "VirtualXRay",
            //  //              "ImagingUS"
            // 
        }
    }
}