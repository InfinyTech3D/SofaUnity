using System;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SRigidPlane : SRigidGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            if (m_impl != null)
                m_context.objectcpt = m_context.objectcpt + 1;
            else
                Debug.LogError("SRigidPlane:: Object not created");

            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
                m_impl = new SofaPlane(_simu, m_context.objectcpt, true);
        }

        protected override void initMesh()
        {
            if (m_impl == null)
                return;

            m_mesh.name = "SofaMesh";
            m_mesh.vertices = new Vector3[0];
            m_impl.updateMesh(m_mesh);
            m_mesh.triangles = m_impl.createTriangulation();
            m_impl.updateMesh(m_mesh);
            m_impl.recomputeTexCoords(m_mesh);

            m_gridSize = new Vector3(10, 1, 10);
        }

        public override Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    m_gridSize[1] = 1;

                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }
        

    }
}
