using UnityEngine;
using System.Collections;

/// <summary>
/// Script to be added to a Camera in order to change the camera direction using the keybord or callbacks.
/// A = turn left
/// S = look down
/// D = turn right
/// W = look up
/// </summary>
public class CameraController : ObjectController
{
   
    /// Use this for initialization
    void Start () 
	{

	}
	

    /// Update is called once per frame
    void FixedUpdate() 
	{
        if (Input.GetKey(KeyCode.Keypad8))
            moveUp();
        else if (Input.GetKey(KeyCode.Keypad5))
            moveDown();
        else if (Input.GetKey(KeyCode.Keypad4))
            moveLeft();
        else if (Input.GetKey(KeyCode.Keypad6))
            moveRight();
        else if (Input.GetKey(KeyCode.Keypad7))
            rotateUp();
        else if (Input.GetKey(KeyCode.Keypad9))
            rotateDown();
        else if (Input.GetKey(KeyCode.Keypad1))
            rotateLeft();
        else if (Input.GetKey(KeyCode.Keypad3))
            rotateRight();
        else if (Input.GetKey(KeyCode.KeypadPlus))
            zoom();
        else if (Input.GetKey(KeyCode.KeypadMinus))
            unZoom();
    }
}
