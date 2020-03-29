using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Main class in charge of creating and reloading the whole DAGNode graph correpsonding to SOFA simulation.
    /// </summary>
    public class SofaDAGNodeManager
    {
        /////////////////////////////////////////////
        //////    SofaDAGNodeManager members    /////
        /////////////////////////////////////////////

        /// List of SofaDAGNode in the graph
        public List<SofaDAGNode> m_dagNodes = null;

        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        /// pointer to the SofaContextAPI of the siulation
        private SofaContextAPI m_sofaContextAPI = null;



        ////////////////////////////////////////////
        /////  SofaDAGNodeManager public API   /////
        ////////////////////////////////////////////

        /// default constructor of the SObjectHiearchy
        public SofaDAGNodeManager(SofaContext context, SofaContextAPI impl)
        {
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
        }


        /// Principal method to parse the SOFA simulation scene and create the same DAGNode Graph
        public void LoadNodeGraph()
        {
            int nbrNode = m_sofaContextAPI.getNbrDAGNode();

            if (nbrNode <= 0)
                return;

            for (int i = 0; i < nbrNode; i++)
            {
                string NodeName = m_sofaContextAPI.getDAGNodeName(i);
                if (NodeName == "root") // skip root node
                {
                    Debug.Log("############## CREATE SofaRootNode - " + NodeName);
                    SofaDAGNode dagNode = m_sofaContext.gameObject.AddComponent<SofaDAGNode>();
                    dagNode.Create(m_sofaContext, NodeName);
                    m_dagNodes.Add(dagNode);
                }
                else if (NodeName != "Error")
                {
                    GameObject nodeGO = new GameObject("SofaNode - " + NodeName);
                    SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
                    //dagNode.UniqueNameId = NodeName;
                    //dagNode.SetSofaContext(m_sofaContext);
                    dagNode.Create(m_sofaContext, NodeName);
                    // need init?
                    Debug.Log("############## CREATE SofaNode - " + NodeName);

                    m_dagNodes.Add(dagNode);
                    nodeGO.transform.parent = m_sofaContext.gameObject.transform;
                }
                else
                {
                    Debug.LogError("SofaDAGNodeManager Error loading node: " + i + "return error: " + NodeName);
                }
            }

            // reorder nodes
            foreach (SofaDAGNode snode in m_dagNodes)
            {
                string parentName = snode.getParentName();
                if (parentName == "None") // root node
                    continue;

                if (parentName == "root") // under root node
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


        /// Principal method to reconnect the whole graph between Sofa simulation and unity GameObjects
        public void ReconnectNodeGraph()
        {
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
        }


        /// Method to propagate dirty value to all DAGNode of the graph.
        public void PropagateSetDirty(bool value)
        {
            foreach (SofaDAGNode snode in m_dagNodes)
            {
                snode.PropagateSetDirty(value);
            }
        }


        /// Method to register a node into this graph
        public void RegisterNode(string NodeName)
        {
            Debug.LogError("Method RegisterNode has not yet been implemented.");
        }



        ////////////////////////////////////////////
        /////  SofaDAGNodeManager internalAPI  /////
        ////////////////////////////////////////////
        
        /// Internal Method to search a component inside a the children of a GameObject.
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
    }
}
