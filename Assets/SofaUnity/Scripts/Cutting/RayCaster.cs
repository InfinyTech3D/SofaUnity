using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used for raycast functionality
public class RayCaster : MonoBehaviour
{

    protected Vector3 origin;
    protected Vector3 direction;
    protected RaycastHit hit;
    public float length = 1f;
    public LayerMask mask;
    public bool checkBackfaces = false;
    private GameObject newTriangle;
    protected bool gotHit = false;
    private bool initialized = false;
   

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;
        direction = transform.forward;
    }

    //cast ray
    public virtual bool castRay()
    {
        if (checkBackfaces)
            gotHit = helpRay();
        else
            gotHit = Physics.Raycast(origin, direction, out hit, length, mask);

        return gotHit;
    }

    //function to handle backfaces and faces close to others when raycasting -> might cause performance issues depending on distance
    private bool helpRay()
    {
        Vector3 backwards = new Vector3(origin.x, origin.y, origin.z + length);
        RaycastHit lastHit = hit;
        //distance for sending a "backwards ray" -> changes when a triangle is hit
        float curDistance = length;
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
                if (Physics.Raycast(origin, direction, out lastHit, curDistance, mask))
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
            else if (Physics.Raycast(backwards, -direction, out lastHit, curDistance, mask))
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

    //casts ray and highlights hit triangle
    public virtual void highlightTriangle()
    {
        if (!castRay())
        {
            if (newTriangle != null)
                newTriangle.SetActive(false);
            return;
        }

        if (!initialized)
            initializeHighlighter();

        LineRenderer lr = newTriangle.GetComponent<LineRenderer>();

        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        //Vector2[] uvs = mesh.uv;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Vector3[] trianglePoints = new Vector3[3] { p0, p1, p2 };

        lr.SetPositions(trianglePoints);
        //Debug.DrawLine(p0, p1);
        //Debug.DrawLine(p1, p2);
        //Debug.DrawLine(p2, p0);
        newTriangle.SetActive(true);
    }

    public virtual void initializeHighlighter()
    {
        initialized = true;
        newTriangle = new GameObject("Highlighter");
        newTriangle.transform.parent = this.transform;
        LineRenderer lr = newTriangle.AddComponent<LineRenderer>();
        //lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.startWidth = 1.5f;
        lr.endWidth = 1.5f;
#if UNITY_5_5 || UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1
        lr.numPositions = 3;
#else
        lr.positionCount = 3;
        lr.loop = true;
#endif
    }
}