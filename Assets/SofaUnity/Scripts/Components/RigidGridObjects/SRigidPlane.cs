using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Rigid Plane Mesh, inherite from SRigidGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Regular Grid Mesh in 2D.
    /// </summary>
    [ExecuteInEditMode]
    public class SRigidPlane : SRigidGrid
    {
        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh in 2D
                m_impl = new SofaPlane(_simu, m_nameId, true);

            if (m_impl == null)
                Debug.LogError("SRigidPlane:: Object creation failed.");
        }

        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            // By default create a large plane
            m_scale = new Vector3(40, 1, 40);

            base.initMesh(false);

            // By default create a plane with Y normal.
            m_gridSize = new Vector3(10, 1, 10);
            m_impl.setGridResolution(m_gridSize);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }


        /// Overwrite Getter/Setter of @see m_gridSize
        public override Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    m_gridSize[1] = 1; // TODO: allow plane in other direction

                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }
    }
}
