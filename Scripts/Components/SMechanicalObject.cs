using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

public class SMechanicalObject : SBaseMesh
{
    /// Pointer to the corresponding SOFA API object
    //protected SofaCustomMesh m_impl = null;
    public GameObject m_object;
    public bool activated = true;

    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
    protected override void createObject()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_context.getSimuContext();

        ////Mesh mesh = m_object.GetComponent<MeshFilter>().sharedMesh;

        //if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh
        //    m_impl = new SofaCustomMesh(_simu, m_nameId, m_object);

        //if (m_impl == null)
        //{
        //    Debug.LogError("SofaCustomMesh:: Object creation failed.");
        //    return;
        //}
        //else
        //{
        //    m_impl.createMesh();
        //}
    }

    protected override void awakePostProcess()
    {
      
    }


    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
		if(activated)
        {
            //m_impl.updateMesh();
        }
	}
}
