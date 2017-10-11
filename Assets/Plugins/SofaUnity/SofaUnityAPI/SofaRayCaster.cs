using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaRayCaster : IDisposable
{
    public SofaRayCaster(IntPtr simu, int type, string nameID, float length)
    {
        m_simu = simu;
        m_name = nameID;

        if (length < 1.0f)
            length = 1.0f;

        int res = 0;
        if (m_simu != IntPtr.Zero)
        {
            if (type == 0)
                res = sofaPhysicsAPI_createRayCaster(m_simu, m_name, length);
            else
                res = sofaPhysicsAPI_createAttachTool(m_simu, m_name, length);
        }
        Debug.Log("creation RAy: " + res);
    }

    // TODO: check if needed
    bool m_isDisposed;

    /// Name of the Sofa object mapped to this Object.
    protected string m_name;

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

        int res = sofaPhysicsAPI_activateCutting(m_simu, m_name, value);

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

        int res = sofaPhysicsAPI_castRay(m_simu, m_name, ori, dir);

        return res;
    }


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createRayCaster(IntPtr obj, string name, float length);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_castRay(IntPtr obj, string name, float[] origin, float[] direction);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_activateCutting(IntPtr obj, string name, bool value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createAttachTool(IntPtr obj, string name, float length);
}
