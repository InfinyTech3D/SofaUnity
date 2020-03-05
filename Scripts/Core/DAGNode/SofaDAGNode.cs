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

        /// List of SofaBaseComponent in this DAGNode
        public List<SofaBaseComponent> m_sofaComponents = null;

        protected SofaMesh m_nodeMesh = null;


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


        protected override void Create_impl()  // if launch by awake should only retrive pointer to sofaContext + name to reconnect to sofaDAGNodeAPI
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

            m_impl = new SofaDAGNodeAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            if (componentsS.Length == 0)
                return;

            SofaLog("####### SofaDAGNode::CreateSofaAPI " + UniqueNameId + " -> " + componentsS);
            m_sofaComponents = new List<SofaBaseComponent>();
            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                string baseType = m_impl.GetBaseComponentType(compoName);

                if (baseType.Contains("Error"))
                    SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);
                else
                {
                    SofaBaseComponent compo = SofaComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
                    if (compo != null)
                    {
                        if (baseType == "SofaMesh")
                        {
                            m_nodeMesh = compo as SofaMesh;
                        }
                        m_sofaComponents.Add(compo);
                    }
                }
            }

            m_parentNodeName = m_impl.GetParentNodeName();
            if (m_parentNodeName.Contains("Error"))
            {
                SofaLog("Node Parent Name return error: " + m_parentNodeName + ", will use None.");
                m_parentNodeName = "None";
            }
        }


        protected override void Reconnect_impl()
        {
            if (m_impl != null)
            {
                SofaLog("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.", 2);
                return;
            }

            m_impl = new SofaDAGNodeAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

            string componentsS = m_impl.GetDAGNodeComponents();
            if (componentsS.Length == 0)
                return;

            SofaLog("####### SofaDAGNode::Reconnect_impl " + UniqueNameId + " -> " + componentsS);
            List<string> compoNames = ConvertStringToList(componentsS);
            m_sofaComponents = new List<SofaBaseComponent>();

            foreach (string compoName in compoNames)
            {
                bool found = false;
                foreach (Transform child in this.gameObject.transform)
                {
                    SofaBaseComponent component = child.GetComponent<SofaBaseComponent>();
                    if (component != null && component.UniqueNameId == compoName)
                    {
                        component.Reconnect(this.m_sofaContext);
                        m_sofaComponents.Add(component);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Debug.LogError("Component: " + compoName + " not found under DAGNode: " + UniqueNameId);
            }
        }

        public void PropagateSetDirty(bool value)
        {
            foreach (SofaBaseComponent scompo in m_sofaComponents)
            {
                scompo.SetDirty(value);
            }
        }



        public SofaMesh GetSofaMesh()
        {
            return m_nodeMesh;
        }
        
        public SofaMesh FindSofaMesh()
        {
            if (m_nodeMesh != null)
                return m_nodeMesh;

            GameObject DAGNode = this.gameObject;

            foreach (Transform child in DAGNode.transform)
            {
                SofaMesh sofaMesh = child.GetComponent<SofaMesh>();
                if (sofaMesh != null)
                {
                    m_nodeMesh = sofaMesh;
                    break;
                }
            }

            return m_nodeMesh;
        }
    }

} // namespace SofaUnity
