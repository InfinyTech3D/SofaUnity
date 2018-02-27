using UnityEngine;
using SofaUnity;

public class Texture2DFromRaw : MonoBehaviour
{
    public GameObject target;
    public string dataName = "";
    public string dataName2 = "";

    protected SData rawImg = null;
    protected SData rawImgDiff = null;
    protected SComponentObject m_object = null;
    protected Texture2D m_texture = null;
    protected float[] m_rawData = null;

    public void Start()
    {
        //// Create a 16x16 texture with PVRTC RGBA4 format
        //// and will it with raw PVRTC bytes.
        //Texture2D tex = new Texture2D(16, 16, TextureFormat.PVRTC_RGBA4, false);
        //// Raw PVRTC4 data for a 16x16 texture. This format is four bits
        //// per pixel, so data should be 16*16/2=128 bytes in size.
        //// Texture that is encoded here is mostly green with some angular
        //// blue and red lines.
        //byte[] pvrtcBytes = new byte[] {
        //    0x30, 0x32, 0x32, 0x32, 0xe7, 0x30, 0xaa, 0x7f, 0x32, 0x32, 0x32, 0x32, 0xf9, 0x40, 0xbc, 0x7f,
        //    0x03, 0x03, 0x03, 0x03, 0xf6, 0x30, 0x02, 0x05, 0x03, 0x03, 0x03, 0x03, 0xf4, 0x30, 0x03, 0x06,
        //    0x32, 0x32, 0x32, 0x32, 0xf7, 0x40, 0xaa, 0x7f, 0x32, 0xf2, 0x02, 0xa8, 0xe7, 0x30, 0xff, 0xff,
        //    0x03, 0x03, 0x03, 0xff, 0xe6, 0x40, 0x00, 0x0f, 0x00, 0xff, 0x00, 0xaa, 0xe9, 0x40, 0x9f, 0xff,
        //    0x5b, 0x03, 0x03, 0x03, 0xca, 0x6a, 0x0f, 0x30, 0x03, 0x03, 0x03, 0xff, 0xca, 0x68, 0x0f, 0x30,
        //    0xaa, 0x94, 0x90, 0x40, 0xba, 0x5b, 0xaf, 0x68, 0x40, 0x00, 0x00, 0xff, 0xca, 0x58, 0x0f, 0x20,
        //    0x00, 0x00, 0x00, 0xff, 0xe6, 0x40, 0x01, 0x2c, 0x00, 0xff, 0x00, 0xaa, 0xdb, 0x41, 0xff, 0xff,
        //    0x00, 0x00, 0x00, 0xff, 0xe8, 0x40, 0x01, 0x1c, 0x00, 0xff, 0x00, 0xaa, 0xbb, 0x40, 0xff, 0xff,
        //};
        //// Load data into the texture and upload it to the GPU.
        //tex.LoadRawTextureData(pvrtcBytes);
        //tex.Apply();
        //// Assign texture to renderer's material.
        //GetComponent<Renderer>().material.mainTexture = tex;

        m_object = target.GetComponent<SComponentObject>();
        foreach (SData entry in m_object.datas)
        {
            if (entry.nameID == dataName)
            {
                Debug.Log("found: " + dataName);
                rawImg = entry;
            }
            else if (entry.nameID == dataName2)
            {
                rawImgDiff = entry;
            }
        }
    }
    protected bool firstTime = true;
    protected bool initDiff = false;
    protected int texWidth = 400;
    protected int texHeight = 400;
    public void Update()
    {
        if (m_object != null)
        {
            if (m_texture == null && rawImg != null) // first time create init texture
            {
                int res = m_object.impl.getVecfSize(rawImg.nameID);
                if (res == 0)
                    return;

                if (m_texture == null)
                {
                    m_texture = new Texture2D(texWidth, texHeight);
                    m_rawData = new float[res];
                    GetComponent<Renderer>().material.mainTexture = m_texture;
                }
                //for (int i = 0; i < 100; i++)
                //    m_rawData[i] = 69;
                

                int resValue = m_object.impl.getVecfValue(rawImg.nameID, res, m_rawData);
                int cpt = 0;
                int cpt1 = 0;
                //var line = "";
                for (int y = 0; y < m_texture.height; y++)
                {                    
                    for (int x = 0; x < m_texture.width; x++)
                    {
                        //Color color = ((x & y) != 0 ? Color.white : Color.gray);
                        float value = m_rawData[cpt];
                       // line = line + value + " ";
                        //if (cpt<1000)
                        //Debug.Log(cpt + " -> " + value);

                        if (value == 1)
                            cpt1++;

                        m_texture.SetPixel(x, y, new Vector4(value, value, value, 1));
                        ////m_texture.SetPixel(x, y, color);
                        cpt++;
                    }

                   // line = line + " || ";                    
                }
               // Debug.Log(line);
                Debug.Log("cpt1: " + cpt1);
                m_texture.Apply();
                return;
            }

            
            if (m_texture != null && rawImgDiff != null) // second time, init diff image (used for optimisation as sparse data)
            {
                int resDiff = m_object.impl.getVecfSize(rawImgDiff.nameID);
                //Debug.Log("resDiff : " + resDiff);
                if (resDiff == 0)
                    return;

                if (initDiff == false)
                {
                    m_rawData = new float[resDiff];
                    initDiff = true;
                }

                int resValue = m_object.impl.getVecfValue(rawImgDiff.nameID, resDiff, m_rawData);

                for (int i = 0; i < resDiff; i += 2)
                {
                    int id = (int)m_rawData[i];
                    if (id == -1)
                    {
                        Debug.Log("Stop at: " + i*0.5);
                        break;
                    }

                    int y = (int)Mathf.Floor(id / texWidth);
                    int x = id % texHeight;
                    float value = m_rawData[i + 1];
                    m_texture.SetPixel(x, y, new Vector4(value, value, value, 1));
                }
                m_texture.Apply();
            }
        }
    }
}