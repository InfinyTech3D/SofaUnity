using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SBox : SBaseGrid
    {        
        /// Mesh of this object
		//private Mesh m_mesh;

        void init()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            Debug.Log("SBox::Update called.");

            /*Vector3[] verts = m_mesh.vertices;
            for (int i = 0; i < verts.Length; i++) {
                verts[i].z += UnityEngine.Random.value-0.5f;
            }
            */
            //m_mesh.vertices = verts;
            updateImpl();
        }

        void createFakeCube()
        {
            Vector3[] verts = new Vector3[8];
            Vector3[] norms = new Vector3[8];
            Vector2[] uvs = new Vector2[8];
            int[] tris = new int[12];

            //create points for a triangle that is arrowhead like pointing at 0,0,0
            verts[0] = new Vector3(0, 0, 0);
            verts[1] = new Vector3(1, 0, 0);
            verts[2] = new Vector3(0, 1, 0);
            verts[3] = new Vector3(1, 1, 0);

            verts[4] = new Vector3(0, 0, 1);
            verts[5] = new Vector3(1, 0, 1);
            verts[6] = new Vector3(0, 1, 1);
            verts[7] = new Vector3(1, 1, 1);


            //simple normals for now, they all just point up
            norms[0] = new Vector3(0, 1, 1);
            norms[1] = new Vector3(0, 1, 1);
            norms[2] = new Vector3(0, 1, 1);
            //simple UVs, traingle drawn in a texture
            uvs[0] = new Vector2(-1, 0.5f);
            uvs[1] = new Vector2(-1, 1);
            uvs[2] = new Vector2(1, 1);
            //create the triangle
            for (int i=0; i<3; ++i)
                tris[i] = i;

            tris[3] = 0;
            tris[4] = 3;
            tris[5] = 2;

            tris[6] = 4;
            tris[7] = 5;
            tris[8] = 6;

            tris[9] = 5;
            tris[10] = 6;
            tris[11] = 7;



            m_mesh.vertices = verts;
            //m_mesh.normals = norms;
            //m_mesh.uv = uvs;
            m_mesh.triangles = tris;
        }


    }
}
