using System;
using UnityEngine;


namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Rigid Box Mesh, inherite from SRigidGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Regular Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SRigidBox : SRigidGrid
    {
        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh
                m_impl = new SofaBoxAPI(_simu, m_nameId, true);

            if (m_impl == null || !m_impl.m_isCreated)
            { 
                Debug.LogError("SRigidBox:: Object creation failed: " + m_nameId);
                this.enabled = false;
            }
        }
    }
}