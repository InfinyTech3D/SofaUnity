using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SofaGraphicOGL_ApplyXRay : AbtractSofaGraphicCall
{
    Texture2D m_tex2D_XRay;

    public string SofaPath = "/XRenderer/XRendererDeOuf";

    protected override int InitCall()
    {
        GameObject plane_XRay = GameObject.Find("Plane_XRay");
        if (plane_XRay != null)
        {
            m_tex2D_XRay = new Texture2D(512, 512, TextureFormat.RGBAHalf, false);
            m_tex2D_XRay.Apply();

            plane_XRay.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D_XRay;
            plane_XRay.transform.GetComponent<Renderer>().enabled = true;
            //plane_XRay.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

            int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetVirtualXRayTexture(m_sofaContext.GetSimuContext(),
                m_tex2D_XRay.GetNativeTexturePtr(), m_tex2D_XRay.width, m_tex2D_XRay.height, SofaPath);

            Debug.Log("Created XRay Texture with ID " + m_tex2D_XRay.GetNativeTexturePtr());
            return renderID;
        }
        return -1;
    }
    
    protected override void BeforeDestroy()
    {
        if (m_tex2D_XRay)
            Destroy(m_tex2D_XRay);
    }

}
