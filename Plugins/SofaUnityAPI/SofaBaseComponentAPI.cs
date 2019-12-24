using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseComponentAPI : SofaBaseAPI
{

    public SofaBaseComponentAPI(IntPtr simu, string nameID)
        : base(simu, nameID)
    { 

    }


    public string GetPossiblesTypes()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getPossibleTypes(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }

    public string GetComponentType()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getComponentType(m_simu, m_name);
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


    /// <summary> Generic method to get value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Bool value of the Data field, return false if field is not found. </returns>
    public bool GetBoolValue(string dataName)
    {
        if (checkNativePointer())
        {
            bool[] val = new bool[1];
            int res = sofaComponentAPI_getBoolValue(m_simu, m_name, dataName, val);

            if (res == 0)
                return val[0];
            else if (displayLog)
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
            if (res != 0 && displayLog)
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
            else if (displayLog)
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

            if (res != 0 && displayLog)
                Debug.LogError("Method setIntValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
            else if (displayLog)
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

            if (res != 0 && displayLog)
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
            else if (displayLog)
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
            int res = sofaComponentAPI_setDoubleValue(m_simu, m_name, dataName, value);

            if (res != 0 && displayLog)
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
            if (res != 0 && displayLog)
                Debug.LogError("Method setStringValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }

  
    /// <summary> Generic method to get value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector3 of the Data field, return Vector3 of float.MinValue if field is not found. </returns>
    public Vector3 GetVector3fValue(string dataName)
    {
        Vector3 values = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        if (checkNativePointer())
        {
            float[] val = new float[3];
            int res = sofaComponentAPI_getVec3fValue(m_simu, m_name, dataName, val);

            if (res == 0)
            {
                for (int i = 0; i < 3; ++i)
                    values[i] = val[i];
            }
            else if (displayLog)
                Debug.LogError("Method getVector3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void SetVector3fValue(string dataName, Vector3 values)
    {
        if (checkNativePointer())
        {
            float[] val = new float[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = sofaComponentAPI_setVec3fValue(m_simu, m_name, dataName, val);

            if (res != 0 && displayLog)
                Debug.LogError("Method setVector3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector3 of the Data field, return Vector3 of float.MinValue if field is not found. </returns>
    public Vector3 GetVector3iValue(string dataName)
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
            else if (displayLog)
                Debug.LogError("Method getVector3iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
        }

        return values;
    }

    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void SetVector3iValue(string dataName, Vector3Int values)
    {
        if (checkNativePointer())
        {
            int[] val = new int[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = sofaComponentAPI_setVec3iValue(m_simu, m_name, dataName, val);

            if (res != 0 && displayLog)
                Debug.LogError("Method setVector3iValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
            if (res != 0 && displayLog)
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
            if (res != 0 && displayLog)
                Debug.LogError("Method setRigid3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVeciSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            string dataType = "int";
            int res = sofaComponentAPI_getVectorSize(m_simu, m_name, dataName, dataType, val);

            if (res != 0 && displayLog)
            {
                Debug.LogError("Method getVecfSize of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
    public int GetVecfSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            string dataType = "float";
            int res = sofaComponentAPI_getVectorSize(m_simu, m_name, dataName, dataType, val);

            if (res != 0 && displayLog)
            {
                Debug.LogError("Method getVecfSize of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
            if (res != 0 && displayLog)
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
            if (res != 0 && displayLog)
                Debug.LogError("Method setVectorfValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
            if (res != 0 && displayLog)
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
            if (res != 0 && displayLog)
                Debug.LogError("Method setVeciValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int GetVecofVec3fSize(string dataName)
    {
        if (checkNativePointer())
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaComponentAPI_getVecofVec3fSize(m_simu, m_name, dataName, val);

            if (res != 0 && displayLog)
            {
                Debug.LogError("Method setVeciValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
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
    public int GetVecofVec3fValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_getVecofVec3fValue(m_simu, m_name, dataName, size, values);
            if (res != 0 && displayLog)
                Debug.LogError("Method getVecofVec3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int SetVecofVec3fValue(string dataName, int size, float[] values)
    {
        if (checkNativePointer())
        {
            int res = sofaComponentAPI_setVecofVec3fValue(m_simu, m_name, dataName, size, values);
            if (res != 0 && displayLog)
                Debug.LogError("Method setVecofVec3fValue of Data: " + dataName + " of object: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            return res;
        }

        return -1;
    }





    /////////////////////////////////////////////////////////////////////////////////////////
    //////////            API to Communication with Sofa component            ///////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getPossibleTypes(IntPtr obj, string componentName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getComponentType(IntPtr obj, string componentName);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getDataFields(IntPtr obj, string componentName);


    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// Bool API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getBoolValue(IntPtr obj, string componentName, string dataName, bool[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setBoolValue(IntPtr obj, string componentName, string dataName, bool value);


    /// Int API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getIntValue(IntPtr obj, string componentName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setIntValue(IntPtr obj, string componentName, string dataName, int value);


    /// Float API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getFloatValue(IntPtr obj, string componentName, string dataName, float[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setFloatValue(IntPtr obj, string componentName, string dataName, float value);


    /// Double API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getDoubleValue(IntPtr obj, string componentName, string dataName, double[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setDoubleValue(IntPtr obj, string componentName, string dataName, double value);


    /// String API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getStringValue(IntPtr obj, string componentName, string dataName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setStringValue(IntPtr obj, string componentName, string dataName, string value);


    /// Vec3f API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3fValue(IntPtr obj, string componentName, string dataName, float[] values);


    /// Vec3i API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3iValue(IntPtr obj, string componentName, string dataName, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3iValue(IntPtr obj, string componentName, string dataName, int[] values);


    /// Vec3 API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVec3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVec3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);


    /// Rigid3f API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getRigid3fValue(IntPtr obj, string componentName, string dataName, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setRigid3fValue(IntPtr obj, string componentName, string dataName, float[] values);


    /// Rigid3 API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getRigid3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setRigid3Value(IntPtr obj, string componentName, string dataName, bool doubleValue, int[] values);



    /// Vector<float> and Vector<int> API, need to get the size before set/get
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectorSize(IntPtr obj, string componentName, string dataName, string dataType, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectorfValue(IntPtr obj, string componentName, string dataName, int size, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVectorfValue(IntPtr obj, string componentName, string dataName, int size, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVectoriValue(IntPtr obj, string componentName, string dataName, int size, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVectoriValue(IntPtr obj, string componentName, string dataName, int size, int[] values);



    /// Vector <vec3f> API, need to get the size before set/get: size the number of vec3f
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec3fSize(IntPtr obj, string componentName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setVecofVec3fValue(IntPtr obj, string componentName, string dataName, int size, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_getVecofVec3fValue(IntPtr obj, string componentName, string dataName, int size, float[] values);
}
