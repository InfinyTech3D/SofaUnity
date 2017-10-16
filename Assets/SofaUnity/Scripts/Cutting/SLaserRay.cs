using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SLaserRay : SRayCaster
{
    private DrawLaser m_laserDraw = null;

    public bool m_isCutting = false;
    
    public enum LaserType
    {
        CuttingTool,
        AttachTool
    };

    public LaserType m_laserType;

    protected override void createSofaRayCaster()
    {
        m_laserDraw = transform.gameObject.AddComponent<DrawLaser>();
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.getSimuContext();
        if (_simu != IntPtr.Zero)
        {   
            if (m_laserType == LaserType.CuttingTool)            
                m_sofaRC = new SofaRayCaster(_simu, 0, base.name, length);
            else
                m_sofaRC = new SofaRayCaster(_simu, 1, base.name, length);

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
            //    Debug.Log("Sofa Collision triId " + triId);

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
