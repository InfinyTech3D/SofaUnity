using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAnimator : MonoBehaviour
{

    public float m_drillSpeed = 2000.0f;
    public bool m_isRunning = false;

    public GameObject m_geomagicObject = null;
    protected GeomagicController m_geomagicController = null;
    // Use this for initialization
    void Start()
    {
        if (m_geomagicObject != null)
            m_geomagicController = m_geomagicObject.GetComponent<GeomagicController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            m_isRunning = !m_isRunning;

        if (m_geomagicController)
            m_isRunning = m_geomagicController.toolactivated;

        if (m_isRunning)
        {
            Vector3 angles = transform.eulerAngles;
            transform.Rotate(new Vector3(0, m_drillSpeed, 0));
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y , transform.eulerAngles.z + m_drillSpeed);
        }
    }

    public void activate()
    {
        m_isRunning = true;
    }

    public void unactivate()
    {
        m_isRunning = false;
    }

    public void setSpeed(float speed)
    {
        m_drillSpeed = speed;
    }
}
