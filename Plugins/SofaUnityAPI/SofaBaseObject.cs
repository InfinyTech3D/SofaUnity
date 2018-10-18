using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Main class of the Sofa plugin. This is the base class representing Sofa 3D Object, handling all bindings to Sofa 3D Object.
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaBaseObject : IDisposable
{
    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaBaseObject(IntPtr simu, string nameID, bool isRigid)
    {
        m_simu = simu;
        m_name = nameID;
        m_isRigid = isRigid;

        createObject();
    }

    /// Destructor
    ~SofaBaseObject()
    {
        Dispose(false);
    }


    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;
    /// Pointer to the Sofa 3D object. TODO: check why this pointer messed up in use.
    protected IntPtr m_native = IntPtr.Zero;

    // TODO: check if needed
    bool m_isDisposed;
    /// Parameter to activate internal logging
    protected bool log = false;

    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;
    /// Type of Sofa 3D Object mapped to this Object.
    protected string m_type;
    /// Sofa 3D Object parent name in Sofa simulation Tree.
    protected string m_parent;

    /// Parameter to store the information if the object is rigid or deformable.
    protected bool m_isRigid = false;


    /// Memory free method
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// Memory free method
    protected virtual void Dispose(bool disposing)
    {
        if (!m_isDisposed)
        {
            m_isDisposed = true;
        }
    }


    /// Implicit method to really create object and link to Sofa object. To be overwritten by child.
    protected virtual void createObject()
    {

    }


    /// Implicit method load the object from the Sofa side. TODO: check if still needed
    public virtual void loadObject()
    {

    }


    /// Getter of the sofa 3D Object parent name.
    public string parent
    {
        get { return m_parent; }
    }


    /// <summary> Generic method to get value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Float value of the Data field, return float.MinValue if field is not found. </returns>
    public float getFloatValue(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        { 
            float[] val = new float[1];
            int res = sofaPhysics3DObject_getFloatValue(m_simu, m_name, dataName, val);

            if (res >= 0)
                return val[0];
        }

        return float.MinValue;
    }


    /// <summary> Generic method to set value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New float value of the Data. </param>
    public void setFloatValue(string dataName, float value)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, dataName, value);

            if (res < 0)
                Debug.LogError("Error setting parameter: " + dataName + " of object: " + m_name + " . Method Return: " + res);
        }
    }


    /// <summary> Generic method to get value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Int value of the Data field, return int.MinValue if field is not found. </returns>
    public int getIntValue(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int[] val = new int[1];
            int res = sofaPhysics3DObject_getIntValue(m_simu, m_name, dataName, val);

            if (res >= 0)
                return val[0];
        }

        return int.MinValue;
    }


    /// <summary> Generic method to set value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New int value of the Data. </param>
    public void setIntValue(string dataName, int value)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setIntValue(m_simu, m_name, dataName, value);

            if (res < 0)
                Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
        }
    }


    /// <summary> Generic method to get value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Bool value of the Data field, return false if field is not found. </returns>
    public bool getBoolValue(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            bool[] val = new bool[1];
            int res = sofaPhysics3DObject_getBoolValue(m_simu, m_name, dataName, val);

            if (res >= 0)
                return val[0];
        }

        return false;
    }


    /// <summary> Generic method to set value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New bool value of the Data. </param>
    public void setBoolValue(string dataName, bool value)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setBoolValue(m_simu, m_name, dataName, value);
            if (res < 0)
                Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
        }
    }


    /// <summary> Generic method to get value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> String value of the Data field, return "None" if field is not found. </returns>
    public string getStringValue(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            string res = sofaPhysics3DObject_getStringValue(m_simu, m_name, dataName);
            return res;
        }

        return "None";
    }


    /// <summary> Generic method to set value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New string value of the Data. </param>
    public void setStringValue(string dataName, string value)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setStringValue(m_simu, m_name, dataName, value);
            if (res < 0)
                Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
        }
    }


    /// <summary> Generic method to get value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector3 of the Data field, return Vector3 of float.MinValue if field is not found. </returns>
    public Vector3 getVector3fValue(string dataName)
    {
        Vector3 values = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        if (checkNativePointerForGetData(dataName))
        {
            float[] val = new float[3];
            int res = sofaPhysics3DObject_getVec3fValue(m_simu, m_name, dataName, val);

            if (res >= 0)
            {
                for (int i = 0; i < 3; ++i)
                    values[i] = val[i];
            }
        }

        return values;
    }



    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void setVector3fValue(string dataName, Vector3 values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            float[] val = new float[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, dataName, val);

            if (res < 0)
                Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
        }
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVecfSize(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaPhysics3DObject_getVecfSize(m_simu, m_name, dataName, val);

            if (res < 0)
            {
                Debug.LogError("Error getting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
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
    public int getVectorfValue(string dataName, int size, float[] values)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int res = sofaPhysics3DObject_getVectorfValue(m_simu, m_name, dataName, size, values);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVectorfValue(string dataName, int size, float[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setVectorfValue(m_simu, m_name, dataName, size, values);
            return res;
        }

        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<int> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVeciSize(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaPhysics3DObject_getVeciSize(m_simu, m_name, dataName, val);

            if (res < 0)
            {
                Debug.LogError("Error getting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
                return res;
            }
            else
                return val[0];
        }

        return -1;
    }

    /// <summary> Generic method to get values of a Data< vector<int> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int getVeciValue(string param, int size, int[] values)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_getVeciValue(m_simu, m_name, param, size, values);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVeciValue(string dataName, int size, int[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setVeciValue(m_simu, m_name, dataName, size, values);
            return res;
        }

        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVecofVec3fSize(string dataName)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int[] val = new int[1];
            val[0] = -2;
            int res = sofaPhysics3DObject_getVecofVec3fSize(m_simu, m_name, dataName, val);

            if (res < 0)
            {
                Debug.LogError("Error getting parameter: " + dataName + " in object: " + m_name + " . Method Return: " + res);
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
    public int getVecofVec3fValue(string dataName, int size, float[] values)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int res = sofaPhysics3DObject_getVecofVec3fValue(m_simu, m_name, dataName, size, values);
            return res;
        }

        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVecofVec3fValue(string dataName, int size, float[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setVecofVec3fValue(m_simu, m_name, dataName, size, values);
            return res;
        }

        return -1;
    }






    public int getVecfValue(string dataName, string dataType, float[] values)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int res = sofaPhysics3DObject_getVecfValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("getVecfValue: " + res);
            return res;
        }

        return -1;
    }

    public int setVecfValue(string dataName, string dataType, float[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setVecfValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("setVecfValue: " + res);
            return res;
        }

        return -1;
    }

    public int getRigidfValue(string dataName, string dataType, float[] values)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int res = sofaPhysics3DObject_getRigidfValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("getRigidfValue: " + res);
            return res;
        }

        return -1;
    }

    public int setRigidfValue(string dataName, string dataType, float[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setRigidfValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("setRigidfValue: " + res);
            return res;
        }

        return -1;
    }

    public int getRigiddValue(string dataName, string dataType, double[] values)
    {
        if (checkNativePointerForGetData(dataName))
        {
            int res = sofaPhysics3DObject_getRigiddValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("getRigiddValue: " + res);
            Debug.Log("getRigiddValue: " + values[0] + values[1] + values[2] + values[3] + values[4] + values[5]);
            return res;
        }

        return -1;
    }

    public int setRigiddValue(string dataName, string dataType, double[] values)
    {
        if (checkNativePointerForSetData(dataName))
        {
            int res = sofaPhysics3DObject_setRigiddValue(m_simu, m_name, dataName, dataType, values);
            Debug.Log("setRigiddValue: " + res);
            return res;
        }

        return -1;
    }



    /// <summary> Method to check pointer of the Sofa object. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> True if pointer is valid otherwise return false. </returns>
    protected bool checkNativePointerForGetData(string dataName)
    {
        if (m_native == IntPtr.Zero)
        {
            Debug.LogError("Error getting parameter: " + dataName + " of object: " + m_name + " . Can't access Object Pointer m_native.");
            return false;
        }
        else
            return true;
    }


    /// <summary> Method to check pointer of the Sofa object. </summary>
    /// <param name="dataName"> Name of the Data field to set. </param>
    /// <returns> True if pointer is valid otherwise return false. </returns>
    protected bool checkNativePointerForSetData(string dataName)
    {
        if (m_native == IntPtr.Zero)
        {
            Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Can't access Object Pointer m_native.");
            return false;
        }
        else
            return true;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////            API to Communication with Sofa objects            ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// Get Object Name 
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

    /// Get Object Type 
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectType(IntPtr obj, int id);

    /// Get Object from Name
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr sofaPhysicsAPI_get3DObject(IntPtr obj, string name);

    /// Get Object Parent Name
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getParentNodeName(IntPtr obj, string name);



    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// String API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysics3DObject_getStringValue(IntPtr obj, string objectName, string dataName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setStringValue(IntPtr obj, string objectName, string dataName, string value);


    /// Float API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getFloatValue(IntPtr obj, string objectName, string dataName, float[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setFloatValue(IntPtr obj, string objectName, string dataName, float value);


    /// Int API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getIntValue(IntPtr obj, string objectName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setIntValue(IntPtr obj, string objectName, string dataName, int value);


    /// Bool API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getBoolValue(IntPtr obj, string objectName, string dataName, bool[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setBoolValue(IntPtr obj, string objectName, string dataName, bool value);


    /// Vec3f API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);


    /// Vec3i API
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVec3iValue(IntPtr obj, string objectName, string dataName, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVec3iValue(IntPtr obj, string objectName, string dataName, int[] values);


    /// Vector<float> API, need to get the size before set/get
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVecfSize(IntPtr obj, string objectName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVectorfValue(IntPtr obj, string objectName, string dataName, int size, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVectorfValue(IntPtr obj, string objectName, string dataName, int size, float[] values);


    /// Vector<int> API, need to get the size before set/get
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVeciSize(IntPtr obj, string objectName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVeciValue(IntPtr obj, string objectName, string dataName, int size, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVeciValue(IntPtr obj, string objectName, string dataName, int size, int[] values);


    /// Vector <vec3f> API, need to get the size before set/get: size the number of vec3f
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVecofVec3fSize(IntPtr obj, string objectName, string dataName, int[] value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVecofVec3fValue(IntPtr obj, string objectName, string dataName, int size, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVecofVec3fValue(IntPtr obj, string objectName, string dataName, int size, float[] values);

    ///
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVeciValue(IntPtr obj, string objectName, string dataName, string dataType, int[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVecfValue(IntPtr obj, string objectName, string dataName, string dataType, float[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVecdValue(IntPtr obj, string objectName, string dataName, string dataType, double[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVeciValue(IntPtr obj, string objectName, string dataName, string dataType, int[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVecfValue(IntPtr obj, string objectName, string dataName, string dataType, float[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVecdValue(IntPtr obj, string objectName, string dataName, string dataType, double[] values);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setRigidfValue(IntPtr obj, string objectName, string dataName, string dataType, float[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setRigiddValue(IntPtr obj, string objectName, string dataName, string dataType, double[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getRigidfValue(IntPtr obj, string objectName, string dataName, string dataType, float[] values);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getRigiddValue(IntPtr obj, string objectName, string dataName, string dataType, double[] values);

}
