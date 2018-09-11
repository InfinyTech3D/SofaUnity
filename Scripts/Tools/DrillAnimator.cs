using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAnimator : MonoBehaviour
{

    public float m_drillSpeed = 2000.0f;
    public bool m_isRunning = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            m_isRunning = !m_isRunning;

        if (m_isRunning)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + m_drillSpeed, transform.eulerAngles.z);
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
