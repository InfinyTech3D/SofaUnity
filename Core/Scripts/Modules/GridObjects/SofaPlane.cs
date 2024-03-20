using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Plane Mesh, inherite from SofaGrid 
    /// This class will create a SofaPlane API object to load the topology from Sofa Regular Grid Mesh in 2D.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaPlane : SofaGrid
    {
        /////////////////////////////////////////////
        //////      SofaPlane API members       /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        protected SofaPlaneAPI m_impl = null;



        /////////////////////////////////////////////
        //////     SofaPlane internal API       /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject() method. To create the object when Sofa context has been set.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                m_impl = new SofaPlaneAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId, m_parentName, false);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaPlane:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                }
                else
                    m_isCreated = true;
            }
            else
                SofaLog("SofaPlane::Create_impl, SofaPlaneAPI already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }
    }
}
