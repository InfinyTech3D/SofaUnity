using System;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Script to Add to a GameOject in order to move it using the keybord.
/// I = move forward
/// K = move backword
/// U = move left
/// O = move right
/// J = move Up
/// L = move down
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// Pointer to the current Mesh of the GameObject
    SBaseMesh m_mesh;
    void Start()
    {
        // Get the Mesh
        m_mesh = GetComponent<SBaseMesh>();
    }

    /// Method calle at each update, will move the object regardings keys pushed.
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.J))
            transform.position = transform.position - transform.right * 0.001f;
        else if (Input.GetKey(KeyCode.L))
            transform.position = transform.position + transform.right * 0.001f;
       
        else if (Input.GetKey(KeyCode.K))
        {
            transform.position = transform.position - transform.forward * 0.001f;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            transform.position = transform.position + transform.forward * 0.001f;
        }

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
