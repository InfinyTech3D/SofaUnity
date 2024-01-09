using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolVRController : BaseVRController
{

    public GameObject m_ToolBase = null;
    public GameObject m_ToolHead = null;

    protected override void movingForward(float value)
    {
        if (m_ToolBase == null || m_ToolHead == null)
            return;

        Debug.Log("Tool movingForward " + value);
    }

    protected override void movingUp(float value)
    {
        if (m_ToolBase == null || m_ToolHead == null)
            return;

        Debug.Log("Tool movingUp " + value);
    }

    protected override void movingSide(float value)
    {
        if (m_ToolBase == null || m_ToolHead == null)
            return;

        Debug.Log("Tool movingSide " + value);
    }
}
