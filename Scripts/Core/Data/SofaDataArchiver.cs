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
    public List<SofaVec2Data> m_vec2Data = null;
    public List<SofaVec3Data> m_vec3Data = null;
    public List<SofaVec4Data> m_vec4Data = null;


    // unssuported types
    public List<string> m_otherNames = new List<string>();
    public List<SofaData> m_otherData = null;

    public void AddData(SofaBaseComponent owner, string dataName, string dataType)
    {
        // filter unwanted data
        if (dataName.Contains("show") || dataName.Contains("draw"))
            return;

        if (dataName == "name" || dataName == "componentState")
            return;

        bool supported = true;
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
            AddIntData(owner, dataName, value, false);
        }
        else if (dataType == "unsigned int")
        {
            int value = owner.m_impl.GetUIntValue(dataName);
            AddIntData(owner, dataName, value, true);
            dataType = "uint";
        }
        else if (dataType == "float")
        {
            float value = owner.m_impl.GetFloatValue(dataName);
            AddFloatData(owner, dataName, value);
        }
        else if (dataType == "double")
        {
            float value = owner.m_impl.GetDoubleValue(dataName);
            AddDoubleData(owner, dataName, value);
        }
        else if (dataType == "Vec2f")
        {
            Vector2 value = owner.m_impl.GetVector2Value(dataName);
            AddVec2Data(owner, dataName, value, false);
        }
        else if (dataType == "Vec2d")
        {
            Vector2 value = owner.m_impl.GetVector2Value(dataName, true);
            AddVec2Data(owner, dataName, value, true);
        }
        else if (dataType == "Vec3f")
        {
            Vector3 value = owner.m_impl.GetVector3Value(dataName);
            AddVec3Data(owner, dataName, value, false);
        }
        else if (dataType == "Vec3d")
        {
            Vector3 value = owner.m_impl.GetVector3Value(dataName, true);
            AddVec3Data(owner, dataName, value, true);
        }
        else if (dataType == "Vec4f")
        {
            Vector4 value = owner.m_impl.GetVector4Value(dataName);
            AddVec4Data(owner, dataName, value, false);
        }
        else if (dataType == "Vec4d")
        {
            Vector4 value = owner.m_impl.GetVector4Value(dataName, true);
            AddVec4Data(owner, dataName, value, true);
        }
        //else if (dataType == "vector < int >" || dataType == "vector<int>")
        //{
        //    Debug.Log(owner.UniqueNameId + " VEC: " + dataType);
        //    int res = owner.m_impl.GetVeciSize(dataName);
        //    Debug.Log(dataName + " size: " + res);
        //}
        //else if (dataType == "vector < unsigned int >" || dataType == "vector<unsigned int>")
        //{
        //    Debug.Log(owner.UniqueNameId + " VEC: " + dataType);
        //    int res = owner.m_impl.GetVeciSize(dataName);
        //    Debug.Log(dataName + " size: " + res);
        //}
        //else if(dataType.Contains("vector<"))
        //{
        //    //owner.m_impl.getV
        //}
        //else if (dataType == "vector < float >" || dataType == "vector<float>")
        //{

        //}



        //else if (dataType == "Rigid3dTypes::Coord")
        //{

        //}
        else
        {
            AddUnssuportedData(owner, dataName, dataType);
            m_otherNames.Add(dataName);
            supported = false;
        }


        if (supported)
        {
            m_names.Add(dataName);
            m_types.Add(dataType);
        }
    }


    public bool UpdateEditedData()
    {
        if (m_stringData != null)
        {
            foreach (SofaStringData data in m_stringData)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_boolData != null)
        {
            foreach (SofaBoolData data in m_boolData)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_intData != null)
        {
            foreach (SofaIntData data in m_intData)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_floatData != null)
        {
            foreach (SofaFloatData data in m_floatData)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_doubleData != null)
        {
            foreach (SofaDoubleData data in m_doubleData)
                if (data.SetValueIfEdited())
                    return true;
        }


        if (m_vec2Data != null)
        {
            foreach (SofaVec2Data data in m_vec2Data)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_vec3Data != null)
        {
            foreach (SofaVec3Data data in m_vec3Data)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_vec4Data != null)
        {
            foreach (SofaVec4Data data in m_vec4Data)
                if (data.SetValueIfEdited())
                    return true;
        }

        return false;
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
    
    public void AddIntData(SofaBaseComponent owner, string dataName, int value, bool isUnsigned = false)
    {
        if (m_intData == null) // first time
            m_intData = new List<SofaIntData>();

        m_intData.Add(new SofaIntData(owner, dataName, value, isUnsigned));
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


    public void AddVec2Data(SofaBaseComponent owner, string nameID, Vector2 value, bool isDouble = false)
    {
        if (m_vec2Data == null) // first time
            m_vec2Data = new List<SofaVec2Data>();

        m_vec2Data.Add(new SofaVec2Data(owner, nameID, value, isDouble));
    }

    public void AddVec3Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble = false)
    {
        if (m_vec3Data == null) // first time
            m_vec3Data = new List<SofaVec3Data>();

        m_vec3Data.Add(new SofaVec3Data(owner, nameID, value, isDouble));
    }


    public void AddVec4Data(SofaBaseComponent owner, string nameID, Vector4 value, bool isDouble = false)
    {
        if (m_vec4Data == null) // first time
            m_vec4Data = new List<SofaVec4Data>();

        m_vec4Data.Add(new SofaVec4Data(owner, nameID, value, isDouble));
    }


    public void AddUnssuportedData(SofaBaseComponent owner, string nameID, string type)
    {
        if (m_otherData == null) // first time
            m_otherData = new List<SofaData>();

        m_otherData.Add(new SofaData(owner, nameID, type));
    }


    public SofaStringData GetSofaStringData(string dataName)
    {
        foreach(SofaStringData data in m_stringData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaBoolData GetSofaBoolData(string dataName)
    {
        foreach (SofaBoolData data in m_boolData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaIntData GetSofaIntData(string dataName)
    {
        foreach (SofaIntData data in m_intData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaFloatData GetSofaFloatData(string dataName)
    {
        foreach (SofaFloatData data in m_floatData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaDoubleData GetSofaDoubleData(string dataName)
    {
        foreach (SofaDoubleData data in m_doubleData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaVec2Data GetSofaVec2Data(string dataName)
    {
        foreach (SofaVec2Data data in m_vec2Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaVec3Data GetSofaVec3Data(string dataName)
    {
        foreach (SofaVec3Data data in m_vec3Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaVec4Data GetSofaVec4Data(string dataName)
    {
        foreach (SofaVec4Data data in m_vec4Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    public SofaData GetGenericData(string dataName)
    {
        foreach (SofaData data in m_otherData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
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
