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

        protected string m_parentNodeName = "None";
        public string getParentName() { return m_parentNodeName; }




        protected override void InitImpl()  // if launch by awake should only retrive pointer to sofaContext + name to reconnect to sofaDAGNodeAPI
        {
            Debug.Log("####### SofaDAGNode::InitImpl: " + UniqueNameId);
            if (m_impl == null) 
                CreateSofaAPI();
            else
                Debug.Log("SofaDAGNode::InitImpl, already created: " + UniqueNameId);
        }


        protected void CreateSofaAPI()
        {
            Debug.Log("####### SofaDAGNode::CreateSofaAPI: " + UniqueNameId);
            if (m_impl != null)
            {
                Debug.LogError("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.");
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();            
            if (componentsS.Length == 0)
                return;

            SofaLog("#####################!!!############################ SofaDAGNode: " + UniqueNameId + " -> " + componentsS);

            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                string baseType = m_impl.GetBaseComponentType(compoName);

                if (baseType.Contains("Error"))
                    SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);                    
                else
                    SComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
            }

            m_parentNodeName = m_impl.GetParentNodeName();
            if (m_parentNodeName.Contains("Error"))
            {
                SofaLog("Node Parent Name return error: " + m_parentNodeName + ", will use None.");
                m_parentNodeName = "None";
            }
        }


        protected override void ReconnectImpl()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.");
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            if (componentsS.Length == 0)
                return;

            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                bool found = false;
                GameObject componentGO = null;
                string compoGOName = "SofaComponent - " + compoName;
                foreach (Transform child in this.gameObject.transform)
                {
                    if (child.name == compoGOName)
                    {
                        componentGO = child.gameObject;                       
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    componentGO = GameObject.Find(compoGOName);
                }

                if (found)
                {
                    SofaBase component = componentGO.GetComponent<SofaBase>();
                    if (component != null)
                        component.Reconnect(this.m_sofaContext);
                }
            }
        }
    }

} // namespace SofaUnity
