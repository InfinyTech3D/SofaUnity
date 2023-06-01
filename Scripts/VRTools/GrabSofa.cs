using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabSofa : MonoBehaviour
{
    [SerializeField] private InputActionReference m_leftGripButton = null;
    [SerializeField] private InputActionReference m_rightGripButton = null;

    [SerializeField] private SofaLaserModel m_leftSofaLaserModel = null;
    [SerializeField] private SofaLaserModel m_rightSofaLaserModel = null;

    private void OnEnable()
    {
        m_leftGripButton.action.performed += ActivateLeftGrab;
        m_leftGripButton.action.canceled += DesactivateLeftGrab;
        m_rightGripButton.action.performed += ActivateRightGrab;
        m_rightGripButton.action.canceled += DesactivateRightGrab;
    }


    private void OnDisable()
    {
        m_leftGripButton.action.performed -= ActivateLeftGrab;
        m_leftGripButton.action.canceled -= DesactivateLeftGrab;
        m_rightGripButton.action.performed -= ActivateRightGrab;
        m_rightGripButton.action.canceled -= DesactivateRightGrab;

    }

    private void ActivateLeftGrab(InputAction.CallbackContext obj)
    {
        m_leftSofaLaserModel.ActivateTool = true;
    }
    private void DesactivateLeftGrab(InputAction.CallbackContext obj)
    {
        m_leftSofaLaserModel.ActivateTool = false;
    }


    private void ActivateRightGrab(InputAction.CallbackContext obj)
    {
        m_rightSofaLaserModel.ActivateTool = true;
    }
    private void DesactivateRightGrab(InputAction.CallbackContext obj)
    {
        m_rightSofaLaserModel.ActivateTool = false;
    }


}
