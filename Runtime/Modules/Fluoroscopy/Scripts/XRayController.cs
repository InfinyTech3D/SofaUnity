using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    public class XRayController : MonoBehaviour
    {
        public GameObject m_XRayRendererObject = null;
        protected SofaComponent m_XRayRenderer = null;

        public bool m_initFromSOFA = true;

        // Unity world position of the target
        protected Vector3 m_targetPosition;

        // Unity world position of the XRay source
        protected Vector3 m_sourcePosition;

        // Unity target distance (||m_targetPosition - m_sourcePosition||)
        protected float m_targetDistance = 1.0f;

        // Unity renderer forward direction in world space
        protected Vector3 m_direction = Vector3.zero;


        protected bool m_isReady = false;


        // Update the current source position in SOFA world
        public Vector3 SourcePosition
        {
            get { return m_sourcePosition; }
            set
            {
                if (value != m_sourcePosition)
                {
                    m_sourcePosition = value;
                    // 1. Transform local to world position in Unity
                    //Vector3 newPosition = this.transform.TransformPoint(m_sourcePosition);
                    // 2. Transform unity world to SOFA world
                    Vector3 newPosition = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(m_sourcePosition);
                    m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("sourcePosition").Value = newPosition;

                    // 3. Update the target position according to new source position without change of direction
                    m_targetPosition = m_sourcePosition + m_direction * m_targetDistance;
                    Vector3 sofaPosition = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(m_targetPosition);
                    m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = sofaPosition;
                }
            }
        }

        public Vector3 TargetPosition
        {
            get { return m_targetPosition; }
            set
            {
                if (value != m_targetPosition)
                {
                    m_targetPosition = value;
                    
                    m_direction = m_targetPosition - m_sourcePosition;
                    m_targetDistance = m_direction.magnitude;
                    m_direction.Normalize();
                    transform.forward = m_direction;

                    Vector3 newPosition = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(m_targetPosition);
                    m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = newPosition;
                }
            }
        }

        public float TargetDistance
        {
            get { return m_targetDistance; }
            set
            {
                if (value != m_targetDistance)
                {
                    m_targetDistance = value;
                    m_targetPosition = m_sourcePosition + m_direction * m_targetDistance;

                    Vector3 newPosition = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(m_targetPosition);
                    m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = newPosition;
                }
            }
        }


        [SerializeField]
        protected float m_beamPower = 0.0f;
        public float BeamPower
        {
            get { return m_beamPower; }
            set
            {
                if (value != m_beamPower)
                {
                    m_beamPower = value;
                    m_XRayRenderer.m_dataArchiver.GetSofaFloatData("beamPower").Value = m_beamPower;
                }
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            if (m_XRayRendererObject == null)
            {
                Debug.LogError("XRayRenderer GameObject not set.");
                return;
            }
            else
            {
                m_XRayRenderer = m_XRayRendererObject.GetComponent<SofaComponent>();
                if (m_XRayRenderer == null)
                {
                    Debug.LogError("No XRayRenderer component found in GameObject.");
                    return;
                }
                
                if (m_initFromSOFA)
                {
                    // Get initial Source position of the XRay renderer from SOFA
                    Vector3 poInSofa = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("sourcePosition").Value;
                    Vector3 poInUnity = m_XRayRenderer.m_sofaContext.transform.TransformPoint(poInSofa);
                    m_sourcePosition = this.transform.InverseTransformPoint(poInUnity);

                    // Get initial target position of the XRay renderer from SOFA
                    Vector3 poInSofa2 = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value;
                    Vector3 poInUnity2 = m_XRayRenderer.m_sofaContext.transform.TransformPoint(poInSofa2);
                    m_targetPosition = this.transform.InverseTransformPoint(poInUnity2);

                    // Get the initial beam power from SOFA
                    m_beamPower = m_XRayRenderer.m_dataArchiver.GetSofaFloatData("beamPower").Value;

                    // Update this GameObject transform data by computing it's direction and position
                    m_direction = m_targetPosition - m_sourcePosition;
                    m_targetDistance = m_direction.magnitude;
                    m_direction.Normalize();


                    transform.position = m_sourcePosition;
                    transform.forward = m_direction;
                    transform.hasChanged = false;
                }
            }

            m_isReady = true;

        }

        // Update is called once per frame
        void Update()
        {
            if (!m_isReady)
                return;

            if (transform.hasChanged)
            {
                if (this.transform.position != m_sourcePosition)
                {
                    // Update the source position in SOFA world
                    SourcePosition = this.transform.position;
                }

                if (transform.forward != m_direction)
                {
                    m_direction = transform.forward;
                    m_targetPosition = m_sourcePosition + m_direction * m_targetDistance;

                    Vector3 sofaPosition = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(m_targetPosition);
                    m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = sofaPosition;
                }
                transform.hasChanged = false;
            }

            if (Input.GetKey(KeyCode.PageUp))
                zoomXRay();

            if (Input.GetKey(KeyCode.PageDown))
                unZoomXRay();

            if (Input.GetKey(KeyCode.KeypadPlus))
                BeamPower += 0.5f;

            if (Input.GetKey(KeyCode.KeypadMinus))
                BeamPower -= 0.5f;
        }

        public void zoomXRay() { TargetDistance -= 0.1f; }

        public void unZoomXRay() { TargetDistance += 0.1f; }


        void OnDrawGizmosSelected()
        {
            if (!m_isReady)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(m_targetPosition, 0.1f);
            Gizmos.DrawLine(m_sourcePosition, m_targetPosition);
        }
    }
}
