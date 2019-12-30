using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [System.Serializable]
    public class SofaBaseData
    {
        [SerializeField]
        protected SofaBaseComponent m_owner;
        [SerializeField]
        protected string m_dataName = "";
        [SerializeField]
        protected string m_dataType = "";

        [SerializeField]
        protected bool m_isReadOnly = false;

        [SerializeField]
        protected bool m_isEdited = false;
        [SerializeField]
        protected bool m_isDirty = false;


        public SofaBaseData(SofaBaseComponent owner, string nameID, string type)
        {
            m_owner = owner;
            m_dataName = nameID;
            m_dataType = type;
        }

        public string DataName
        {
            get { return m_dataName; }
        }

        public string DataType
        {
            get { return m_dataType; }
        }

        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
        }
    }





    public class old_SofaBaseData
    {
        protected string m_nameID = "";
        protected bool m_isReadOnly = false;
        private SBaseObject m_owner;

        public old_SofaBaseData(string nameID, SBaseObject owner)
        {
            m_nameID = nameID;
            m_owner = owner;
        }

        public string nameID
        {
            get { return m_nameID; }
        }

        public bool isReadOnly
        {
            get { return m_isReadOnly; }
        }

        public virtual string getType()
        {
            return "None";
            //Debug.Log("SofaBaseData::Type");
        }
    }
}
