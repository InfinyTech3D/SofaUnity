using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBox : IDisposable
{
    internal IntPtr m_native;
    internal IntPtr m_simu;
    bool m_isDisposed;

    public SofaBox(IntPtr simu)
    {
        m_simu = simu;
        // add new box
        // test get apiname
       // sofaPhysicsAPI_addCube(m_simu, "toto");

    }

    ~SofaBox()
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

    public void test()
    {
        Debug.Log("sofa_test1: " + sofaPhysicsAPI_getNumberObjects(m_simu));
        Debug.Log("NAME: " + sofaPhysicsAPI_APIName(m_simu));
    }

    public void addCube()
    {
        Debug.Log("sofa_test3: " + sofaPhysicsAPI_getNumberObjects(m_simu));
        if (m_native == IntPtr.Zero) // first time create object only
            sofaPhysicsAPI_addCube(m_native, "truc1");
        Debug.Log("sofa_test4: " + sofaPhysicsAPI_getNumberObjects(m_simu));
    }

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern void sofaPhysicsAPI_addCube(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

}
