using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaRayCaster : IDisposable
{
    public SofaRayCaster(IntPtr simu, float length)
    {
        m_simu = simu;

        if (length < 1.0f)
            length = 1.0f;

        if (m_simu != IntPtr.Zero)
            sofaPhysicsAPI_createRayCaster(m_simu, length);
    }

    // TODO: check if needed
    bool m_isDisposed;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    /// Type of Sofa object mapped to this Object.
    protected string m_type;

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

    public int activateCuttingTool(bool value)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_activateCutting(m_simu, value);

        return res;
    }

    public int castRay(Vector3 origin, Vector3 direction)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        float[] ori = new float[3];
        float[] dir = new float[3];

        for (int i = 0; i < 3; ++i)
        {
            ori[i] = origin[i];
            dir[i] = direction[i];
        }

        int res = sofaPhysicsAPI_castRay(m_simu, ori, dir);

        return res;
    }


    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_createRayCaster(IntPtr obj, float length);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_castRay(IntPtr obj, float[] origin, float[] direction);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_activateCutting(IntPtr obj, bool value);
}
