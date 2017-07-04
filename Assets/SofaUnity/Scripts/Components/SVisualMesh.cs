using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    class SVisualMesh : SBaseMesh
    {

        protected override void awakePostProcess()
        {
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();
        }

        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            base.initMesh(false);

            m_mesh.name = "SofaVisualMesh";
            m_mesh.vertices = new Vector3[0];
            m_impl.updateMesh(m_mesh);
            m_mesh.triangles = m_impl.createTriangulation();            
            m_impl.recomputeTexCoords(m_mesh);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }

        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                m_impl = new SofaMesh(_simu, m_nameId, false);                
                m_impl.loadObject();
            }

            if (m_impl == null)
                Debug.LogError("SVisualMesh:: Object not created");
        }

        protected override void updateImpl()
        {
            if (m_log)
                Debug.Log("SVisualMesh::updateImpl called.");

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
                //m_mesh.RecalculateNormals();
            }
        }
    }
}
