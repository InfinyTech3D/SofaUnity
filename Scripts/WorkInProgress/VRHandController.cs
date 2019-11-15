using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class VRHandController : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public bool triggerButtonEvents = true;
    public bool gripButtonEvents = true;
    public bool touchpadButtonEvents = true;

    protected SofaVR_API m_sofaVR_API = null;
    protected bool m_gripPessed = false;
    protected bool m_triggerPressed = false;
    protected bool m_touchPadPressed = false;

    public bool isGripPressed() { return m_gripPessed; }
    public bool isTriggerPressed() { return m_triggerPressed; }
    public bool isTouchPadPressed() { return m_touchPadPressed; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnEnable()
    {
        controllerEvents = (controllerEvents == null ? GetComponent<VRTK_ControllerEvents>() : controllerEvents);
        if (controllerEvents == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        //Setup controller event listeners
        controllerEvents.TriggerPressed += DoTriggerPressed;
        controllerEvents.TriggerReleased += DoTriggerReleased;

        controllerEvents.GripPressed += DoGripPressed;
        controllerEvents.GripReleased += DoGripReleased;

        controllerEvents.TouchpadPressed += DoTouchpadPressed;
        controllerEvents.TouchpadReleased += DoTouchpadReleased;
    }

    private void OnDisable()
    {
        if (controllerEvents != null)
        {
            controllerEvents.TriggerPressed -= DoTriggerPressed;
            controllerEvents.TriggerReleased -= DoTriggerReleased;

            controllerEvents.GripPressed -= DoGripPressed;
            controllerEvents.GripReleased -= DoGripReleased;

            controllerEvents.TouchpadPressed -= DoTouchpadPressed;
            controllerEvents.TouchpadReleased -= DoTouchpadReleased;
        }
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        string debugString = "Controller on index '" + index + "' " + button + " has been " + action
                             + " with a pressure of " + e.buttonPressure + " / Primary Touchpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)" + " / Secondary Touchpad axis at: " + e.touchpadTwoAxis + " (" + e.touchpadTwoAngle + " degrees)";
        //VRTK_Logger.Info(debugString);
        Debug.Log(debugString);
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (triggerButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "pressed", e);
            m_triggerPressed = true;
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (triggerButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
            m_triggerPressed = false;
        }
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (gripButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "pressed", e);
            m_gripPessed = true;
        }
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (gripButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "released", e);
            m_gripPessed = false;
        }
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (touchpadButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "pressed down", e);
            m_touchPadPressed = true;
        }
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (touchpadButtonEvents)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "released", e);
            m_touchPadPressed = false;
        }
    }
}
