using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SLaserRay : SRayCaster
{
    private GameObject laser;
    private LineRenderer lr;

    [SerializeField]
    public Color startColor = Color.red;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float startWidth = 1f;
    [SerializeField]
    public float endWidth = 0.5f;

    public bool m_isCutting = false;

    protected override void createSofaRayCaster()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.getSimuContext();
        if (_simu != IntPtr.Zero)
        {
            m_sofaRC = new SofaRayCaster(_simu);

            base.createSofaRayCaster();
        }
    }

    public override bool castRay()
    {
        return false;
    }

    void Update()
    {
        origin = transform.position;
        direction = transform.forward;

        if (m_sofaRC != null)
        {
            int triId = m_sofaRC.castRay(origin, direction);
            if (triId < 10000)
                Debug.Log("Sofa Collision triId " + triId);

            if (Input.GetKey(KeyCode.C))
            {
                Debug.Log("middle button");
                m_isCutting = !m_isCutting;
                m_sofaRC.activateCuttingTool(m_isCutting);
            }
        }
    }
}
