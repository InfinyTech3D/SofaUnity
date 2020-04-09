using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class of a SofaCustomMesh. This is the base class that will create a physical node on the SofaAdvancePhysicsAPI.
/// It will consist into a MechanicalObject and a collisionSphereModel
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaCustomMeshAPI : SofaBaseObjectAPI
{
    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    public SofaCustomMeshAPI(IntPtr simu, string parentName, string nameID)
        : base(simu, nameID, parentName, true)
    {

    }


    protected string m_dofName = "";
    protected string m_collisionName = "";

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override bool createObject()
    {
        if (m_hasObject == false) // first time create object only
        {
            m_dofName = m_name + "_dof";
            m_collisionName = m_name + "_col";

            // Create a Node in Sofa simulation tree and add a mechanicalObject into it
            int res = sofaPhysicsAPI_addSphereCollisionsObject(m_simu, m_name, m_parentName);
            m_name += "_node";

            if (res != 0)
            {
                Debug.LogError("SofaCustomMesh::createObject creation method return error: " + SofaDefines.msg_error[res] + " for object " + m_name);
                return false;
            }

            if (displayLog)
            {
                Debug.Log("SofaCustomMesh Added! " + m_name);
                Debug.Log("SofaCustomMesh: m_parentName: " + m_parentName);
                Debug.Log("SofaCustomMesh: m_dofName: " + m_dofName);
            }

            // Set created object to native pointer
            int res1 = sofaPhysicsAPI_has3DObject(m_simu, m_name);
            if (res == 0)
                m_hasObject = true;
            else
            {
                Debug.LogError("SofaCustomMesh::createObject get3DObject method returns: " + SofaDefines.msg_error[res1]);
                m_hasObject = false;
                return false;
            }

            return true;
        }

        return false;        
    }

   
    /// <summary> Method to set the number of vertices to this 3D Object. </summary>
    /// <param name="nbr"> Number of vertices </param>
    public void SetNumberOfVertices(int nbr)
    {
        if (!m_isCreated)
            return;

        int res = sofaMeshAPI_setNbVertices(m_simu, m_dofName, nbr);
        if (res < 0)
            Debug.LogError("mechanicalObject size: " + m_dofName + " " + res);
        else
            sofaComponentAPI_reinitComponent(m_simu, m_dofName);
    }



    /// <summary> Method to update the mesh position from unity position.</summary>
    /// <param name="trans"> Local GameObject Transform to get Unity world position.</param>
    /// <param name="vertices"> List of vertices from Unity GameObject.</param>
    /// <param name="scaleUnityToSofa"> scale to transform Unity to Sofa positions.</param>
    public void UpdateMesh(Transform trans, Vector3[] vertices, Transform sofaCTransform)
    {
        if (!m_isCreated)
            return;
        
        int nbrV = vertices.Length;
        float[] val = new float[(nbrV) * 3];

        for (int i = 0; i < nbrV; i++)
        {
            Vector3 vert = trans.TransformPoint(vertices[i]);
            Vector3 vertS = sofaCTransform.InverseTransformPoint(vert);

            val[i * 3] = vertS.x;
            val[i * 3 + 1] = vertS.y;
            val[i * 3 + 2] = vertS.z;
        }

        int resUpdate = sofaMeshAPI_setVertices(m_simu, m_dofName, val);
        if (resUpdate < 0)
            Debug.LogError("SofaCustomMesh updateMesh: " + m_dofName + " return error: " + resUpdate);
    }


    public void SetFloatValue(string dataName, float value)
    {
        if (!m_isCreated)
            return;


        int res = sofaComponentAPI_setDoubleValue(m_simu, m_collisionName, dataName, (double)value);

        if (res != 0)
            Debug.LogError("Method setFloatValue of Data: " + dataName + " of object: " + m_collisionName + " returns error: " + SofaDefines.msg_error[res]);
        else
            sofaComponentAPI_reinitComponent(m_simu, m_collisionName);
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addSphereCollisionsObject(IntPtr obj, string name, string parentNodeName);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setNbVertices(IntPtr obj, string name, int nbrV);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setVertices(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_setDoubleValue(IntPtr obj, string componentName, string dataName, double value);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaComponentAPI_reinitComponent(IntPtr obj, string componentName);
}