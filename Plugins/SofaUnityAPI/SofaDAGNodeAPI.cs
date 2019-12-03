using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaDAGNodeAPI : IDisposable
{
    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;
    /// Type of Sofa 3D Object mapped to this Object.
    //protected string m_type;

    private bool m_isReady = false;

    // TODO: check if needed
    bool m_isDisposed;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    protected string m_componentListS = "";

    public SofaDAGNodeAPI(IntPtr simu, string nameID)
    {
        m_simu = simu;
        m_name = nameID;

        if (m_simu == IntPtr.Zero)
        {
            Debug.LogError("SofaDAGNodeAPI created with null SofaContextAPI pointer.");
            m_isReady = false;
            return;
        }

        m_componentListS = sofaPhysicsAPI_getComponentsAsString(m_simu, m_name);
    }

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

    public string GetDAGNodeComponents()
    {
        return m_componentListS;
    }


    public string RecomputeDAGNodeComponents()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getComponentsAsString(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getComponentsAsString(IntPtr obj, string nodeName);
}
