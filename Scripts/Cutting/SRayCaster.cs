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

    public bool startOnPlay = true;
    public bool automaticCast = false;

    protected bool m_isReady = false;

    public void stopRay()
    {
        if (m_sofaRC != null)
        {
            m_sofaRC.activateTool(false);
            m_sofaRC.Dispose();
            m_sofaRC = null;
        }
        m_isReady = false;
    }

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        if (!startOnPlay)
            return;

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

        m_isReady = true;
    }

    public void startSofaRayCaster(SofaUnity.SofaContext _context)
    {
        if (m_sofaContext != null || m_sofaRC != null)
        {
            stopRay();
        }

        m_sofaContext = _context;

        // Call internal method that will create a ray caster in Sofa.
        createSofaRayCaster();

        if (m_sofaContext.testAsync == true)
            m_sofaContext.registerCaster(this);
        else
            automaticCast = true;

        m_isReady = true;
    }

    public void unloadSofaRayCaster()
    {
        stopRay();
        m_sofaContext = null;
    }
    
    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual void createSofaRayCaster()
    {

    }

    /// Method to display touched triangle. Not yet implemented from Sofa-Unity.
    public override void highlightTriangle()
    {

    }

    public virtual void updateImpl()
    {

    }
}
