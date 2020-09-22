// Uncomment this line to use the compatibility with VRTK. TODO: find a way to automatically detect if VRTK asset is present
//#define USING_VRTK
using UnityEngine;

#if USING_VRTK
using VRTK;
#endif

/// <summary>
/// Specialisation of SLaserRay class
/// Allow the same interaction but using a VR interface thanks to VRTK
/// </summary>
class SVRLaserRay : SofaRayCaster
{
#if USING_VRTK
    bool logController = true;

    public ButtonType m_actionButton;
#endif

    public enum ButtonType
    {
        Trigger,
        Grip
    };

    

    void Start()
    {
        //m_axisDirection.Normalize();

#if USING_VRTK
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

        GetComponent<VRTK_ControllerEvents>().GripClicked += new ControllerInteractionEventHandler(DoGripClicked);
        GetComponent<VRTK_ControllerEvents>().GripUnclicked += new ControllerInteractionEventHandler(DoGripUnclicked);
#endif
        //if(laser != null)
        //{
        //    laser.transform.localPosition = new Vector3(-0.035f, -0.005f, 0.005f);
        //}
        //activeTool(true);
        //if (m_sofaContext.testAsync == true)
        //    m_sofaContext.registerCaster(this);
        //else
        //    automaticCast = true;
    }

#if USING_VRTK
    /// Debugger method for VR interaction
    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        if (logController)
            VRTK_Logger.Info("SLaserRay::Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    /// CallBack method for controller interactions
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
        if (m_actionButton == ButtonType.Trigger)
        {
            activeTool(true);
        }
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "unclicked", e);
        if (m_actionButton == ButtonType.Trigger)
        {
            activeTool(false);
        }
    }

    private void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "clicked", e);
        if (m_actionButton == ButtonType.Grip)
        {
            activeTool(true);
        }
    }

    private void DoGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "unclicked", e);
        if (m_actionButton == ButtonType.Grip)
        {
            activeTool(false);
        }
    }
#endif
}
