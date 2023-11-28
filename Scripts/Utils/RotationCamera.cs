using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCamera : MonoBehaviour
{
    public Renderer m_visualTarget = null;
    // Start is called before the first frame update

    public float m_radius = 100.0f;
    public float m_speed = 1.0f;

    protected Vector3 m_targetPosition = Vector3.zero;

    void Start()
    {
        if (m_visualTarget != null)
            m_targetPosition = m_visualTarget.bounds.center;
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_targetPosition);
        transform.Translate(Vector3.right * Time.deltaTime * m_speed);
    }
}
