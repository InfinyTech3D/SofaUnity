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

        void Awake()
        {
            Debug.Log("#### SofaDAGNode: " + UniqueNameId);

            if (m_impl == null)
                Debug.Log("###### HAS impl");
            else
                Debug.Log("###### NO impl");
        }

        override public void Init() 
        {
            if (m_impl == null)
                CreateSofaAPI();
        }


        void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaDAGNode " + UniqueNameId + " already has SofaDAGNode.");
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            Debug.Log("##!!!## SofaDAGNode: " + UniqueNameId + " -> " + componentsS);

            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                string baseType = m_impl.GetBaseComponentType(compoName);

                if (baseType.Contains("Error"))
                    SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);                    
                else
                    SComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
            }
        }

    }

} // namespace SofaUnity
