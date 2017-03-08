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

        int resX = 5;
        int resY = 5;
        int resZ = 5;

        int nbrTri = (resX - 1) * (resY - 1) * 4 + (resX - 1) * (resZ - 1) * 4 + (resY - 1) * (resZ - 1) * 4;
        //int nbrTri = 2;
        int[] tris = new int[nbrTri*3];

        //tris[0] = 0;
        //tris[1] = 5;
        //tris[2] = 6;

        //tris[3] = 0;
        //tris[4] = 6;
        //tris[5] = 1;

        Debug.Log("nbrTri: " + (resX - 1) * (resY - 1) * 2 * 3);
        int cpt = 0;
        for (int k = 0; k < 2; ++k)
        {
            int face = k * resX * resY * (resZ - 1);
            for (int j = 0; j < resY - 1; ++j)
            {
                int lvl = face + j * resX;
                for (int i = 0; i < resX - 1; ++i)
                {                    
                    int id1 = cpt * 6 + 1;
                    int id2 = cpt * 6 + 2;
                    if (k != 0)
                    {
                        id1 = cpt * 6 + 2;
                        id2 = cpt * 6 + 1;
                    }

                    tris[cpt * 6] = lvl + i;
                    tris[id1] = lvl + i + resX;
                    tris[id2] = lvl + i + resX + 1;

                    id1 += 3;
                    id2 += 3;                  

                    tris[cpt * 6 + 3] = lvl + i;
                    tris[id1] = lvl + i + resX + 1;
                    tris[id2] = lvl + i + 1;

                    cpt++;
                }                
            }
        }


        
        for (int i = 0; i < 2; ++i)
        {
            int face = i * (resX-1);

            for (int k = 0; k < resZ - 1; ++k)
            {
                int lvl1 = face + k * resX * resY;
                int lvl2 = face + (k + 1) * resX * resY;
                for (int j = 0; j < resY - 1; ++j)
                {
                    int id1 = cpt * 6 + 1;
                    int id2 = cpt * 6 + 2;
                    if (i != 0)
                    {
                        id1 = cpt * 6 + 2;
                        id2 = cpt * 6 + 1;
                    }

                    tris[cpt * 6] = lvl2 + j * resX;
                    tris[id1] = lvl2 + (j + 1) * resX;
                    tris[id2] = lvl1 + (j + 1) * resX;

                    id1 += 3;
                    id2 += 3;

                    tris[cpt * 6 + 3] = lvl2 + j * resX;
                    tris[id1] = lvl1 + (j + 1) * resX;
                    tris[id2] = lvl1 + j * resX;
                    cpt++;
                }
            }
        }
        

        for (int j = 0; j < 2; ++j)
        {
            int face = j * resX * (resY - 1);

            for (int k = 0; k < resZ - 1; ++k)
            {
                int lvl1 = face + k * resX * resY;
                int lvl2 = face + (k + 1) * resX * resY;
                for (int i = 0; i < resX - 1; ++i)
                {
                    int id1 = cpt * 6 + 1;
                    int id2 = cpt * 6 + 2;
                    if (j != 0)
                    {
                        id1 = cpt * 6 + 2;
                        id2 = cpt * 6 + 1;
                    }

                    tris[cpt * 6] = lvl2 + i;
                    tris[id1] = lvl1 + i;
                    tris[id2] = lvl1 + (i + 1);

                    id1 += 3;
                    id2 += 3;

                    tris[cpt * 6 + 3] = lvl2 + i;
                    tris[id1] = lvl1 + (i + 1);
                    tris[id2] = lvl2 + (i + 1);
                    cpt++;
                }
            }
        }


        //int[] tris = new int[nbrIndices*2];
        //for (int i = 0; i < nbrQuads; ++i)
        //{
        //    //Debug.Log(i + " -> " + 
        //    //    quads[i * 4] + " " +
        //    //    quads[i * 4+1] + " " +
        //    //    quads[i * 4+2] + " " +
        //    //    quads[i * 4+3]);

        //    tris[i * 6] = quads[i * 4];
        //    tris[i * 6 + 1] = quads[i * 4 + 1];
        //    tris[i * 6 + 2] = quads[i * 4 + 2];

        //    tris[i * 6 + 3] = quads[i * 4 + 2];
        //    tris[i * 6 + 4] = quads[i * 4 + 3];
        //    tris[i * 6 + 5] = quads[i * 4];

        //    //Debug.Log(i + " -> " +
        //    //    tris[i * 6] + " " +
        //    //    tris[i * 6 + 1] + " " +
        //    //    tris[i * 6 + 2]);

        //    //Debug.Log(i + " -> " +
        //    //    tris[i * 6 + 3] + " " +
        //    //    tris[i * 6 + 4] + " " +
        //    //    tris[i * 6 + 5]);
        //}

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
            //Debug.Log("vertices: " + nbrV);
            Debug.Log("vert: " + mesh.vertices.Length);
            //Debug.Log("normals: " + normals.Length);
            //Debug.Log(vertices.Length);

            float[] vertices = new float[nbrV * 3]; 
            sofaPhysics3DObject_getVertices(m_simu, "truc1_node", vertices);
            float[] normals = new float[nbrV * 3];
            sofaPhysics3DObject_getNormals(m_simu, "truc1_node", normals);

            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            bool first = false;
            if (verts.Length == 0)// first time
            {
                Debug.Log("init");
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
            sofaPhysics3DObject_getQuads(m_simu, "truc1_n   ode", toto);
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
    public static extern int sofaPhysics3DObject_getQuads(IntPtr obj, string name, int [] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTriangles(IntPtr obj, string name, int[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVertices(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNormals(IntPtr obj, string name, float[] arr);


    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysics3DObject_getNbVertices(IntPtr obj);

}
