using UnityEngine;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Visual Mesh, inherite from SBaseMesh 
    /// This class will add a meshRenderer and create a SofaMesh API object to load the topology from Sofa Object.
    /// </summary>
    [ExecuteInEditMode]
    class SVisualMesh : SBaseMesh
    {
        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                // Create the API object for SofaMesh
                m_impl = new SofaMesh(_simu, m_nameId, false);

                // TODO: check if this is still needed (and why not in children)
                m_impl.loadObject();

                // Call SBaseMesh.createObject() to init value loaded from the scene.// Set init value loaded from the scene.
                base.createObject();
            }

            if (m_impl == null)
                sofaLog("SVisualMesh:: Object creation failed.", 2);
        }


        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void awakePostProcess()
        {
            // Call SBaseMesh.awakePostProcess()
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();

            if (mr.sharedMaterial == null)
            {
                    mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }

            gameObject.AddComponent<MeshCollider>();
        }

        Material m_currentMaterial = null;
        public bool m_isWireframe = false;
        public void ShowWireframe(bool value)
        {
            Debug.Log("ShowWireframe: " + value);
            if (value)
            {
                Material wireMaterial = (Material)Resources.Load("Wireframe", typeof(Material));
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    if (m_currentMaterial == null)
                        m_currentMaterial = mr.sharedMaterial;
                    mr.sharedMaterial = wireMaterial;
                    m_isWireframe = true;
                }
            }
            else
            {
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    if (m_currentMaterial != null)
                        mr.sharedMaterial = m_currentMaterial;
                    else
                        mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
                    m_isWireframe = false;
                }
            }
        }

        public bool m_isSelected = false;
        private void OnTriggerEnter(Collider other)
        {
           // if (enter)
            {
                m_isSelected = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            m_isSelected = false;
        }

        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

            /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;            

            m_mesh.name = "SofaVisualMesh";
            m_mesh.vertices = new Vector3[0];
            m_impl.updateMesh(m_mesh);
            m_mesh.triangles = m_impl.createTriangulation();            
            m_impl.recomputeTexCoords(m_mesh);

            base.initMesh(false);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }


        /// Method called by @sa Update() method.
        public override void updateImpl()
        {
            sofaLog("SVisualMesh::updateImpl called.");

            if (m_impl != null)
            {
                if (m_impl.hasTopologyChanged())
                {
                    m_mesh.triangles = m_impl.createTriangulation();
                    if (m_invertNormals)
                    {
                        m_impl.m_invertNormals = m_invertNormals;
                        invertMeshNormals();
                    }
                    m_impl.setTopologyChange(false);
                    m_impl.updateMesh(m_mesh);
                    m_mesh.RecalculateNormals();                    
                }
                else
                {
                    int res = m_impl.updateMeshVelocity(m_mesh, m_context.timeStep);
                    if (res == -1)
                        m_context.breakerProcedure();
                }
                m_mesh.RecalculateBounds();
            }
            
        }
    }
}
