using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

public class GeomagicController : MonoBehaviour
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaGeomagic m_sofaGeomagic = null;


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
            StartCoroutine(createGeomagicManager());
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    public virtual IEnumerator createGeomagicManager()
    {
        // Get access to the sofaContext // TODO remove this HACK: All components need to be created before the Geomagic Manager
        yield return new WaitForSeconds(1);

        IntPtr _simu = m_sofaContext.getSimuContext();
        m_sofaGeomagic = new SofaGeomagic(_simu, name);
    }



    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_sofaGeomagic != null)
            m_sofaGeomagic.geomagicPosition();
        //if (Input.GetKeyDown(KeyCode.End))
        //{
        //    if (m_sofaGeomagic != null)
        //        m_sofaGeomagic.numberOfTools();
        //}
    }
}