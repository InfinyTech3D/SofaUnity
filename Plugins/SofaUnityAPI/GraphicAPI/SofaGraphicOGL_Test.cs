using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SofaGraphicOGL_Test : MonoBehaviour
{
    bool m_bNewStep = false;
    IntPtr m_glTexID;
    Texture2D m_tex2D_XRay;
    Texture2D m_tex2D_US;
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

        if (m_sofaContext != null)
        {
            GameObject plane_XRay = GameObject.Find("Plane_XRay");
            if (plane_XRay != null)
            {
                string sofaPath = "/XRay/XRendererDeOuf";

                m_tex2D_XRay = new Texture2D(512, 512, TextureFormat.RGBAHalf, false);
                m_tex2D_XRay.Apply();

                plane_XRay.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D_XRay;
                plane_XRay.transform.GetComponent<Renderer>().enabled = true;
                plane_XRay.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

                int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetVirtualXRayTexture(m_sofaContext.GetSimuContext(), 
                    m_tex2D_XRay.GetNativeTexturePtr(), m_tex2D_XRay.width, m_tex2D_XRay.height, sofaPath);

                m_registeredRenderIDList.Add(renderID);
                Debug.Log("Created XRay Texture with ID " + m_tex2D_XRay.GetNativeTexturePtr());
            }

            GameObject plane_US = GameObject.Find("Plane_US");
            if (plane_US != null)
            {
                string sofaPath = "/Renderer";

                m_tex2D_US = new Texture2D(512, 512, TextureFormat.RGBAFloat, false); //US does not allow half float
                m_tex2D_US.Apply();

                plane_US.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D_US;
                plane_US.transform.GetComponent<Renderer>().enabled = true;
                plane_US.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

                int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetImagingUSTexture(m_sofaContext.GetSimuContext(), 
                    m_tex2D_US.GetNativeTexturePtr(), m_tex2D_US.width, m_tex2D_US.height, sofaPath);

                m_registeredRenderIDList.Add(renderID);
                Debug.Log("Created US Texture with ID " + m_tex2D_US.GetNativeTexturePtr());
            }
        }
        
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
        if (m_tex2D_XRay)
            Destroy(m_tex2D_XRay);
        if (m_tex2D_US)
            Destroy(m_tex2D_US);
    }

}

