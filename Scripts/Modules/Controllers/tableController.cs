using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class tableController : MonoBehaviour {

    private SofaContext sofaContext = null;

    // Use this for initialization
    void Start()
    {
        // find SofaContext and register this object
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            if (sofaContext == null)
            {
                Debug.LogError("tableController::Start - sofaContext not found");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.U))
            rotateXMore();
        if (Input.GetKey(KeyCode.J))
            rotateXLess();


    }

    void rotateXMore()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x + 0.5f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        if (sofaContext != null)
            sofaContext.Gravity = new Vector3(this.transform.forward.x * 10.0f, this.transform.forward.y * 10.0f, this.transform.forward.z * 200.0f);
        //Debug.Log(this.transform.forward);
    }

    void rotateXLess()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x - 0.5f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }
}
