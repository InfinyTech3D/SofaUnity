using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTool : RayCaster {

    //CuttingTool based on RayCaster
    //TODO: Output cutting area based on depth and width value, a line defined by 2+ points on the surface hit by the raycast and a normal
    
    private GameObject hitObject;
    [Tooltip("Used if you want to delete the old and create a new collider. WARNING: Can create lags!")]
    public bool createCollider = false;
    [Tooltip("Used for constant collider creation. WARNING: Might be slow!")]
    public bool constantColliderUpdate = false;
    private List<GameObject> hitObjects = new List<GameObject>();
    [Tooltip("Value defining the depth of the cut.")]
    public float depth = 1f;
    [Tooltip("Value defining the width of the cut.")]
    public float width = 1f;

    private List<Vector3> points = new List<Vector3>();

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        m_origin = transform.position;
        m_direction = transform.forward;

        if (!CastRay())
            return;
        
        if (hit.triangleIndex != -1)
        {
            hitObject = hit.collider.gameObject;

            bool found = false;

            //save hit objects for collider creation/deletion
            //don't add object if it is already in list
            for (int i = 0; i < hitObjects.Count; i++)
            {
                if (hitObject == hitObjects[i])
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                hitObjects.Add(hitObject);

            //add new hit point
            points.Add(hit.point);

            if (points.Count == 2)
                calculateArea();


        }
    }

    private void calculateArea()
    {

        points.RemoveAt(0);
    }

    //resets the colliders to match their current mesh by destroying the old one and adding a new one
    public void resetCollider()
    {
        for (int i = 0; i < hitObjects.Count; i++)
        {
            Destroy(hitObjects[i].GetComponent<MeshCollider>());
            hitObjects[i].AddComponent<MeshCollider>();
        }
        hitObjects.Clear();
        hitObjects = new List<GameObject>();

        if (!constantColliderUpdate)
            createCollider = false;
    }
}
