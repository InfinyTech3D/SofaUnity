using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using System.Linq;

namespace SofaUnity
{
    /// <summary>
    /// Base class that design a SComponentObject mapped to a SofaComponent listener object.
    /// The SofaComponent allows to get all Data of a component that is listened. 
    /// This class inherite from @see SofaBaseObject and add the creation of Mesh and handle transformation
    /// </summary>
    [ExecuteInEditMode]
    public class SComponentObject : SofaBaseObject
    {
        ////////////////////////////////////////////
        /////        Object members API        /////
        ////////////////////////////////////////////

        /// Pointer to the corresponding SOFA API object
        protected SofaComponentObjectAPI m_impl = null;
        public SofaComponentObjectAPI impl
        {
            get { return m_impl; }
        }

        /// List of Data parsed
        protected List<old_SofaData> m_datas = null;
        public List<old_SofaData> datas
        {
            get { return m_datas; }
        }

        /// Map of the Data parsed. Key is the dataName of the Data, value is the type of this Data.
        protected Dictionary<string, string> m_dataMap = null;
        public Dictionary<string, string> dataMap
        {
            get { return m_dataMap; }
        }




        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_sofaContext.GetSimuContext();
            if (_simu != IntPtr.Zero)
            {
                // Create the API object for SofaComponent
                m_impl = new SofaComponentObjectAPI(_simu, m_uniqueNameId, false);

                if (m_impl == null)
                {
                    Debug.LogError("SofaComponent:: Object creation failed.");
                    return;
                }

                m_impl.loadObject();

                // Call SofaBaseMesh.createObject() to init value loaded from the scene.
                base.createObject();
            }
        }

        /// Method called by \sa Awake after the loadcontext method.
        protected override void awakePostProcess()
        {
            if (m_impl == null)
                return;

            string allData = m_impl.loadAllData();

            if (allData == "None")
                return;
                      
            List<String> datas = allData.Split(';').ToList();
            m_dataMap = new Dictionary<string, string>();
            m_datas = new List<old_SofaData>();
            foreach (String data in datas)
            {
                String[] values = data.Split(',');
                if (values.GetLength(0) == 2)
                {
                    m_datas.Add(new old_SofaData(values[0], values[1], this));
                }
            }
        }




        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called at GameObject init (after creation or when starting play).
        void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SComponentObject::start - " + m_uniqueNameId);
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
