using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    public class SDAGNode : SBase
    {
        /// Pointer to the Sofa Context API.
        SofaDAGNode m_impl;

        SofaContextAPI m_sofaContext;

        protected string m_nodeName = "UnSet";

        public void setDAGNodeName(string _name)
        {
            m_nodeName = _name;
        }

        public void init(SofaContextAPI sofacontext)
        {
            Debug.Log("#### SDAGNode::init: " + m_nodeName);
            m_sofaContext = sofacontext;
        }

        void Awake()
        {
            Debug.Log("#### SDAGNode: " + m_nodeName);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

} // namespace SofaUnity
