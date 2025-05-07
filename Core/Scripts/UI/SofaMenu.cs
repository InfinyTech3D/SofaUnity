using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SofaUnity
{
    public class SofaMenu : MonoBehaviour
    {
        ////////////////////////////////////////////
        /////     SofaMenu public members      /////
        ////////////////////////////////////////////

        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;
        /// Dedicated text object to display the SOFA scene.
        public Text t_sofaFilename = null;
        /// Dedicated text object to display the unity simulation FPS.
        public Text t_unityFPS = null;
        /// Dedicated text object to display the sofa simulation FPS.
        public Text t_sofaFPS = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
