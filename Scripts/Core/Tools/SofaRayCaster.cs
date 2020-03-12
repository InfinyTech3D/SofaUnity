using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RayCaster class, inherite from RayCaster which is a MonoBehavior.
/// This class will link to Sofa Ray casting system and will not use Unity raycasting.
/// </summary>
public class SofaRayCaster : RayCaster
{
    ////////////////////////////////////////////
    //////      SofaRayCaster members      /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    public SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaRayCasterAPI m_sofaRC = null;

    /// Direction of the laser ray in local coordinate 
    public Vector3 m_axisDirection = new Vector3(1.0f, 0.0f, 0.0f);
    /// Translation of the origin of the laser ray from the origin of the GameObject in world coordinate
    public Vector3 m_translation = new Vector3(0.0f, 0.0f, 0.0f);

    /// Booleen to activate or not that tool
    public bool m_isActivated = false;


    public bool startOnPlay = true;
    public bool automaticCast = false;

    /// Enum that set the type of interaction to plug to this tool on sofa side
    public SofaDefines.SRayInteraction m_laserType;

    public float m_stiffness = 10000f;
    protected float oldStiffness = 10000f;

    

    ////////////////////////////////////////////
    //////     SofaRayCaster accessors     /////
    ////////////////////////////////////////////




    ////////////////////////////////////////////
    //////     SofaRayCaster public API    /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        if (!startOnPlay)
            return;

        CreateSofaRayCaster();
    }

    public virtual void CreateSofaRayCaster()
    {
        if (m_sofaContext == null)
        {
            GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
            if (_contextObject != null)
            {
                // Get Sofa context
                m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            }
            else
            {
                Debug.LogError("RayCaster::loadContext - No SofaContext found.");
                return;
            }
        }

        // Call internal method that will create a ray caster in Sofa.
        CreateSofaRayCaster_impl();
    }

    public void LoadSofaRayCaster(SofaUnity.SofaContext _context)
    {
        StopRay();
        m_sofaContext = _context;

        // Call internal method that will create a ray caster in Sofa.
        CreateSofaRayCaster_impl();
    }

    public void unloadSofaRayCaster()
    {
        StopRay();
        m_sofaContext = null;
    }

    public override void StopRay()
    {
        if (m_sofaRC != null)
        {
            m_sofaRC.activateTool(false);
            m_sofaRC.Dispose();
            m_sofaRC = null;
        }
        base.StopRay();
    }

    public virtual void activeTool(bool value)
    {
        m_isActivated = value;

        if (m_sofaRC != null)
            m_sofaRC.activateTool(m_isActivated);
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual void CreateSofaRayCaster_impl()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.GetSimuContext();
        if (_simu != IntPtr.Zero && m_sofaRC == null)
        {

            float raySofaLength = m_length * m_sofaContext.GetFactorUnityToSofa(1);
            if (m_laserType == SofaDefines.SRayInteraction.CuttingTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 0, base.name, raySofaLength * 2);
                Debug.Log(this.name + " create SofaRayCaster CuttingTool with length: " + raySofaLength);
            }
            else if (m_laserType == SofaDefines.SRayInteraction.AttachTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 1, base.name, raySofaLength);
                Debug.Log(this.name + " create SofaRayCaster AttachTool with length: " + raySofaLength);
            }
            else if (m_laserType == SofaDefines.SRayInteraction.FixTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 2, base.name, raySofaLength);
                Debug.Log(this.name + " create SofaRayCaster FixTool with length: " + raySofaLength);
            }
            else
            {
                m_sofaRC = null;
                m_initialized = false;
            }
        }

        if (m_sofaRC == null)
        {
            Debug.Log(this.name + " No SofaRayCaster created");
        }
        else
        {
            m_initialized = true;
            if (m_sofaContext.testAsync == true)
                m_sofaContext.RegisterRayCaster(this);
            else
                automaticCast = true;
        }
    }


    public override bool CastRay()
    {        
        // compute the direction and origin of the ray by adding object transform + additional manual transform
        Vector3 transLocal = transform.TransformVector(m_translation);
        m_origin = transform.position + transLocal;
        m_direction = transform.forward * m_axisDirection[0] + transform.right * m_axisDirection[1] + transform.up * m_axisDirection[2];

        
        if (automaticCast && m_sofaRC != null)
        {
            int triId = -1;
            // get the id of the selected triangle. If < 0, no intersection
            if (m_isActivated)
            {
                Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
                Vector3 directionS = m_sofaContext.transform.InverseTransformDirection(m_direction);
                triId = m_sofaRC.castRay(originS, directionS);
                if (triId >= 0)
                    Debug.Log("origin: " + m_origin + " => originS: " + originS + " |  directionS: " + directionS + " | triId: " + triId);

                //if (m_laserType == SofaDefines.SRayInteraction.AttachTool)
                //{
                //    if (oldStiffness != m_stiffness)
                //    {
                //        oldStiffness = m_stiffness;
                //        m_sofaRC.setToolAttribute("stiffness", m_stiffness);
                //    }
                //}
            }
        }

        return false;
    }

    public virtual void updateImpl()
    {
        Vector3 transLocal = transform.TransformVector(m_translation);
        m_origin = transform.position + transLocal;
        m_direction = transform.forward * m_axisDirection[0] + transform.right * m_axisDirection[1] + transform.up * m_axisDirection[2];

        if (m_sofaRC != null)
        {
            int triId = -1;
            // get the id of the selected triangle. If < 0, no intersection
            if (m_isActivated)
            {
                Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
                Vector3 directionS = m_sofaContext.transform.InverseTransformDirection(m_direction);
                triId = m_sofaRC.castRay(originS, directionS);
            }
        }
    }
}
