using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseComponentAPI : IDisposable
{
    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;

    private bool m_isReady = false;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    public SofaBaseComponentAPI(IntPtr simu, string nameID)
    {
        m_simu = simu;
        m_name = nameID;

        if (m_simu == IntPtr.Zero)
        {
            Debug.LogError("SofaBaseComponentAPI created with null SofaContextAPI pointer.");
            m_isReady = false;
            return;
        }

        m_isReady = true;
    }

    /// Memory free method
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // TODO: check if needed
    bool m_isDisposed;

    /// Memory free method
    protected virtual void Dispose(bool disposing)
    {
        if (!m_isDisposed)
        {
            m_isDisposed = true;
        }
    }

    public string GetPossiblesTypes()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getPossibleTypes(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }

    public string GetComponentType()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getComponentType(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getPossibleTypes(IntPtr obj, string componentName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getComponentType(IntPtr obj, string componentName);


}
