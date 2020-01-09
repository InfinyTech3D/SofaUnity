using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    [System.Serializable]
    public class PluginManager
    {
        protected SofaContextAPI m_sofaAPI = null;

        public PluginManager(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
            InitDefaultPlugins();
        }

        public void SetSofaContextAPI(SofaContextAPI sofaAPI)
        {
            m_sofaAPI = sofaAPI;
        }

        [SerializeField]
        protected List<string> m_plugins = null;

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

        public string GetPluginName(int id)
        {
            if (id < m_plugins.Count)
                return m_plugins[id];
            else
                return "";
        }

        public void SetPluginName(int id, string value)
        {
            if (id < m_plugins.Count)
                m_plugins[id] = value;
        }
        

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

        protected void InitDefaultPlugins()
        {
            m_plugins = new List<string>
            {
                "SofaOpenglVisual",
                "SofaMiscCollision"
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