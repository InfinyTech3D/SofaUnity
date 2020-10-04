using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to draw a laser from the tip of an object when another object is in front of it (raycast)
public class DrawLaser : MonoBehaviour {

    private GameObject laser;
    private LineRenderer lr;
    private RaycastHit touch;
    
    [SerializeField]
    public Color startColor = Color.green;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float startWidth = 0.01f;
    [SerializeField]
    public float endWidth = 0.005f;
    //private int layerMask;

    void OnValidate()
    {
        //layerMask = LayerMask.GetMask("Interactable");
        startWidth = Mathf.Max(0, startWidth);
        endWidth = Mathf.Max(0, endWidth);
    }

    // Use this for initialization
    void Start () {
        laser = new GameObject("Laser");
        laser.transform.parent = transform;
        initializeRay();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        var tip = transform.position;
        var backwardsTip = new Vector3(tip.x, tip.y, tip.z + maxRayDistance);

        if (Physics.Raycast(tip, transform.forward, out touch, maxRayDistance, layerMask) || Physics.Raycast(backwardsTip, -transform.forward, out touch, maxRayDistance, layerMask))
        {
            draw(tip, touch.point);
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }*/
    }

    public void draw(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void updateLaser()
    {
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
    }


    private void initializeRay()
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
