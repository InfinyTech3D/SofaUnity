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
using System.IO;

namespace SofaUnity
{
    /// <summary>
    /// Class to manage the Sofa scene file loaded.
    /// </summary>
    [System.Serializable]
    public class SceneFileManager
    {
        ////////////////////////////////////////////
        //////    SceneFileManager members     /////
        ////////////////////////////////////////////

        /// Pointer to the SofaContext
        protected SofaContext m_sofaContext = null;

        [SerializeField]
        protected bool m_hasScene = false;

        [SerializeField]
        protected string m_filename = "";


        ////////////////////////////////////////////
        //////   SceneFileManager accessors    /////
        ////////////////////////////////////////////

        /// Default constructor taking a SofaContext as argument
        public SceneFileManager(SofaContext sofaContext)
        {
            m_sofaContext = sofaContext;
        }


        /// Method to set the SofaContext to be used by this FileManager
        public void SetSofaContext(SofaContext sofaContext)
        {
            m_sofaContext = sofaContext;
        }


        /// getter to \sa m_hasScene
        public bool HasScene
        {
            get { return m_hasScene; }
        }


        /// getter/setter of the \sa m_filename
        public string SceneFilename
        {
            get { return m_filename; }
            set
            {
                Debug.Log("setFilename: " + value);
                if (CheckValidFilename(value))
                {
                    if (HasScene) // already one loaded
                    {
                        m_sofaContext.ClearSofaScene();
                    }

                    LoadFilename();
                }
            }
        }

        //public string PythonSceneFilename
        //{
        //    get { return m_filename; }
        //    set
        //    {
        //        Debug.Log("Set Python Filename: " + value);
        //        if (CheckValidFilename(value))
        //        {
        //            if (HasScene) // already one loaded
        //            {
        //                m_sofaContext.ClearSofaScene();
        //            }

        //            // load python plugin first
        //            m_sofaContext.PluginManagerInterface.LoadPlugin("SofaPython3");

        //            LoadFilename();
        //        }
        //    }
        //}



        /// method to get the full path of the file inside the unity asset
        public string AbsoluteFilename()
        {
            return Application.dataPath + m_filename;
        }



        ////////////////////////////////////////////
        //////  SceneFileManager internal API  /////
        ////////////////////////////////////////////

        /// Internal method to check if the filename is valid
        protected bool CheckValidFilename(string newFilename)
        {
            if (!File.Exists(Application.dataPath + newFilename)) // if not found test with relative path
            {
                int pos = newFilename.IndexOf("Assets", 0);
                if (pos > 0)
                {
                    newFilename = newFilename.Substring(pos + 6); // remove all path until Assets/ to make it relative
                }

                // Fix due to change of scene folder:
                int pos2 = newFilename.IndexOf("SofaUnity", 0);
                if (pos2 < 0)
                    newFilename = "/SofaUnity/" + newFilename;
            }

            if (!File.Exists(Application.dataPath + newFilename)) // try again with relative path
            {
                return false;
            }

            // if ok update m_filename for loading
            m_filename = newFilename;
            return true;
        }

        /// Internal method to load the file in the SofaContext.
        protected void LoadFilename()
        {
            m_sofaContext.LoadSofaScene();
            m_hasScene = true;
        }
    }
}
