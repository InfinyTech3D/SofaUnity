using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseObject : IDisposable
{
    protected IntPtr m_native = IntPtr.Zero;
    protected IntPtr m_simu = IntPtr.Zero;

    protected string m_name;
    protected string m_type;
    bool m_isDisposed;
    protected bool m_isRigid = false;
    protected bool log = false;


    protected string m_parent;
    public string parent
    {
        get { return m_parent; }
    }


    public SofaBaseObject(IntPtr simu, string nameID, bool isRigid)
    {
        m_simu = simu;
        m_name = nameID;
        m_isRigid = isRigid;   

        createObject();
    }

    ~SofaBaseObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!m_isDisposed)
        {
            m_isDisposed = true;

            //if (!_preventDelete)
            //{
            //    IntPtr userPtr = btCollisionShape_getUserPointer(_native);
            //    GCHandle.FromIntPtr(userPtr).Free();

            //    btCollisionShape_delete(_native);
            //}
        }
    }

    protected virtual void createObject()
    {
        //m_name = "baseObject" + m_idObject + "_node";        
    }

    public virtual void loadObject()
    {

    }


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


