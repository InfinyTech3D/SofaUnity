using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    /// <summary>
    /// Class to store the info of a Sofa Link representation which is Serializable. to store value in Unity scenes.
    /// Just showing the info. Not used in Unity for the moment.
    /// The link is hold by a component and link to another Sofa Component
    /// </summary>
    [System.Serializable]
    public class SofaLink
    {
        ////////////////////////////////////////////
        //////        SofaLink members         /////
        ////////////////////////////////////////////

        /// Pointer to the Sofa component owner of this Data
        [SerializeField]
        protected SofaBaseComponent m_owner;

        /// Name of the Sofa link
        [SerializeField]
        protected string m_linkName = "";

        /// Path to the linked object.
        [SerializeField]
        protected string m_linkPath = "";


        ////////////////////////////////////////////
        //////       SofaLink accessors        /////
        ////////////////////////////////////////////

        /// Default constructor taking the SofaComponent owner @sa m_owner, the link name @sa m_linkName and the path to the other component @sa m_linkPath
        public SofaLink(SofaBaseComponent owner, string linkName, string linkPath)
        {
            m_owner = owner;
            m_linkName = linkName;
            m_linkPath = linkPath;
        }

        /// Getter for @sa m_linkName . This value can only be set in default constructor
        public string LinkName
        {
            get { return m_linkName; }
        }

        /// Getter for @sa m_linkPath . This value can only be set in default constructor
        public string LinkPath
        {
            get { return m_linkPath; }
        }
    }
}