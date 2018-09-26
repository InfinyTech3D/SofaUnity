using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeController : MonoBehaviour {


    public GameObject m_player;
    public Camera cam1;
    public Camera cam2;
 
    // Use this for initialization
    void Start () {
        cam1.enabled = true;
        cam2.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (m_player != null && other.gameObject == m_player)
        {
            Debug.Log("entered");
            cam1.enabled = false;
            cam2.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_player != null && other.gameObject == m_player)
        {
            cam1.enabled = true;
            cam2.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
