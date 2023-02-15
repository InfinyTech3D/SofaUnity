using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class XRayController : MonoBehaviour
{
    public GameObject m_XRayRendererObject = null;
    protected SofaComponent m_XRayRenderer = null;


    // position in unity world of the target position
    //[SerializeField]
    protected Vector3 m_targetPosition;

    //[SerializeField]
    protected Vector3 m_sourcePosition;

    [SerializeField]
    protected float m_targetDistance = 0.0f;

    protected Vector3 m_direction = Vector3.zero;


    protected bool m_isReady = false;
    

    public Vector3 SourcePosition
    {
        get { return m_sourcePosition; }
        set
        {
            if (value != m_sourcePosition)
            {
                m_sourcePosition = value;
                m_direction = m_targetPosition - m_sourcePosition;
                m_targetDistance = m_direction.magnitude;
                m_direction.Normalize();
                transform.forward = m_direction;
                Vector3 po = this.transform.TransformPoint(m_sourcePosition);
                po = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(po);
                m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("sourcePosition").Value = po;
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
                Vector3 po = this.transform.TransformPoint(m_targetPosition);
                po = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(po);
                m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = po;
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
                Vector3 po = this.transform.TransformPoint(m_targetPosition);
                po = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(po);
                m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = po;
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


            Vector3 poInSofa = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("sourcePosition").Value;
            Vector3 poInUnity = m_XRayRenderer.m_sofaContext.transform.TransformPoint(poInSofa);
            m_sourcePosition = this.transform.InverseTransformPoint(poInUnity);

            Vector3 poInSofa2 = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value;
            Vector3 poInUnity2 = m_XRayRenderer.m_sofaContext.transform.TransformPoint(poInSofa2);
            m_targetPosition = this.transform.InverseTransformPoint(poInUnity2);

            //m_sourcePosition = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("sourcePosition").Value;
            //m_targetPosition = m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value;
            m_beamPower = m_XRayRenderer.m_dataArchiver.GetSofaFloatData("beamPower").Value;

            // Update this object data:
            m_direction = m_targetPosition - m_sourcePosition;
            m_targetDistance = m_direction.magnitude;
            m_direction.Normalize();

            transform.position = m_sourcePosition;
            transform.forward = m_direction;
            transform.hasChanged = false;
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
            SourcePosition = this.transform.position;
            if (transform.forward != m_direction)
            {
                m_direction = transform.forward;
                m_targetPosition = m_sourcePosition + m_direction * m_targetDistance;

                Vector3 po = this.transform.TransformPoint(m_targetPosition);
                po = m_XRayRenderer.m_sofaContext.transform.InverseTransformPoint(po);

                m_XRayRenderer.m_dataArchiver.GetSofaVec3Data("detectorPosition").Value = po;
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

    public void zoomXRay() { TargetDistance -= 0.1f;}

    public void unZoomXRay() { TargetDistance += 0.1f;}


    void OnDrawGizmosSelected()
    {
        if (!m_isReady)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_targetPosition, 0.1f);
        Gizmos.DrawLine(m_sourcePosition, m_targetPosition);
    }
}
