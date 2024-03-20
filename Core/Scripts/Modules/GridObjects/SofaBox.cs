using UnityEngine;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Box Mesh, inherite from SofaGrid 
    /// This class will create a SofaBox API object to load the topology from Sofa Regular Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaBox : SofaGrid
    {
        /////////////////////////////////////////////
        //////        SofaBox API members       /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        protected SofaBoxAPI m_impl = null;



        /////////////////////////////////////////////
        //////      SofaBox internal API        /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject() method. To create the object when Sofa context has been set.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                m_impl = new SofaBoxAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId, m_parentName, false);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaBox:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                }
                else
                    m_isCreated = true;
            }
            else
                SofaLog("SofaBox::Create_impl, SofaBoxAPI already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }
    }
}
