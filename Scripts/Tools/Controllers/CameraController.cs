using UnityEngine;
using System.Collections;

/// <summary>
/// Script to be added to a Camera in order to change the camera direction using the keybord or callbacks.
/// A = turn left
/// S = look down
/// D = turn right
/// W = look up
/// </summary>
public class CameraController : MonoBehaviour 
{
    /// factor to change the rotation speed of the camera.
    public float m_rotationFactor = 0.05f;

    /// factor to change the movement speed of the camera.
    public float m_moveFactor = 0.001f;

    /// Use this for initialization
    void Start () 
	{

	}
	

    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void zoomCamera()
    {
        transform.position = transform.position + transform.forward * m_moveFactor;
    }

    /// Callback Method that can be linked in Unity GUI to unzoom this camera
    public void unZoomCamera()
    {
        transform.position = transform.position - transform.forward * m_moveFactor;
    }


    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void rotateCameraUp()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + m_rotationFactor, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// Callback Method that can be linked in Unity GUI to zoom this camera
    public void rotateCameraDown()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - m_rotationFactor, transform.eulerAngles.y, transform.eulerAngles.z);
    }



    /// Update is called once per frame
    void Update () 
	{
        if (Input.GetKey(KeyCode.A))
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.5f, transform.eulerAngles.z);
        else if (Input.GetKey(KeyCode.D))
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 0.5f, transform.eulerAngles.z);
        else if (Input.GetKey(KeyCode.W))
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
        else if (Input.GetKey(KeyCode.S))
            transform.eulerAngles = new Vector3(transform.eulerAngles.x - 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
