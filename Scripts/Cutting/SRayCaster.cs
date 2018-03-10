using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RayCaster class, inherite from RayCaster which is a MonoBehavior.
/// This class will link to Sofa Ray casting system and will not use Unity raycasting.
/// </summary>
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

        // Call internal method that will create a ray caster in Sofa.
        createSofaRayCaster();
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual void createSofaRayCaster()
    {

    }

    /// Method to display touched triangle. Not yet implemented from Sofa-Unity.
    public override void highlightTriangle()
    {

    }
}
