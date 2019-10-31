using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small component script that will set a random position to the GameObject inside a 3D sphere
/// </summary>
public class randomPosition : MonoBehaviour {

    // radius of the 3D area sphere
    public float oscilationRange = 1.0f;
    protected Vector3 initPosition;
    public int rate = 10;
    // Use this for initialization
    void Start ()
    {
        initPosition = transform.position;
    }
    private int fixedUpdateCount = 0;
    // Update is called once per frame
    void FixedUpdate ()
    {
        fixedUpdateCount++;
        if (fixedUpdateCount == rate)
        {
            transform.position = initPosition + Random.insideUnitSphere * oscilationRange;
            fixedUpdateCount = 0;
        }
        
    }
}
