using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;
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

        public string PythonSceneFilename
        {
            get { return m_filename; }
            set
            {
                Debug.Log("Set Python Filename: " + value);
                if (CheckValidFilename(value))
                {
                    if (HasScene) // already one loaded
                    {
                        m_sofaContext.ClearSofaScene();
                    }

                    // load python plugin first
                    m_sofaContext.PluginManagerInterface.LoadPlugin("SofaPython3");

                    LoadFilename();
                }
            }
        }



        /// method to get the full path of the file inside the unity asset
        public string AbsoluteFilename()
        {
            return SofaContextAPI.getResourcesPath() + m_filename;
        }



        ////////////////////////////////////////////
        //////  SceneFileManager internal API  /////
        ////////////////////////////////////////////

        /// Internal method to check if the filename is valid
        protected bool CheckValidFilename(string newFilename)
        {
            if (!File.Exists(SofaContextAPI.getResourcesPath() + newFilename)) // if not found test with relative path
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

            if (!File.Exists(SofaContextAPI.getResourcesPath() + newFilename)) // try again with relative path
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
