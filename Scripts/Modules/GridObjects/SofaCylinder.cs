using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Cylinder Mesh, inherite from SofaGrid 
    /// This class will create a SofaCylinder API object to load the topology from Sofa Cylinder Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaCylinder : SofaGrid
    {
        /////////////////////////////////////////////
        //////     SofaCylinder API members     /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        protected SofaCylinderAPI m_impl = null;



        /////////////////////////////////////////////
        //////    SofaCylinder internal API     /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject() method. To create the object when Sofa context has been set.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                m_impl = new SofaCylinderAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId, m_parentName, false);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaCylinder:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                }
                else
                    m_isCreated = true;
            }
            else
                SofaLog("SofaCylinder::Create_impl, SofaCylinderAPI already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }
    }
}
