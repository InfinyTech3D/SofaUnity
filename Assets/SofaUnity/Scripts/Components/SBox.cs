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
                m_impl = new SofaBox(_simu, m_context.objectcpt);
        }

        void init()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            Debug.Log("SBox::Update called.");

            /*Vector3[] verts = m_mesh.vertices;
            for (int i = 0; i < verts.Length; i++) {
                verts[i].z += UnityEngine.Random.value-0.5f;
            }
            */
            //m_mesh.vertices = verts;
            updateImpl();
        }

    }
}
