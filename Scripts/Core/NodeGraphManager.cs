using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    public class NodeGraphManager
    {

        static int value = -1;

        /// List of SofaDAGNode in the graph
        public List<SofaDAGNode> m_dagNodes = null;

        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        private SofaContextAPI m_sofaContextAPI = null;

        // default constructor of the SObjectHiearchy
        public NodeGraphManager(SofaContext context, SofaContextAPI impl)
        {
            // set the sofa Context
            m_sofaContext = context;
            m_sofaContextAPI = impl;
            if (m_sofaContext == null)
            {
                Debug.LogError("## SObjectHierarchy::IsPlaying: " + Application.isPlaying + " >> SofaContext is null");
                return;
            }

            // create the list of SBaseObject
            m_dagNodes = new List<SofaDAGNode>();
            value++;

            Debug.Log(Application.isPlaying + " value: " + value);
        }


        public void loadGraph()
        {
            int nbrNode = m_sofaContextAPI.getNbrDAGNode();
            Debug.Log("## NodeGraphManager nbr DAG: " + nbrNode);

            if (nbrNode <= 0)
                return;
            
            for (int i=0; i<nbrNode; i++)            
            {
                string NodeName = m_sofaContextAPI.getDAGNodeName(i);
                if (NodeName != "Error")
                {
                    GameObject nodeGO = new GameObject("SofaNode - " + NodeName);
                    SofaDAGNode dagNode = nodeGO.AddComponent<SofaDAGNode>();
                    dagNode.UniqueNameId = NodeName;
                    dagNode.init(m_sofaContext);

                    m_dagNodes.Add(dagNode);
                    nodeGO.transform.parent = m_sofaContext.gameObject.transform;
                }
            }
            value += nbrNode;
            Debug.Log(Application.isPlaying + " value: " + value);
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