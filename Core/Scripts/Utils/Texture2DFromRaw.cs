using UnityEngine;
using SofaUnity;

/// <summary>
/// Script that create a 2D texture from a raw data.
/// work in progress.
/// </summary>
public class Texture2DFromRaw : SofaBaseObject
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////

    /// GameObject having the SComponentObject that is listening the raw img Data
    public GameObject m_target = null;
    /// SComponentObject that is listening the raw img Data 
    protected SofaComponent m_object = null;

    /// 2D texture object created in this component
    protected Texture2D m_texture = null;
    /// Size of the texture
    protected int texWidth = 400;
    protected int texHeight = 400;

    /// Name of the Data containing the raw data
    public string dataName = "";
    /// Pointer to the Data containing the raw data
    //protected old_SofaData rawImg = null;

    /// Name of the Data containing the raw diff data
    public string dataName2 = "";
    /// Pointer to the Data containing the raw diff data
    //protected old_SofaData rawImgDiff = null;

    /// raw data of the 2d texture
    protected float[] m_rawData = null;




    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////
    void Awake()
    {
        // override the default SofaBaseObject Awake
    }

    public void Start()
    {
        if (m_target == null)
            return;

        // TODO: restore that
        //m_object = m_target.GetComponent<SComponentObject>();
        //foreach (old_SofaData entry in m_object.datas)
        //{
        //    if (entry.nameID == dataName)
        //    {
        //        Debug.Log("found: " + dataName);
        //        rawImg = entry;
        //    }
        //    else if (entry.nameID == dataName2)
        //    {
        //        rawImgDiff = entry;
        //    }
        //}

        // find SofaContext and register this object
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            SofaContext _sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            if (_sofaContext == null)
            {
                Debug.LogError("Texture2DFromRaw::loadContext - GetComponent<SofaUnity.SofaContext> failed.");
            }
            //else
            //    _sofaContext.registerObject(this);
        }
        else
        {
            Debug.LogError("Texture2DFromRaw::loadContext - No SofaContext found.");
        }
    }


    protected bool firstTime = true;
    protected bool initDiff = false;
    protected int textId = -1;

    /// Method called by @sa Update() method. To be implemented by child class.
    //public override void updateImpl()
    public void Update()
    {
        //int resId = m_object.impl.getIntValue_deprecated("frameId");
        
        //if (resId == textId)
        //    return;

        //textId = resId;

        //if (m_object != null)
        //{            
        //    if (m_texture == null && rawImg != null) // first time create init texture
        //    {
        //        //Debug.Log("Texture2DFromRaw::updateImpl:   m_object != null ");
        //        int res = m_object.impl.getVecfSize_deprecated(rawImg.nameID);
        //        if (res == 0)
        //        {
        //            Debug.LogError("Texture2DFromRaw image size is 0");
        //            return;
        //        }

        //        if (m_texture == null)
        //        {
        //            m_texture = new Texture2D(texWidth, texHeight);
        //            m_rawData = new float[res];
        //            GetComponent<Renderer>().material.mainTexture = m_texture;                    
        //        }


        //        int resValue = m_object.impl.getVectorfValue_deprecated(rawImg.nameID, res, m_rawData);
        //        int cpt = 0;
        //        int cpt1 = 0;
        //        //var line = "";
        //        for (int y = 0; y < m_texture.height; y++)
        //        {
        //            for (int x = 0; x < m_texture.width; x++)
        //            {
        //                //Color color = ((x & y) != 0 ? Color.white : Color.gray);
        //                float value = m_rawData[cpt];
        //                // line = line + value + " ";
        //                //if (cpt<1000)
        //                //    Debug.Log(cpt + " -> " + value);

        //                if (value == 1)
        //                    cpt1++;

        //                m_texture.SetPixel(x, y, new Vector4(value, value, value, 1));
        //                ////m_texture.SetPixel(x, y, color);
        //                cpt++;
        //            }

        //            // line = line + " || ";                    
        //        }
        //        // Debug.Log(line);
        //        // Debug.Log("cpt1: " + cpt1);
        //        m_texture.Apply();
        //        return;
        //    }


        //    if (m_texture != null && rawImgDiff != null) // second time, init diff image (used for optimisation as sparse data)
        //    {
        //        int resDiff = m_object.impl.getVecfSize_deprecated(rawImgDiff.nameID);
        //        //Debug.Log("resDiff : " + resDiff);
        //        if (resDiff == 0)
        //            return;

        //        if (initDiff == false)
        //        {
        //            m_rawData = new float[resDiff];
        //            initDiff = true;
        //        }

        //        int resValue = m_object.impl.getVectorfValue_deprecated(rawImgDiff.nameID, resDiff, m_rawData);

        //        for (int i = 0; i < resDiff; i += 2)
        //        {
        //            int id = (int)m_rawData[i];
        //            if (id == -1)
        //            {
        //                // Debug.Log("Stop at: " + i*0.5);
        //                break;
        //            }

        //            int y = (int)Mathf.Floor(id / texWidth);
        //            int x = id % texHeight;
        //            float value = m_rawData[i + 1];
        //            m_texture.SetPixel(x, y, new Vector4(value, value, value, 1));
        //        }
        //        m_texture.Apply();
        //    }
        //}
    }

}