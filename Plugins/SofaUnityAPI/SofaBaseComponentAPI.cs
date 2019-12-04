using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseComponentAPI : SBaseAPI
{

    public SofaBaseComponentAPI(IntPtr simu, string nameID)
        : base(simu, nameID)
    { 

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
