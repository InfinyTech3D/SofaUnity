using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MagnifyingGlass : MonoBehaviour
{
    public Camera mainCam = null;
    protected Transform holderT = null;

    //Vector3 initDir;


    // Start is called before the first frame update
    void Start()
    {
        //initDir = this.transform.forward;
        holderT = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam == null)
            return;

        Vector3 dir = this.transform.position - mainCam.transform.position;
        Vector3 dirLoc = holderT.transform.InverseTransformVector(dir);
        dirLoc.Normalize();
        dirLoc.y *= -1;

        
        //Vector3 eulers = holderT.transform.eulerAngles;
        //eulers.y = 0;
        //eulers.x *= -1;
        //Debug.Log("eulers " + eulers);
        Quaternion rot = Quaternion.Euler(0, 0, -35);
        dir = rot * dir;
        //dir += initDir;
        //dir.Normalize();
        //this.transform.LookAt(this.transform.position + dir);

        //this.transform.up = holderT.up;
        this.transform.forward = holderT.transform.TransformVector(dirLoc);
        //Vector3 fwd = holderT.transform.TransformVector(dirLoc);
        //Vector3 right = holderT.right;
        //Vector3 upVec = Vector3.Cross(fwd, right);
        //this.transform.up = upVec;

        //this.transform.right = xVec;
        //Debug.Log("euler " + this.transform.eulerAngles);
    }

    void OnDrawGizmosSelected()
    {
        if (mainCam == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(mainCam.transform.position, this.transform.position);
        //Gizmos.color = Color.red;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward*10);

    }
}
