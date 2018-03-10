using UnityEngine;
using System.Collections;

/// <summary>
/// Script to Add to a Camera in order to change the camera direction using the keybord.
/// A = turn left
/// S = look down
/// D = turn right
/// W = look up
/// </summary>
public class CameraController : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
	}
	
    public void zoomCamera()
    {
        transform.position = transform.position + transform.forward * 0.001f;
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f);
    }

    public void unZoomCamera()
    {
        transform.position = transform.position - transform.forward * 0.001f;
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
    }


    public void rotateCameraUp()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 0.05f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void rotateCameraDown()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - 0.05f, transform.eulerAngles.y, transform.eulerAngles.z);
    }



    // Update is called once per frame
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
