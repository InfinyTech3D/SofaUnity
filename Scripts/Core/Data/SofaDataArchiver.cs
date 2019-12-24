using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

[System.Serializable]
public class SofaDataArchiver //: MonoBehaviour, ISerializationCallbackReceiver
{
    public List<float> myList = new List<float>();
    public List<int> _keys = new List<int> { 3, 4, 5 };

    public List<SofaDataFloat> m_floatData = new List<SofaDataFloat>();
    public List<SofaDataVec3Float> m_vec3fData = null;
    public List<string> m_names = new List<string>();

    //SofaDataFloat

    public void addFloatData(ComponentDataTest owner, string nameID, float value)
    {
        m_floatData.Add(new SofaDataFloat(owner, nameID, value));
        m_names.Add(nameID);
    }

    public void addVec3FloatData(ComponentDataTest owner, string nameID, float value0, float value1, float value2)
    {
        if (m_vec3fData == null) // first time
            m_vec3fData = new List<SofaDataVec3Float>();

        m_vec3fData.Add(new SofaDataVec3Float(owner, nameID, value0, value1, value2));
        m_names.Add(nameID);
    }

    public void addFloatValue(float value)
    {
        myList.Add(value);
    }

    public void Log()
    {
        Debug.Log("SofaDataArchiver myList: " + myList.Count);
        Debug.Log("SofaDataArchiver m_floatData: " + m_floatData.Count);

        if (m_vec3fData != null)
            Debug.Log("SofaDataArchiver m_vec3fData: " + m_vec3fData.Count);

        foreach (SofaDataFloat data in m_floatData)
        {
            data.Log();
        }
    }
}
