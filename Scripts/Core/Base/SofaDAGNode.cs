using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    public class SofaDAGNode : SofaBase
    {
        /// Pointer to the Sofa Context API.
        SofaDAGNodeAPI m_impl = null;

        public void init(SofaContext sofacontext)
        {
            Debug.Log("#### SofaDAGNode::init: " + UniqueNameId);
            m_sofaContext = sofacontext;

            loadSofaObject();
        }

        void Awake()
        {
            Debug.Log("#### SofaDAGNode: " + UniqueNameId);
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
                Debug.LogError("SofaDAGNode " + UniqueNameId + " already has SofaDAGNode.");
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            Debug.Log("#### SofaDAGNode: " + UniqueNameId + " -> " + componentsS);
        }

    }

} // namespace SofaUnity
