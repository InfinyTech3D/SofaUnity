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


        /// init() is called by nodeMgr with sofaContext and names
        /// - Set sofaContext and name and call InitImpl()
        /// - create connection with the good api
        /// - create Object if needed
        /// - get all data + types
        /// - get data values and fill data with default values or loaded values
        /// 
        /// When play
        /// - Awake should be done delayed (loop) while SofaContext is init
        /// - GraphNodeMgr should only init itself with empty vectors
        /// - Awake is called on each DAGNode which should retrieve SofaContext pointer
        /// - Using saved Name, connect to API
        /// - Register to GraphNodeMgr
        /// - get all data + types
        /// - Set value modified by editor!!!!!
        /// ---> Start()


        protected override void InitImpl()  // if launch by awake should only retrive pointer to sofaContext + name to reconnect to sofaDAGNodeAPI
        {
            if (m_impl == null) 
                CreateSofaAPI();
            else
                SofaLog("SofaDAGNode::InitImpl, already created: " + UniqueNameId, 1);
        }


        protected void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                SofaLog("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.", 2);
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.getSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();            
            if (componentsS.Length == 0)
                return;

            SofaLog("####### SofaDAGNode: " + UniqueNameId + " -> " + componentsS);

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
                SofaLog("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.", 2);
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
