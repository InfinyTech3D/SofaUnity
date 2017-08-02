using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Cylinder Mesh, inherite from SGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Cylinder Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SCylinder : SGrid
    {
        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero) // Create the API object for Sofa Cylinder Grid Mesh
                m_impl = new SofaCylinder(_simu, m_nameId, false);

            if (m_impl == null)
                Debug.LogError("SCylinder:: Object creation failed.");
        }

        // Update is called once per frame
        protected override void updateImpl()
        {
            if (m_log)
                Debug.Log("SCylinder::updateImpl called.");

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
                m_mesh.RecalculateNormals(); // TODO check if needed
            }
        }
    }
}
