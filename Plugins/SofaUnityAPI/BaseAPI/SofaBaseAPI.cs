using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseAPI : IDisposable
{
    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    protected bool m_isReady = false;

    protected bool m_isCustom = false;

    // TODO: check if needed
    bool m_isDisposed;

    /// Parameter to activate internal logging
    protected bool displayLog = false;

    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    public SofaBaseAPI(IntPtr simu, string nameID, bool isCustom)
    {
        m_isReady = false;
        m_simu = simu;
        m_name = nameID;
        m_isCustom = isCustom;

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
        //Debug.Log("SBaseAPI::Init() " + m_name);

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


    protected bool checkNativePointer()
    {
        if (m_simu == IntPtr.Zero) // use native pointer?? m_native
        {
            Debug.LogError("Can't access Sofa native API for: " + m_name);
            return false;
        }
        else
            return true;
    }




}
