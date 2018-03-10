using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class of a SofaCustomMesh. This is the base class that will create a physical node on the SofaAdvancePhysicsAPI.
/// It will consist into a MechanicalObject and a collisionSphereModel
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaCustomMesh : SofaBaseObject
{
    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    public SofaCustomMesh(IntPtr simu, string nameID)
        : base(simu, nameID, true)
    {

    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override void createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create a Node in Sofa simulation tree and add a mechanicalObject into it
            int res = sofaPhysicsAPI_addMechanicalObject(m_simu, m_name);

            m_name += "_node";
            if (res == 1) 
            {
                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error SofaCustomMesh created can't be found!");
        }
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
    public void updateMesh(Transform trans, Vector3[] vertices, Vector3 scaleUnityToSofa)
    {
        if (m_native == IntPtr.Zero)
            return;
        
        int nbrV = vertices.Length;
        float[] val = new float[(nbrV) * 3];

        for (int i = 0; i < nbrV; i++)
        {
            Vector3 vert = trans.TransformPoint(vertices[i]);
            val[i * 3] = vert.x * scaleUnityToSofa.x;
            val[i * 3 + 1] = vert.y * scaleUnityToSofa.y;
            val[i * 3 + 2] = vert.z * scaleUnityToSofa.z;
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