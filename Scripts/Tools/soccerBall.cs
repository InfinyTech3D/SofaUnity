using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soccerBall : MonoBehaviour {

    private Vector3 restPos;
    private Quaternion restRot;
    private Rigidbody rb;
    public float force = 1.0f;
    private bool flying = false;
    // Use this for initialization
    void Start ()
    {
        restPos = this.transform.position;
        restRot = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C))
        {
            this.transform.position = restPos;
            this.transform.rotation = restRot;
            rb.velocity = Vector3.zero;
            flying = false;
        }



        if (Input.GetKey(KeyCode.V) && !flying)
        {
            Debug.Log("Shoot!");
            Vector3 movement = this.transform.up * 2 + this.transform.forward * 3;
            rb.AddForce(movement * force * 100);
            flying = true;
        }

    }
    
}
