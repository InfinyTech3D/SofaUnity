using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesactivateCollision : MonoBehaviour
{
    [SerializeField] private InputActionReference m_trigger;
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


    private void Desactivate(InputAction.CallbackContext obj)
    {
        m_sofaSphereCollisionHand.SofaSphereCollision.Activated = false;
    }

    private void Activate(InputAction.CallbackContext obj)
    {
        m_sofaSphereCollisionHand.SofaSphereCollision.Activated = true;
    }

    public void SetSofaSphereCollisionHand(SofaSphereCollisionHand value)
    {
        m_sofaSphereCollisionHand = value;
    }

}
