using UnityEngine;
using SofaUnity;

public class Texture2DFromRaw : MonoBehaviour
{
    public GameObject target;
    public string dataName = "";

    protected SData rawImg = null;
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
        }
    }
    protected bool firstTime = true;
    public void Update()
    {
        if (rawImg != null && m_object != null)
        {
            int res = m_object.impl.getVecfSize(rawImg.nameID);
            Debug.Log("value: " + res);

            if (res > 0)
            {
                if (m_texture == null)
                {
                    m_texture = new Texture2D(10, 10);
                    m_rawData = new float[res];
                    for (int i = 0; i < 100; i++)
                        m_rawData[i] = 69;
                    GetComponent<Renderer>().material.mainTexture = m_texture;
                }

                int resValue = m_object.impl.getVecfValue(rawImg.nameID, res, m_rawData);
                Debug.Log("resValue: " + resValue);
                int cpt = 0;
                for (int y = 0; y < m_texture.height; y++)
                {
                    for (int x = 0; x < m_texture.width; x++)
                    {
                        //Color color = ((x & y) != 0 ? Color.white : Color.gray);
                        float value = m_rawData[cpt]*10;
                        if (firstTime)
                            Debug.Log(cpt + " -> " + value);
                        m_texture.SetPixel(x, y, new Vector4(value, value, value, 1));
                        ////m_texture.SetPixel(x, y, color);
                        cpt++;
                    }
                }
                m_texture.Apply();
                GetComponent<Renderer>().material.mainTexture = m_texture;

                if (firstTime)
                    firstTime = false;
            }
        }
    }
}