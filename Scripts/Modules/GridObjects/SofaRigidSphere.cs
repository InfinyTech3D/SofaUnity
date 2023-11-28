using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Rigid Sphere Mesh, inherite from SofaRigidGrid 
    /// This class will create a SofaSphere API object to load the topology from Sofa Sphere Grid Mesh.
    /// </summary>

    [ExecuteInEditMode]
    public class SofaRigidSphere : SofaGrid
    {
        /////////////////////////////////////////////
        //////    SofaRigidSphere API members   /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        protected SofaSphereAPI m_impl = null;



        /////////////////////////////////////////////
        //////  SofaRigidSphere internal API    /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject() method. To create the object when Sofa context has been set.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                m_impl = new SofaSphereAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId, m_parentName, true);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaRigidSphere:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                }
                else
                    m_isCreated = true;
            }
            else
                SofaLog("SofaRigidSphere::Create_impl, SofaSphereAPI already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }
    }
}