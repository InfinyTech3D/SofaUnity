using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaPlane : SofaMeshObject
{
    public SofaPlane(IntPtr simu, int idObject, bool isRigid)
        : base(simu, idObject, isRigid)
    {

    }

    ~SofaPlane()
    {
        Dispose(false);
    }

    public override int[] createTriangulation()
    {
        return base.createTriangulation();
    }

    protected override void createObject()
    {
        m_name = "plane_" + m_idObject + "_node";

        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the plane
            int res = sofaPhysicsAPI_addPlane(m_simu, "plane_" + m_idObject, m_isRigid);

            if (res == 1) // plane added
            {
                Debug.Log("plane Added! " + m_name);

                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            //    m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1");

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error plane created can't be found!");
        }
    }

    public override void recomputeTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;

        float[] texCoords = new float[nbrV * 2];
        Vector2[] uv = new Vector2[nbrV];

        sofaPhysics3DObject_getTexCoords(m_simu, m_name, texCoords);

        for (int i = 0; i < nbrV; i++)
        {
            uv[i].x = texCoords[i * 2];
            uv[i].y = texCoords[i * 2 + 1];

            if (uv[i].x == 0.0 && uv[i].y == 0.0)
                uv[i] = new Vector2(1-(verts[i].x + verts[i].y), verts[i].z + verts[i].y);
        }

        mesh.uv = uv;
    }

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addPlane(IntPtr obj, string name, bool isRigid);

}
