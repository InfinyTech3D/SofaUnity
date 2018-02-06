using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SComponentObject : SBaseObject
    {
        /// Pointer to the corresponding SOFA API object
        protected SofaComponent m_impl = null;


        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                // Create the API object for SofaComponent
                m_impl = new SofaComponent(_simu, m_nameId, false);

                if (m_impl == null)
                {
                    Debug.LogError("SofaComponent:: Object creation failed.");
                    return;
                }

                m_impl.loadObject();

                // Call SBaseMesh.createObject() to init value loaded from the scene.
                base.createObject();
            }
        }

        /// Method called by \sa Awake after the loadcontext method.
        protected override void awakePostProcess()
        {

        }

        /// Method called at GameObject init (after creation or when starting play).
        void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SComponentObject::start - " + m_nameId);
        }

        /// Getter of parentName of this Sofa Object.
        public override string parentName()
        {
            if (m_impl == null)
                return "No impl";
            else
                return m_impl.parent;
        }

    }

}
