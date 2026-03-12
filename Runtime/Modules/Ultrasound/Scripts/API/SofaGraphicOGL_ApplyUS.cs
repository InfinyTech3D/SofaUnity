using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


namespace SofaUnityAPI
{

    public class SofaGraphicOGL_ApplyUS : AbtractSofaGraphicCall
    {
        Texture2D m_tex2D_US;

        public GameObject plane_US;

        [SerializeField]
        public string PlaneObjectName = "Plane_US";

        [SerializeField]
        public string SofaPath = "/Renderer";

        [SerializeField]
        private int widthResolution = 512;

        [SerializeField]
        private int heightResolution = 512;


        protected override bool checkGraphicsDeviceType()
        {
            if (!(SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore) && !(SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11))
            {
                Debug.LogError("SofaGraphicCall - The current Graphics device type does not support Ultrasound rendering.");
                return false;
            }

            return true;
        }

        protected override int InitCall()
        {
            if (plane_US == null)
            {
                plane_US = GameObject.Find(PlaneObjectName);          
            }

            if (plane_US != null)
            {
                //m_tex2D_US = new Texture2D(widthResolution, heightResolution, TextureFormat.RGBAFloat, false); //US does not allow half float
                m_tex2D_US = new Texture2D(widthResolution, heightResolution, TextureFormat.RGBA32, false); //US does not allow half float
                m_tex2D_US.Apply();
          

                plane_US.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D_US;
                plane_US.transform.GetComponent<Renderer>().enabled = true;
                plane_US.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));


                int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetImagingUSTexture(m_sofaContext.GetSimuContext(),
                    m_tex2D_US.GetNativeTexturePtr(), m_tex2D_US.width, m_tex2D_US.height, SofaPath, ((int)SystemInfo.graphicsDeviceType));

                Debug.Log("Created US Texture with ID " + m_tex2D_US.GetNativeTexturePtr());
                //int test = (int)(m_tex2D_US.GetNativeTexturePtr());
                return renderID;
            }
            else
            {
                Debug.LogError("US texture GameObject couldn't be found, specify it in the US config");
            }
            return -1;
        }

        protected override void BeforeDestroy()
        {
            if (m_tex2D_US)
            {
                Destroy(m_tex2D_US);
            }
        }

    }

}
