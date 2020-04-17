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
                    dagNode.Create(m_sofaContext, NodeName);
                    m_dagNodes.Add(dagNode);

                    // temporary child of sofaContext until reordering ndoes
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
            //Debug.Log("ReconnectNodeGraph: " + nbrNode);
            if (nbrNode <= 0)
                return;

            // add root node first
            SofaDAGNode dagNode = m_sofaContext.GetComponent<SofaDAGNode>();
            if (dagNode == null)
            {
                Debug.LogError("SofaDAGNodeManager Error accessing root node: ");
                return;
            }
            m_dagNodes.Add(dagNode);

            // Find all other DAGNode
            FindDAGNodeGameObject(m_sofaContext.transform);

            if (nbrNode != m_dagNodes.Count)
            {
                Debug.LogError("SofaDAGNodeManager Error while reconnecting the graph: " + m_dagNodes.Count + " DAGNode found instead of : " + nbrNode);
                string NodeToFound = "";
                for (int i = 0; i < nbrNode; i++)
                    NodeToFound = NodeToFound + m_sofaContextAPI.getDAGNodeName(i) + ",";
                Debug.LogError("Node to be found: " + NodeToFound);

                string NodeFound = "";
                for (int i = 0; i < m_dagNodes.Count; i++)
                    NodeFound = NodeFound + m_dagNodes[i].UniqueNameId + ",";
                Debug.LogError("Node found: " + NodeFound);
                return;
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


        /// Method to register a SofaObject. Will create the object, store it inside SofaContext for serialisation and refresh the DAGNode graph
        public void RegisterCustomObject(GameObject sofaGameObject, SofaDAGNode parentNode)
        {
            SofaBaseObject obj = sofaGameObject.GetComponent<SofaBaseObject>();
            if (obj == null)
            {
                Debug.LogWarning("SofaBaseObject component not found in gameobject: " + sofaGameObject.name);
                return;
            }

            // move this new node below the parentNode
            sofaGameObject.transform.parent = parentNode.gameObject.transform;

            // create the sofa object
            int idNode = m_dagNodes.Count;
            string objectName = sofaGameObject.name + "_" + idNode.ToString();
            obj.CreateObject(m_sofaContext, objectName, parentNode.UniqueNameId);
            m_sofaContext.RegisterSofaObject(obj);

            // parse node now
            if (obj.IsCreated())
            {
                string nodeName = objectName + "_node";
                SofaDAGNode dagNode = sofaGameObject.AddComponent<SofaDAGNode>();
                dagNode.Create(m_sofaContext, nodeName);
                m_dagNodes.Add(dagNode);

                // in case several nodes have been created below
                RefreshDAGNodeGraph();
            }
        }


        ////////////////////////////////////////////
        /////  SofaDAGNodeManager internalAPI  /////
        ////////////////////////////////////////////
        
        /// Internal Method to search a component inside the children of a GameObject.
        protected void FindDAGNodeGameObject(Transform parentTransform)
        {          
            foreach (Transform child in parentTransform)
            {
                FindDAGNodeGameObject(child);

                SofaDAGNode dagNode = child.GetComponent<SofaDAGNode>();
                if (dagNode)
                {
                    m_dagNodes.Add(dagNode);
                }
            }
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


        /// Method to refresh the full DAGNode graph. Will ask the number of DAGNode on the Sofa side and compare to what is stored.
        protected void RefreshDAGNodeGraph()
        {
            int nbrNode = m_sofaContextAPI.getNbrDAGNode();
            if (nbrNode == m_dagNodes.Count) // nothing to do
                return;


            List<SofaDAGNode> tmpNodes = new List<SofaDAGNode>();
            // search for new nodes
            for (int i = 0; i < nbrNode; i++)
            {
                string nodeName = m_sofaContextAPI.getDAGNodeName(i);
                bool found = false;
                foreach (SofaDAGNode node in m_dagNodes)
                {
                    if (node.UniqueNameId == nodeName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) // new node, need to be added
                {
                    GameObject nodeGO = new GameObject("SofaNode - " + nodeName);
                    SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
                    dagNode.Create(m_sofaContext, nodeName);
                    m_dagNodes.Add(dagNode);

                    // add in tmp list before reordering
                    tmpNodes.Add(dagNode);
                }
            }


            // reorder new nodes
            foreach (SofaDAGNode snode in tmpNodes)
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
    }
}
