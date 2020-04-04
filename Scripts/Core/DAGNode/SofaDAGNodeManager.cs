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
        private List<SofaDAGNode> m_dagNodes = null;

        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        /// pointer to the SofaContextAPI of the simulation
        private SofaContextAPI m_sofaContextAPI = null;

        /// Pointer to the root DAGNode of this simulation
        private SofaDAGNode m_rootDAGNode = null;


        /////////////////////////////////////////////
        //////   SofaDAGNodeManager accessors   /////
        /////////////////////////////////////////////

        /// Return the number of node registered in this manager
        public int NumberOfDAGNodes()
        {
            if (m_dagNodes != null)
                return m_dagNodes.Count;
            else
                return 0;
        }


        /// Return the SofaDAGNode given its name. return null object if not found
        SofaDAGNode GetDAGNodeByName(string nodeName)
        {
            if (m_dagNodes == null)
                return null;

            foreach(SofaDAGNode node in m_dagNodes)
            {
                if (node.UniqueNameId == nodeName)
                    return node;
            }

            return null;
        }

        /// Return the SofaDAGNode given its range. return null object if out of range
        SofaDAGNode GetDAGNodeById(int nodeId)
        {
            if (m_dagNodes == null)
                return null;

            if (nodeId >= m_dagNodes.Count)
                return null;

            return m_dagNodes[nodeId];
        }


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

            // create empty the list of SofaBaseObject
            m_dagNodes = new List<SofaDAGNode>();
        }


        /// Principal method to parse the SOFA simulation scene and create the same DAGNode Graph
        public void LoadNodeGraph()
        {
            // first clear previous hiearchy
            ClearManager();

            int nbrNode = m_sofaContextAPI.getNbrDAGNode();
            if (nbrNode <= 0)
                return;

            for (int i = 0; i < nbrNode; i++)
            {
                string NodeName = m_sofaContextAPI.getDAGNodeName(i);
                if (NodeName == "root") // skip root node
                {
                    if (m_rootDAGNode == null)
                    {
                        m_rootDAGNode = m_sofaContext.gameObject.AddComponent<SofaDAGNode>();
                        m_rootDAGNode.Create(m_sofaContext, NodeName);
                        m_dagNodes.Add(m_rootDAGNode);
                    }
                    else
                    {
                        m_rootDAGNode.Create(m_sofaContext, NodeName);
                    }
                }
                else if (NodeName != "Error")
                {
                    GameObject nodeGO = new GameObject("SofaNode - " + NodeName);
                    SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
                    //dagNode.UniqueNameId = NodeName;
                    //dagNode.SetSofaContext(m_sofaContext);
                    dagNode.Create(m_sofaContext, NodeName);
                    // need init?

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
                string parentName = snode.ParentNodeName;
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


        /// Method to register a node into this graph under another parentNode, if parent is not found, will add it under root
        public void RegisterCustomNode(string nodeName, string parentNodeName)
        {
            SofaDAGNode parentNode = GetDAGNodeByName(parentNodeName);
            if (parentNode == null)
            {
                Debug.LogWarning("Parent Node name " + parentNodeName + " not found in graph. Will add Node " + nodeName + " under root node.");
                parentNode = m_rootDAGNode;
            }

            int idNode = m_dagNodes.Count;
            nodeName = nodeName + "_" + idNode.ToString();

            GameObject nodeGO = new GameObject("SofaNode - " + nodeName);
            SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
            dagNode.ParentNodeName = parentNodeName;
            dagNode.Create(m_sofaContext, nodeName, true);

            m_dagNodes.Add(dagNode);
            nodeGO.transform.parent = parentNode.gameObject.transform;
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


        /// Internal Method to clear a previous Node hierarchy
        protected void ClearManager()
        {
            for (int i=0; i<m_dagNodes.Count; i++)
            {
                SofaDAGNode node = m_dagNodes[i];
                node.DestroyDAGNode(true);
                node = null;
            }
            m_dagNodes.Clear();

            // copy back the root node pointer
            if (m_rootDAGNode != null)
                m_dagNodes.Add(m_rootDAGNode);
        }
    }
}
