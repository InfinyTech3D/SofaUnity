using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// SofaPliers Class is used to create a specific Plier tool on Sofa side and comunicate with it.
/// the plier is composed of 2 jaws each one represented by a mesh. The last object is the object to interact with.
/// Can only interact with a single physical object for the moment.
/// </summary>
public class SofaPliers : IDisposable
{
    /// <summary> Default constuctor of the SofaPliers. </summary>
    /// <param name="simu">Pointer to the implicit sofaAPI</param>
    /// <param name="nameID">unique name id sofaPlier</param>
    /// <param name="nameMord1">Model name of the 1st plier jaw</param>
    /// <param name="nameMord2">Model name of the 2nd plier jaw</param>
    /// <param name="nameModel">model name to interact with</param>
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
            if (res < 0)
                Debug.LogError("SofaPliers creation: " + m_name + " returns error: " + res
                    + " | m_nameMord1: " + m_nameMord1
                    + " | m_nameMord2: " + m_nameMord2
                    + " | m_nameModel: " + m_nameModel);
        }
        else
            Debug.LogError("SofaPliers creation: " + nameID + " failed. Can't access Object Pointer simulation.");
    }

    // TODO: check if needed
    bool m_isDisposed;

    /// Name of the Sofa object mapped to this Object.
    protected string m_name;
    /// Name of the 1st plier jaw
    protected string m_nameMord1;
    /// Name of the 2nd plier jaw
    protected string m_nameMord2;
    /// Name of the physical model to interact with
    protected string m_nameModel;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    /// Parameter to activate internal logging
    protected bool log = false;


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


    /// <summary> Method to activate the plier when it is being closed.</summary>
    /// <returns> The number of vertices being grabed. Return negative value if encountered an error.</returns>
    public int closePliers()
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_closePliers(m_simu, m_name);
        if(log)
            Debug.Log("SofaPliers closePliers: " + m_name + " -> return: " + res);

        return res;
    }


    /// Method to release the plier when it is being opened.
    /// <returns> Negative value if encountered an error.</returns>
    public int releasePliers()
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_releasePliers(m_simu, m_name);
        if (log)
            Debug.Log("SofaPliers releasePliers: " + m_name + " -> return: " + res);

        return res;        
    }


    /// <summary> Method to get the vertex ids grabed by the plier.</summary>
    /// <param name="ids"> Buffer used to exchange the vertex ids.</param>
    /// <returns> Negative value if encountered an error.</returns>
    public int getIdsGrabed(int[] ids)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        return sofaPhysicsAPI_idGrabed(m_simu, m_name, ids);
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createPliers(IntPtr obj, string nameID, string nameMord1, string nameMord2, string nameModel);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_closePliers(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_releasePliers(IntPtr obj, string nameID);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_idGrabed(IntPtr obj, string nameID, int[] ids);
}
