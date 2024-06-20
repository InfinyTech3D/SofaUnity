using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseComponentAPI : SofaBaseAPI
{

    public SofaBaseComponentAPI(IntPtr simu, string nameID, bool isCustom)
        : base(simu, nameID, isCustom)
    { 

    }


    public string GetPossiblesTypes()
    {
        if (m_isReady)
        {
            string type = sofaComponentAPI_getPossibleTypes(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }

    public string GetComponentType()
    {
        if (m_isReady)
        {
            string type = sofaComponentAPI_getComponentType(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }

    public string GetComponentDisplayName()
    {
        if (m_isReady)
        {
            string type = sofaComponentAPI_getComponentDisplayName(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }
    

    /// Method to get all data listen by this component as a json unique string.
    public string LoadAllData()
    {
        if (m_isReady)
            return sofaComponentAPI_getDataFields(m_simu, m_name);
        else
            return "None";
    }

    public string LoadAllLinks()
    {
        if (m_isReady)
            return sofaComponentAPI_getLinks(m_simu, m_name);
        else
            return "None";
    }

    public int ReinitComponent()
    {
        if (m_isReady)
            return sofaComponentAPI_reinitComponent(m_simu, m_name);
        else
            return -1;
    }


    public int GetDataCounter(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -1;
            int res = sofaComponentAPI_getDataCounter(m_simu, m_name, dataName, val);

            if (res == 0)
                return val[0];
            else
                Debug.LogError("Method GetDataCounter of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return 0;
    }


    public int GetDataFlags(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            int res = sofaComponentAPI_getDataFlags(m_simu, m_name, dataName, val);

            if (res == 0)
                return val[0];
            else
                Debug.LogError("Method GetDataCounter of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return 0;
    }


    /// <summary> Generic method to get value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Bool value of the Data field, return false if field is not found. </returns>
    public bool GetBoolValue(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = 0;
            int res = sofaComponentAPI_getBoolValueAsInt(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                if (val[0] == 1)
                    return true;
                else
                    return false;
            }
            else
                Debug.LogError("Method getBoolValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]); 
        }

        return false;
    }

    /// <summary> Generic method to set value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New bool value of the Data. </param>
    public void SetBoolValue(string dataName, bool value)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setBoolValue(m_simu, m_name, dataName, value);
            if (res != 0)
                Debug.LogError("Method setBoolValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Int value of the Data field, return int.MinValue if field is not found. </returns>
    public int GetIntValue(string dataName)
    {
        if (checkNativePointer())
        {            
            int[] val = new int[1];
            int res = sofaComponentAPI_getIntValue(m_simu, m_name, dataName, val);

            if (res == 0)
                return val[0];
            else
                Debug.LogError("Method getIntValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]); 
        }

        return int.MinValue;
    }

    /// <summary> Generic method to set value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New int value of the Data. </param>
    public void SetIntValue(string dataName, int value)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setIntValue(m_simu, m_name, dataName, value);

            if (res != 0)
                Debug.LogError("Method setIntValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<unsigned int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> unsigned int value cast in int of the Data field, return int.MinValue if field is not found. </returns>
    public int GetUIntValue(string dataName)
    {
        if (checkNativePointer())
        {
            uint[] val = new uint[1];
            int res = sofaComponentAPI_getUIntValue(m_simu, m_name, dataName, val);

            if (res == 0)
                return (int)val[0]; // TODO see if conversion limit test is needed
            else
                Debug.LogError("Method getUIntValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return int.MinValue;
    }


    /// <summary> Generic method to set value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New unsigned int value of the Data. </param>
    public void SetUIntValue(string dataName, int value)
    {
        if (checkNativePointer())
        {
            uint val = (uint)value;
            int res = sofaComponentAPI_setUIntValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setUIntValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Float value of the Data field, return float.MinValue if field is not found. </returns>
    public float GetFloatValue(string dataName)
    {
        if (checkNativePointer())
        {
            float[] val = new float[1];
            int res = sofaComponentAPI_getFloatValue(m_simu, m_name, dataName, val);

            if (res == 0)
                return val[0];
            else
                Debug.LogError("Method getFloatValue of Data: " + dataName + " of object: " + m_name + ", returns error: " + SofaDefines.msg_error[res]);
        }

        return float.MinValue;
    }

    /// <summary> Generic method to set value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New float value of the Data. </param>
    public void SetFloatValue(string dataName, float value)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setFloatValue(m_simu, m_name, dataName, value);

            if (res != 0)
                Debug.LogError("Method setFloatValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Float value casted from Double of the Data field, return float.MinValue if field is not found. </returns>
    public float GetDoubleValue(string dataName)
    {
        if (checkNativePointer())
        {
            double[] val = new double[1];
            int res = sofaComponentAPI_getDoubleValue(m_simu, m_name, dataName, val);

            if (res == 0)
                return (float)val[0];
            else
                Debug.LogError("Method GetDoubleValue of Data: " + dataName + " of object: " + m_name + ", returns error: " + SofaDefines.msg_error[res]);
        }

        return float.MinValue;
    }

    /// <summary> Generic method to set value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New float value of the Data. </param>
    public void SetDoubleValue(string dataName, float value)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setDoubleValue(m_simu, m_name, dataName, (double)value);

            if (res != 0)
                Debug.LogError("Method SetDoubleValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> String value of the Data field, return "None" if field is not found. </returns>
    public string getStringValue(string dataName)
    {
        if (checkNativePointer())
        {
            string res = sofaComponentAPI_getStringValue(m_simu, m_name, dataName);
            return res;
        }

        return "None";
    }

    /// <summary> Generic method to set value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New string value of the Data. </param>
    public void SetStringValue(string dataName, string value)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setStringValue(m_simu, m_name, dataName, value);
            if (res != 0)
                Debug.LogError("Method setStringValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec2i> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector2 of int of the Data field, return Vector2 of int.MinValue if field is not found. </returns>
    public Vector2Int GetVector2iValue(string dataName)
    {
        Vector2Int values = new Vector2Int(int.MinValue, int.MinValue);
        if (checkNativePointer())
        {
            int[] val = new int[2];
            int res = sofaComponentAPI_getVec2iValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 2; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Method getVector2iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec2i> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector2 values of the Data. </param>
    public void SetVector2iValue(string dataName, Vector2Int values, bool isUnsigned = false)
    {
        if (checkNativePointer())
        {
            int[] val = new int[2];
            for (int i = 0; i < 2; ++i)
                val[i] = values[i];

            int res = sofaComponentAPI_setVec2iValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setVector2iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec2> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <returns> Vector2 of the Data field, return Vector2 of float.MinValue if field is not found. </returns>
    public Vector3 GetVector2Value(string dataName, bool doubleValue = false)
    {
        Vector2 values = new Vector2(float.MinValue, float.MinValue);
        if (checkNativePointer())
        {
            float[] val = new float[2];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_getVec2Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_getVec2fValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 2; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Method getVector2Value of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void SetVector2Value(string dataName, Vector2 values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            float[] val = new float[2];
            for (int i = 0; i < 2; ++i)
                val[i] = values[i];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_setVec2Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_setVec2fValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setVector2Value of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec3i> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector3 of int of the Data field, return Vector3 of int.MinValue if field is not found. </returns>
    public Vector3Int GetVector3iValue(string dataName)
    {
        Vector3Int values = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        if (checkNativePointer())
        {
            int[] val = new int[3];
            int res = sofaComponentAPI_getVec3iValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 3; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Method getVector3iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector3Int values of the Data. </param>
    public void SetVector3iValue(string dataName, Vector3Int values, bool isUnsigned = false)
    {
        if (checkNativePointer())
        {
            int[] val = new int[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = sofaComponentAPI_setVec3iValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setVector3iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <returns> Vector3 of the Data field, return Vector3 of float.MinValue if field is not found. </returns>
    public Vector3 GetVector3Value(string dataName, bool doubleValue = false)
    {
        Vector3 values = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        if (checkNativePointer())
        {
            float[] val = new float[3];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_getVec3Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_getVec3fValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 3; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Method getVector3Value of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void SetVector3Value(string dataName, Vector3 values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            float[] val = new float[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_setVec3Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_setVec3fValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setVector3fValue of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec4f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <returns> Vector4 of the Data field, return Vector4 of float.MinValue if field is not found. </returns>
    public Vector4 GetVector4Value(string dataName, bool doubleValue = false)
    {
        Vector4 values = new Vector4(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
        if (checkNativePointer())
        {
            float[] val = new float[4];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_getVec4Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_getVec4fValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 4; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Method getVector4Value of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec4f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="doubleValue"> Parameter to inform if data is in double and should be converted to float. </param>
    /// <param name="values"> New Vector4 values of the Data. </param>
    public void SetVector4Value(string dataName, Vector4 values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            float[] val = new float[4];
            for (int i = 0; i < 4; ++i)
                val[i] = values[i];

            int res = -1;
            if (doubleValue)
                res = sofaComponentAPI_setVec4Value(m_simu, m_name, dataName, true, val);
            else
                res = sofaComponentAPI_setVec4fValue(m_simu, m_name, dataName, val);

            if (res != 0)
                Debug.LogError("Method setVector4fValue of Data: " + dataName + " of object: " + m_name + " in double: " + doubleValue + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    ///  Generic method to get value of a Data<Rigid3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> Buffer to get the values of the Rigid3f. </param>
    /// <returns></returns>
    public int GetRigidfValue(string dataName, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getRigid3fValue(m_simu, m_name, dataName, values);
            if (res != 0)
                Debug.LogError("Method getRigidfValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }

    ///  Generic method to set value of a Data<Rigid3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> Buffer with the new values of the Rigid3f to be set. </param>
    /// <returns></returns>
    public int SetRigidfValue(string dataName, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setRigid3fValue(m_simu, m_name, dataName, values);
            if (res != 0)
                Debug.LogError("Method setRigid3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVectoriSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            string dataType = "unsigned int";
            int res = sofaComponentAPI_getVectorSize(m_simu, m_name, dataName, dataType, val);

            if (res != 0)
            {
                Debug.LogError("Method getVeciSize of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }

    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVectorfSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            string dataType = "float";
            int res = sofaComponentAPI_getVectorSize(m_simu, m_name, dataName, dataType, val);

            if (res != 0)
            {
                Debug.LogError("Method getVecfSize of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }

    /// <summary> Generic method to get size of a Data< vector<double> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVectordSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            string dataType = "double";
            int res = sofaComponentAPI_getVectorSize(m_simu, m_name, dataName, dataType, val);

            if (res != 0)
            {
                Debug.LogError("Method getVecdSize of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }



    /// <summary> Generic method to get values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int GetVectorfValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVectorfValue(m_simu, m_name, dataName, size, values);
            if (res != 0)
                Debug.LogError("Method getVectorfValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }

    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVectorfValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVectorfValue(m_simu, m_name, dataName, size, values);
            if (res != 0)
                Debug.LogError("Method setVectorfValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<double> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int GetVectordValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVectorValue(m_simu, m_name, dataName, "double", size, values);
            if (res != 0)
                Debug.LogError("Method getVectorValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }

    /// <summary> Generic method to set values of a Data< vector<double> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVectordValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVectorValue(m_simu, m_name, dataName, "double", size, values);
            if (res != 0)
                Debug.LogError("Method setVectorValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<int> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int GetVectoriValue(string dataName, int size, int[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVectoriValue(m_simu, m_name, dataName, size, values);
            if (res != 0)
                Debug.LogError("Method getVeciValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }

    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVectoriValue(string dataName, int size, int[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVectoriValue(m_simu, m_name, dataName, size, values);
            if (res != 0)
                Debug.LogError("Method setVeciValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVecofVec2Size(string dataName, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaComponentAPI_getVecofVec2Size(m_simu, m_name, dataName, doubleValue, val);

            if (res != 0)
            {
                Debug.LogError("Method getVecofVec2Size of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int GetVecofVec2Value(string dataName, int size, float[] values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVecofVec2Value(m_simu, m_name, dataName, size, doubleValue, values);
            if (res != 0)
                Debug.LogError("Method getVecofVec2Value of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVecofVec2Value(string dataName, int size, float[] values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVecofVec2Value(m_simu, m_name, dataName, size, doubleValue, values);
            if (res != 0)
                Debug.LogError("Method setVecofVec2Value of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVecofVec3Size(string dataName, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaComponentAPI_getVecofVec3Size(m_simu, m_name, dataName, doubleValue, val);

            if (res != 0)
            {
                Debug.LogError("Method getVecofVec3Size of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int GetVecofVec3Value(string dataName, int size, float[] values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVecofVec3Value(m_simu, m_name, dataName, size, doubleValue, values);
            if (res != 0)
                Debug.LogError("Method getVecofVec3Value of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVecofVec3Value(string dataName, int size, float[] values, bool doubleValue = false)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVecofVec3Value(m_simu, m_name, dataName, size, doubleValue, values);
            if (res != 0)
                Debug.LogError("Method setVecofVec3Value of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }





    /////////////////////////////////////////////////////////////////////////////////////////
    //////////            API to Communication with Sofa component            ///////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getPossibleTypes(IntPtr obj, string componentName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getComponentType(IntPtr obj, string componentName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getComponentDisplayName(IntPtr obj, string componentName);


    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_reinitComponent(IntPtr obj, string componentName);


    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getDataFields(IntPtr obj, string componentName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getLinks(IntPtr obj, string componentName);

    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// Data API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getDataCounter(IntPtr obj, string componentName, string dataName, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getDataFlags(IntPtr obj, string componentName, string dataName, int[] value);


    /// Bool API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getBoolValue(IntPtr obj, string componentName, string dataName, bool[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getBoolValueAsInt(IntPtr obj, string componentName, string dataName, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setBoolValue(IntPtr obj, string componentName, string dataName, bool value);


    /// Int API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getIntValue(IntPtr obj, string componentName, string dataName, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setIntValue(IntPtr obj, string componentName, string dataName, int value);


    /// UInt API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getUIntValue(IntPtr obj, string componentName, string dataName, uint[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setUIntValue(IntPtr obj, string componentName, string dataName, uint value);


    /// Float API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getFloatValue(IntPtr obj, string componentName, string dataName, float[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setFloatValue(IntPtr obj, string componentName, string dataName, float value);


    /// Double API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getDoubleValue(IntPtr obj, string componentName, string dataName, double[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setDoubleValue(IntPtr obj, string componentName, string dataName, double value);


    /// String API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getStringValue(IntPtr obj, string componentName, string dataName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setStringValue(IntPtr obj, string componentName, string dataName, string value);



    // Vec2i API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec2iValue(IntPtr obj, string componentName, string dataName, int[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec2iValue(IntPtr obj, string componentName, string dataName, int[] values);


    // Vec2f API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec2fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec2fValue(IntPtr obj, string componentName, string dataName, float[] values);


    // Vec2 API 
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec2Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec2Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);



    /// Vec3i API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3iValue(IntPtr obj, string componentName, string dataName, int[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3iValue(IntPtr obj, string componentName, string dataName, int[] values);


    /// Vec3f API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3fValue(IntPtr obj, string componentName, string dataName, float[] values);
    

    /// Vec3 API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);


    /// Vec4f API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec4fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec4fValue(IntPtr obj, string componentName, string dataName, float[] values);


    /// Vec4 API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec4Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec4Value(IntPtr obj, string componentName, string dataName, bool doubleValue, float[] values);



    /// Rigid3f API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getRigid3fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setRigid3fValue(IntPtr obj, string componentName, string dataName, float[] values);


    /// Rigid3 API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getRigid3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setRigid3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);



    /// Vector<float> and Vector<int> API, need to get the size before set/get
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectorSize(IntPtr obj, string componentName, string dataName, string dataType, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectorValue(IntPtr obj, string componentName, string dataName, string dataType, int size, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVectorValue(IntPtr obj, string componentName, string dataName, string dataType, int size, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectorfValue(IntPtr obj, string componentName, string dataName, int size, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVectorfValue(IntPtr obj, string componentName, string dataName, int size, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectoriValue(IntPtr obj, string componentName, string dataName, int size, int[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVectoriValue(IntPtr obj, string componentName, string dataName, int size, int[] values);


    /// Vector <vec2> API, need to get the size before set/get: size the number of vec2
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec2Size(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVecofVec2Value(IntPtr obj, string componentName, string dataName, int size, bool doubleValue, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec2Value(IntPtr obj, string componentName, string dataName, int size, bool doubleValue, float[] values);


    /// Vector <vec3> API, need to get the size before set/get: size the number of vec3
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec3Size(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVecofVec3Value(IntPtr obj, string componentName, string dataName, int size, bool doubleValue, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec3Value(IntPtr obj, string componentName, string dataName, int size, bool doubleValue, float[] values);
}
