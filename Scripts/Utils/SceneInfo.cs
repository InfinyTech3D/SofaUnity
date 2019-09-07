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
        public GameObject textUI;
        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;

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
            nextUpdate = Time.time;
            SContext = this.GetComponent<SofaContext>();           
        }
	
	    // Update is called once per frame
	    void LateUpdate ()
        {
            if (SContext == null)
                return;

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
            }

            if (!computed)
            {
                foreach (Transform child in this.transform)
                {
                    SVisualMesh obj = child.GetComponent<SVisualMesh>();
                    if (obj != null)
                    {
                        nbrModel++;
                        nbrVertices += obj.nbVertices();
                        nbrTriangles += obj.nbTriangles();
                    }
                }
                computed = true;
            }


            Text txt = textUI.GetComponent<Text>();
            txt.text = "<b>Number Models: </b>" + nbrModel + "\n" +
                "<b>Vertices: </b>" + nbrVertices + "\n" +
                "<b>Triangles: </b>" + nbrTriangles + "\n";

            if (displayFPS)
                txt.text = txt.text + "\n<b>FPS: </b>" + fps;
        }
    }
}
