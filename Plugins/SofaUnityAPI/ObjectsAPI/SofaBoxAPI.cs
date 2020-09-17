using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class used to handle bindings to the Sofa Cube object, using a RegularGrid.
/// </summary>
public class SofaBoxAPI : SofaBaseObjectAPI
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaBoxAPI(IntPtr simu, string nameID, string parentName, bool isRigid) 
        : base (simu, nameID, parentName, isRigid)        
    {

    }

    /// Destructor
    ~SofaBoxAPI()
    {
        Dispose(false);
    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override bool createObject()
    {
        if (m_hasObject == false) // first time create object only
        {
            // Create the cube
            int res = sofaPhysicsAPI_addCube(m_simu, m_name, m_parentName, m_isRigid);
            m_name += "_node";

            if (res != 0)
            {
                Debug.LogError("SofaBoxAPI::createObject cube creation method return error: " + SofaDefines.msg_error[res] + " for object " + m_name);
                return false;
            }

            if(displayLog)
                Debug.Log("cube Added! " + m_name);

            m_hasObject = true;   
            return true;
        }

        return false;
    }


    /// Method to create the triangulation from Sofa topology to Unity buffers
    public override int[] createTriangulation()
    {
        //if (!m_hasObject)
        //{
            return new int[0];
        //}

        //int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, m_name);

        //if (nbrQuads < 0)
        //{
        //    Debug.LogError("createTriangulation failed, method returns: " + SofaDefines.msg_error[nbrQuads]);
        //    return new int[0];
        //}

        ////Debug.Log("NbrQuad: " + nbrQuads);
        //int nbrIndices = nbrQuads * 4;
        //int[] quads = new int[nbrIndices];
        //sofaPhysics3DObject_getQuads(m_simu, m_name, quads);

        //int resX = 5;
        //int resY = 5;
        //int resZ = 5;

        //int nbrTri = (resX - 1) * (resY - 1) * 4 + (resX - 1) * (resZ - 1) * 4 + (resY - 1) * (resZ - 1) * 4;
        ////int nbrTri = 2;
        //int[] tris = new int[nbrTri*3];

        ////Debug.Log("nbrTri: " + (resX - 1) * (resY - 1) * 2 * 3);
        //int cpt = 0;
        //for (int k = 0; k < 2; ++k)
        //{
        //    int face = k * resX * resY * (resZ - 1);
        //    for (int j = 0; j < resY - 1; ++j)
        //    {
        //        int lvl = face + j * resX;
        //        for (int i = 0; i < resX - 1; ++i)
        //        {                    
        //            int id1 = cpt * 6 + 1;
        //            int id2 = cpt * 6 + 2;
        //            if (k != 0)
        //            {
        //                id1 = cpt * 6 + 2;
        //                id2 = cpt * 6 + 1;
        //            }

        //            tris[cpt * 6] = lvl + i;
        //            tris[id1] = lvl + i + resX;
        //            tris[id2] = lvl + i + resX + 1;

        //            id1 += 3;
        //            id2 += 3;                  

        //            tris[cpt * 6 + 3] = lvl + i;
        //            tris[id1] = lvl + i + resX + 1;
        //            tris[id2] = lvl + i + 1;

        //            cpt++;
        //        }                
        //    }
        //}

        
        //for (int i = 0; i < 2; ++i)
        //{
        //    int face = i * (resX-1);

        //    for (int k = 0; k < resZ - 1; ++k)
        //    {
        //        int lvl1 = face + k * resX * resY;
        //        int lvl2 = face + (k + 1) * resX * resY;
        //        for (int j = 0; j < resY - 1; ++j)
        //        {
        //            int id1 = cpt * 6 + 1;
        //            int id2 = cpt * 6 + 2;
        //            if (i != 0)
        //            {
        //                id1 = cpt * 6 + 2;
        //                id2 = cpt * 6 + 1;
        //            }

        //            tris[cpt * 6] = lvl2 + j * resX;
        //            tris[id1] = lvl2 + (j + 1) * resX;
        //            tris[id2] = lvl1 + (j + 1) * resX;

        //            id1 += 3;
        //            id2 += 3;

        //            tris[cpt * 6 + 3] = lvl2 + j * resX;
        //            tris[id1] = lvl1 + (j + 1) * resX;
        //            tris[id2] = lvl1 + j * resX;
        //            cpt++;
        //        }
        //    }
        //}
        

        //for (int j = 0; j < 2; ++j)
        //{
        //    int face = j * resX * (resY - 1);

        //    for (int k = 0; k < resZ - 1; ++k)
        //    {
        //        int lvl1 = face + k * resX * resY;
        //        int lvl2 = face + (k + 1) * resX * resY;
        //        for (int i = 0; i < resX - 1; ++i)
        //        {
        //            int id1 = cpt * 6 + 1;
        //            int id2 = cpt * 6 + 2;
        //            if (j != 0)
        //            {
        //                id1 = cpt * 6 + 2;
        //                id2 = cpt * 6 + 1;
        //            }

        //            tris[cpt * 6] = lvl2 + i;
        //            tris[id1] = lvl1 + i;
        //            tris[id2] = lvl1 + (i + 1);

        //            id1 += 3;
        //            id2 += 3;

        //            tris[cpt * 6 + 3] = lvl2 + i;
        //            tris[id1] = lvl1 + (i + 1);
        //            tris[id2] = lvl2 + (i + 1);
        //            cpt++;
        //        }
        //    }
        //}
        
        //return tris;
    }


    /// Method to recompute the Tex coords according to mesh position and geometry.
    public override void recomputeTexCoords(Mesh mesh)
    {
        //Vector3[] verts = mesh.vertices;
        //Vector2[] uvs = new Vector2[verts.Length];

        //this.computeBoundingBox(mesh);

        //float rangeX = 1 / (m_max.x - m_min.x);
        //float rangeY = 1 / (m_max.y - m_min.y);
        //float rangeZ = 1 / (m_max.z - m_min.z);

        //for (int i = 0; i < verts.Length; i++)
        //{
        //    if (verts[i].z == m_max.z)
        //        uvs[i] = new Vector2((verts[i].x - m_min.x) * rangeX, (verts[i].y - m_min.y) * rangeY);
        //    else if (verts[i].z == m_min.z)
        //        uvs[i] = new Vector2((m_max.x - verts[i].x) * rangeX, (verts[i].y - m_min.y) * rangeY);
        //    else if (verts[i].x == m_max.x)
        //        uvs[i] = new Vector2((verts[i].z - m_min.z) * rangeZ, (verts[i].y - m_min.y) * rangeY);
        //    else if (verts[i].x == m_min.x)
        //        uvs[i] = new Vector2((m_max.z - verts[i].z) * rangeZ, (verts[i].y - m_min.y) * rangeY);
        //    else if (verts[i].y == m_max.y)
        //        uvs[i] = new Vector2((m_max.x - verts[i].x) * rangeX, (verts[i].z - m_min.z) * rangeZ);
        //    else if (verts[i].y == m_min.y)
        //        uvs[i] = new Vector2((verts[i].x - m_min.x) * rangeX, (verts[i].z - m_min.z) * rangeZ);
        //}

        //mesh.uv = uvs;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addCube(IntPtr obj, string nodeName, string parentNodeName, bool isRigid);

}
