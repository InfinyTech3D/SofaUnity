using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used to grab Sofa simulation with attach tools (1 on each hand)
/// </summary>
public class GrabSofa : MonoBehaviour
{
    /// <summary>
    /// Reference of the right and left brab button (here trigger)
    /// </summary>
    [SerializeField] private InputActionReference m_leftGripButton = null;
    [SerializeField] private InputActionReference m_rightGripButton = null;

    /// <summary>
    /// Reference to the right and left grab tool (Laser on attach mode)
    /// </summary>
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

    /// <summary>
    /// Activate left grab 
    /// </summary>
    /// <param name="obj"></param>
    private void ActivateLeftGrab(InputAction.CallbackContext obj)
    {
        m_leftSofaLaserModel.ActivateTool = true;
    }

    /// <summary>
    /// Desactivate left grab 
    /// </summary>
    /// <param name="obj"></param>
    private void DesactivateLeftGrab(InputAction.CallbackContext obj)
    {
        m_leftSofaLaserModel.ActivateTool = false;
    }

    /// <summary>
    /// Activate right grab 
    /// </summary>
    /// <param name="obj"></param>
    private void ActivateRightGrab(InputAction.CallbackContext obj)
    {
        m_rightSofaLaserModel.ActivateTool = true;
    }

    /// <summary>
    /// Desactivate right grab 
    /// </summary>
    /// <param name="obj"></param>
    private void DesactivateRightGrab(InputAction.CallbackContext obj)
    {
        m_rightSofaLaserModel.ActivateTool = false;
    }


}
