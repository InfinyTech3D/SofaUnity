using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBox : SofaMeshObject
{ 

    public SofaBox(IntPtr simu, string name, bool isRigid)
        : base (simu, name, isRigid)        
    {
        //m_simu = simu;
    }

    ~SofaBox()
    {
        Dispose(false);
    }


    public override int[] createTriangulation()
    {        
        int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, m_name);

        //Debug.Log("NbrQuad: " + nbrQuads);
        int nbrIndices = nbrQuads * 4;
        int[] quads = new int[nbrIndices];
        sofaPhysics3DObject_getQuads(m_simu, m_name, quads);

        int resX = 5;
        int resY = 5;
        int resZ = 5;

        int nbrTri = (resX - 1) * (resY - 1) * 4 + (resX - 1) * (resZ - 1) * 4 + (resY - 1) * (resZ - 1) * 4;
        //int nbrTri = 2;
        int[] tris = new int[nbrTri*3];

        //Debug.Log("nbrTri: " + (resX - 1) * (resY - 1) * 2 * 3);
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
        
        return tris;
    }


    public override void recomputeTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        Vector2[] uvs = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            if (verts[i].z == 0.5 || verts[i].z == -0.5)
                uvs[i] = new Vector2(1-(verts[i].x + 0.5f), (verts[i].y + 0.5f));

            else if (verts[i].x == 0.5 || verts[i].x == -0.5) // along X fix
                uvs[i] = new Vector2(1-(verts[i].z + 0.5f), verts[i].y + 0.5f);
            else if (verts[i].y == 0.5 || verts[i].y == -0.5)
                uvs[i] = new Vector2(1 - (verts[i].x + 0.5f), (verts[i].y + 0.5f));

            //uvs[i] = new Vector2(verts[i].y + 0.5f, verts[i].z + 0.5f);
            /*            if (verts[i].x == 0.5 || verts[i].x == -0.5) // along X fix
                            uvs[i] = new Vector2(verts[i].y + 0.5f, verts[i].z + 0.5f);
                        else if (verts[i].y == 0.5 || verts[i].y == -0.5)
                            uvs[i] = new Vector2(verts[i].x + 0.5f, verts[i].z + 0.5f);
                        else if (verts[i].z == 0.5 || verts[i].z == -0.5)
                            uvs[i] = new Vector2(verts[i].x + 0.5f, verts[i].y + 0.5f);
                            */
        }

        mesh.uv = uvs;
    }


    protected override void createObject()
    {
        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the cube
           /* int res = sofaPhysicsAPI_addCube(m_simu, m_name, m_isRigid);

            if (res == 1) // cube added
            {
                Debug.Log("cube Added! " + m_name);

                // Set created object to native pointer
               // m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            //    m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1");

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Cube created can't be found!");
                */
        }
    }

    
    public void test()
    {
        Debug.Log("sofa_test1: " + sofaPhysicsAPI_getNumberObjects(m_simu));
        Debug.Log("NAME: " + sofaPhysicsAPI_APIName(m_simu));
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);
            int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, m_name);

            Debug.Log("NbrV: " + nbrV);
            Debug.Log("NbrTri: " + sofaPhysics3DObject_getNbTriangles(m_simu, m_name));

            Debug.Log("NbrQuad: " + nbrQuads);
            int[] toto = new int[nbrQuads];
            sofaPhysics3DObject_getQuads(m_simu, "truc1_n   ode", toto);
            for (int i = 0; i < 10; ++i)
            {
                Debug.Log(i + " => " + toto[i]);
            }


            /*  var quads = sofaPhysics3DObject_getQuads(m_simu, m_name);
              var vertices = sofaPhysics3DObject_getVertices(m_simu, m_name);
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

    
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addCube(IntPtr obj, string name, bool isRigid);


    //[DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    //public static extern IntPtr sofaPhysicsAPI_getOutputMesh(IntPtr obj, string name);

    //[DllImport("SofaAdvancePhysicsAPI")]
    //public static extern IntPtr sofaOutputMesh_getVertices(IntPtr obj);

    
    //[DllImport("SofaAdvancePhysicsAPI")]
    //public static extern int sofaPhysics3DObject_getNbVertices(IntPtr obj);

}
