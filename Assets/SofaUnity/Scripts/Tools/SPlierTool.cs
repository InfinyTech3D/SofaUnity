using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Base class inherite from MonoBehavior that design a Ray casting object.
/// </summary>
public class SPlierTool : MonoBehaviour
{
    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaPliers m_sofaPlier = null;

    public string nameMord1 = "mord1";
    public string nameMord2 = "mord2";
    public string nameModel = "estomac";

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
        createSofaPlier();
    }

    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual void createSofaPlier()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.getSimuContext();

        if (_simu != IntPtr.Zero)
        {
            m_sofaPlier = new SofaPliers(_simu, name, nameMord1, nameMord2, nameModel);
        }
    }

    public bool clampSofaPlier()
    {
        int res = m_sofaPlier.closePliers();

        if (res > 0)
            return true;
        else
            return false;
    }

    public bool releaseSofaPlier()
    {
        m_sofaPlier.releasePliers();

        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
         //   new WaitForSeconds(5);
            m_sofaPlier.closePliers();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {            
            m_sofaPlier.releasePliers();
        }

    }
}
