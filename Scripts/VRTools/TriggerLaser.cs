using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLaser : MonoBehaviour
{
    private enum Hand
    {
        LeftHand,
        RightHand,
    }
    [SerializeField]
    private Hand m_hand;

    [SerializeField] private SofaLaserModel m_handCuttingLaser = null;

    private void Start()
    {
        m_handCuttingLaser.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (m_hand)
        {
            case Hand.LeftHand:
                if (other.tag == "RightTouch")
                {
                    m_handCuttingLaser.gameObject.SetActive(!m_handCuttingLaser.gameObject.activeSelf);
                    m_handCuttingLaser.ActivateTool = !m_handCuttingLaser.ActivateTool;
                }
                break;
            case Hand.RightHand:
                if (other.tag == "LeftTouch")
                {
                    m_handCuttingLaser.gameObject.SetActive(!m_handCuttingLaser.gameObject.activeSelf);
                    m_handCuttingLaser.ActivateTool = !m_handCuttingLaser.ActivateTool;
                }
                break;
        }
    }




}
