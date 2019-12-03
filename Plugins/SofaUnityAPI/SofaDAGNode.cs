using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaDAGNode : IDisposable
{
    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;
    /// Type of Sofa 3D Object mapped to this Object.
    protected string m_type;

    // TODO: check if needed
    bool m_isDisposed;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    public SofaDAGNode(IntPtr simu, string nameID)
    {
        m_simu = simu;
        m_name = nameID;
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

}
