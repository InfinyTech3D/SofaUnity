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
        internal SofaBox m_impl;
        public GameObject m_context;
        internal Vector3 m_translation;
        internal Vector3 m_rotation;
        internal Vector3 m_scale;


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
                    m_impl = new SofaBox(_simu, context.objectcpt);
                    if (m_impl != null)
                    {
                        m_impl.addCube();
                        context.objectcpt = context.objectcpt + 1;
                    }
                }
            }
            else
                Debug.Log("SBox::No context.");

            //createFakeCube();
            MeshFilter mf = gameObject.AddComponent<MeshFilter>();
            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        }


        // Use this for initialization
        void Start()
        {
            Debug.Log("SBox::Start called.");

            if (m_impl != null)
            {
#if UNITY_EDITOR
                //Only do this in the editor
                MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
                //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
                Mesh meshCopy = new Mesh();
                m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes
                MeshRenderer mr = GetComponent<MeshRenderer>();
                mr.material = new Material(Shader.Find("Diffuse"));
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
#endif

                m_mesh.name = "IMadeThis";                

                m_mesh.vertices = new Vector3[0];
                m_impl.updateMesh(m_mesh);
                //m_mesh.triangles = tris; 
                m_mesh.triangles = m_impl.createTriangulation(m_mesh.vertices.Length);
                m_impl.updateMesh(m_mesh);
                //m_mesh.RecalculateNormals();
            }
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("SBox::Update called.");

            if (transform.hasChanged)
                updateTransform();
    
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

        float m_mass = 1.0f;
        public float mass
        {
            get { return m_mass; }
            set {
                if (value != m_mass)
                {
                    m_mass = value;
                    if (m_impl != null)
                        m_impl.setMass(m_mass);
                }
            }
        }

        float m_young = 300.0f;
        public float young
        {
            get { return m_young; }
            set
            {
                if (value != m_young)
                {
                    m_young = value;
                    if (m_impl != null)
                        m_impl.setYoungModulus(m_young);
                }
            }
        }

        float m_poisson = 1.0f;
        public float poisson
        {
            get { return m_poisson; }
            set
            {
                if (value != m_poisson)
                {
                    m_poisson = value;
                    if (m_impl != null)
                        m_impl.setPoissonRatio(m_poisson);
                }
            }
        }

        void updateTransform()
        {
            transform.hasChanged = false;

            if (m_impl == null)
                return;

            if (transform.position != m_translation)
            {
                m_translation = transform.position;
                Debug.Log("Update pos");
                Debug.Log(m_translation);
                m_impl.setTranslation(m_translation);
            }

            //if (transform.rotation != m_rotation)
            //{
            //    m_rotation = transform.rotation;
            //    m_impl.setRotation(m_rotation);
            //}

            if (transform.localScale != m_scale)
            {
                m_scale = transform.localScale;
                m_impl.setScale(m_scale);
            }
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
