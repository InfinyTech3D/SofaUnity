using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SLaserRay : SRayCaster
{
    private DrawLaser m_laserDraw = null;

    public bool m_isCutting = false;

    protected override void createSofaRayCaster()
    {
        m_laserDraw = transform.gameObject.AddComponent<DrawLaser>();
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
            //if (triId < 10000)
              //  Debug.Log("Sofa Collision triId " + triId);

            if (Input.GetKey(KeyCode.C))
            {                
                m_isCutting = true;
                m_sofaRC.activateCuttingTool(m_isCutting);
                if (m_laserDraw)
                {
                    m_laserDraw.endColor = Color.red;
                    m_laserDraw.updateLaser();
                }
            }
            else if (Input.GetKey(KeyCode.V))
            {
                m_isCutting = false;
                m_sofaRC.activateCuttingTool(m_isCutting);
                if (m_laserDraw)
                {
                    m_laserDraw.endColor = Color.green;
                    m_laserDraw.updateLaser();
                }
            }
        }

        if(m_laserDraw)
            m_laserDraw.draw(transform.position, transform.position + direction * length);

    }
}
