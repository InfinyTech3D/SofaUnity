using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;
using System.IO;

namespace SofaUnity
{
    [System.Serializable]
    public class SceneFileManager
    {
        protected SofaContext m_sofaContext = null;
        public SceneFileManager(SofaContext sofaContext)
        {
            m_sofaContext = sofaContext;
        }

        public void SetSofaContext(SofaContext sofaContext)
        {
            m_sofaContext = sofaContext;
        }

        [SerializeField]
        protected bool m_hasScene = false;
        public bool HasScene
        {
            get { return m_hasScene; }
        }

        [SerializeField]
        protected string m_filename = "";
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

        public string AbsoluteFilename()
        {
            return Application.dataPath + m_filename;
        }


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

        public void LoadFilename()
        {
            m_sofaContext.LoadSofaScene();
            m_hasScene = true;
        }
    }
}
