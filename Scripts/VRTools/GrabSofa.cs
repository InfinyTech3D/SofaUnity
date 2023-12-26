using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
    // New input system backends are enabled.
    using UnityEngine.InputSystem;
#endif


/// <summary>
/// Used to grab Sofa simulation with attach tools (1 on each hand)
/// </summary>
public class GrabSofa : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    /// <summary>
    /// Reference of the right and left brab button (here trigger)
    /// </summary>
    [SerializeField] private InputActionReference m_leftGripButton = null;
    [SerializeField] private InputActionReference m_rightGripButton = null;
#endif

    /// <summary>
    /// Reference to the right and left grab tool (Laser on attach mode)
    /// </summary>
    [SerializeField] private SofaLaserModel m_leftSofaLaserModel = null;
    [SerializeField] private SofaLaserModel m_rightSofaLaserModel = null;

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        m_leftGripButton.action.performed += ActivateLeftGrab;
        m_leftGripButton.action.canceled += DesactivateLeftGrab;
        m_rightGripButton.action.performed += ActivateRightGrab;
        m_rightGripButton.action.canceled += DesactivateRightGrab;
#endif
    }


    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        m_leftGripButton.action.performed -= ActivateLeftGrab;
        m_leftGripButton.action.canceled -= DesactivateLeftGrab;
        m_rightGripButton.action.performed -= ActivateRightGrab;
        m_rightGripButton.action.canceled -= DesactivateRightGrab;
#endif
    }

    #if ENABLE_INPUT_SYSTEM
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

#endif
}
