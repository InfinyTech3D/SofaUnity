using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class to create a ray caster in Sofa and comunicate with it.
/// </summary>
public class SofaPliers : IDisposable
{
    /// <summary>
    /// default constuctor of the sofa ray caster
    /// </summary>
    /// <param name="simu">Pointer to the implicit sofaAPI</param>
    /// <param name="nameID">unique name id of this ray caster</param>
    public SofaPliers(IntPtr simu, string nameID, string nameMord1, string nameMord2, string nameModel)
    {
        m_simu = simu;
        m_name = nameID;
        m_nameMord1 = nameMord1;
        m_nameMord2 = nameMord2;
        m_nameModel = nameModel;
        
        int res = 0;
        if (m_simu != IntPtr.Zero)
        {
            res = sofaPhysicsAPI_createPliers(m_simu, m_name, m_nameMord1, m_nameMord2, m_nameModel);
        }
        Debug.Log("SofaPliers creation: " + nameID + " -> " + res);
    }

    // TODO: check if needed
    bool m_isDisposed;

    /// Name of the Sofa object mapped to this Object.
    protected string m_name;
    protected string m_nameMord1;
    protected string m_nameMord2;
    protected string m_nameModel;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;


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

    /// Method to activate or not the tool attached to the ray caster
    public int closePliers()
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_closePliers(m_simu, m_name);
        Debug.Log("SofaPliers closePliers: " + m_name + " -> " + res);
        return res;
    }

    public int releasePliers()
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_releasePliers(m_simu, m_name);

        return res;        
    }
    

    public int getIdsGrabed(int[] ids)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        return sofaPhysicsAPI_idGrabed(m_simu, m_name, ids);
    }


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createPliers(IntPtr obj, string nameID, string nameMord1, string nameMord2, string nameModel);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_closePliers(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_releasePliers(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_idGrabed(IntPtr obj, string nameID, int[] ids);
}
