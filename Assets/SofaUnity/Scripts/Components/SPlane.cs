using System;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SRigidPlane : SBaseGrid
    {
        /// Mesh of this object
        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
                m_impl = new SofaPlane(_simu, m_context.objectcpt, true);
        }

        void init()
        {
            m_gridSize = new Vector3(10, 1, 10);
        }

        public override Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    m_gridSize[1] = 1;

                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (m_log)
                Debug.Log("SRigidPlane::Update called.");

            updateImpl();
        }

    }
}
