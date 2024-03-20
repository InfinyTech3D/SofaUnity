using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerController : MonoBehaviour
{
    public bool m_gripPessed = false;
    public bool m_triggerPressed = false;
    public bool m_touchPadPressed = false;

    public bool m_buttonOnePressed = false;
    public bool m_buttonTwoPressed = false;

    public bool isGripPressed() { return m_gripPessed; }
    public bool isTriggerPressed() { return m_triggerPressed; }
    public bool isTouchPadPressed() { return m_touchPadPressed; }
    public bool isButtonOnePressed() { return m_buttonOnePressed; }
    public bool isButtonTwoPressed() { return m_buttonTwoPressed; }
}
