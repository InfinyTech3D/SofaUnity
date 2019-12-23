using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    public class SofaBaseData
    {
        protected string m_nameID = "";
        protected bool m_isReadOnly = false;
        private SBaseObject m_owner;

        public SofaBaseData(string nameID, SBaseObject owner)
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
