using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;

namespace SofaUnity
{
    /// <summary>
    /// Component that will update a text GameObject using information from its SofaBaseMesh
    /// This class need to be added to a child class of @see SofaBaseMesh and linked to a text GameObject
    /// </summary>
    public class ObjectInfo : MonoBehaviour
    {
        /// Pointer to the text GameObject.
        public GameObject textUI;
        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;

        /// Internal parameters for FPS
        /// {
        int frameCount = 0;
        float nextUpdate = 0.0f;
        float fps = 0.0f;
        float updateRate = 4.0f;  // 4 updates per sec.
        /// }

        // Use this for initialization
        void Start()
        {
            nextUpdate = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            Text txt = textUI.GetComponent<Text>();
            SofaMeshObject baseMesh = this.GetComponent<SofaMeshObject>();

            int nbV = baseMesh.nbVertices();
            int nbTri = baseMesh.nbTriangles();

            // if true will compute the FPS of the scene
            if (displayFPS)
            {
                frameCount++;
                if (Time.time > nextUpdate)
                {
                    nextUpdate += 1.0f / updateRate;
                    fps = frameCount * updateRate;
                    frameCount = 0;
                }
            }

            txt.text = "<b>Model: </b>" + baseMesh.name + "\n" +
                "<b>Vertices: </b>" + nbV + "\n" +
                "<b>Elements: </b>" + nbTri + "\n";

            if (displayFPS)
                txt.text = txt.text + "\n<b>FPS: </b>" + fps;

        }
    }
}
