using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class to create a ray caster in SofaAdvancePhysicsAPI and to comunicate with it.
/// </summary>
public class SofaRayCaster : IDisposable
{
    /// <summary> Default constuctor of the sofa ray caster. </summary>
    /// <param name="simu">Pointer to the implicit sofaAPI</param>
    /// <param name="type">type of tool to attach to this ray caster</param>
    /// <param name="nameID">unique name id of this ray caster</param>
    /// <param name="length">length of the ray</param>
    public SofaRayCaster(IntPtr simu, int type, string nameID, float length)
    {
        m_simu = simu;
        m_name = nameID;

        if (length < 0.0f)
            length = 1.0f;

        int res = 0;
        if (m_simu != IntPtr.Zero)
        {
            if (type == 0)
                res = sofaPhysicsAPI_createResectionTool(m_simu, m_name, length);
            else if (type == 1)
                res = sofaPhysicsAPI_createAttachTool(m_simu, m_name, length);
            else
                res = sofaPhysicsAPI_createFixConstraintTool(m_simu, m_name, length);
        }
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

    /// Method to activate or not the tool attached to the ray caster
    public int activateTool(bool value)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_activateTool(m_simu, m_name, value);

        return res;
    }

    /// Method to send a new ray. TODO: could be optimise here and send only if tool is activated
    public int castRay(Vector3 origin, Vector3 direction, Vector3 scaleUnityToSofa)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        float[] ori = new float[3];
        float[] dir = new float[3];

        for (int i = 0; i < 3; ++i)
        {
            ori[i] = origin[i] * scaleUnityToSofa[i];
            dir[i] = direction[i];
        }

        int res = sofaPhysicsAPI_castRay(m_simu, m_name, ori, dir);

        ori = null;
        dir = null;
        return res;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    //////////      Communication API to SofaAdvancePhysicsAPI ray caster     ///////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// API to create a specific ray caster tool.
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createResectionTool(IntPtr obj, string name, float length);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createAttachTool(IntPtr obj, string name, float length);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createFixConstraintTool(IntPtr obj, string name, float length);

    /// Binding to activate or desactivate the tool.
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_activateTool(IntPtr obj, string name, bool value);

    /// Binding to propagate the ray position and direction.
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_castRay(IntPtr obj, string name, float[] origin, float[] direction);

}
