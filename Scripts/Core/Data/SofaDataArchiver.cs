using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Class to create SofaData with the different specialization and store them in Lists
/// </summary>
[System.Serializable]
public class SofaDataArchiver
{
    /// List of Data names stored in this Archvier over all List of Data
    public List<string> m_names = new List<string>();
    /// List of Data types stored in this Archvier over all List of Data
    public List<string> m_types = new List<string>();

    /// List of basic types of Sofa Data
    public List<SofaStringData> m_stringData = null;
    public List<SofaBoolData> m_boolData = null;
    public List<SofaIntData> m_intData = null;
    public List<SofaFloatData> m_floatData = null;
    public List<SofaDoubleData> m_doubleData = null;

    // List of vector types of Sofa Data
    public List<SofaVec2Data> m_vec2Data = null;
    public List<SofaVec3Data> m_vec3Data = null;
    public List<SofaVec4Data> m_vec4Data = null;

    public List<SofaVec2IntData> m_vec2iData = null;
    public List<SofaVec3IntData> m_vec3iData = null;


    /// List of unssuported type names stored in this Archiver
    public List<string> m_otherNames = new List<string>();
    /// List of unssuported Data stored in this Archiver
    public List<SofaData> m_otherData = null;


    /// Method to add a Sofa Data to be stored with all the info to create it will call the right specialised Add method
    public void AddData(SofaBaseComponent owner, string dataName, string dataType)
    {
        // filter unwanted data
        if (dataName.Contains("show") || dataName.Contains("draw")) // no gui Data
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
        else if (dataType == "int" || dataType == "i")
        {
            int value = owner.m_impl.GetIntValue(dataName);
            dataType = "i";
            AddIntData(owner, dataName, value, false);
        }
        else if (dataType == "unsigned int" || dataType == "I")
        {
            int value = owner.m_impl.GetUIntValue(dataName);
            AddIntData(owner, dataName, value, true);
            dataType = "uint";
        }
        else if (dataType == "float" || dataType == "f")
        {
            float value = owner.m_impl.GetFloatValue(dataName);
            dataType = "f";
            AddFloatData(owner, dataName, value);
        }
        else if (dataType == "double" || dataType == "d")
        {
            float value = owner.m_impl.GetDoubleValue(dataName);
            dataType = "d";
            AddDoubleData(owner, dataName, value);
        }
        else if (dataType == "Vec2i")
        {
            Vector2Int value = owner.m_impl.GetVector2iValue(dataName);
            AddVec2IntData(owner, dataName, value, false);
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
        else if (dataType == "Vec3i")
        {
            Vector3Int value = owner.m_impl.GetVector3iValue(dataName);
            AddVec3IntData(owner, dataName, value, false);
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


    /// Method to check if any Data stored int he Archiver has been modified by the Editor (TODO: optimize that part)
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


        if (m_vec2iData != null)
        {
            foreach (SofaVec2IntData data in m_vec2iData)
                if (data.SetValueIfEdited())
                    return true;
        }

        if (m_vec2Data != null)
        {
            foreach (SofaVec2Data data in m_vec2Data)
                if (data.SetValueIfEdited())
                    return true;
        }


        if (m_vec3iData != null)
        {
            foreach (SofaVec3IntData data in m_vec3iData)
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



    /// Method to create a String Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddStringData(SofaBaseComponent owner, string dataName, string value)
    {
        if (m_stringData == null) // first time
            m_stringData = new List<SofaStringData>();

        m_stringData.Add(new SofaStringData(owner, dataName, value));
    }


    /// Method to create a Bool Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddBoolData(SofaBaseComponent owner, string dataName, bool value)
    {
        if (m_boolData == null) // first time
            m_boolData = new List<SofaBoolData>();

        m_boolData.Add(new SofaBoolData(owner, dataName, value));
    }


    /// Method to create a Int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddIntData(SofaBaseComponent owner, string dataName, int value, bool isUnsigned = false)
    {
        if (m_intData == null) // first time
            m_intData = new List<SofaIntData>();

        m_intData.Add(new SofaIntData(owner, dataName, value, isUnsigned));
    }


    /// Method to create a Float Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddFloatData(SofaBaseComponent owner, string nameID, float value)
    {
        if (m_floatData == null) // first time
            m_floatData = new List<SofaFloatData>();

        m_floatData.Add(new SofaFloatData(owner, nameID, value));
    }


    /// Method to create a Double Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddDoubleData(SofaBaseComponent owner, string nameID, float value)
    {
        if (m_doubleData == null) // first time
            m_doubleData = new List<SofaDoubleData>();

        m_doubleData.Add(new SofaDoubleData(owner, nameID, value));
    }


    /// Method to create a Vec2 int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddVec2IntData(SofaBaseComponent owner, string nameID, Vector2Int value, bool isUnsigned = false)
    {
        if (m_vec2iData == null) // first time
            m_vec2iData = new List<SofaVec2IntData>();

        m_vec2iData.Add(new SofaVec2IntData(owner, nameID, value, isUnsigned));
    }

    /// Method to create a Vec2 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddVec2Data(SofaBaseComponent owner, string nameID, Vector2 value, bool isDouble = false)
    {
        if (m_vec2Data == null) // first time
            m_vec2Data = new List<SofaVec2Data>();

        m_vec2Data.Add(new SofaVec2Data(owner, nameID, value, isDouble));
    }


    /// Method to create a Vec3 int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddVec3IntData(SofaBaseComponent owner, string nameID, Vector3Int value, bool isUnsigned = false)
    {
        if (m_vec3iData == null) // first time
            m_vec3iData = new List<SofaVec3IntData>();

        m_vec3iData.Add(new SofaVec3IntData(owner, nameID, value, isUnsigned));
    }

    /// Method to create a Vec3 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddVec3Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble = false)
    {
        if (m_vec3Data == null) // first time
            m_vec3Data = new List<SofaVec3Data>();

        m_vec3Data.Add(new SofaVec3Data(owner, nameID, value, isDouble));
    }


    /// Method to create a Vec4 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddVec4Data(SofaBaseComponent owner, string nameID, Vector4 value, bool isDouble = false)
    {
        if (m_vec4Data == null) // first time
            m_vec4Data = new List<SofaVec4Data>();

        m_vec4Data.Add(new SofaVec4Data(owner, nameID, value, isDouble));
    }


    /// Method to create a SofaData for unsupported type and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    public void AddUnssuportedData(SofaBaseComponent owner, string nameID, string type)
    {
        if (m_otherData == null) // first time
            m_otherData = new List<SofaData>();

        m_otherData.Add(new SofaData(owner, nameID, type));
    }




    /// Getter of generic SofaData given the Data name
    public SofaData GetGenericData(string dataName)
    {
        if (m_otherData == null)
            return null;
        
        foreach (SofaData data in m_otherData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData string value given the Data name
    public SofaStringData GetSofaStringData(string dataName)
    {
        foreach(SofaStringData data in m_stringData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData bool value given the Data name
    public SofaBoolData GetSofaBoolData(string dataName)
    {
        foreach (SofaBoolData data in m_boolData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData int value given the Data name
    public SofaIntData GetSofaIntData(string dataName)
    {
        foreach (SofaIntData data in m_intData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData float value given the Data name
    public SofaFloatData GetSofaFloatData(string dataName)
    {
        foreach (SofaFloatData data in m_floatData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData double value given the Data name
    public SofaDoubleData GetSofaDoubleData(string dataName)
    {
        foreach (SofaDoubleData data in m_doubleData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData Vec2<int> value given the Data name
    public SofaVec2IntData GetSofaVec2IntData(string dataName)
    {
        foreach (SofaVec2IntData data in m_vec2iData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    /// Getter for SofaData Vec2<float> value given the Data name
    public SofaVec2Data GetSofaVec2Data(string dataName)
    {
        foreach (SofaVec2Data data in m_vec2Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData Vec3<int> value given the Data name
    public SofaVec3IntData GetSofaVec3IntData(string dataName)
    {
        foreach (SofaVec3IntData data in m_vec3iData)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }

    /// Getter for SofaData Vec3<float> value given the Data name
    public SofaVec3Data GetSofaVec3Data(string dataName)
    {
        foreach (SofaVec3Data data in m_vec3Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter for SofaData Vec4<float> value given the Data name
    public SofaVec4Data GetSofaVec4Data(string dataName)
    {
        foreach (SofaVec4Data data in m_vec4Data)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }
}
