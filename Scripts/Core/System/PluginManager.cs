using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Class to manage the list of Sofa plugin dll to load
    /// </summary>
    [System.Serializable]
    public class PluginManager
    {
        ////////////////////////////////////////////
        //////      PluginManager members      /////
        ////////////////////////////////////////////

        /// Pointer to the SofaContext
        protected SofaContextAPI m_sofaAPI = null;

        /// List of plugin dll library names to load
        [SerializeField]
        protected List<string> m_plugins = null;



        ////////////////////////////////////////////
        //////     PluginManager accessors     /////
        ////////////////////////////////////////////

        /// Default constructor taking a SofaContext as argument
        public PluginManager(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            InitDefaultPlugins();
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

            foreach (string pluginName in m_plugins)
            {
                m_sofaAPI.loadPlugin(Application.dataPath + pluginPath + pluginName + ".dll");
            }
        }


        ////////////////////////////////////////////
        //////    PluginManager public API     /////
        ////////////////////////////////////////////

        /// method to add a plugin name into the list
        public void SetPluginName(int id, string value)
        {
            if (id < m_plugins.Count)
                m_plugins[id] = value;
        }


        /// method to get the plugin name from an id of the list
        public string GetPluginName(int id)
        {
            if (id < m_plugins.Count)
                return m_plugins[id];
            else
                return "";
        }


        /// method to get/set the number of plugin name of the list
        public int NbrPlugin
        {
            get
            {
                return m_plugins.Count;
            }
            set
            {
                int diff = value - m_plugins.Count;
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                        m_plugins.Add("");
                }
                else if (diff < 0)
                {
                    m_plugins.RemoveRange(value, -diff);
                }
            }
        }

        
        /// Internal method to set a default list of plugin to be loaded.
        protected void InitDefaultPlugins()
        {
            m_plugins = new List<string>
            {
                "SofaOpenglVisual",
                "SofaMiscCollision",
                "VirtualXRay",
                "ImagingUS"
            };

            //m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaSparseSolver.dll");
            //m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaPreconditioner.dll");

            //m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaSphFluid.dll");
            //m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaHaptics.dll");
            //m_impl.loadPlugin(Application.dataPath + pluginPath + "Geomagic.dll");
            //m_impl.loadPlugin(Application.dataPath + pluginPath + "InteractionTools.dll");

            //m_impl.loadPlugin(Application.dataPath + pluginPath + "MultiCoreGPU.dll");

        }
    }
}