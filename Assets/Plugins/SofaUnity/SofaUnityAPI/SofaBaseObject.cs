using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseObject : IDisposable
{
    protected IntPtr m_native = IntPtr.Zero;
    protected IntPtr m_simu = IntPtr.Zero;

    protected int m_idObject;
    protected string m_name;
    bool m_isDisposed;

    protected bool log = false;

    public SofaBaseObject(IntPtr simu, int idObject)
    {
        m_simu = simu;
        m_idObject = idObject;        

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
        m_name = "baseObject" + m_idObject + "_node";
    }


    // API to get objects and object Name
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr sofaPhysicsAPI_get3DObject(IntPtr obj, string name);
    //}


    // API to set values to Sofa API
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setFloatValue(IntPtr obj, string objectName, string dataName, float value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);
    //}
}


