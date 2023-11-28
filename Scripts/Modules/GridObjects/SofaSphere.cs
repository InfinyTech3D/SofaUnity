using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Deformable Sphere Mesh, inherite from SofaGrid 
    /// This class will create a SofaSphere API object to load the topology from Sofa Sphere Grid Mesh.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaSphere : SofaGrid
    {
        /////////////////////////////////////////////
        //////      SofaSphere API members      /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        protected SofaSphereAPI m_impl = null;



        /////////////////////////////////////////////
        //////     SofaSphere internal API      /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject() method. To create the object when Sofa context has been set.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                m_impl = new SofaSphereAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId, m_parentName, false);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaSphere:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                }
                else
                    m_isCreated = true;
            }
            else
                SofaLog("SofaSphere::Create_impl, SofaSphereAPI already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }
    }
}
