using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBox : IDisposable
{
    internal IntPtr m_native;
    internal IntPtr m_simu;
    bool m_isDisposed;

    public SofaBox(IntPtr simu)
    {
        m_simu = simu;
        // add new box
        // test get apiname
       // sofaPhysicsAPI_addCube(m_simu, "toto");

    }

    ~SofaBox()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!m_isDisposed)
        {
            m_isDisposed = true;

            //if (!_preventDelete)
            //{
            //    IntPtr userPtr = btCollisionShape_getUserPointer(_native);
            //    GCHandle.FromIntPtr(userPtr).Free();

            //    btCollisionShape_delete(_native);
            //}
        }
    }

    public int[] createTriangulation(int nbr)
    {
        int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, "truc1_node");

        //Debug.Log("NbrQuad: " + nbrQuads);
        int nbrIndices = nbrQuads * 4;
        int[] quads = new int[nbrIndices];
        sofaPhysics3DObject_getQuads(m_simu, "truc1_node", quads);

        int[] tris = new int[nbrIndices*2];

        for (int i = 0; i < nbrQuads; ++i)
        {
            //Debug.Log(i + " -> " + 
            //    quads[i * 4] + " " +
            //    quads[i * 4+1] + " " +
            //    quads[i * 4+2] + " " +
            //    quads[i * 4+3]);

            tris[i * 6] = quads[i * 4];
            tris[i * 6 + 1] = quads[i * 4 + 1];
            tris[i * 6 + 2] = quads[i * 4 + 2];

            tris[i * 6 + 3] = quads[i * 4 + 2];
            tris[i * 6 + 4] = quads[i * 4 + 3];
            tris[i * 6 + 5] = quads[i * 4];

            //Debug.Log(i + " -> " +
            //    tris[i * 6] + " " +
            //    tris[i * 6 + 1] + " " +
            //    tris[i * 6 + 2]);

            //Debug.Log(i + " -> " +
            //    tris[i * 6 + 3] + " " +
            //    tris[i * 6 + 4] + " " +
            //    tris[i * 6 + 5]);
        }

        return tris;
    }

    public int[] createQuads()
    {
       /* var quads = sofaPhysics3DObject_getQuads(m_simu, "truc1_node");
        int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, "truc1_node");

        int quadArraySize = sofaPhysics3DObject_getSizeQuadArray(m_simu, "truc1_node");
        int triArraySize = sofaPhysics3DObject_getSizeTriangleArray(m_simu, "truc1_node");
        

        Debug.Log("NbrQuad: " + nbrQuads);
        Debug.Log("NbrTri: " + sofaPhysics3DObject_getNbTriangles(m_simu, "truc1_node"));
        
        Debug.Log(quads.Length);

        for (int i = 0; i < nbrQuads; ++i)
            Debug.Log(i + " => " + quads[i]);

        Debug.Log("quadArraySize: " + quadArraySize);
        Debug.Log("triArraySize: " + triArraySize);
        */

        return new int[] { };
    }

    public void updateMesh(Mesh mesh)
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, "truc1_node");
            var vertices = sofaPhysics3DObject_getVertices(m_simu, "truc1_node");
            var normals = sofaPhysics3DObject_getNormals(m_simu, "truc1_node");

            //Debug.Log("vertices: " + nbrV);
            //Debug.Log("vert: " + mesh.vertices.Length);
            //Debug.Log("normals: " + normals.Length);
            //Debug.Log(vertices.Length);

            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            bool first = false;
            if (verts.Length == 0)// first time
            {
                verts = new Vector3[nbrV];
                norms = new Vector3[nbrV];
                first = true;                
            }


            if (vertices.Length != 0 && normals.Length != 0)
            {

                for (int i = 0; i < verts.Length; ++i)
                {
                    // Debug.Log(i + " -> " + verts[i]);
                    // Debug.Log(i + " vert -> " + vertices[i]);
                    if (first)
                    {
                        verts[i] = new Vector3(0, 0, 0);
                        norms[i] = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        verts[i].x = vertices[i * 3];
                        verts[i].z = vertices[i * 3 + 1];
                        verts[i].y = vertices[i * 3 + 2];

                        norms[i].x = normals[i * 3];
                        norms[i].z = normals[i * 3 + 1];
                        norms[i].y = normals[i * 3 + 2];
                    }
                }
            }

            mesh.vertices = verts;
            mesh.normals = norms;
        }
    }

    public void test()
    {
        Debug.Log("sofa_test1: " + sofaPhysicsAPI_getNumberObjects(m_simu));
        Debug.Log("NAME: " + sofaPhysicsAPI_APIName(m_simu));
        if (m_native != IntPtr.Zero) {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, "truc1_node");
            int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, "truc1_node");            

            Debug.Log("NbrV: " + nbrV);
            Debug.Log("NbrTri: " + sofaPhysics3DObject_getNbTriangles(m_simu, "truc1_node"));
            
            Debug.Log("NbrQuad: " + nbrQuads);
            int[] toto = new int[nbrQuads];
            sofaPhysics3DObject_getQuads(m_simu, "truc1_node", toto);
            for (int i = 0; i < 10; ++i)
            {
                Debug.Log(i + " => " + toto[i]);
            }
            

          /*  var quads = sofaPhysics3DObject_getQuads(m_simu, "truc1_node");
            var vertices = sofaPhysics3DObject_getVertices(m_simu, "truc1_node");
            Debug.Log(quads.Length);
            Debug.Log(vertices.Length);

            for (int i = 0; i < nbrV; ++i)
                Debug.Log(i + " => " + vertices[i]);
                */
          /*for (int i=0; i<nbrQuads; ++i)
          {
              Debug.Log(i + " => " + quads[i]);
          }*/

            //int toto[];
            //Debug.Log("NbrVV: " + sofaPhysics3DObject_getNbVertices(m_native));
        }
    }

    public void addCube()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        { 
            int res = sofaPhysicsAPI_addCube(m_simu, "truc1");
            if (res == 1) // cube added
            {
                //Debug.Log("cube Added!");
                //for (int i = 0; i < sofaPhysicsAPI_getNumberObjects(m_simu)+1; ++i)
                //    Debug.Log("obj found: " + i + " -> " + sofaPhysicsAPI_get3DObjectName(m_simu, i));

                m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1_node");
            //    Debug.Log("NbrV: " + sofaOutputMesh_getNbVertices(m_native));
            }

            //    m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1");

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Cube created can't be found!");
            else
            {
                //Debug.Log("cube found!");
              //  Debug.Log("NbrV: " + sofaOutputMesh_getNbVertices(m_native));
            }
        }
    }



    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addCube(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr sofaPhysicsAPI_get3DObject(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr sofaPhysicsAPI_getOutputMesh(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaOutputMesh_getNbVertices(IntPtr obj);

    //[DllImport("SofaAdvancePhysicsAPI")]
    //public static extern IntPtr sofaOutputMesh_getVertices(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getNbVertices(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbTriangles(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbQuads(IntPtr obj, string name);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern float[] sofaPhysics3DObject_getVertices(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern float[] sofaPhysics3DObject_getNormals(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getQuads(IntPtr obj, string name, int [] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTriangles(IntPtr obj, string name, int[] arr);


    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysics3DObject_getNbVertices(IntPtr obj);

}
