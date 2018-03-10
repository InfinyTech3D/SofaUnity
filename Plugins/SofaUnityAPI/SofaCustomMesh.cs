using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaCustomMesh : SofaBaseObject
{
    public SofaCustomMesh(IntPtr simu, string nameID)
        : base(simu, nameID, true)
    {

    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override void createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the cube
            int res = sofaPhysicsAPI_addMechanicalObject(m_simu, m_name);

            m_name += "_node";
            if (res == 1) // cube added
            {
                if (log)
                    Debug.Log("mechanicalObject Added! " + m_name);

                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error mechanicalObject created can't be found!");

        }
    }

    //public void createMesh()
    //{
    //    if (m_native == IntPtr.Zero || uObject == null)
    //        return;

    //    m_mesh = uObject.GetComponent<MeshFilter>().sharedMesh;

    //    Vector3[] vertices = m_mesh.vertices;

    //    //int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, 13);
    //    int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, vertices.Length);
    //    Debug.Log("mechanicalObject size: " + m_name + " " + res);

    //    updateMesh();      
    //}

    public void setNumberOfVertices(int nbr)
    {
        int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, nbr);
        if (res < 0)
            Debug.LogError("mechanicalObject size: " + m_name + " " + res);
    }

    


    public void updateMesh(Transform trans, Vector3[] vertices, Vector3 scaleUnityToSofa)
    {
        if (m_native == IntPtr.Zero)
            return;
        
        int nbrV = vertices.Length;
        //float[] val = new float[(nbrV * 2+1) * 3];
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
            Debug.LogError("mechanicalObject updateMesh: " + m_name + " " + resUpdate);
    }


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addMechanicalObject(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setNbVertices(IntPtr obj, string name, int nbrV);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVertices(IntPtr obj, string name, float[] arr);

}