using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SBox : SBaseGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
                m_impl = new SofaBox(_simu, m_context.objectcpt, false);
        }

        void init()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            if (m_log)
                Debug.Log("SBox::Update called.");

            updateImpl();
        }

    }


    [ExecuteInEditMode]
    public class SRigidBox : SBaseGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
                m_impl = new SofaBox(_simu, m_context.objectcpt, true);
        }

        void init()
        {

        }


        // Update is called once per frame
        void Update()
        {
            if (m_log)
                Debug.Log("SRigidBox::Update called.");

            //updateImpl();
        }

    }
}
