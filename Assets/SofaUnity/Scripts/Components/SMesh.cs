using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SMesh : SBaseObject
    {
        protected SofaMeshObject m_impl = null;
        protected SofaContext m_context = null;

        void Awake()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SMesh::Awake");

            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                // get Sofa context
                m_context = _contextObject.GetComponent<SofaContext>();

                // really Create the gameObject linked to sofaObject
                createObject();

                if (m_impl != null)
                    m_context.objectcpt = m_context.objectcpt + 1;
                else
                    Debug.LogError("SMesh:: Object not created");
            }
            else
                Debug.LogError("SMesh::No context.");

            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();
            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();
        }

        protected int nbTetra = 0;
        protected int[] m_tetra;

        void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SMesh::start");

            if (m_impl != null)
            {
#if UNITY_EDITOR
                //Only do this in the editor
                MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
                //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
                Mesh meshCopy = new Mesh();
                m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes

                if (m_log)
                    Debug.Log("SMesh::Start editor mode.");
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
                if (m_log)
                    Debug.Log("SBox::Start play mode.");
#endif

                m_mesh.name = "SofaMesh";
                m_mesh.vertices = new Vector3[0];
                m_impl.updateMesh(m_mesh);
                //m_mesh.triangles = m_impl.createTriangulation();
                m_impl.updateMesh(m_mesh);
                m_impl.recomputeTexCoords(m_mesh);

                m_impl.getTranslation();

                if (nbTetra == 0)
                {
                    nbTetra = m_impl.getNbTetrahedra();
                    if (nbTetra != 0)
                    {
                        m_tetra = new int[nbTetra * 4];

                        m_impl.getTetrahedra(m_tetra);
                        Debug.Log("Tetra: " + nbTetra);
                        Debug.Log("tetra found start: " + m_tetra[0] + " " + m_tetra[1] + " " + m_tetra[2] + " " + m_tetra[3]);

                    }
                }
                    
                //initMesh();
            }
        }


        protected virtual void initMesh()
        {
            if (m_impl == null)
                return;

            m_impl.setTranslation(m_translation);
            m_impl.setRotation(m_rotation);

            m_impl.updateMesh(m_mesh);
        }

        protected virtual void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                m_impl = new SofaMeshObject(_simu, m_context.objectcpt, false);
                m_impl.loadObject();
            }
        }

        void Update()
        {
            if (m_log)
                Debug.Log("SBox::Update called.");

            updateImpl();
        }

        protected virtual void updateImpl()
        {
            if (m_log)
                Debug.Log("SMesh::updateImpl called.");

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
                m_mesh.RecalculateNormals();
            }

            if (m_drawFF)
                drawForceField();
        }


        public Vector3 m_translation;
        public Vector3 translation
        {
            get { return m_translation; }
            set
            {
                if (value != m_translation)
                {
                    Vector3 diffTrans = value - m_translation;
                    Debug.Log("diffTrans: " + diffTrans);
                    m_translation = value;
                    if (m_impl != null)
                    {
                        m_impl.setTranslation(diffTrans);
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

        public Vector3 m_rotation;
        public Vector3 rotation
        {
            get { return m_rotation; }
            set
            {
                if (value != m_rotation)
                {
                    Vector3 diffRot = value - m_rotation;
                    m_rotation = value;
                    if (m_impl != null)
                    {
                        m_impl.setRotation(diffRot);
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

        public bool m_drawFF = false;
        public bool drawFF
        {
            get { return m_drawFF; }
            set { m_drawFF = value;
                Debug.Log("set ff!!");
            }
        }

        //public Material mat;
        //void OnPostRender()
        //{
        //    if (m_drawFF)
        //        drawForceField();
        //}

        public void drawForceField()
        {
            if (m_mesh.vertices.Length == 0)
                return;

            GL.Begin(GL.TRIANGLES);

            for (int i=0; i<nbTetra; ++i)
            {
                Vector3 v0 = m_mesh.vertices[m_tetra[i * 4 + 0]];
                Vector3 v1 = m_mesh.vertices[m_tetra[i * 4 + 1]];
                Vector3 v2 = m_mesh.vertices[m_tetra[i * 4 + 2]];
                Vector3 v3 = m_mesh.vertices[m_tetra[i * 4 + 3]];

             //   Debug.Log("v0: " + v0 + " v1: " + v1 );

                GL.Vertex3(v0.x, v0.y, v0.z);
                GL.Vertex3(v1.x, v1.y, v1.z);
                GL.Vertex3(v2.x, v2.y, v2.z);

                GL.Vertex3(v1.x, v1.y, v1.z);
                GL.Vertex3(v2.x, v2.y, v2.z);
                GL.Vertex3(v3.x, v3.y, v3.z);

                GL.Vertex3(v2.x, v2.y, v2.z);
                GL.Vertex3(v3.x, v3.y, v3.z);
                GL.Vertex3(v0.x, v0.y, v0.z);

                GL.Vertex3(v3.x, v3.y, v3.z);
                GL.Vertex3(v0.x, v0.y, v0.z);
                GL.Vertex3(v1.x, v1.y, v1.z);
            }

            GL.End();            
        }
    }
}



