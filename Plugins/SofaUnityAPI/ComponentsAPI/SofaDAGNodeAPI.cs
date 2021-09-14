using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaDAGNodeAPI : SofaBaseAPI
{
    protected string m_componentListS = "";
    protected string m_parentName = "root";

    /// <summary>
    /// Default constructor inherite from SofaBaseAPI. Will be in charge to synchrosize info with a DAG node in sofa context
    /// </summary>
    /// <param name="simu"></param>
    /// <param name="nameID"></param>
    /// <param name="parentName"></param>
    /// <param name="isCustom"></param>
    public SofaDAGNodeAPI(IntPtr simu, string nameID, string parentName, bool isCustom)
        : base(simu, nameID, isCustom)        
    {
        m_parentName = parentName;
    }


    /// Main method to create get access or create (if custom) the DAG Node on the sofa context
    override protected bool Init()
    {
        if (m_isCustom)
        {
            int res = sofaPhysicsAPI_addDAGNode(m_simu, m_parentName, m_name);
            if (res != 1)
            {
                Debug.LogError("Error while creating DAGNode: " + m_name + " under " + m_parentName + " Code error return: " + res);
                return false;
            }
        }
        else
        {
            m_componentListS = sofaPhysicsAPI_getDAGNodeComponentsName(m_simu, m_name);
        }
        //Debug.Log("SofaDAGNodeAPI::Init(): " + m_name);
        //Debug.Log("SofaDAGNodeAPI::Init() Found: " + m_componentListS);
        return true;
    }

    /// Method to get the list of component own by this DAGnode. The list is static and need to be asked again for refresh @sa RecomputeDAGNodeComponents
    public string GetDAGNodeComponents()
    {
        return m_componentListS;
    }


    /// Method to update the list of components own by this DAGNode
    public string RecomputeDAGNodeComponents()
    {
        if (m_isReady)
        {
            m_componentListS = sofaPhysicsAPI_getDAGNodeComponentsName(m_simu, m_name);
            return m_componentListS;
        }
        else
            return "Error";
    }


    /// Get the Base component of a given component identified by its name.
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

    /// Get the name of the DAGNode parent of this one.
    public string GetParentNodeName()
    {
        if (m_isReady)
        {
            string type = sofaPhysicsAPI_getDAGNodeParentAPIName(m_simu, m_name);
            return type;
        }
        else
            return "Error";
    }

    /// Method to check if transformation is possible
    public bool GetTransformationTest(string transfoType)
    {
        float[] val = new float[3];
        int res = sofaPhysicsAPI_getDAGNodeTransform(m_simu, m_name, transfoType, val);
        if (res == 0)
            return true;
        else
            return false;
    }


    /// <summary>
    /// Method to get the current transformation of type @param transfoType of the full node in the SOFA context.
    /// </summary>
    /// <param name="transfoType">type of transformation targeted, 'translation', 'rotation' or 'scale3d'</param>
    /// <returns></returns>
    public Vector3 GetTransformation(string transfoType)
    {
        Vector3 result;
        if (transfoType == "scale3d")
            result = Vector3.one;
        else
            result = Vector3.zero;

        if (!m_isReady)
            return result;
        
        float[] val = new float[3];
        int res = sofaPhysicsAPI_getDAGNodeTransform(m_simu, m_name, transfoType, val);

        if (res == 0)
        {
            for (int i = 0; i < 3; ++i)
                result[i] = val[i];
        }
        else
            Debug.LogError("Method GetTransformation of type: " + transfoType + " of DAGNode: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
    
        return result;
    }


    /// <summary>
    /// Method to set a new transformation of type @param transfoType to the full node in the SOFA context
    /// </summary>
    /// <param name="transfoType">type of transformation targeted, 'translation', 'rotation' or 'scale3d'</param>
    /// <param name="values">xyz values to set</param>
    public void SetTransformation(string transfoType, Vector3 values)
    {
        if (!m_isReady)
            return;

        float[] val = new float[3];
        for (int i = 0; i < 3; ++i)
            val[i] = values[i];

        int res = sofaPhysicsAPI_setDAGNodeTransform(m_simu, m_name, transfoType, val);
        if (res != 0)
            Debug.LogError("Method SetTransformation of type: " + transfoType + " of DAGNode: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
    }


  
    /////////////////////////////////////////////////////////////////////////////////////////
    //////////            API to Communication with Sofa DAGNODE              ///////////////
    /////////////////////////////////////////////////////////////////////////////////////////


    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getDAGNodeComponentsName(IntPtr obj, string nodeName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_getDAGNodeParentAPIName(IntPtr obj, string nodeName);


    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaComponentAPI_getBaseComponentType(IntPtr obj, string componentName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addDAGNode(IntPtr obj, string parentNodeName, string nodeName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_changeDAGNodeAPIName(IntPtr obj, string oldNodeName, string newNodeName);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getDAGNodeTransform(IntPtr obj, string nodeName, string transformType, float[] values);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setDAGNodeTransform(IntPtr obj, string nodeName, string transformType, float[] values);


}
