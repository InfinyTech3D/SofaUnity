using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to draw a laser from the tip of an object when another object is in front of it (raycast)
public class DrawLaser : MonoBehaviour {

    private GameObject laser;
    private LineRenderer lr;
    private RaycastHit touch;
    
    [SerializeField]
    public Color startColor = Color.red;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float startWidth = 1f;
    [SerializeField]
    public float endWidth = 0.5f;
    public float maxRayDistance = 100f;
    private int layerMask;

    void OnValidate()
    {
        layerMask = LayerMask.GetMask("Interactable");
        startWidth = Mathf.Max(0, startWidth);
        endWidth = Mathf.Max(0, endWidth);
    }

    // Use this for initialization
    void Start () {
        laser = new GameObject("Laser");
        laser.transform.parent = transform;
        InitializeRay();
	}
	
	// Update is called once per frame
	void Update () {

        var tip = transform.position;
        var backwardsTip = new Vector3(tip.x, tip.y, tip.z + maxRayDistance);

        if (Physics.Raycast(tip, transform.forward, out touch, maxRayDistance, layerMask) || Physics.Raycast(backwardsTip, -transform.forward, out touch, maxRayDistance, layerMask))
        {
            Draw(tip, touch.point);
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }
    }

    private void Draw(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


    private void InitializeRay()
    {
        laser.AddComponent<LineRenderer>();
        lr = laser.GetComponent<LineRenderer>();
        //lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
        lr.positionCount = 2;
    }
}
