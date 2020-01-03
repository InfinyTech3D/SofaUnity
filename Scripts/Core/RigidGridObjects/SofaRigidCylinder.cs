using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Rigid Cylinder Mesh, inherite from SRigidGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Cylinder Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SRigidCylinder : SRigidGrid
    {
        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero) // Create the API object for Sofa Cylinder Grid Mesh
                m_impl = new SofaCylinderAPI(_simu, m_nameId, true);

            if (m_impl == null || !m_impl.m_isCreated)
            {
                Debug.LogError("SRigidCylinder:: Object creation failed: " + m_nameId);
                this.enabled = false;
            }
        }        
    }
}
