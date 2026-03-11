using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    
    public class ScriptedCamera : MonoBehaviour
    {
        [SerializeField]
        public bool m_logTime = false;

        [SerializeField]
        public List<Vector3> m_positions = new List<Vector3>();

        [SerializeField]
        public List<Vector3> m_targets = new List<Vector3>();

        [SerializeField]
        public List<float> m_keyframes = new List<float>();


        // Start is called before the first frame update
        int cptFrame = 0;
        bool m_isReady = false;

        protected Vector3 m_targetPosition = Vector3.zero;        

        void Start()
        {
            m_isReady = true;
            
            if (m_keyframes.Count == 0)
                m_isReady = false;

            if (m_keyframes.Count != m_positions.Count)
                m_isReady = false;

            if (m_keyframes.Count != m_targets.Count)
                m_isReady = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (m_logTime)
                Debug.Log("Time: " + Time.time);

            if (!m_isReady)
                return;

            if (cptFrame == m_keyframes.Count-1)
                return;

            float curTime = Time.time;
            if (curTime >= m_keyframes[cptFrame + 1])
            {                
                cptFrame++;
                if (m_logTime)
                    Debug.Log("Update Frame: " + cptFrame);

                return;
            }

            float xAbs = (curTime - m_keyframes[cptFrame]) / (m_keyframes[cptFrame + 1] - m_keyframes[cptFrame]);

            Vector3 curPos = m_positions[cptFrame] + (m_positions[cptFrame + 1] - m_positions[cptFrame]) * xAbs;
            m_targetPosition = m_targets[cptFrame] + (m_targets[cptFrame + 1] - m_targets[cptFrame]) * xAbs;

            transform.position = curPos;
            transform.LookAt(m_targetPosition);
        }


       // [ExecuteInEditMode]
        void OnDrawGizmosSelected()
        {
            if (!m_isReady)
                return;

            Gizmos.color = Color.green;
            foreach (Vector3 pos in m_positions)
            {
                Gizmos.DrawSphere(pos, 1.0f);
            }

            Gizmos.color = Color.red;
            foreach (Vector3 pos in m_targets)
            {
                Gizmos.DrawSphere(pos, 1.0f);
            }
        }
    }
}