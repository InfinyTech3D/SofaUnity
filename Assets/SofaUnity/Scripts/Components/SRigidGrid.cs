using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    public class SRigidGrid : SRigidMesh
    {
        protected override void initMesh()
        {
            if (m_impl == null)
                return;
            
            m_impl.setTranslation(m_translation);
            m_impl.setRotation(m_rotation);
            m_impl.setScale(m_scale);
            m_impl.updateMesh(m_mesh);
            m_impl.setGridResolution(m_gridSize);
        }

        public Vector3 m_gridSize = new Vector3(5, 5, 5);
        public virtual Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }
    }
}
