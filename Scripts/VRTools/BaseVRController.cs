using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVRController : MonoBehaviour {

    public GameObject m_controllerObject = null;
    public bool isActive = false;

    private Vector3 restPosition;

    // Use this for initialization
    void Start () {
        if (m_controllerObject == null)
        {
            Debug.LogError("VRController::Start - Controllers pointer not set.");
            this.enabled = false;
            return;
        }

        restPosition = m_controllerObject.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {

        //if (m_controllerObject.activeSelf)
        //{
        //    this.isActive = true;
        //    restPosition = m_controllerObject.transform.position;
        //}


        if (isActive)
        {
            Vector3 diffP = m_controllerObject.transform.position - restPosition;
            float normP = diffP.magnitude;

            if (normP < 0.01)
                return;

            Vector3 diffPInLoc = m_controllerObject.transform.worldToLocalMatrix * diffP;
            Vector3 diffPInLocNorm = diffPInLoc.normalized;
            
            float threshold = 0.5f;
            if (diffPInLocNorm.x > threshold || diffPInLocNorm.x < -threshold)
                movingForward(diffPInLoc.x);

            if (diffPInLocNorm.y > threshold || diffPInLocNorm.y < -threshold)
                movingUp(diffPInLoc.y);

            if (diffPInLocNorm.z > threshold || diffPInLocNorm.z < -threshold)
                movingSide(diffPInLoc.z);

            restPosition = m_controllerObject.transform.position;
        }

        

    }


    protected virtual void movingForward(float value)
    {
        Debug.Log("movingForward " + value);
    }

    protected virtual void movingUp(float value)
    {
        Debug.Log("movingUp " + value);
    }

    protected virtual void movingSide(float value)
    {
        Debug.Log("movingSide " + value);
    }

}
