/*****************************************************************************
 *                 - Copyright (C) - 2022 - InfinyTech3D -                   *
 *                                                                           *
 * This file is part of the SofaUnity-Renderer asset from InfinyTech3D       *
 *                                                                           *
 * GNU General Public License Usage:                                         *
 * This file may be used under the terms of the GNU General                  *
 * Public License version 3. The licenses are as published by the Free       *
 * Software Foundation and appearing in the file LICENSE.GPL3 included in    *
 * the packaging of this file. Please review the following information to    *
 * ensure the GNU General Public License requirements will be met:           *
 * https://www.gnu.org/licenses/gpl-3.0.html.                                *
 *                                                                           *
 * Commercial License Usage:                                                 *
 * Licensees holding valid commercial license from InfinyTech3D may use this *
 * file in accordance with the commercial license agreement provided with    *
 * the Software or, alternatively, in accordance with the terms contained in *
 * a written agreement between you and InfinyTech3D. For further information *
 * on the licensing terms and conditions, contact: contact@infinytech3d.com  *
 *                                                                           *
 * Authors: see Authors.txt                                                  *
 * Further information: https://infinytech3d.com                             *
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnity
{
    [System.Serializable]
    public class SofaUnityRenderer
    {
        /// pointer to the SofaContext root object
        [SerializeField]
        private SofaContext m_sofaContext = null;

        [SerializeField]
        public List<SofaVisualModel> m_visualModels = null;
        

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaUnityRenderer(SofaContext context)
        {
            m_sofaContext = context;

            Init();
        }

        public void Init()
        {
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


        public void createScene(bool logResults = false)
        {
            SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();
            
            if (logResults)
                Debug.Log("### SofaUnityRenderer::createScene access API");
            
            int nbrVM = sofaPhysicsAPI_getNbrVisualModel(sofaAPI.getSimuContext());

            if (logResults)
                Debug.Log("### SofaUnityRenderer::createScene: Nbr VisualM: " + nbrVM);

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


        public void Reconnect(bool logResults = false)
        {
            if (logResults)
                Debug.Log("### SofaUnityRenderer::Reconnect method");

            if (m_visualModels == null)
                m_visualModels = new List<SofaVisualModel>();

            SofaVisualModel[] Vms = m_sofaContext.GetComponentsInChildren<SofaVisualModel>();

            if (logResults)
                Debug.Log("### SofaUnityRenderer::Reconnect: Nbr VisualM: " + Vms.Length);

            foreach (SofaVisualModel visuM in Vms)
            {
                if (logResults)
                    Debug.Log("### SofaUnityRenderer::SofaVisualModel found: " + visuM.m_uniqName);

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