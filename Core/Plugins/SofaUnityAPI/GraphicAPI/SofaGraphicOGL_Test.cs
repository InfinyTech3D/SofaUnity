using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace SofaUnityAPI
{
    public class SofaGraphicOGL_Test : MonoBehaviour
    {
        bool m_bNewStep = false;
        IntPtr m_glTexID;
        Texture2D m_tex2D;
        bool m_GLRenderEnd = true;
        List<int> m_registeredRenderIDList = new List<int>();

        public SofaUnity.SofaContext m_sofaContext = null;

        IEnumerator Start()
        {
            //Get SofaContext
            if (m_sofaContext == null)
            {
                GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
                if (_contextObject != null)
                {
                    // Get Sofa context
                    m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();

                }
                else
                {
                    Debug.LogError("SofaGraphicOGL_Test - No SofaContext found.");
                }
            }

            m_tex2D = new Texture2D(512, 512, TextureFormat.RGBAHalf, false);
            m_tex2D.Apply();

            GameObject plane = GameObject.Find("Plane");
            plane.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D;
            plane.transform.GetComponent<Renderer>().enabled = true;
            plane.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

            if (m_sofaContext != null)
            {
                //int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetTexture(m_sofaContext.GetSimuContext(), m_tex2D.GetNativeTexturePtr(), m_tex2D.width, m_tex2D.height);
                //m_registeredRenderIDList.Add(renderID);
                //int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetPluginTexture(m_sofaContext.GetSimuContext(), m_tex2D.GetNativeTexturePtr(), m_tex2D.width, m_tex2D.height, "/Liver/builder1");
                //m_registeredRenderIDList.Add(renderID);
                int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetVirtualXRayTexture(m_sofaContext.GetSimuContext(), m_tex2D.GetNativeTexturePtr(), m_tex2D.width, m_tex2D.height, "/XRay/XRendererDeOuf");
                m_registeredRenderIDList.Add(renderID);
            }

            Debug.Log("Created Texture with ID " + m_tex2D.GetNativeTexturePtr());

            yield return StartCoroutine("CallWaitForEndOfFrame");
        }

        void Update()
        {
            m_bNewStep = true;
        }

        private IEnumerator CallWaitForEndOfFrame()
        {
            while (true)
            {
                // Wait until all frame rendering is done
                yield return new WaitForEndOfFrame();

                m_GLRenderEnd = false;

                // Issue a plugin event with arbitrary integer identifier.
                // The plugin can distinguish between different
                // things it needs to do based on this ID.
                // For our simple plugin, it does not matter which ID we pass here.
                if (m_bNewStep && m_sofaContext != null)
                {
                    foreach (int renderID in m_registeredRenderIDList)
                    {
                        //Debug.Log("Calling renderID " + renderID);
                        GL.IssuePluginEvent(SofaUnityAPI.SofaGraphicAPI.getRenderEventFunc(), renderID);
                    }
                }

                m_GLRenderEnd = true;
                m_bNewStep = false;
            }
        }

        void OnDestroy()
        {
            SofaUnityAPI.SofaGraphicAPI.clearUp(m_sofaContext.GetSimuContext());
            Destroy(m_tex2D);
        }

    }
}
