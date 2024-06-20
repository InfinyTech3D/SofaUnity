using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SofaUnity;




namespace SofaUnity
{
    public class SofaCollisionDisplay : MonoBehaviour
    {
        [SerializeField]
        public SofaComponent m_sofaDetectionOutputs = null;
        [SerializeField]
        public bool m_drawCollisions = false;

        private bool isReady = false;

        protected float[] m_sofaCollisionsValues = null;
        protected Vector3[] m_collisionPoints = null;
        protected SofaDataVectorVec3 d_sofaCollisionPoints = null;

        public bool DrawCollisions
        {
            get { return m_drawCollisions; }
            set { m_drawCollisions = value; }
        }

        void Start()
        {
            if (m_sofaDetectionOutputs == null)
            {
                Debug.LogError("SofaDetectionDisplay component not set.");
                isReady = false;
                return;
            }
            else
            {
                if (d_sofaCollisionPoints == null)
                {
                    d_sofaCollisionPoints = (SofaDataVectorVec3)m_sofaDetectionOutputs.m_dataArchiver.GetVectorData("collisionPoints");
                }
                // if no collision outputs, exit
                if (d_sofaCollisionPoints == null)
                {
                    isReady = false;
                    return;
                }
            }

            isReady = true;
        }


        void Update()
        {
            if (isReady && m_drawCollisions)
            {
                UpdateDetectionOutputs();
            }
        }

        void UpdateDetectionOutputs()
        {
            d_sofaCollisionPoints.UpdateSize();

            int nbrV = d_sofaCollisionPoints.GetSize();

            m_collisionPoints = new Vector3[nbrV];
            int res = d_sofaCollisionPoints.GetValues(m_collisionPoints, true);
            if (res == -1)
                return;
        }


        // To show the lines in the game window whne it is running
        void OnPostRender()
        {
            DrawCollisionLines();
        }

        void OnDrawGizmos()
        {
            DrawCollisionLines();
        }


        // Fill/drag these in from the editor

        // Choose the Unlit/Color shader in the Material Settings
        // You can change that color, to change the color of the connecting lines
        public Material lineMat;
        // Connect all of the `points` to the `mainPoint`
        void DrawCollisionLines()
        {
            if (!m_drawCollisions)
                return;

            for (int i=0; i<m_collisionPoints.Length; i+=2)
            {
                Vector3 ptA = m_collisionPoints[i];
                Vector3 ptB = m_collisionPoints[i + 1];

                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                GL.Vertex3(ptA.x, ptA.y, ptA.z);
                GL.Vertex3(ptB.x, ptB.y, ptB.z);
                GL.End();
            }
        }

    }

}
