using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Disable / enable collision of the grabbing hand   
/// </summary>
public class DesactivateCollision : MonoBehaviour
{
    /// <summary>
    /// grip with trgger button seems more natural
    /// </summary>
    [SerializeField] private InputActionReference m_trigger;

    /// <summary>
    /// reference of SofaSphereCollisionHand hand 
    /// </summary>
    [SerializeField] private SofaSphereCollisionHand m_sofaSphereCollisionHand;

    private void OnEnable()
    {
        m_trigger.action.performed += Desactivate;
        m_trigger.action.canceled += Activate;
    }

    private void OnDisable()
    {
        m_trigger.action.performed -= Desactivate;
        m_trigger.action.canceled -= Activate;
    }

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

    /// <summary>
    /// Setter for reference the SofaSphereCollisionHand of the hand
    /// </summary>
    /// <param name="value"></param>
    public void SetSofaSphereCollisionHand(SofaSphereCollisionHand value)
    {
        m_sofaSphereCollisionHand = value;
    }

}
