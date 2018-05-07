using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

public class EntactManager : MonoBehaviour
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaEntact m_sofaEntact = null;

    protected bool rightInit = false;
    protected bool leftInit = false;

    ////////////////////////////////////////////
    /////       Object creation API        /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        bool contextOk = true;
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            if (m_sofaContext == null)
            {
                Debug.LogError("EntactManager::loadContext - GetComponent<SofaUnity.SofaContext> failed.");
                contextOk = false;
            }
        }
        else
        {
            Debug.LogError("EntactManager::loadContext - No SofaContext found.");
            contextOk = false;
        }

        // Call internal method that will create a ray caster in Sofa.
        if (contextOk)
            StartCoroutine(createEntactManager());
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    public virtual IEnumerator createEntactManager()
    {
        // Get access to the sofaContext // TODO remove this HACK: All components need to be created before the SofaPlier
        yield return new WaitForSeconds(1);

        IntPtr _simu = m_sofaContext.getSimuContext();
        m_sofaEntact = new SofaEntact(_simu, name);
    }



    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Debug.Log("Right Homing");
            if (m_sofaEntact != null)
            {
                int res = m_sofaEntact.SofaRightHoming();
                if (res == 1)
                    rightInit = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (rightInit == false)
            {
                Debug.Log("Right homing is needed first.");
            }
            else
            {
                Debug.Log("Left Homing");
                if (m_sofaEntact != null)
                    m_sofaEntact.SofaLeftHoming();
            }
        }
        else if(Input.GetKeyDown(KeyCode.End))
        {
            if (m_sofaEntact != null)
                m_sofaEntact.numberOfTools();
        }
    }
}
