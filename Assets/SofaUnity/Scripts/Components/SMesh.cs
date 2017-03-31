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
                m_mesh.triangles = m_impl.createTriangulation();
                m_impl.updateMesh(m_mesh);
                m_impl.recomputeTexCoords(m_mesh);

                //initMesh();
            }
        }

        protected virtual void initMesh()
        {
            if (m_impl == null)
                return;

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
                m_impl.updateMesh(m_mesh);
        }
    }
}



