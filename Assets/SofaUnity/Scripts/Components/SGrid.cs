using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SGrid : SDeformableMesh
    {
        protected bool m_useTex = true;

        private void Awake()
        {
#if UNITY_EDITOR
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::Awake");

            loadContext();

            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
            {
                mr = gameObject.AddComponent<MeshRenderer>();
                mr.material = new Material(Shader.Find("Diffuse"));

                if (this.m_useTex)
                    mr.material = Resources.Load("Materials/BoxSofa") as Material;
            }

#else
            Debug.Log("UNITY_PLAY - SBox::Awake called.");
#endif
        }

        private void Start()
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

                if (m_log)
                    Debug.Log("SBaseGrid::Start editor mode.");
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
                if (m_log)
                    Debug.Log("SBox::Start play mode.");
#endif


                m_mesh.name = "SofaGrid";
                m_mesh.vertices = new Vector3[0];
                m_impl.updateMesh(m_mesh);
                m_mesh.triangles = m_impl.createTriangulation();
                m_impl.updateMesh(m_mesh);
                m_impl.recomputeTriangles(m_mesh);
                m_impl.recomputeTexCoords(m_mesh);

                initMesh();
            }
        }

        protected override void initMesh()
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
            m_impl.setGridResolution(m_gridSize);
        }


        protected override void updateImpl()
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

    }    
}
