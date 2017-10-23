using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

public class SLaserRay : SRayCaster
{
    private DrawLaser m_laserDraw = null;

    public bool m_isCutting = false;
    
    public enum LaserType
    {
        CuttingTool,
        AttachTool,
        FixTool
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
            else if (m_laserType == LaserType.AttachTool)
                m_sofaRC = new SofaRayCaster(_simu, 1, base.name, length);
            else
                m_sofaRC = new SofaRayCaster(_simu, 2, base.name, length);

            base.createSofaRayCaster();
        }
    }

    public override bool castRay()
    {
        return false;
    }

    void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "SLaserRay", "VRTK_ControllerEvents", "the same"));
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

    }

    void Update()
    {
        


        if (GetComponent<VRTK_ControllerEvents>() == null)
            //if (GetComponent<VRTK_InteractGrab>().GetGrabbedObject != null)
        {
        //    var controllerEvents = GetComponent<VRTK_ControllerEvents>();
        //    if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.Trigger_Press) {
        //        //Do something on trigger press
        //    }

        //    if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.Grip_Press) {
        //        //Do something on grip press
        //    }
        }


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
                m_sofaRC.activateTool(m_isCutting);
                if (m_laserDraw)
                {
                    m_laserDraw.endColor = Color.red;
                    m_laserDraw.updateLaser();
                }
            }
            else if (Input.GetKey(KeyCode.V))
            {
                m_isCutting = false;
                m_sofaRC.activateTool(m_isCutting);
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

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        VRTK_Logger.Info("SLaserRay::Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("SLaserRay TRIGGER pressed");
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "pressed", e);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "released", e);
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "clicked", e);
        m_isCutting = true;
        m_sofaRC.activateTool(m_isCutting);
        if (m_laserDraw)
        {
            m_laserDraw.endColor = Color.red;
            m_laserDraw.updateLaser();
        }
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "unclicked", e);
        m_isCutting = false;
        m_sofaRC.activateTool(m_isCutting);
        if (m_laserDraw)
        {
            m_laserDraw.endColor = Color.green;
            m_laserDraw.updateLaser();
        }
    }


}
