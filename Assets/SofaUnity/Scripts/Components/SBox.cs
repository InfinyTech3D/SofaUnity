using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    public class SBox : SBaseGrid
    {
        internal SofaBox m_impl;
        public GameObject m_context;

        /// Mesh of this object
		//private Mesh m_mesh;

        void Awake()
        {
            Debug.Log("SBox::Awake called.");
            m_context = GameObject.Find("SofaContext");
            if (m_context != null)
            {
                SofaContext context = m_context.GetComponent<SofaContext>();
                IntPtr _simu = context.getSimuContext();
                if (_simu != IntPtr.Zero)
                {
                    m_impl = new SofaBox(_simu);
                    if (m_impl != null)
                        m_impl.addCube();
                }
            }
            else
                Debug.Log("SBox::No context.");
                  
        }
    

        // Use this for initialization
        void Start()
        {
            Debug.Log("SBox::Start called.");
            //add mesh fileter to this GameObject
            MeshFilter mf = gameObject.AddComponent<MeshFilter>();
            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            //set the mesh filter's mesh to our mesh
            m_mesh = mf.mesh;

            m_mesh.name = "IMadeThis";

            //// create point data, [3] for 3 verticies of a simple triangle
            //Vector3[] verts = new Vector3[3];
            //Vector3[] norms = new Vector3[3];
            //Vector2[] uvs = new Vector2[3];
            int[] tris = new int[3];

            ////create points for a triangle that is arrowhead like pointing at 0,0,0
            ////verts[0] = new Vector3(0, 0, 0);
            ////verts[1] = new Vector3(1, 1, 1);
            ////verts[2] = new Vector3(-1, 1, 1);
            ////simple normals for now, they all just point up
            //norms[0] = new Vector3(0, 1, 1);
            //norms[1] = new Vector3(0, 1, 1);
            //norms[2] = new Vector3(0, 1, 1);
            ////simple UVs, traingle drawn in a texture
            //uvs[0] = new Vector2(-1, 0.5f);
            //uvs[1] = new Vector2(-1, 1);
            //uvs[2] = new Vector2(1, 1);
            //create the triangle
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
                //m_mesh.triangles = tris; 
                m_mesh.triangles = m_impl.createTriangulation(m_mesh.vertices.Length);

                m_impl.createQuads();
                m_mesh.RecalculateNormals();
            }

            //set the mesh up
            // m_mesh.vertices = verts;
            //            m_mesh.normals = norms;
            //          m_mesh.uv = uvs;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (m_impl != null)
                    m_impl.test();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                if (m_impl != null)
                    m_impl.test();
            }

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
            }

                /*Vector3[] verts = m_mesh.vertices;
                for (int i = 0; i < verts.Length; i++) {
                    verts[i].z += UnityEngine.Random.value-0.5f;
                }
                */
                //m_mesh.vertices = verts;
            }


    }
}
