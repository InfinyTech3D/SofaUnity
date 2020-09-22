using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class to create a ray caster in SofaAdvancePhysicsAPI and to comunicate with it.
/// </summary>
public class SofaRayCasterAPI : IDisposable
{
    /// <summary> Default constuctor of the sofa ray caster. </summary>
    /// <param name="simu">Pointer to the implicit sofaAPI</param>
    /// <param name="type">type of tool to attach to this ray caster</param>
    /// <param name="nameID">unique name id of this ray caster</param>
    /// <param name="length">length of the ray</param>
    public SofaRayCasterAPI(IntPtr simu, int type, string nameID, float length)
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

            if (res != 0)
                Debug.LogError("SofaRayCasterAPI::Create Tool returns error: " + SofaDefines.msg_error[res]);
        }
    }

    // TODO: check if needed
    //bool m_isDisposed = false;

    /// Name of the Sofa object mapped to this Object.
    protected string m_name;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    /// Type of Sofa object mapped to this Object.
    protected string m_type;

    /// Memory free method
    public void Dispose()
    {
        activateTool(false);

        //m_isDisposed = true;
    }   

    /// Method to activate or not the tool attached to the ray caster
    public int activateTool(bool value)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        int res = sofaPhysicsAPI_activateTool(m_simu, m_name, value);

        return res;
    }

    public int setToolAttribute(string attribute, float value)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        float[] val = new float[1];
        val[0] = value;
        int res = sofaPhysicsAPI_setToolAttribute(m_simu, m_name, attribute, val);
        val = null;

        if (res != 0)
            Debug.LogError("SofaRayCasterAPI::setToolAttribute return error: " + SofaDefines.msg_error[res]);

        return res;
    }

    /// Method to send a new ray. TODO: could be optimise here and send only if tool is activated
    public int castRay(Vector3 originInSofa, Vector3 directionInSofa)
    {
        if (m_simu == IntPtr.Zero)
            return -10;

        float[] ori = new float[3];
        float[] dir = new float[3];

        for (int i = 0; i < 3; ++i)
        {
            ori[i] = originInSofa[i];
            dir[i] = directionInSofa[i];
        }

        int res = sofaPhysicsAPI_castRay(m_simu, m_name, ori, dir);

        ori = null;
        dir = null;
        return res;
    }

    public string getTouchedObjectName()
    {
        if (m_simu == IntPtr.Zero)
            return "Error";

        string res = sofaPhysicsAPI_getInteractObjectName(m_simu, m_name);
        return res;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    //////////      Communication API to SofaAdvancePhysicsAPI ray caster     ///////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// API to create a specific ray caster tool.
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createResectionTool(IntPtr obj, string toolName, float length);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createAttachTool(IntPtr obj, string toolName, float length);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createFixConstraintTool(IntPtr obj, string toolName, float length);

    /// Binding to activate or desactivate the tool.
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_activateTool(IntPtr obj, string toolName, bool value);

    /// Binding to propagate the ray position and direction.
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_castRay(IntPtr obj, string toolName, float[] origin, float[] direction);

    /// Binding to propagate the ray position and direction.
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setToolAttribute(IntPtr obj, string toolName, string dataName, float[] value);

    /// Binding to get the name of the mesh touched by the ray.
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getInteractObjectName(IntPtr obj, string toolName);

    
}
