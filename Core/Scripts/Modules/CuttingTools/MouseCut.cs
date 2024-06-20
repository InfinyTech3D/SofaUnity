using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for cutting via mouse input
public class MouseCut : MonoBehaviour
{

    private RaycastHit touch;
    private GameObject hitObject;
    [Tooltip("Used if you want to delete the old and create a new collider. WARNING: Creates lags!")]
    public bool createCollider = false;
    private List<GameObject> hitObjects;
    //maximal distance the ray gets sent to -> normally rather close, here very far
    //only gets used if version that handles backfaces is used
    private int layerMask = 10;

    // Use this for initialization
    void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
        hitObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //delete via middle mouse button
        if (Input.GetMouseButton(2))
        {
            //get ray from current mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //version without handling backfaces
            if (!Physics.Raycast(ray, out touch, layerMask))
                return;

            if (touch.triangleIndex != -1)
            {
                hitObject = touch.collider.gameObject;

                bool found = false;

                //save hit objects for collider creation/deletion
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

                int hitTri = touch.triangleIndex;

                //set area of the hit triangle to zero
                emptyTriangle(hitTri);
            }

        }
    }

    //sets all 3 triangle vertices to the same vertex index so that the triangle does have zero area
    //collider still remains the same unless one destroys the old and adds a new collider which might result in performance issues
    void emptyTriangle(int numTriangle)
    {
        Mesh mesh = hitObject.transform.GetComponent<MeshFilter>().mesh;
        int[] newTriangles = mesh.triangles;
        int triIndex = numTriangle * 3;
        newTriangles[triIndex + 1] = mesh.triangles[triIndex];
        newTriangles[triIndex + 2] = mesh.triangles[triIndex];

        hitObject.transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;


        //for collider deletion/creation -> still too slow for high-res meshes
        //resetCollider();

        //use if constant collider creation/deletion is too slow -> update only when set to true through editor
        if (createCollider)
        {
            resetCollider();
            createCollider = false;
        }
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
    }

    //function to handle backfaces and faces close to others when raycasting -> might cause performance issues depending on distance
    private bool rayHelper(Vector3 origin, Vector3 direction, ref RaycastHit hit, float maxDistance)
    {
        Vector3 backwards = new Vector3(origin.x, origin.y, origin.z + maxDistance);
        RaycastHit lastHit = hit;
        //distance for sending a "backwards ray" -> changes when a triangle is hit
        float curDistance = maxDistance;
        bool back = false;
        bool hitRay = false;
        //true if there is no more hits inbetween the last hit triangle and the starting position
        bool noMoreHits = false;

        while (!noMoreHits)
        {
            //sending ray forwards once, then only backwards
            if (!back)
            {
                //CASE 1: hit front face -> save that triangle but check whether another backface is inbetween
                if (Physics.Raycast(origin, direction, out lastHit, curDistance, layerMask))
                {
                    back = true;
                    hitRay = true;
                    hit = lastHit;

                    //new distance the ray has to be sent back from
                    curDistance = lastHit.distance;
                }
                //CASE 2: hit no front face -> check for backfaces
                else
                {
                    back = true;
                }
            }
            //sending ray backwards
            else if (Physics.Raycast(backwards, -direction, out lastHit, curDistance, layerMask))
            {
                hitRay = true;
                hit = lastHit;
                curDistance -= lastHit.distance;
                backwards = lastHit.point;
            }
            else
                noMoreHits = true;
        }
        return hitRay;
    }
}