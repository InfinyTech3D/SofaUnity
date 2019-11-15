using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerController : MonoBehaviour
{
    public bool m_gripPessed = false;
    public bool m_triggerPressed = false;
    public bool m_touchPadPressed = false;

    public bool isGripPressed() { return m_gripPessed; }
    public bool isTriggerPressed() { return m_triggerPressed; }
    public bool isTouchPadPressed() { return m_touchPadPressed; }
}
