using System;
using UnityEngine;


namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Rigid Box Mesh, inherite from SofaRigidGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Regular Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaRigidBox : SofaGrid
    {
    //    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
    //    protected override void createObject()
    //    {
    //        // Get access to the sofaContext
    //        IntPtr _simu = m_sofaContext.GetSimuContext();
    //        if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh
    //            m_impl = new SofaBoxAPI(_simu, m_uniqueNameId, true);

    //        if (m_impl == null || !m_impl.m_isCreated)
    //        { 
    //            Debug.LogError("SofaRigidBox:: Object creation failed: " + m_uniqueNameId);
    //            this.enabled = false;
    //        }
    //    }
    }
}