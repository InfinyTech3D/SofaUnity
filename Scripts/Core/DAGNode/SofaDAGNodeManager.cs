using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    public class SofaDAGNodeManager
    {

        static int value = -1;

        /// List of SofaDAGNode in the graph
        [SerializeField]
        public List<SofaDAGNode> m_dagNodes = null;

        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        private SofaContextAPI m_sofaContextAPI = null;

        // default constructor of the SObjectHiearchy
        public SofaDAGNodeManager(SofaContext context, SofaContextAPI impl)
        {
            Debug.Log("SofaDAGNodeManager creation");
            // set the sofa Context
            m_sofaContext = context;
            m_sofaContextAPI = impl;
            if (m_sofaContext == null)
            {
                Debug.LogError("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " >> SofaContext is null");
                return;
            }

            // create the list of SofaBaseObject
            m_dagNodes = new List<SofaDAGNode>();
            value++;
        }


        public void RegisterNode(string NodeName)
        {

        }


        public void ReconnectNodeGraph()
        {
            Debug.Log("## SofaDAGNodeManager RecreateNodeGraph: nbr DAG: " + m_dagNodes.Count);

            int nbrNode = m_sofaContextAPI.getNbrDAGNode();
            if (nbrNode <= 0)
                return;

            for (int i = 0; i < nbrNode; i++)
            {
                string NodeName = m_sofaContextAPI.getDAGNodeName(i);
                if (NodeName == "Error")
                {
                    Debug.LogError("NodeName: " + NodeName);
                    continue;
                }

                string nodeGOName = "SofaNode - " + NodeName;

                // will look first for node child of SofaContext
                bool res = FindNodeGameObject(m_sofaContext.transform, nodeGOName);
                if (res == false) // not found
                {
                    GameObject GONode = GameObject.Find(nodeGOName);
                    if (GONode != null)
                    {
                        SofaDAGNode dagNode = GONode.GetComponent<SofaDAGNode>();
                        m_dagNodes.Add(dagNode);
                    }
                }
            }


            // Reconnect each Node
            for (int i=0; i< m_dagNodes.Count; i++)
            {
                m_dagNodes[i].Reconnect(m_sofaContext);
            }
            Debug.Log("## SofaDAGNodeManager RecreateNodeGraph: nbr DAG AFTER: " + m_dagNodes.Count);
        }


        protected bool FindNodeGameObject(Transform parentTransform, string NodeName)
        {
            bool found = false;
            
            foreach (Transform child in parentTransform)
            {
                found = FindNodeGameObject(child, NodeName);

                if (found)
                    return found;

                if (child.name == NodeName)
                {                    
                    SofaDAGNode dagNode = child.GetComponent<SofaDAGNode>();
                    m_dagNodes.Add(dagNode);
                    return true;
                }
            }

            return found;
        }
        //hand = GameObject.Find("Hand");


        public void loadGraph()
        {
            int nbrNode = m_sofaContextAPI.getNbrDAGNode();
            Debug.Log("## SofaDAGNodeManager loadGraph: nbr DAG: " + nbrNode);

            if (nbrNode <= 0)
                return;
            
            for (int i=0; i<nbrNode; i++)            
            {
                string NodeName = m_sofaContextAPI.getDAGNodeName(i);
                if (NodeName != "Error")
                {
                    GameObject nodeGO = new GameObject("SofaNode - " + NodeName);
                    SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
                    //dagNode.UniqueNameId = NodeName;
                    //dagNode.SetSofaContext(m_sofaContext);
                    dagNode.Init(m_sofaContext, NodeName);
                    // need init?

                    m_dagNodes.Add(dagNode);
                    nodeGO.transform.parent = m_sofaContext.gameObject.transform;
                }
            }
            value += nbrNode;
            Debug.Log(Application.isPlaying + " value: " + value);

            // reorder nodes
            foreach (SofaDAGNode snode in m_dagNodes)
            {
                string parentName = snode.getParentName();
                if (parentName == "None") // root node
                    continue;

                // search for parent (no optimisation needed here)
                foreach (SofaDAGNode snodeP in m_dagNodes)
                {
                    if (snodeP.UniqueNameId == parentName)
                    {
                        snode.gameObject.transform.parent = snodeP.gameObject.transform;
                        break;
                    }
                }
            }
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
}