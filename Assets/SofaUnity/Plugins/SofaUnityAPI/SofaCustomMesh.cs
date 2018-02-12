using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaCustomMesh : SofaBaseObject
{
    /// Booleen to warn mesh normals have to be inverted
    protected Mesh m_mesh = null;
    protected GameObject uObject = null;

    public SofaCustomMesh(IntPtr simu, string nameID, GameObject obj)
        : base(simu, nameID, true)
    {
        uObject = obj;
    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override void createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the cube
            int res = sofaPhysicsAPI_addMechanicalObject(m_simu, m_name);
            Debug.Log("mechanicalObject result: " + m_name + " -> " + res);
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

    public void createMesh()
    {
        if (m_native == IntPtr.Zero || uObject == null)
            return;

        m_mesh = uObject.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = m_mesh.vertices;

        //int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, 13);
        int res = sofaPhysicsAPI_setNbVertices(m_simu, m_name, vertices.Length);
        Debug.Log("mechanicalObject size: " + m_name + " " + res);

        updateMesh();      
    }

    public void updateMesh()
    {
        if (m_native == IntPtr.Zero || m_mesh == null)
            return;

        Vector3[] vertices = m_mesh.vertices;
        int nbrV = vertices.Length;
        //float[] val = new float[(nbrV * 2+1) * 3];
        float[] val = new float[(nbrV) * 3];

        Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < nbrV; i++)
        {
            Vector3 vert = uObject.transform.TransformPoint(vertices[i]);
            bary = bary + vert;
            val[i * 3] = -vert.x;
            val[i * 3 + 1] = vert.y;
            val[i * 3 + 2] = vert.z;
            //Debug.Log(i + " : " + val[i * 3] + " | " + val[i * 3 + 1] + " | " + val[i * 3 + 2]);
        }
        bary /= nbrV;

        //for (int i = 0; i < nbrV; i++)
        //{
        //    Vector3 vert = uObject.transform.TransformPoint(vertices[i]);

        //    val[nbrV + i * 3] = -(vert.x + bary.x) * 0.5f;
        //    val[nbrV + i * 3 + 1] = (vert.y + bary.y) * 0.5f;
        //    val[nbrV + i * 3 + 2] = (vert.z + bary.z) * 0.5f;

        //    //Debug.Log(i + nbrV + " : " + val[nbrV + i * 3] + " | " + val[nbrV + i * 3 + 1] + " | " + val[nbrV + i * 3 + 2]);
        //}

        //val[nbrV] = -bary.x;
        //val[nbrV + 1] = bary.y;
        //val[nbrV + 2] = bary.z;

        sofaPhysics3DObject_setVertices(m_simu, m_name, val);
    }


    public void updateMeshBary()
    {
        if (m_native == IntPtr.Zero)
            return;

        Vector3[] vertices = m_mesh.vertices;
        int[] triangles = m_mesh.triangles;

        float[] val = new float[13 * 3];
        Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);

        // compute 6 cube faces bary
        for (int i=0; i<6; i++)
        {
            Vector3 vert = new Vector3(0.0f, 0.0f, 0.0f);
            for (int j = 0; j < 6; j++)
            {
                int id = triangles[i * 6 + j];
                vert = vert + uObject.transform.TransformPoint(vertices[id]);
            }

            vert /= 6;
            val[i * 3] = -vert.x;
            val[i * 3 + 1] = vert.y;
            val[i * 3 + 2] = vert.z;

            bary = bary + vert;
        }


        // compute full bary
        bary /= 6;
        val[18] = -bary.x;
        val[19] = bary.y;
        val[20] = bary.z;

        // Add mid distance between bary and bary faces
        for (int i = 0; i < 6; i++)
        {
            val[21 + i * 3] = (val[18] + val[i * 3]) / 2;
            val[21 + i * 3 + 1] = (val[19] + val[i * 3 + 1]) / 2;
            val[21 + i * 3 + 2] = (val[20] + val[i * 3 + 2]) / 2;
        }
                
        sofaPhysics3DObject_setVertices(m_simu, m_name, val);
        //Debug.Log("nbrV: " + vertices.Length);
        //for (int i = 0; i < vertices.Length; ++i)
        //    //Debug.Log(i + " : " + transform.TransformPoint(vertices[i]));
        //    
    }

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addMechanicalObject(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setNbVertices(IntPtr obj, string name, int nbrV);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_setVertices(IntPtr obj, string name, float[] arr);

}