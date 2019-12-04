using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SBaseAPI : IDisposable
{
    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    protected bool m_isReady = false;

    // TODO: check if needed
    bool m_isDisposed;

    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    public SBaseAPI(IntPtr simu, string nameID)
    {
        m_isReady = false;
        m_simu = simu;
        m_name = nameID;

        if (m_simu == IntPtr.Zero)
        {
            Debug.LogError("SBaseAPI created with null SofaContextAPI pointer.");
            m_isReady = false;
            return;
        }
        
        m_isReady = Init();
    }

    /// method to be overriden by cildren
    virtual protected bool Init()
    {
        Debug.Log("SBaseAPI::Init()");

        return true;
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
