using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArmVRController : BaseVRController
{
    public GameObject m_CArmBase = null;
    public GameObject m_CArmHead = null;

    protected override void movingForward(float value)
    {
        if (m_CArmBase == null || m_CArmHead == null)
            return;

        Debug.Log("CArm movingForward " + value);
    }

    protected override void movingUp(float value)
    {
        if (m_CArmBase == null || m_CArmHead == null)
            return;

        Debug.Log("CArm movingUp " + value);
    }

    protected override void movingSide(float value)
    {
        if (m_CArmBase == null || m_CArmHead == null)
            return;

        Debug.Log("CArm movingSide " + value);
    }

    public void rotateArmRight()
    {
        m_CArmHead.transform.eulerAngles = new Vector3(m_CArmHead.transform.eulerAngles.x, m_CArmHead.transform.eulerAngles.y, m_CArmHead.transform.eulerAngles.z+0.5f);
    }

    public void rotateArmLeft()
    {
        m_CArmHead.transform.eulerAngles = new Vector3(m_CArmHead.transform.eulerAngles.x, m_CArmHead.transform.eulerAngles.y, m_CArmHead.transform.eulerAngles.z - 0.5f);
    }

    public void rotateArmfront()
    {
        m_CArmHead.transform.eulerAngles = new Vector3(m_CArmHead.transform.eulerAngles.x - 0.5f, m_CArmHead.transform.eulerAngles.y, m_CArmHead.transform.eulerAngles.z);
    }

    public void rotateArmBack()
    {
        m_CArmHead.transform.eulerAngles = new Vector3(m_CArmHead.transform.eulerAngles.x + 0.5f, m_CArmHead.transform.eulerAngles.y, m_CArmHead.transform.eulerAngles.z);
    }

    public void moveArmDown()
    {
        m_CArmBase.transform.position = new Vector3(m_CArmBase.transform.position.x, m_CArmBase.transform.position.y, m_CArmBase.transform.position.z - 0.01f);
    }

    public void moveArmUp()
    {
        m_CArmBase.transform.position = new Vector3(m_CArmBase.transform.position.x, m_CArmBase.transform.position.y, m_CArmBase.transform.position.z + 0.01f);
    }
}
