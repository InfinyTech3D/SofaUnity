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
    public SofaCustomMeshAPI(IntPtr simu, string nameID)
        : base(simu, nameID, true)
    {

    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override bool createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create a Node in Sofa simulation tree and add a mechanicalObject into it
            int res = sofaPhysicsAPI_addMechanicalObject(m_simu, m_name);
            m_name += "_node";

            if (res != 0)
            {
                Debug.LogError("SofaCustomMesh::createObject creation method return error: " + SofaDefines.msg_error[res] + " for object " + m_name);
                return false;
            }

            if (displayLog)
                Debug.Log("SofaCustomMesh Added! " + m_name);

            // Set created object to native pointer
            int[] res1 = new int[1];
            res1[0] = -101;

            m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name, res1);
            if (res1[0] != 0 || m_native == IntPtr.Zero)
            {
                Debug.LogError("SofaCustomMesh::createObject get3DObject method returns: " + SofaDefines.msg_error[res1[0]]);
                res1 = null;
                return false;
            }

            res1 = null;
            return true;
        }

        return false;        
    }

   
    /// <summary> Method to set the number of vertices to this 3D Object. </summary>
    /// <param name="nbr"> Number of vertices </param>
    public void setNumberOfVertices(int nbr)
    {
        int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, nbr);
        if (res < 0)
            Debug.LogError("mechanicalObject size: " + m_name + " " + res);
    }



    /// <summary> Method to update the mesh position from unity position.</summary>
    /// <param name="trans"> Local GameObject Transform to get Unity world position.</param>
    /// <param name="vertices"> List of vertices from Unity GameObject.</param>
    /// <param name="scaleUnityToSofa"> scale to transform Unity to Sofa positions.</param>
    public void updateMesh(Transform trans, Vector3[] vertices, Transform sofaCTransform)
    {
        if (m_native == IntPtr.Zero)
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

        int resUpdate = sofaPhysics3DObject_setVertices(m_simu, m_name, val);
        if (resUpdate < 0)
            Debug.LogError("SofaCustomMesh updateMesh: " + m_name + " return error: " + resUpdate);
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addMechanicalObject(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setNbVertices(IntPtr obj, string name, int nbrV);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVertices(IntPtr obj, string name, float[] arr);

}