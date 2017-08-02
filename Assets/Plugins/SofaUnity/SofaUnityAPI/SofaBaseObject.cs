using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Main class of the Sofa plugin. This is the base class representing Sofa Object, handling all bindings to Sofa 3D Object.
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaBaseObject : IDisposable
{
    /// <summary>
    /// Default constructor, will call impl method: @see createObject()
    /// </summary>
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
    /// Pointer to the Sofa object. TODO: check why this pointer messed up in use.
    protected IntPtr m_native = IntPtr.Zero;

    // TODO: check if needed
    bool m_isDisposed;
    /// Parameter to activate logging
    protected bool log = false;

    /// Name of the Sofa object mapped to this Object.
    protected string m_name;
    /// Type of Sofa object mapped to this Object.
    protected string m_type;
    /// Parameter to store the information if the object is rigid or deformable.
    protected bool m_isRigid = false;

    /// Sofa object parent name
    protected string m_parent;


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

    /// Getter of the sofa object parent name
    public string parent
    {
        get { return m_parent; }
    }


    /// <summary>
    /// Generic Method to get the Float value of the parameter param
    /// </summary>
    /// <param name="param">field parameter requested</param>
    /// <returns></returns>
    protected float getFloatValue(string param)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] val = new float[1];
            int res = sofaPhysics3DObject_getFloatValue(m_simu, m_name, param, val);
            if (res >= 0)
                return val[0];
            else
                Debug.LogError("Error getting parameter: " + param +  " . Method Return: " + res);
        }
        else
            Debug.LogError("Error getting parameter: " + param + " . Can't access Object Pointer m_native.");

        return 0.0f;
    }

    /// <summary>
    /// Generic Method to set the Float value to the parameter param
    /// </summary>
    /// <param name="param">field parameter to change</param>
    /// <param name="value">new value of the parameter</param>
    protected void setFloatValue(string param, float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, param, value);

            if (res < 0)
                Debug.LogError("Error setting parameter: " + param + " . Method Return: " + res);
        }
        else
            Debug.LogError("Error setting parameter: " + param + " . Can't access Object Pointer m_native.");
    }

    /// <summary>
    /// Generic Method to get a 3D vector of Float of the parameter param
    /// </summary>
    /// <param name="param">field parameter requested</param>
    /// <returns></returns>
    protected Vector3 getVector3fValue(string param)
    {
        Vector3 values = new Vector3(-1.0f, -1.0f, -1.0f);
        if (m_native != IntPtr.Zero)
        {
            float[] val = new float[3];
            int res = sofaPhysics3DObject_getVec3fValue(m_simu, m_name, param, val);

            if (res >= 0)
            {
                for (int i = 0; i < 3; ++i)
                    values[i] = val[i];
            }
            else
                Debug.LogError("Error getting parameter: " + param + " . Method Return: " + res);
        }
        else
            Debug.LogError("Error getting parameter: " + param + " . Can't access Object Pointer m_native.");

        return values;
    }

    /// <summary>
    /// Generic Method to set a 3D vector of Float to the parameter param
    /// </summary>
    /// <param name="param">field parameter to change</param>
    /// <param name="values">new value of the parameter</param>
    protected void setVector3fValue(string param, Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] val = new float[3];
            for (int i = 0; i < 3; ++i)
                val[i] = values[i];

            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, param, val);

            if (res < 0)
                Debug.LogError("Error setting parameter: " + param + " . Method Return: " + res);
        }
        else
            Debug.LogError("Error setting parameter: " + param + " . Can't access Object Pointer m_native.");
    }



    // API to get objects and object Name
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberMeshes(IntPtr obj);
    

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectType(IntPtr obj, int id);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofa3DObject_getObjectType(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr sofaPhysicsAPI_get3DObject(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getParentNodeName(IntPtr obj, string name);
    //}


    // API to set values to Sofa API
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setFloatValue(IntPtr obj, string objectName, string dataName, float value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVec3iValue(IntPtr obj, string objectName, string dataName, int[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getFloatValue(IntPtr obj, string objectName, string dataName, float[] value);
    
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVec3iValue(IntPtr obj, string objectName, string dataName, int[] values);    
    //}
}


