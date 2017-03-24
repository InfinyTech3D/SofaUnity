using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SBaseGrid : SBaseObject
    {
        protected SofaMeshObject m_impl = null;
        protected SofaContext m_context = null;        

        void Awake()
        {
#if UNITY_EDITOR
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseGrid::Awake");

            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                // get Sofa context
                m_context = _contextObject.GetComponent<SofaContext>();
                this.gameObject.transform.parent = _contextObject.transform;

                // really Create the gameObject linked to sofaObject
                createObject();

                if (m_impl != null)
                    m_context.objectcpt = m_context.objectcpt + 1;
                else
                    Debug.LogError("SBaseGrid:: Object not created");
            }
            else
                Debug.LogError("SBaseGrid::No context.");

            //createFakeCube();
            //MeshFilter mf = gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshFilter>();
            //to see it, we have to add a renderer
            //MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshRenderer>();
#else
            Debug.Log("UNITY_PLAY - SBox::Awake called.");
#endif
        }

        void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseGrid::start");

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

                if (m_log)
                    Debug.Log("SBaseGrid::Start editor mode.");
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
                if (m_log)
                    Debug.Log("SBox::Start play mode.");
#endif

                m_mesh.name = "IMadeThis";
                m_mesh.vertices = new Vector3[0];
                m_impl.updateMesh(m_mesh);
                m_mesh.triangles = m_impl.createTriangulation();
                m_impl.updateMesh(m_mesh);
                m_impl.recomputeTriangles(m_mesh);

                initMesh();
            }
        }

        protected virtual void createObject()
        {
            m_impl = null;
        }

        protected virtual void initMesh()
        {
            if (m_impl == null)
                return;            

            m_impl.setMass(m_mass);
            m_impl.setYoungModulus(m_young);
            m_impl.setPoissonRatio(m_poisson);

            m_impl.setTranslation(m_translation);
            m_impl.setRotation(m_rotation);
            m_impl.setScale(m_scale);
            m_impl.updateMesh(m_mesh);
        }


        protected virtual void updateImpl()
        {
            if (m_log)
                Debug.Log("SBaseGrid::updateImpl called.");

            if (m_impl != null) {
                m_impl.updateMesh(m_mesh);
                //m_mesh.RecalculateNormals();
                    }


        }



        public Vector3 m_gridSize = new Vector3(5, 5, 5);
        public virtual Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }


        public float m_mass = 10.0f;
        public float mass
        {
            get { return m_mass; }
            set
            {
                if (value != m_mass)
                {
                    m_mass = value;
                    if (m_impl != null)
                        m_impl.setMass(m_mass);
                }
            }
        }


        public float m_young = 800.0f;
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

        public float m_poisson = 0.45f;
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

        public Vector3 m_translation;
        public Vector3 translation
        {
            get { return m_translation; }
            set
            {
                if (value != m_translation)
                {
                    Vector3 diffTrans = value - m_translation;
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

        public Vector3 m_scale = new Vector3(1.0f, 1.0f, 1.0f);
        public Vector3 scale
        {
            get { return m_scale; }
            set
            {
                if (value != m_scale)
                {
                    Vector3 diffScale = value - m_scale;
                    m_scale = value;
                    if (m_impl != null)
                    {
                        m_impl.setScale(diffScale);
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

    }    
}
