using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaEntact : IDisposable
{
    /// Name of the Sofa object mapped to this Object.
    protected string m_name;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    /// Parameter to activate internal logging
    protected bool log = true;

    // TODO: check if needed
    bool m_isDisposed;

    /// Memory free method
    public void Dispose()
    {
        //Dispose(true);
        //GC.SuppressFinalize(this);
    }

    /// Memory free method
    protected virtual void Dispose(bool disposing)
    {
        //if (!m_isDisposed)
        //{
        //    m_isDisposed = true;
        //}
    }

    public SofaEntact(IntPtr simu, string nameID)
    {
        m_simu = simu;
        m_name = nameID;

        int res = 0;
        if (m_simu != IntPtr.Zero)
        {
            res = sofaPhysicsAPI_createEntactManager(m_simu, m_name);
            if (res < 0)
                Debug.LogError("SofaPliers creation: " + m_name + " returns error: " + res);
        }
        else
            Debug.LogError("SofaEntact creation: " + nameID + " failed. Can't access Object Pointer simulation.");
        Debug.Log("Creation returns: " + res);
    }

    public int SofaRightHoming()
    {
        int res = sofaPhysicsAPI_entactRightToolHoming(m_simu, m_name);
        if (log)
            Debug.Log("SofaRightHoming: " + m_name + " -> return: " + res);

        return res;
    }

    public int SofaLeftHoming()
    {
        int res = sofaPhysicsAPI_entactLeftToolHoming(m_simu, m_name);
        if (log)
            Debug.Log("SofaLeftHoming: " + m_name + " -> return: " + res);

        return res;
    }

    public int numberOfTools()
    {
        int res = sofaPhysicsAPI_entactLeftToolHoming(m_simu, m_name);
        if (log)
            Debug.Log("Sofa Number of tools detected: " + m_name + " -> return: " + res);

        return res;
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createEntactManager(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_entactRightToolHoming(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_entactLeftToolHoming(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getNumberOfTools(IntPtr obj, string nameID);
}
