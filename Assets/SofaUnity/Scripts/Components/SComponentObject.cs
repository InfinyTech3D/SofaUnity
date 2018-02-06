using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using System.Linq;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SComponentObject : SBaseObject
    {
        /// Pointer to the corresponding SOFA API object
        protected SofaComponent m_impl = null;
        public SofaComponent impl
        {
            get { return m_impl; }
        }


        protected List<SData> m_datas = null;
        public List<SData> datas
        {
            get { return m_datas; }
        }
        protected Dictionary<string, string> m_dataMap = null;
        public Dictionary<string, string> dataMap
        {
            get { return m_dataMap; }
        }

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
            if (m_impl == null)
                return;

            string allData = m_impl.loadAllData();            
            List<String> datas = allData.Split(';').ToList();
            m_dataMap = new Dictionary<string, string>();
            m_datas = new List<SData>();
            foreach (String data in datas)
            {
                String[] values = data.Split(',');
                if (values.GetLength(0) == 2)
                {
                    m_datas.Add(new SData(values[0], values[1], this));

                    Debug.Log("Data: " + values[0] + " -> " + values[1]);
                }
            }
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
