using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;

namespace SofaUnity
{

    public class SceneInfo : MonoBehaviour
    {
        /// Pointer to the text GameObject.
        public GameObject textUI = null;

        public Text t_nbrModels = null;
        public Text t_nbrVertices = null;
        public Text t_nbrTriangles = null;
        public Text t_FPS = null;

        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;
        public bool m_startOnPlay = true;

        protected SofaContext SContext = null;

        /// Internal parameters for FPS
        /// {
        int frameCount = 0;
        float nextUpdate = 0.0f;
        float fps = 0.0f;
        float updateRate = 4.0f;  // 4 updates per sec.
                                  /// }

        bool computed = false;
        int nbrModel = 0;
        int nbrVertices = 0;
        int nbrTriangles = 0;

        // Use this for initialization
        void Start ()
        {
            if (!m_startOnPlay)
                return;

            nextUpdate = Time.time;
            SContext = this.GetComponent<SofaContext>();           
        }


        public void setSofaContext(SofaUnity.SofaContext _context)
        {
            SContext = _context;
            if (SContext)
                computeInfo();

            updateInfo();
        }

        public void unloadSofaContext()
        {
            SContext = null;
            nbrModel = 0;
            nbrVertices = 0;
            nbrTriangles = 0;
            updateInfo();
        }

        protected void computeInfo()
        {
            if (SContext == null)
                return;

            Debug.Log("computeInfo");
            foreach (Transform child in SContext.transform)
            {
                parseInfoChilds(child);                
                SVisualMesh obj = child.GetComponent<SVisualMesh>();
                if (obj != null)
                {
                    Debug.Log("obj " + obj.name);
                    nbrModel++;
                    nbrVertices += obj.nbVertices();
                    nbrTriangles += obj.nbTriangles();
                }
            }
           
            computed = true;
        }

        protected void parseInfoChilds(Transform parent)
        {
            foreach (Transform child in parent)
            {
                parseInfoChilds(child);

                SVisualMesh obj = child.GetComponent<SVisualMesh>();
                if (obj != null)
                {
                    Debug.Log("obj " + obj.name);
                    nbrModel++;
                    nbrVertices += obj.nbVertices();
                    nbrTriangles += obj.nbTriangles();
                }
            }
        }

        protected void updateInfo()
        {
            if (t_nbrModels)
            {
                t_nbrModels.text = "<b>Number Models: </b>" + nbrModel;
            }

            if (t_nbrVertices)
            {
                t_nbrVertices.text = "<b>Vertices: </b>" + nbrVertices;
            }

            if (t_nbrTriangles)
            {
                t_nbrTriangles.text = "<b>Triangles: </b>" + nbrTriangles;
            }
        }


        // Update is called once per frame
        void LateUpdate ()
        {
            if (SContext == null)
                return;

            if (!computed)
            {
                computeInfo();
                updateInfo();
            }

            // if true will compute the FPS of the scene
            if (displayFPS)
            {
                frameCount++;
                if (Time.time > nextUpdate)
                {
                    nextUpdate += 1.0f / updateRate;
                    fps = frameCount * updateRate * 1.5f;
                    frameCount = 0;
                }

                if (t_FPS)
                {
                    t_FPS.text = "<b>FPS: </b>" + fps;
                }
            }

            if (textUI)
            {
                Text txt = textUI.GetComponent<Text>();
                txt.text = "<b>Number Models: </b>" + nbrModel + "\n" +
                    "<b>Vertices: </b>" + nbrVertices + "\n" +
                    "<b>Triangles: </b>" + nbrTriangles + "\n";

                if (displayFPS)
                    txt.text = txt.text + "\n<b>FPS: </b>" + fps;
            }
        }
    }
}
