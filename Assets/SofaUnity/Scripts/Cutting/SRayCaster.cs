using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRayCaster : RayCaster
{

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaRayCaster m_sofaRC = null;


    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
        }
        else
        {
            Debug.LogError("RayCaster::loadContext - No SofaContext found.");
        }

        createSofaRayCaster();
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual void createSofaRayCaster()
    {

    }


    public override void highlightTriangle()
    {

    }
}
