using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganTouched : MonoBehaviour {

    public bool isTouched = false;
    private bool oldTouched = false;

    public Material defaultMat;
    public Material touchedMat;

    public Material visuNewdefaultMat;
    public Material visuOlddefaultMat;
    public Material visutouchedMat;

    public GameObject newVisu1 = null;
    public GameObject newVisu2 = null;
    public GameObject newVisu3 = null;

    public GameObject oldVisu1 = null;
    public GameObject oldVisu2 = null;
    public GameObject oldVisu3 = null;
    // Use this for initialization
    void Start ()
    {
        //defaultTexture = this.GetComponent<Renderer>().material.mainTexture;
        //touchedTexture = (Texture2D)Resources.Load("clouds1024");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (isTouched && !oldTouched) // new touched
        {
            oldTouched = true;
            this.GetComponent<Renderer>().material = touchedMat;
            if (newVisu1) newVisu1.GetComponent<Renderer>().material = visutouchedMat;
            if (newVisu2) newVisu2.GetComponent<Renderer>().material = visutouchedMat;
            if (newVisu3) newVisu3.GetComponent<Renderer>().material = visutouchedMat;

            if (oldVisu1) oldVisu1.GetComponent<Renderer>().material = visutouchedMat;
            if (oldVisu2) oldVisu2.GetComponent<Renderer>().material = visutouchedMat;
            if (oldVisu3) oldVisu3.GetComponent<Renderer>().material = visutouchedMat;
        }
        else if (!isTouched && oldTouched)
        {
            oldTouched = false;
            this.GetComponent<Renderer>().material = defaultMat;

            if (newVisu1) newVisu1.GetComponent<Renderer>().material = visuNewdefaultMat;
            if (newVisu2) newVisu2.GetComponent<Renderer>().material = visuNewdefaultMat;
            if (newVisu3) newVisu3.GetComponent<Renderer>().material = visuNewdefaultMat;

            if (oldVisu1) oldVisu1.GetComponent<Renderer>().material = visuOlddefaultMat;
            if (oldVisu2) oldVisu2.GetComponent<Renderer>().material = visuOlddefaultMat;
            if (oldVisu3) oldVisu3.GetComponent<Renderer>().material = visuOlddefaultMat;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Geomagic")
        {
            isTouched = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Geomagic")
        {
            isTouched = false;
        }
    }
}
