using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SofaGraphicOGL_ApplyUS : AbtractSofaGraphicCall
{
    Texture2D m_tex2D_US;

    public string SofaPath = "/Renderer";
    [SerializeField]
    private int widthResolution = 512;

    [SerializeField]
    private int heightResolution = 512;

    protected override int InitCall() 
    {
        GameObject plane_US = GameObject.Find("Plane_US");
        if (plane_US != null)
        {
            m_tex2D_US = new Texture2D(widthResolution, heightResolution, TextureFormat.RGBAFloat, false); //US does not allow half float
            m_tex2D_US.Apply();

            plane_US.transform.GetComponent<Renderer>().material.mainTexture = m_tex2D_US;
            plane_US.transform.GetComponent<Renderer>().enabled = true;
            plane_US.transform.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

            int renderID = SofaUnityAPI.SofaGraphicAPI.AddRenderEvent_SetImagingUSTexture(m_sofaContext.GetSimuContext(),
                m_tex2D_US.GetNativeTexturePtr(), m_tex2D_US.width, m_tex2D_US.height, SofaPath);

            Debug.Log("Created US Texture with ID " + m_tex2D_US.GetNativeTexturePtr());
            //int test = (int)(m_tex2D_US.GetNativeTexturePtr());
            return renderID;
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

