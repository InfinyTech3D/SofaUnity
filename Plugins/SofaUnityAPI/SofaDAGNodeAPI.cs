using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaDAGNodeAPI : SofaBaseAPI
{
    protected string m_componentListS = "";

    public SofaDAGNodeAPI(IntPtr simu, string nameID, bool isCustom)
        : base(simu, nameID, isCustom)
    {

    }

    override protected bool Init()
    {
        
        m_componentListS = sofaPhysicsAPI_getDAGNodeComponentsName(m_simu, m_name);
        //Debug.Log("SofaDAGNodeAPI::Init(): " + m_name);
        //Debug.Log("SofaDAGNodeAPI::Init() Found: " + m_componentListS);
        return true;
    }

    public string GetDAGNodeComponents()
    {
        return m_componentListS;
    }


    public string RecomputeDAGNodeComponents()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getDAGNodeComponentsName(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }


    public string GetBaseComponentType(string componentName)
    {
        if (m_isReady)
        {
            string type = sofaComponentAPI_getBaseComponentType(m_simu, componentName);
            return type;
        }
        else
            return "Error";
    }

    public string GetParentNodeName()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getDAGNodeParentName(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }



    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getDAGNodeComponentsName(IntPtr obj, string nodeName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getDAGNodeParentName(IntPtr obj, string nodeName);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getBaseComponentType(IntPtr obj, string componentName);

    
}
