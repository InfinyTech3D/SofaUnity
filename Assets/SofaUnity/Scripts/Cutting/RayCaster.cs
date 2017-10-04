using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used for raycast functionality
public class RayCaster : MonoBehaviour {

    protected Vector3 origin;
    protected Vector3 direction;
    protected RaycastHit hit;
    public float length = 1f;
    public LayerMask mask;
    public bool checkBackfaces = false;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        origin = transform.position;
        direction = transform.forward;
	}

    public virtual bool CastRay()
    {
        if (checkBackfaces)
            return helpRay();
        else
            return Physics.Raycast(origin, direction, out hit, length, mask);
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
}