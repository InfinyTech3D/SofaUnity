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

    /// List of Sofa supported Data
    [SerializeReference]
    public List<SofaBaseData> m_dataArray = null;


    /// Method to add a Sofa Data to be stored with all the info to create it will call the right specialised Add method
    public void AddData(SofaBaseComponent owner, string dataName, string dataType)
    {
        // filter unwanted data
        if (dataName.Contains("show") || dataName.Contains("draw")) // no gui Data
            return;

        if (dataName == "name" || dataName == "componentState")
            return;

        if (m_dataArray == null) // first time
            m_dataArray = new List<SofaBaseData>();


        if (dataType == "string")
        {
            AddStringData(owner, dataName);
        }
        else if (dataType == "bool")
        {            
            AddBoolData(owner, dataName);
        }
        else if (dataType == "int" || dataType == "i")
        {
            dataType = "i";
            AddIntData(owner, dataName, false);
        }
        else if (dataType == "unsigned int" || dataType == "I")
        {
            dataType = "uint";
            AddIntData(owner, dataName, true);           
        }
        else if (dataType == "float" || dataType == "f")
        {
            dataType = "f";
            AddFloatData(owner, dataName);
        }
        else if (dataType == "double" || dataType == "d")
        {
            dataType = "d";
            AddDoubleData(owner, dataName);
        }
        else if (dataType == "Vec2i")
        {
            AddVec2IntData(owner, dataName, false);
        }
        else if (dataType == "Vec2f")
        {            
            AddVec2Data(owner, dataName, false);
        }
        else if (dataType == "Vec2d")
        {            
            AddVec2Data(owner, dataName, true);
        }
        else if (dataType == "Vec3i")
        {
            AddVec3IntData(owner, dataName, false);
        }
        else if (dataType == "Vec3f")
        {
            AddVec3Data(owner, dataName, false);
        }
        else if (dataType == "Vec3d")
        {
            AddVec3Data(owner, dataName, true);
        }
        else if (dataType == "Vec4f")
        {
            AddVec4Data(owner, dataName, false);
        }
        else if (dataType == "Vec4d")
        {
            AddVec4Data(owner, dataName, true);
        }
        else if (dataType == "vector<I>" || dataType == "vector < int >" || dataType == "vector<int>")
        {
            m_dataArray.Add(new SofaDataVectorInt(owner, dataName, dataType));
        }
        else if (dataType == "vector<d>" || dataType == "vector < double >" || dataType == "vector<double>")
        {
            m_dataArray.Add(new SofaDataVectorDouble(owner, dataName, dataType));
        }
        else if (dataType == "vector<f>" || dataType == "vector < float >" || dataType == "vector<float>")
        {
            m_dataArray.Add(new SofaDataVectorFloat(owner, dataName, dataType));
        }
        else if (dataType == "vector<Vec2f>" || dataType == "vector < Vec2f >")
        {
            m_dataArray.Add(new SofaDataVectorVec2(owner, dataName, dataType, false));
        }
        else if (dataType == "vector<Vec2d>" || dataType == "vector < Vec2d >" || dataType == "vector<Vec3>")
        {
            m_dataArray.Add(new SofaDataVectorVec2(owner, dataName, dataType, true));
        }
        else if (dataType == "vector<Vec3f>" || dataType == "vector < Vec3f >")
        {
            m_dataArray.Add(new SofaDataVectorVec3(owner, dataName, dataType, false));
        }
        else if (dataType == "vector<Vec3d>" || dataType == "vector < Vec3d >" || dataType == "vector<Vec3>")
        {
            m_dataArray.Add(new SofaDataVectorVec3(owner, dataName, dataType, true));
        }
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
        

        //else if (dataType == "Rigid3dTypes::Coord")
        //{

        //}
        else
        {
            AddUnssuportedData(owner, dataName, dataType);
        }

        m_names.Add(dataName);
        m_types.Add(dataType);
    }


    /// Method to check if any Data stored int he Archiver has been modified by the Editor (TODO: optimize that part)
    public bool UpdateEditedData()
    {
        if (m_dataArray != null)
        {
            foreach (SofaBaseData data in m_dataArray)
                if (data.SetValueIfEdited())
                    return true;
        }

        return false;
    }


    public string GetDataType(string dataName)
    {
        if (m_dataArray != null)
        {
            foreach (SofaBaseData data in m_dataArray)
                if (data.DataName == dataName)
                    return data.DataType;
        }

        return "None";
    }

    /// Getter of generic SofaData given the Data name
    public SofaBaseData GetBaseData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return data;
        }
        return null;
    }


    /// Getter of generic SofaData given the Data name
    public SofaData GetGenericData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaData)(data);
        }
        return null;
    }


    /// Getter of generic SofaData given the Data name
    public SofaDataVector GetVectorData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaDataVector)(data);
        }
        return null;
    }


    /// Getter for SofaData string value given the Data name
    public SofaStringData GetSofaStringData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaStringData)(data);
        }
        return null;
    }


    /// Getter for SofaData bool value given the Data name
    public SofaBoolData GetSofaBoolData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaBoolData)(data);
        }
        return null;
    }


    /// Getter for SofaData int value given the Data name
    public SofaIntData GetSofaIntData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaIntData)(data);
        }
        return null;
    }


    /// Getter for SofaData float value given the Data name
    public SofaFloatData GetSofaFloatData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaFloatData)(data);
        }
        return null;
    }


    /// Getter for SofaData double value given the Data name
    public SofaDoubleData GetSofaDoubleData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaDoubleData)(data);
        }
        return null;
    }


    /// Getter for SofaData Vec2<int> value given the Data name
    public SofaVec2IntData GetSofaVec2IntData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaVec2IntData)(data);
        }
        return null;
    }

    /// Getter for SofaData Vec2<float> value given the Data name
    public SofaVec2Data GetSofaVec2Data(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaVec2Data)(data);
        }
        return null;
    }


    /// Getter for SofaData Vec3<int> value given the Data name
    public SofaVec3IntData GetSofaVec3IntData(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaVec3IntData)(data);
        }
        return null;
    }

    /// Getter for SofaData Vec3<float> value given the Data name
    public SofaVec3Data GetSofaVec3Data(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaVec3Data)(data);
        }
        return null;
    }


    /// Getter for SofaData Vec4<float> value given the Data name
    public SofaVec4Data GetSofaVec4Data(string dataName)
    {
        foreach (SofaBaseData data in m_dataArray)
        {
            if (data.DataName == dataName)
                return (SofaVec4Data)(data);
        }
        return null;
    }



    /// Internal Method to create a String Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddStringData(SofaBaseComponent owner, string dataName)
    {
        string value = owner.m_impl.getStringValue(dataName);
        m_dataArray.Add(new SofaStringData(owner, dataName, value));
    }


    /// Internal Method to create a Bool Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddBoolData(SofaBaseComponent owner, string dataName)
    {
        bool value = owner.m_impl.GetBoolValue(dataName);
        m_dataArray.Add(new SofaBoolData(owner, dataName, value));
    }


    /// Internal Method to create a Int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddIntData(SofaBaseComponent owner, string dataName, bool isUnsigned = false)
    {
        int value = 0;
        if (isUnsigned)
            value = owner.m_impl.GetUIntValue(dataName);
        else
            value = owner.m_impl.GetIntValue(dataName);
        m_dataArray.Add(new SofaIntData(owner, dataName, value, isUnsigned));
    }


    /// Internal Method to create a Float Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddFloatData(SofaBaseComponent owner, string dataName)
    {
        float value = owner.m_impl.GetFloatValue(dataName);
        m_dataArray.Add(new SofaFloatData(owner, dataName, value));
    }


    /// Internal Method to create a Double Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddDoubleData(SofaBaseComponent owner, string dataName)
    {
        float value = owner.m_impl.GetDoubleValue(dataName);
        m_dataArray.Add(new SofaDoubleData(owner, dataName, value));
    }


    /// Internal Method to create a Vec2 int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddVec2IntData(SofaBaseComponent owner, string dataName, bool isUnsigned = false)
    {
        Vector2Int value = owner.m_impl.GetVector2iValue(dataName);
        m_dataArray.Add(new SofaVec2IntData(owner, dataName, value, isUnsigned));
    }

    /// Internal Method to create a Vec2 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddVec2Data(SofaBaseComponent owner, string dataName, bool isDouble = false)
    {
        Vector2 value = owner.m_impl.GetVector2Value(dataName, isDouble);
        m_dataArray.Add(new SofaVec2Data(owner, dataName, value, isDouble));
    }


    /// Internal Method to create a Vec3 int Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddVec3IntData(SofaBaseComponent owner, string dataName, bool isUnsigned = false)
    {
        Vector3Int value = owner.m_impl.GetVector3iValue(dataName);
        m_dataArray.Add(new SofaVec3IntData(owner, dataName, value, isUnsigned));
    }

    /// Internal Method to create a Vec3 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddVec3Data(SofaBaseComponent owner, string dataName, bool isDouble = false)
    {
        Vector3 value = owner.m_impl.GetVector3Value(dataName, isDouble);
        m_dataArray.Add(new SofaVec3Data(owner, dataName, value, isDouble));
    }


    /// Internal Method to create a Vec4 Data and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddVec4Data(SofaBaseComponent owner, string dataName, bool isDouble = false)
    {
        Vector4 value = owner.m_impl.GetVector4Value(dataName, isDouble);
        m_dataArray.Add(new SofaVec4Data(owner, dataName, value, isDouble));
    }


    /// Internal Method to create a SofaData for unsupported type and add it to the List. Will create the container if it is the first Data. Called by @sa AdddData
    private void AddUnssuportedData(SofaBaseComponent owner, string nameID, string type)
    {
        m_dataArray.Add(new SofaData(owner, nameID, type));
    }


}
