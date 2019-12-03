using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    public class SDAGNode : SBase
    {
        /// Pointer to the Sofa Context API.
        SofaDAGNode m_impl = null;

        public void init(SofaContext sofacontext)
        {
            Debug.Log("#### SDAGNode::init: " + UniqueNameId);
            m_sofaContext = sofacontext;

            loadSofaObject();
        }

        void Awake()
        {
            Debug.Log("#### SDAGNode: " + UniqueNameId);
            if (m_impl == null)
            {
                loadSofaObject();
            }

            if (m_impl == null)
                Debug.Log("###### HAS impl");
            else
                Debug.Log("###### NO impl");
        }

        void loadSofaObject()
        {
            if (m_impl != null)
            {
                Debug.LogError("SDAGNode " + UniqueNameId + " already has SofaDAGNode.");
                return;
            }

            m_impl = new SofaDAGNode(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            Debug.Log("#### SDAGNode: " + UniqueNameId + " -> " + componentsS);
        }

    }

} // namespace SofaUnity
