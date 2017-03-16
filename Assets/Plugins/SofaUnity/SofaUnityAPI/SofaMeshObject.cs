using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaMeshObject : SofaBaseObject
{
    public SofaMeshObject(IntPtr simu, int idObject)
        : base (simu, idObject)
    {

    }

    public void setMass(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "totalMass", value);
            //Debug.Log("Change Mass res: " + res);
        }
    }

    public void setYoungModulus(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "youngModulus", value);
            Debug.Log("Change youngModulus res: " + res);
        }
    }

    public void setPoissonRatio(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "poissonRatio", value);
            Debug.Log("Change poissonRatio res: " + res);
        }
    }

    public void setTranslation(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] trans = new float[3];
            for (int i = 0; i < 3; ++i)
                trans[i] = values[i];
            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, "translation", trans);
            Debug.Log("Change translation res: " + res);
        }
    }

    public void setRotation(Vector3 values)
    {

    }

    public void setScale(Vector3 values)
    {

    }

    public virtual void updateMesh(Mesh mesh)
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);
            //Debug.Log("vertices: " + nbrV);
            //Debug.Log("vert: " + mesh.vertices.Length);
            //Debug.Log("normals: " + normals.Length);
            //Debug.Log(vertices.Length);

            float[] vertices = new float[nbrV * 3];
            sofaPhysics3DObject_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            sofaPhysics3DObject_getNormals(m_simu, m_name, normals);

            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            bool first = false;
            if (verts.Length == 0)// first time
            {
                //Debug.Log("init");
                verts = new Vector3[nbrV];
                norms = new Vector3[nbrV];
                first = true;
            }


            if (vertices.Length != 0 && normals.Length != 0)
            {

                for (int i = 0; i < verts.Length; ++i)
                {
                    // Debug.Log(i + " -> " + verts[i]);
                    //Debug.Log(i + " vert -> " + vertices[i]);
                    if (first)
                    {
                        verts[i] = new Vector3(0, 0, 0);
                        norms[i] = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        verts[i].x = vertices[i * 3];
                        verts[i].y = vertices[i * 3 + 1];
                        verts[i].z = vertices[i * 3 + 2];

                        norms[i].x = normals[i * 3];
                        norms[i].y = normals[i * 3 + 1];
                        norms[i].z = normals[i * 3 + 2];
                    }
                }
            }

            mesh.vertices = verts;
            mesh.normals = norms;
        }
    }


    // API to update Mesh
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getNbVertices(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVertices(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNormals(IntPtr obj, string name, float[] arr);
    //}

    // API to access Topology
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbTriangles(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getQuads(IntPtr obj, string name, int[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbQuads(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTriangles(IntPtr obj, string name, int[] arr);
    //}
}
