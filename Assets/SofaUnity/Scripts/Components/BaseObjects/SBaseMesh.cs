using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SBaseMesh : SBaseObject
    {
        /// Mesh of this object
		protected Mesh m_mesh;

        /// Mesh renderer of this object
        //private MeshRenderer m_meshRenderer;

        /// Pointer to the SOFA API object
        protected SofaMeshObject m_impl = null;

        private void Awake()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::Awake");

            loadContext();

            awakePostProcess();
        }


        private void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::start");

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

                initMesh(true);
            }
        }

        private void Update()
        {
            if (m_log)
                Debug.Log("SBaseMesh::Update called.");

            updateImpl();
        }


        /// Method called by \sa Awake after the loadcontext method. To be implemented by child class.
        protected virtual void awakePostProcess()
        {
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();
        }

        /// Method called by \sa Start() method. To be implemented by child class.
        protected virtual void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            m_impl.setTranslation(m_translation);
            m_impl.setRotation(m_rotation);
            m_impl.setScale(m_scale);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }

        /// Method called by \sa Update() method. To be implemented by child class.
        protected virtual void updateImpl()
        {

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
