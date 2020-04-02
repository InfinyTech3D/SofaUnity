using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Base class for a Rigid Mesh, inherite from SofaBaseMesh 
    /// This class will add a meshRenderer and create a SofaMesh API object to load the topology from Sofa Object.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaRigidMesh : SofaBaseMesh
    {
        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_sofaContext.GetSimuContext();
            if (_simu != IntPtr.Zero)
            {
                // Create the API object for SofaMesh
                m_impl = new SofaBaseObjectAPI(_simu, m_uniqueNameId, false);

                // TODO: check if this is still needed (and why not in children)
                m_impl.loadObject();

                // Call SofaBaseMesh.createObject() to init value loaded from the scene.
                base.createObject();
            }

            // Increment Context name counter if object has been created.
            //if (m_impl != null) // TODO: duplicate code with baseObject method?
            //    m_sofaContext.objectcpt = m_sofaContext.objectcpt + 1;
        }

        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void awakePostProcess()
        {
            // Call SofaBaseMesh.awakePostProcess()
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
            {
                mr = gameObject.AddComponent<MeshRenderer>();
                mr.enabled = false;
            }
        }




        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;
            
            // Create the mesh structure.
            m_mesh.name = "SofaRigidMesh";
            m_mesh.vertices = new Vector3[0];
            // TODO: check why 2 updateMesh
            m_impl.updateMesh(m_mesh);
            m_mesh.triangles = m_impl.createTriangulation();
            m_impl.updateMesh(m_mesh);
           // m_mesh.RecalculateNormals();

            base.initMesh(false);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }

        /// Method called by @sa Update() method.
        public override void updateImpl()
        {
            if (m_log)
                Debug.Log("SofaRigidMesh::updateImpl called.");

            if (m_impl != null)
            {
                //m_impl.updateMesh(m_mesh);
             //   m_impl.updateMeshVelocity(m_mesh, m_context.timeStep);
                //m_mesh.RecalculateNormals(); // TODO: not sure it is needed anymore
            }
        }
    }
}
