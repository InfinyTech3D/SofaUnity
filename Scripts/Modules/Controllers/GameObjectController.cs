using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class GameObjectController : ObjectController
{
    /// Pointer to the current Mesh of the GameObject
    public GameObject light = null;
    public GameObject otherTool = null;
    public SLaserRay toolImpl = null;
    
    public bool m_isactive = false;
    protected GameObjectController otherObjectCtrl = null;

    public float factor = 0.1f;
    void Start()
    {
        if (light)
            light.SetActive(m_isactive);

        if (otherTool)
            otherObjectCtrl = otherTool.GetComponent<GameObjectController>();
    }

    /// Method calle at each update, will move the object regardings keys pushed.
    void FixedUpdate()
    {
        if (!m_isactive)
            return;

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.Keypad4))
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.5f, transform.eulerAngles.z);
            else if (Input.GetKey(KeyCode.Keypad6))
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 0.5f, transform.eulerAngles.z);
            else if (Input.GetKey(KeyCode.Keypad8))
                transform.eulerAngles = new Vector3(transform.eulerAngles.x + 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            else if (Input.GetKey(KeyCode.Keypad2))
                transform.eulerAngles = new Vector3(transform.eulerAngles.x - 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            else if (Input.GetKey(KeyCode.Keypad5))
                transform.position = transform.position - transform.forward * factor;
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
                transform.position = transform.position + transform.up * factor;
            else if (Input.GetKey(KeyCode.Keypad2))
                transform.position = transform.position - transform.up * factor;
            else if (Input.GetKey(KeyCode.Keypad4))
                transform.position = transform.position - transform.right * factor;
            else if (Input.GetKey(KeyCode.Keypad6))
                transform.position = transform.position + transform.right * factor;
            else if (Input.GetKey(KeyCode.Keypad5))
                transform.position = transform.position + transform.forward * factor;
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                factor += 0.01f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (factor > 0.02f)
                    factor -= 0.01f;
            }
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (light)
            {
                Light lt = light.GetComponent<Light>();
                lt.color = Color.red;
            }

            if (toolImpl)
                toolImpl.ActivateTool = true;
        }
        else if (Input.GetKey(KeyCode.V))
        {
            if (light)
            {
                Light lt = light.GetComponent<Light>();
                lt.color = Color.green;
            }

            if (toolImpl)
                toolImpl.ActivateTool = false;
        }
    }

    void activateTool(bool value)
    {
        m_isactive = value;
        if (light)
            light.SetActive(m_isactive);
    }

    public bool isToolActive()
    {
        return m_isactive;
    }

    void OnMouseDown()
    {
        activateTool(m_isactive = !m_isactive);

        if (m_isactive && otherObjectCtrl && otherObjectCtrl.isToolActive())
            otherObjectCtrl.activateTool(false);
    }
}
