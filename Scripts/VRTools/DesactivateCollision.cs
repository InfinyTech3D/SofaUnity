using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
    // New input system backends are enabled.
    using UnityEngine.InputSystem;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
// Old input backends are enabled.
#endif


/// <summary>
/// Disable / enable collision of the grabbing hand   
/// </summary>
public class DesactivateCollision : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    /// <summary>
    /// grip with trgger button seems more natural
    /// </summary>
    [SerializeField] private InputActionReference m_trigger;
#endif

    /// <summary>
    /// reference of SofaSphereCollisionHand hand 
    /// </summary>
    [SerializeField] private SofaSphereCollisionHand m_sofaSphereCollisionHand;

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        m_trigger.action.performed += Desactivate;
        m_trigger.action.canceled += Activate;
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        m_trigger.action.performed -= Desactivate;
        m_trigger.action.canceled -= Activate;
#endif
    }

#if ENABLE_INPUT_SYSTEM
    /// <summary>
    /// Disable collision of the grbbintg hand
    /// </summary>
    /// <param name="obj"></param>
    private void Desactivate(InputAction.CallbackContext obj)
    {
        m_sofaSphereCollisionHand.SofaSphereCollision.Activated = false;
    }

    /// <summary>
    /// Enable collisions of the the grabbing hand
    /// </summary>
    /// <param name="obj"></param>
    private void Activate(InputAction.CallbackContext obj)
    {
        m_sofaSphereCollisionHand.SofaSphereCollision.Activated = true;
    }
#endif

    /// <summary>
    /// Setter for reference the SofaSphereCollisionHand of the hand
    /// </summary>
    /// <param name="value"></param>
    public void SetSofaSphereCollisionHand(SofaSphereCollisionHand value)
    {
        m_sofaSphereCollisionHand = value;
    }

}
