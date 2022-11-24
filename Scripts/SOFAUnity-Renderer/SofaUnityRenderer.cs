using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnity
{
    public class SofaUnityRenderer
    {
        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        public List<SofaVisualModel> m_visualModels = null;

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaUnityRenderer(SofaContext context)
        {
            m_sofaContext = context;

            SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();
            if (sofaAPI == null)
            {
                Debug.LogError("SofaContextAPI is still null. Renderer has been created before creating a SOFA context.");
                return;
            }

            // load default plugins to load SOFA scene
            string pluginPath = "";
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Plugins/Native/x64/";
            else
                pluginPath = "/Plugins/x86_64/";


            sofaAPI.loadPlugin(Application.dataPath + pluginPath + "Sofa.Component.dll");
            sofaAPI.loadPlugin(Application.dataPath + pluginPath + "Sofa.GL.Component.dll");
            sofaAPI.loadPlugin(Application.dataPath + pluginPath + "Sofa.GUI.Component.dll");
        }


        /// Method to perform one step of simulation in Sofa
        public void step()
        {
            foreach(SofaVisualModel visuM in m_visualModels)
            {
                visuM.SetDirty(true);
            }
        }


        public void createScene()
        {
            SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();
            Debug.Log("### SofaVisualModel:createScene");
            int nbrVM = sofaPhysicsAPI_getNbrVisualModel(sofaAPI.getSimuContext());
            Debug.Log("createScene: Nbr VisualM: " + nbrVM);

            if (m_visualModels == null)
                m_visualModels = new List<SofaVisualModel>();

            for (int i=0; i< nbrVM; i++)
            {
                string nameVM = sofaVisualModel_getName(sofaAPI.getSimuContext(), i);
                GameObject obj = new GameObject("SofaVisualModel - " + nameVM);
                SofaVisualModel visuM = obj.AddComponent<SofaVisualModel>();
                visuM.m_uniqName = nameVM;
                visuM.setSofaContext(m_sofaContext);
                obj.transform.parent = m_sofaContext.gameObject.transform;

                m_visualModels.Add(visuM);
            }
        }


        public void Reconnect()
        {
            Debug.Log("## SofaVisualModel:Reconnect ");

            if (m_visualModels == null)
                m_visualModels = new List<SofaVisualModel>();

            SofaVisualModel[] Vms = m_sofaContext.GetComponentsInChildren<SofaVisualModel>();

            foreach (SofaVisualModel visuM in Vms)
            {
                Debug.Log("Reconnect SofaVisualModel: " + visuM.m_uniqName);
                visuM.setSofaContext(m_sofaContext);
                m_visualModels.Add(visuM);
            }
        }

        ///////////////////////////////////////////////////////////
        //////////          API Communication         /////////////
        ///////////////////////////////////////////////////////////
        [DllImport("SofaPhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNbrVisualModel(IntPtr ptr);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaVisualModel_getName(IntPtr ptr, int VModelID);
    }
}