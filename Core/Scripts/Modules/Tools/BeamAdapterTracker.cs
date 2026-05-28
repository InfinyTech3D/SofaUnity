using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    public class BeamAdapterTracker : MonoBehaviour
    {
        [SerializeField]
        public SofaComponent m_tool1 = null;

        [SerializeField]
        public SofaComponent m_tool2 = null;

        [SerializeField]
        public SofaComponent m_tool3 = null;

        /// Pointer to the dofs of the 3 tools
        public SofaMesh m_sofaMesh = null;


        private bool m_isReady = false;
        private int m_nbrDofs = 0;

        protected SofaIntData d_tool1_id = null;
        protected SofaIntData d_tool2_id = null;
        protected SofaIntData d_tool3_id = null;

        public Vector3 m_tool1_position = Vector3.zero;
        public Vector3 m_tool2_position = Vector3.zero;
        public Vector3 m_tool3_position = Vector3.zero;

        public GameObject tool1_slave = null;
        public GameObject tool2_slave = null;
        public GameObject tool3_slave = null;



        void Start()
        {
            if (m_tool1 != null)
            {
                Debug.Log("BeamAdapterTracker: Tool 1 found. Connecting wireTipIndex");
                d_tool1_id = (SofaIntData)m_tool1.m_dataArchiver.GetSofaIntData("wireTipIndex");
            }

            if (m_tool2 != null)
            {
                Debug.Log("BeamAdapterTracker: Tool 2 found. Connecting wireTipIndex");
                d_tool2_id = (SofaIntData)m_tool2.m_dataArchiver.GetSofaIntData("wireTipIndex");
            }

            if (m_tool3 != null)
            {
                Debug.Log("BeamAdapterTracker: Tool 3 found. Connecting wireTipIndex");
                d_tool3_id = (SofaIntData)m_tool3.m_dataArchiver.GetSofaIntData("wireTipIndex");
            }

            if (m_sofaMesh == null)
            {
                Debug.LogError("BeamAdapterTracker: SofaMesh not set. Can't track the wire tip positions.");
                m_isReady = false;
            }
            else
            {
                m_sofaMesh.AddListener(); // TODO m_sofaMesh.RemoveListener(); when exit?
                m_nbrDofs = m_sofaMesh.NbVertices();
                m_isReady = true;
            }
        }

        // TODO for later: track orientation in addition to the position.
        private void Update()
        {
            if (d_tool1_id != null)
            {
                int id1 = d_tool1_id.Value;
                if (id1 >= 0 && id1 < m_nbrDofs)
                {
                    // TODO: check why we need to do a transform point. The position should be world position already no?
                    m_tool1_position = m_sofaMesh.GetPosition(id1);
                    if (tool1_slave != null)
                        tool1_slave.transform.position = m_sofaMesh.m_sofaContext.transform.TransformPoint(m_tool1_position);
                }

            }

            if (d_tool2_id != null)
            {
                int id2 = d_tool2_id.Value;
                if (id2 >= 0 && id2 < m_nbrDofs)
                {
                    m_tool2_position = m_sofaMesh.GetPosition(id2);
                    if (tool2_slave != null)
                        tool2_slave.transform.localPosition = m_sofaMesh.m_sofaContext.transform.TransformPoint(m_tool2_position);
                }
            }
            
            if (d_tool3_id != null)
            {
                int id3 = d_tool3_id.Value;
                if (id3 >= 0 && id3 < m_nbrDofs) 
                {
                    
                    m_tool3_position = m_sofaMesh.GetPosition(id3);
                    if (tool3_slave != null)
                        tool3_slave.transform.position = m_sofaMesh.m_sofaContext.transform.TransformPoint(m_tool3_position);
                }
            }
        }

    }

} // namespace SofaUnity
