using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPosition : MonoBehaviour {

    public float oscilationRange = 1.0f;
    protected Vector3 initPosition;
    // Use this for initialization
    void Start () {
        initPosition = transform.position;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = initPosition + Random.insideUnitSphere * oscilationRange;
    }
}
