using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SBox : SGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            Debug.Log("SBox::createObject: " + m_nameId);
            
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                Debug.Log("SBox::createObject: " + m_nameId);
                m_impl = new SofaBox(_simu, m_nameId, false);
            }

            this.m_useTex = true;
        }

        // Update is called once per frame
        protected override void updateImpl()
        {
            if (m_log)
                Debug.Log("SBox::updateImpl called.");

            if (m_impl != null)
            {
                m_impl.updateMesh(m_mesh);
                m_mesh.RecalculateNormals();
            }
        }

    }    
}
