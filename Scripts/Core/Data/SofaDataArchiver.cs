using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

[System.Serializable]
public class SofaDataArchiver //: MonoBehaviour, ISerializationCallbackReceiver
{
    public List<string> m_names = new List<string>();
    public List<string> m_types = new List<string>();

    // basic types
    public List<SofaStringData> m_stringData = null;
    public List<SofaBoolData> m_boolData = null;
    public List<SofaIntData> m_intData = null;
    public List<SofaFloatData> m_floatData = null;
    public List<SofaDoubleData> m_doubleData = null;

    // vector types
    public List<SofaVec3fData> m_vec3fData = null;


    // unssuported types
    public List<SofaData> m_otherData = null;

    public void AddData(SofaBaseComponent owner, string dataName, string dataType)
    {
        if (dataType == "string")
        {
            string value = owner.m_impl.getStringValue(dataName);
            AddStringData(owner, dataName, value);
        }
        else if (dataType == "bool")
        {
            bool value = owner.m_impl.GetBoolValue(dataName);
            AddBoolData(owner, dataName, value);
        }
        else if (dataType == "int")
        {
            int value = owner.m_impl.GetIntValue(dataName);
            AddIntData(owner, dataName, value);
        }
        else if (dataType == "float")
        {
            float value = owner.m_impl.GetFloatValue(dataName);
            AddFloatData(owner, dataName, value);
        }
        else if (dataType == "double")
        {
            float value = owner.m_impl.GetFloatValue(dataName);
            AddDoubleData(owner, dataName, value);
        }
        else if (dataType == "Vec3f")
        {
            Vector3 value = owner.m_impl.GetVector3fValue(dataName);
            AddVec3fData(owner, dataName, value);
        }
        //else if (dataType == "Vec3d" || dataType == "Vec3f")
        //{

        //}
        //else if (dataType == "vector < float >" || dataType == "vector<float>")
        //{

        //}
        //else if (dataType == "vector < int >" || dataType == "vector<int>")
        //{

        //}
        
       
        //else if (dataType == "Rigid3dTypes::Coord")
        //{

        //}
        else
        {

        }

        m_names.Add(dataName);
        m_types.Add(dataType);
    }


    public void AddStringData(SofaBaseComponent owner, string dataName, string value)
    {
        if (m_stringData == null) // first time
            m_stringData = new List<SofaStringData>();

        m_stringData.Add(new SofaStringData(owner, dataName, value));
    }
    
    public void AddBoolData(SofaBaseComponent owner, string dataName, bool value)
    {
        if (m_boolData == null) // first time
            m_boolData = new List<SofaBoolData>();

        m_boolData.Add(new SofaBoolData(owner, dataName, value));
    }
    
    public void AddIntData(SofaBaseComponent owner, string dataName, int value)
    {
        if (m_intData == null) // first time
            m_intData = new List<SofaIntData>();

        m_intData.Add(new SofaIntData(owner, dataName, value));
    }
    
    public void AddFloatData(SofaBaseComponent owner, string nameID, float value)
    {
        if (m_floatData == null) // first time
            m_floatData = new List<SofaFloatData>();

        m_floatData.Add(new SofaFloatData(owner, nameID, value));
    }

    public void AddDoubleData(SofaBaseComponent owner, string nameID, float value)
    {
        if (m_doubleData == null) // first time
            m_doubleData = new List<SofaDoubleData>();

        m_doubleData.Add(new SofaDoubleData(owner, nameID, value));
    }



    public void AddVec3fData(SofaBaseComponent owner, string nameID, Vector3 value)
    {
        if (m_vec3fData == null) // first time
            m_vec3fData = new List<SofaVec3fData>();

        m_vec3fData.Add(new SofaVec3fData(owner, nameID, value));
    }


    public void AddUnssuportedData(SofaBaseComponent owner, string nameID, string type)
    {
        if (m_otherData == null) // first time
            m_otherData = new List<SofaData>();

        m_otherData.Add(new SofaData(owner, nameID, type));
    }


    //public void Log()
    //{
    //    Debug.Log("SofaDataArchiver m_floatData: " + m_floatData.Count);

    //    if (m_vec3fData != null)
    //        Debug.Log("SofaDataArchiver m_vec3fData: " + m_vec3fData.Count);

    //    foreach (SofaDataFloat data in m_floatData)
    //    {
    //        data.Log();
    //    }
    //}
}
