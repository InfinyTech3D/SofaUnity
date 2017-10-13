using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserRay : RayCaster {

    private GameObject laser;
    private LineRenderer lr;

    [SerializeField]
    public Color startColor = Color.red;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float startWidth = 1f;
    [SerializeField]
    public float endWidth = 0.5f;

    
    

    void OnValidate()
    {
        startWidth = Mathf.Max(0, startWidth);
        endWidth = Mathf.Max(0, endWidth);
    }

    // Use this for initialization
    void Start () {
        laser = new GameObject("Laser");
        laser.transform.parent = transform;
        initializeLR();
    }
	
	// Update is called once per frame
	void Update () {

        origin = transform.position;
        direction = transform.forward;     

        highlightTriangle();

        if (gotHit)
        {
            draw(transform.position, hit.point);
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }
    }

    private void draw(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


    private void initializeLR()
    {
        laser.AddComponent<LineRenderer>();
        lr = laser.GetComponent<LineRenderer>();
        //lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
#if UNITY_5_5 || UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1
        lr.numPositions = 2;
#else
        lr.positionCount = 2;
#endif

    }
}
