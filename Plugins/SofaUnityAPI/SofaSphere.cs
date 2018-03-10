using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class used to handle bindings to the Sofa Sphere object, using a Sphere RegularGrid topology.
/// </summary>
public class SofaSphere : SofaBaseMesh
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaSphere(IntPtr simu, string nameID, bool isRigid)
        : base(simu, nameID, isRigid)
    {

    }

    /// Destructor
    ~SofaSphere()
    {
        Dispose(false);
    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override void createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the sphere
            int res = sofaPhysicsAPI_addSphere(m_simu, m_name, m_isRigid);
            m_name += "_node";
            if (res == 1) // sphere added
            {
                if (log)
                    Debug.Log("sphere Added! " + m_name);

                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error sphere created can't be found!");
        }
    }


    /// Method to recompute triangulation if needed in Grid.
    public override void recomputeTopology(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        Vector3[] verts = mesh.vertices;
        Vector3[] norms = mesh.normals;

        int cpt = 0;
        while (cpt< triangles.Length)
        {
            Vector3 AB = verts[triangles[cpt + 1]] - verts[triangles[cpt]];
            Vector3 AC = verts[triangles[cpt + 2]] - verts[triangles[cpt]];

            Vector3 AD = Vector3.Cross(AB, AC);
            Vector3 norm = norms[triangles[cpt]];
            float dot = Vector3.Dot(AD, norm);

            if (dot <0) // need to inverse triangle
            {
                int tmp = triangles[cpt + 1];
                triangles[cpt + 1] = triangles[cpt + 2];
                triangles[cpt + 2] = tmp;
            }

            cpt = cpt + 3;
        }

        mesh.triangles = triangles;
    }

    /// Method to recompute the Tex coords according to mesh position and geometry.
    public override void recomputeTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        Vector2[] uvs = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i++)
            uvs[i] = new Vector2(verts[i].x + verts[i].y, verts[i].z + verts[i].y);

        mesh.uv = uvs;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addSphere(IntPtr obj, string name, bool isRigid);
}
