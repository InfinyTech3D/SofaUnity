using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaGeomagic : IDisposable
{
    /// Name of the Sofa object mapped to this Object.
    protected string m_name;

    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;

    /// Parameter to activate internal logging
    protected bool log = true;
    protected bool m_isReady = false;
    public bool IsReady() { return m_isReady; }

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

    public SofaGeomagic(IntPtr simu, string deviceNameID)
    {
        m_simu = simu;
        m_name = deviceNameID;

        SofaDefines.msg_error[-700] = "SOFA Geomagic Plugin has not been activated.";
        SofaDefines.msg_error[-701] = "SOFA Geomagic manager creation failed.";
        SofaDefines.msg_error[-702] = "No SOFA Geomagic Drivers found in the scene.";
        SofaDefines.msg_error[-703] = "SOFA Geomagic Driver object is invalid.";
        SofaDefines.msg_error[-704] = "No SOFA Geomagic Driver found with this name in the scene.";
        SofaDefines.msg_error[-705] = "SOFA Geomagic Driver not registered in Geomagic manager.";
        SofaDefines.msg_error[-708] = "SOFA Geomagic manager can't access to simulation thread.";

        int res = -1;
        if (m_simu != IntPtr.Zero)
        {
            res = sofaPhysicsAPI_createGeomagicManager(m_simu, m_name);
            if (res != 0)
            {
                Debug.LogError("SofaGeomagic creation: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                m_isReady = false;
                return;
            }
            
        }
        else
            Debug.LogError("SofaGeomagic creation: " + deviceNameID + " failed. Can't access Object Pointer simulation.");

        Debug.Log("SofaGeomagic Creation returns: " + SofaDefines.msg_error[res]);
    }

    public void GeomagicDevice_init()
    {
        int res = sofaPhysicsAPI_initGeomagicDevice(m_simu, m_name);
        if (res == 0)
        {
            Debug.Log("Init returns: " + SofaDefines.msg_error[res]);
            m_isReady = true;
        }
        else
        {
            Debug.LogError("SofaGeomagic init device: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
            m_isReady = false;
        }
    }

    public int GeomagicDevice_getPosition(float[] val)
    {        
        int res = sofaPhysicsAPI_getGeomagicPosition(m_simu, m_name, val);
        return res;
    }

    public int GeomagicDevice_getButton1Status(int[] val)
    {
        int res = sofaPhysicsAPI_getGeomagicButtonStatus1(m_simu, m_name, val);
        return res;
    }
	
	public int GeomagicDevice_getButton2Status(int[] val)
    {
        int res = sofaPhysicsAPI_getGeomagicButtonStatus2(m_simu, m_name, val);
        return res;
    }

    public int GeomagicDevice_getStatus(int[] val)
    {
        int res = sofaPhysicsAPI_getGeomagicStatus(m_simu, m_name, val);
        return res;
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////      Communication API to set/get basic values into Sofa     ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_createGeomagicManager(IntPtr obj, string deviceNameID);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_initGeomagicDevice(IntPtr obj, string deviceNameID);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getGeomagicPosition(IntPtr obj, string deviceNameID, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getGeomagicStatus(IntPtr obj, string deviceNameID, int[] value);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getGeomagicButtonStatus1(IntPtr obj, string deviceNameID, int[] value);
	
	[DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getGeomagicButtonStatus2(IntPtr obj, string deviceNameID, int[] value);
}
