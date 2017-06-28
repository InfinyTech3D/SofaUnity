using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseObject : IDisposable
{
    protected IntPtr m_native = IntPtr.Zero;
    protected IntPtr m_simu = IntPtr.Zero;

   // protected int m_idObject;
    protected string m_name;
    //protected string m_type;
    bool m_isDisposed;
    protected bool m_isRigid = false;

    protected bool log = false;

    public SofaBaseObject(IntPtr simu, string name, bool isRigid)
    {
        //m_simu = simu;
        m_name = name;
        m_isRigid = isRigid;   

        //createObject();
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
    //    m_name = "baseObject" + m_idObject + "_node";
    }

    public virtual void loadObject()
    {

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
    public static extern int sofaPhysics3DObject_getVec3fValue(IntPtr obj, string objectName, string dataName, float[] values);

    //[DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    //public static extern int sofaPhysics3DObject_getVec3iValue(IntPtr obj, string objectName, string dataName, float[] values);


    //}
}


