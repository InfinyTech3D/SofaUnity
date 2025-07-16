using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;

namespace SofaUnity
{
    public class SofaRigidMapper : MonoBehaviour
    {
        public SofaMesh m_sofaMesh = null;

        private bool m_updatedFromSofa = true;

        private bool m_ready = false;
        private float[] newPositionRigid;


        // Start is called before the first frame update
        void Start()
        {
            if (m_sofaMesh == null)
            {
                Debug.LogError("m_sofaMesh is not set.");
                m_ready = false;
                return;
            }


            int nbrV = m_sofaMesh.NbVertices();
            if (nbrV != 1)
            {
                Debug.LogError("This controller can only act on rigid object with a unique coordinate position.");
                m_ready = false;
                return;
            }

            newPositionRigid = new float[7];
            for (int i = 0; i < 6; i++)
            {
                newPositionRigid[i] = 0;
            }
            newPositionRigid[6] = 1;

            m_ready = true;
        }


        void FixedUpdate()
        {
            if (!m_ready)
                return;

            if (m_updatedFromSofa) {
                UpdateFromSofa();
            }

            UpdateToSofa();
        }


        protected void UpdateFromSofa()
        {
            float[] val = new float[7];
            m_sofaMesh.GetRawPositions(val);

            // Get raw values from SOFA, need to inverse left-right hand coordinate system
            Vector3 worldPos = new Vector3(-val[0], val[1], val[2]);
            // Project world position into SofaContext frame
            Vector3 localPos = m_sofaMesh.m_sofaContext.transform.TransformPoint(worldPos);
            this.transform.localPosition = localPos;

            // Get SOFA quaternion and inverse rotation
            var rotation = new Quaternion(val[3], val[4], val[5], val[6]);
            Vector3 angles = rotation.eulerAngles;
            this.transform.localEulerAngles = new Vector3(angles[0], -angles[1], -angles[2]);

            // Combine current rotation with SofaContext one
            this.transform.rotation = m_sofaMesh.m_sofaContext.transform.rotation * this.transform.rotation;

            m_updatedFromSofa = false;
        }


        protected void UpdateToSofa()
        {
            // Get World position
            Vector3 wordPos = this.transform.position;
            Vector3 sofaPos = m_sofaMesh.m_sofaContext.transform.InverseTransformPoint(wordPos);

            // Get inverse rotation to match SOFA and convert into quaternion
            Vector3 angles = this.transform.localEulerAngles;
            Vector3 sofaAngles = new Vector3(angles[0], -angles[1], -angles[2]);
            Quaternion rot = Quaternion.Euler(sofaAngles[0], sofaAngles[1], sofaAngles[2]);

            newPositionRigid[0] = -sofaPos[0];
            newPositionRigid[1] = sofaPos[1];
            newPositionRigid[2] = sofaPos[2];
            newPositionRigid[3] = rot[0];
            newPositionRigid[4] = rot[1];
            newPositionRigid[5] = rot[2];
            newPositionRigid[6] = rot[3];

            m_sofaMesh.SetRawPositions(newPositionRigid);
        }
    }
}