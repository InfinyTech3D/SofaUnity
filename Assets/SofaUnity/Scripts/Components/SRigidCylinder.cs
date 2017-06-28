using System;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SRigidCylinder : SRigidGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
                m_impl = new SofaCylinder(_simu, m_context.objectcpt, true);

            if (m_impl == null)
                Debug.LogError("SRigidCylinder:: Object not created");
        }
        
    }
}
