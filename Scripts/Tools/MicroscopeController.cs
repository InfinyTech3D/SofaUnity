using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MicroscopeController : MonoBehaviour
{


    public GameObject m_player;
    public Camera mainCam;
    public Camera microCam;

    private int rollMaxValue = 30;
    private int titleMaxValue = 30;
    private int panMaxValue = 30;

    private Vector2 xBounds = new Vector2(-0.13f, 0.05f);
    private Vector2 yBounds = new Vector2(1.6f, 1.85f);
    private Vector2 zBounds = new Vector2(-1.2f, -0.8f);

    private RenderTexture m_microTex = null;

    // Use this for initialization
    void Start()
    {
        //mainCam.enabled = true;
        //microCam.enabled = false;
        m_microTex = microCam.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        // translation code
        if (Input.GetKey(KeyCode.UpArrow))
            moveCameraUp(0.01f);
        else if (Input.GetKey(KeyCode.RightArrow))
            slideCamera(-0.01f);
        else if (Input.GetKey(KeyCode.LeftArrow))
            slideCamera(0.01f);
        else if (Input.GetKey(KeyCode.DownArrow))
            moveCameraUp(-0.01f);
        else if (Input.GetKey(KeyCode.O))
            moveCameraForward(0.01f);
        else if (Input.GetKey(KeyCode.L))
            moveCameraForward(-0.01f);


        // rotation code
        if (Input.GetKey(KeyCode.U))
            tiltCamera(0.5f);
        else if (Input.GetKey(KeyCode.J))
            tiltCamera(-0.5f);
        else if (Input.GetKey(KeyCode.H))
            roolCamera(0.5f);
        else if (Input.GetKey(KeyCode.K))
            roolCamera(-0.5f);
        else if (Input.GetKey(KeyCode.Y))
            panCamera(0.5f);
        else if (Input.GetKey(KeyCode.I))
            panCamera(-0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_player != null && other.gameObject == m_player)
        {
            Debug.Log("entered");

            microCam.targetTexture = null;
            mainCam.enabled = false;
            microCam.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_player != null && other.gameObject == m_player)
        {
            mainCam.enabled = true;
            microCam.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    public void zoomCamera(float value)
    {
        
    }

    public void focusCamera(float value)
    {

    }

    public void moveCameraUp(float value)
    {
        float position = transform.parent.localPosition.y + value;
        Debug.Log("slideCamera: " + position);
        if (position > yBounds[0] && position < yBounds[1])
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, position, transform.parent.localPosition.z);
    }

    public void moveCameraForward(float value)
    {
        float position = transform.parent.localPosition.x + value;
        Debug.Log("moveCameraForward: " + position);
        if (position > xBounds[0] && position < xBounds[1])
            transform.parent.localPosition = new Vector3(position, transform.parent.localPosition.y, transform.parent.localPosition.z);
    }

    public void slideCamera(float value)
    {
        float position = transform.parent.localPosition.z + value;
        Debug.Log("moveCameraUp: " + position);
        if (position > zBounds[0] && position < zBounds[1])
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, position);
    }

    // rotation on the side
    public void roolCamera(float value)
    {
        float newAngle = (transform.localEulerAngles.x + value);        
        if (newAngle < rollMaxValue || newAngle > 360 - rollMaxValue)
            transform.localEulerAngles = new Vector3(newAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    // rotate up/down
    public void tiltCamera(float value)
    {
        float newAngle = transform.localEulerAngles.y + value;
        if (newAngle < titleMaxValue || newAngle > 360 - titleMaxValue)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newAngle, transform.localEulerAngles.z);
    }

    public void panCamera(float value)
    {
        float newAngle = transform.localEulerAngles.z + value;
        if (newAngle < panMaxValue || newAngle > 360 - panMaxValue)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newAngle);
    }
}
