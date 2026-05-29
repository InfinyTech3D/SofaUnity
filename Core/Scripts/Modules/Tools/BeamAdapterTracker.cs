using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    [System.Serializable]
    public class BeamToolEntry
    {
        public SofaComponent m_tool = null;
        public GameObject m_slave = null;

        // Runtime (not shown in inspector)
        [HideInInspector] public SofaIntData d_tool_id = null;
        public Vector3 m_position = Vector3.zero;
    }

    public class BeamAdapterTracker : MonoBehaviour
    {
        /// Pointer to the unique sofa mesh
        public SofaMesh m_sofaMesh = null;

        [SerializeField]
        public List<BeamToolEntry> m_tools = new List<BeamToolEntry>();

        private bool m_isReady = false;
        private int m_nbrDofs = 0;

        void Start()
        {
            // Connect wireTipIndex for each tool
            foreach (BeamToolEntry entry in m_tools)
            {
                if (entry.m_tool != null)
                {
                    Debug.Log("BeamAdapterTracker: Tool found. Connecting wireTipIndex");
                    entry.d_tool_id = (SofaIntData)entry.m_tool.m_dataArchiver.GetSofaIntData("wireTipIndex");
                }
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
            if (!m_isReady) return;

            foreach (BeamToolEntry entry in m_tools)
            {
                if (entry.d_tool_id == null) continue;

                int id = entry.d_tool_id.Value;
                if (id >= 0 && id < m_nbrDofs)
                {
                    // TODO: check why we need to do a transform point. The position should be world position already no?
                    entry.m_position = m_sofaMesh.GetPosition(id);
                    if (entry.m_slave != null)
                        entry.m_slave.transform.position = m_sofaMesh.m_sofaContext.transform.TransformPoint(entry.m_position);
                }
            }
        }
    }
} // namespace SofaUnity