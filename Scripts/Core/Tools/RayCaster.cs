using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class inherite from MonoBehavior that design a Ray casting object.
/// From a Vector3 origin to Vector3 direction and a float length.
/// </summary>
public class RayCaster : MonoBehaviour
{
    ////////////////////////////////////////////
    //////        RayCaster members        /////
    ////////////////////////////////////////////

    /// Ray Origin position in Unity world
    protected Vector3 m_origin;

    /// Ray direction in Unity world
    protected Vector3 m_direction;

    /// Ray length in Unity world
    [SerializeField]
    protected float m_length = 1f;

    ///Ray status, if is casting ray or not
    [SerializeField]
    protected bool m_activateRay = false;

    ///Ray internal status, if ray has well be init
    protected bool m_initialized = false;
    

    //old struct {
    protected RaycastHit hit;
    public LayerMask mask;
    public bool checkBackfaces = false;
    public bool useHighlight = false;
    //private GameObject newTriangle;
    protected bool gotHit = false;

    //}

    ////////////////////////////////////////////
    //////       RayCaster accessors       /////
    ////////////////////////////////////////////

    /// Getter/Setter of the ray origin \sa m_origin
    public Vector3 Origin
    {
        get { return m_origin; }
        set { m_origin = value; }
    }

    /// Getter/Setter of the ray direction \sa m_direction
    public Vector3 Direction
    {
        get { return m_direction; }
        set { m_direction = value; }
    }

    /// Getter/Setter of the ray length \sa m_length
    public float Length
    {
        get { return m_length; }
        set
        {

            if (value != m_length)
            {
                m_length = value;
            }
        }
    }

    /// Getter/Setter of the status \sa m_activateRay
    public bool ActivateRay
    {
        get { return m_activateRay; }
        set {
            if (m_activateRay != value)
            {
                m_activateRay = value;
                if (m_activateRay)
                    StartRay();
                else
                    StopRay();
            }
        }
    }


    ////////////////////////////////////////////
    //////      RayCaster public API       /////
    ////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad0))
            StartRay();
        else if (Input.GetKey(KeyCode.Keypad1))
            StopRay();


        if (m_initialized && m_activateRay)
        {
            //m_origin = transform.position;
            //m_direction = transform.forward;
            CastRay();
        }
    }

    /// Public Method to start this Ray casting \sa m_activateRay
    public virtual void StartRay()
    {
        Debug.Log("StartRay");
        m_activateRay = true;
    }

    /// Public Method to stop this Ray casting \sa m_activateRay
    public virtual void StopRay()
    {
        Debug.Log("StopRay");
        m_activateRay = false;
    }

    
    /// Main method to cast ray
    public virtual bool CastRay()
    {
        if (checkBackfaces)
            gotHit = HelpRay();
        else
            gotHit = Physics.Raycast(m_origin, m_direction, out hit, m_length, mask);

        if (useHighlight)
            HighlightTriangle();

        return gotHit;
    }




    ////////////////////////////////////////////
    //////     RayCaster internal API      /////
    ////////////////////////////////////////////

    //function to handle backfaces and faces close to others when raycasting -> might cause performance issues depending on distance
    private bool HelpRay()
    {
        //Vector3 backwards = new Vector3(origin.x, origin.y, origin.z + length);
        //RaycastHit lastHit = hit;
        ////distance for sending a "backwards ray" -> changes when a triangle is hit
        //float curDistance = length;
        //bool back = false;
        //bool hitRay = false;
        ////true if there is no more hits inbetween the last hit triangle and the starting position
        //bool noMoreHits = false;

        //while (!noMoreHits)
        //{
        //    //sending ray forwards once, then only backwards
        //    if (!back)
        //    {
        //        //CASE 1: hit front face -> save that triangle but check whether another backface is inbetween
        //        if (Physics.Raycast(origin, direction, out lastHit, curDistance, mask))
        //        {
        //            back = true;
        //            hitRay = true;
        //            hit = lastHit;

        //            //new distance the ray has to be sent back from
        //            curDistance = lastHit.distance;
        //        }
        //        //CASE 2: hit no front face -> check for backfaces
        //        else
        //        {
        //            back = true;
        //        }
        //    }
        //    //sending ray backwards
        //    else if (Physics.Raycast(backwards, -direction, out lastHit, curDistance, mask))
        //    {
        //        hitRay = true;
        //        hit = lastHit;
        //        curDistance -= lastHit.distance;
        //        backwards = lastHit.point;
        //    }
        //    else
        //        noMoreHits = true;
        //}
        //return hitRay;
        return false;
    }

    //casts ray and highlights hit triangle
    public virtual void HighlightTriangle()
    {
        //if (!gotHit)
        //{
        //    if (newTriangle != null)
        //        newTriangle.SetActive(false);
        //    return;
        //}

        //if (!initialized)
        //    initializeHighlighter();

        //LineRenderer lr = newTriangle.GetComponent<LineRenderer>();

        //MeshCollider meshCollider = hit.collider as MeshCollider;

        //if (meshCollider == null || meshCollider.sharedMesh == null)
        //    return;

        //Renderer rend = hit.transform.GetComponent<Renderer>();

        //Mesh mesh = meshCollider.sharedMesh;
        //Vector3[] vertices = mesh.vertices;
        //int[] triangles = mesh.triangles;
        ////Vector2[] uvs = mesh.uv;
        //Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        //Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        //Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        //Transform hitTransform = hit.collider.transform;
        //p0 = hitTransform.TransformPoint(p0);
        //p1 = hitTransform.TransformPoint(p1);
        //p2 = hitTransform.TransformPoint(p2);
        //Vector3[] trianglePoints = new Vector3[3] { p0, p1, p2 };

        //lr.SetPositions(trianglePoints);
        ////Debug.DrawLine(p0, p1);
        ////Debug.DrawLine(p1, p2);
        ////Debug.DrawLine(p2, p0);
        //newTriangle.SetActive(true);
    }

    public virtual void InitializeHighlighter()
    {
//        initialized = true;
//        newTriangle = new GameObject("Highlighter");
//        newTriangle.transform.parent = this.transform;
//        LineRenderer lr = newTriangle.AddComponent<LineRenderer>();
//        //lr.useWorldSpace = false;
//        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
//        lr.startColor = Color.green;
//        lr.endColor = Color.green;
//        lr.startWidth = 1.5f;
//        lr.endWidth = 1.5f;
//#if UNITY_5_5 || UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1
//        lr.numPositions = 3;
//#else
//        lr.positionCount = 3;
//        lr.loop = true;
//#endif
    }
}