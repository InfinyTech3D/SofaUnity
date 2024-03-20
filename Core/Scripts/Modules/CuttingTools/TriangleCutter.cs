using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCutter : RayCaster
{

    private GameObject hitObject;
    [Tooltip("Used if you want to delete the old and create a new collider. WARNING: Can create lags!")]
    public bool createCollider = false;
    [Tooltip("Used for constant collider creation. WARNING: Might be slow!")]
    public bool constantColliderUpdate = false;
    private List<GameObject> hitObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_origin = transform.position;
        m_direction = transform.forward;

        //highlightTriangle();
        CastRay();

        cutTriangles();

        resetCollider();
    }

    public virtual void cutTriangles()
    {
        if (!gotHit || hit.triangleIndex == -1)
            return;

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

        int hitTri = hit.triangleIndex;

        //set area of the hit triangle to zero
        emptyTriangle(hitTri);
    }

    //sets all 3 triangle vertices to the same vertex index so that the triangle does have zero area
    //collider still remains the same unless one destroys the old and adds a new collider -> short lag
    public virtual void emptyTriangle(int numTriangle)
    {
        Mesh mesh = hitObject.transform.GetComponent<MeshFilter>().mesh;
        int[] newTriangles = mesh.triangles;
        int triIndex = numTriangle * 3;
        newTriangles[triIndex + 1] = mesh.triangles[triIndex];
        newTriangles[triIndex + 2] = mesh.triangles[triIndex];

        hitObject.transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
    }

    //resets the colliders to match their current mesh by destroying the old one and adding a new one
    public virtual void resetCollider()
    {
        if (!constantColliderUpdate && !createCollider)
            return;

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
