using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    [System.Serializable]
    public class SofaLink
    {
        [SerializeField]
        protected string m_linkName = "";
        [SerializeField]
        protected string m_linkPath = "";

        public SofaLink(SofaBaseComponent owner, string linkName, string linkPath)
        {
            m_linkName = linkName;
            m_linkPath = linkPath;
        }


        public string LinkName
        {
            get { return m_linkName; }
        }

        public string LinkPath
        {
            get { return m_linkPath; }
        }
    }
}