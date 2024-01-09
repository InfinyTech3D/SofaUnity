using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For a given hand, enable / disable cutting tool
/// </summary>
public class TriggerLaser : MonoBehaviour
{
    // Determine the hand
    private enum Hand
    {
        LeftHand,
        RightHand,
    }
    [SerializeField]
    private Hand m_hand;

    /// <summary>
    /// cutting tool reference
    /// </summary>
    [SerializeField] private SofaLaserModel m_handCuttingLaser = null;

    private void Start()
    {
        // Desactivate tool on start
        m_handCuttingLaser.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the opposite hand touch the collider (white cube next to the hand )
        switch (m_hand)
        {
            case Hand.LeftHand: // opposite hand : right hand 
                if (other.tag == "RightTouch") // on the right index
                {
                    // change the state of the left cutting tool
                    m_handCuttingLaser.gameObject.SetActive(!m_handCuttingLaser.gameObject.activeSelf);
                    m_handCuttingLaser.ActivateTool = !m_handCuttingLaser.ActivateTool;
                }
                break;
            case Hand.RightHand: // opposite hand : left hand
                if (other.tag == "LeftTouch") // on the left index
                {
                    // change the state of the right cutting tool
                    m_handCuttingLaser.gameObject.SetActive(!m_handCuttingLaser.gameObject.activeSelf);
                    m_handCuttingLaser.ActivateTool = !m_handCuttingLaser.ActivateTool;
                }
                break;
        }
    }




}
