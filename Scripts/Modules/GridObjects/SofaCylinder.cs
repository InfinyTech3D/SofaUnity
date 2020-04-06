using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Cylinder Mesh, inherite from SofaGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Cylinder Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaCylinder : SofaGrid
    {
        ///// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        //protected override void createObject()
        //{
        //    // Get access to the sofaContext
        //    IntPtr _simu = m_sofaContext.GetSimuContext();
        //    if (_simu != IntPtr.Zero) // Create the API object for Sofa Cylinder Grid Mesh
        //        m_impl = new SofaCylinderAPI(_simu, m_uniqueNameId, false);

        //    if (m_impl == null || !m_impl.m_isCreated)
        //    {
        //        Debug.LogError("SofaCylinder:: Object creation failed: " + m_uniqueNameId);
        //        this.enabled = false;
        //    }
        //}

        //// Update is called once per frame
        //public override void updateImpl()
        //{
        //    if (m_log)
        //        Debug.Log("SofaCylinder::updateImpl called.");

        //    if (m_impl != null)
        //    {
        //        // TODO: need to find why velocity doesn't work for grid
        //        //m_impl.updateMeshVelocity(m_mesh, m_context.timeStep);
        //        m_impl.updateMesh(m_mesh);
        //        m_mesh.RecalculateBounds();
        //        m_mesh.RecalculateNormals(); // TODO check if needed
        //    }
        //}
    }
}
