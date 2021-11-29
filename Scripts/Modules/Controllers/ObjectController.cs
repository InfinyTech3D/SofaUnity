using System;
using UnityEngine;

/// <summary>
/// Script to Add to a GameOject in order to move it using the keybord.

public class ObjectController : MonoBehaviour
{
    /// factor to change the rotation speed of the camera.
    public float m_rotationFactor = 0.05f;

    /// factor to change the movement speed of the camera.
    public float m_moveFactor = 0.001f;


    /// Callback Method that can be linked in Unity GUI to zoom object 
    public void zoom()
    {
        transform.position = transform.position + transform.forward * m_moveFactor;
    }

    /// Callback Method that can be linked in Unity GUI to unzoom object
    public void unZoom()
    {
        transform.position = transform.position - transform.forward * m_moveFactor;
    }


    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void rotateUp()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - m_rotationFactor, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void rotateDown()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + m_rotationFactor, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void rotateLeft()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - m_rotationFactor, transform.eulerAngles.z);
    }

    public void rotateRight()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + m_rotationFactor, transform.eulerAngles.z);
    }



    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void moveUp()
    {
        transform.position = transform.position + transform.up * m_moveFactor;
    }

    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void moveDown()
    {
        transform.position = transform.position - transform.up * m_moveFactor;
    }

    public void moveLeft()
    {
        transform.position = transform.position - transform.right * m_moveFactor;
    }

    public void moveRight()
    {
        transform.position = transform.position + transform.right * m_moveFactor;
    }

}
