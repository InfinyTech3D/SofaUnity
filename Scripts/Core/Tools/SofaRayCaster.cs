using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SofaRayCaster class, inherite from RayCaster which is a MonoBehavior.
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

    /// Parameter to activate or unactivate ray at game start
    public bool startOnPlay = true;

    /// Booleen to activate or not that tool
    [SerializeField]
    protected bool m_isActivated = false;

    /// Enum that set the type of interaction to plug to this tool on sofa side
    [SerializeField]
    protected SofaDefines.SRayInteraction m_rayType;

    /// Booleen to draw the effective ray sent to Sofa ray caster
    [SerializeField]
    public bool m_drawRay = false;

    /// Laser renderer
    protected LineRenderer m_rayRenderer = null;
    [SerializeField]
    /// Ray color parameter for drawing
    protected Color m_rayColor = Color.green;
    /// Ray width parameter for drawing
    [SerializeField]
    protected float m_rayWidth = 0.5f;

    public bool automaticCast = false;


    /// Specific parameter for attach tool interaction
    [SerializeField]
    protected float m_stiffness = 10000f;



    ////////////////////////////////////////////
    //////     SofaRayCaster accessors     /////
    ////////////////////////////////////////////

    /// Public Method to change the tool activation of this Ray \sa m_isActivated will call \sa SofaRayCasterAPI.activateTool
    public bool ActivateTool
    {
        get { return m_isActivated; }
        set
        {
            if (m_isActivated != value)
            {
                ActivateTool_impl(value);
            }
        }
    }

    /// Getter/setter to access this SofaRayCaster type \sa m_rayType
    public SofaDefines.SRayInteraction RayInteractionType
    {
        get { return m_rayType; }
        set
        {
            if (m_rayType != value)
            {
                m_rayType = value;
                //StopRay();
                //CreateSofaRayCaster_impl();
            }
        }
    }

    /// Getter/setter for ray width drawing \sa m_rayWidth
    public float RayWidth
    {
        get { return m_rayWidth; }
        set
        {
            if (m_rayWidth != value)
            {
                m_rayWidth = value;
                UpdateRayRenderer();
            }
        }
    }

    /// Getter/setter for ray color drawing \sa m_rayColor
    public Color RayColor
    {
        get { return m_rayColor; }
        set
        {
            if (m_rayColor != value)
            {
                m_rayColor = value;
                UpdateRayRenderer();
            }
        }
    }


    /// Getter/setter to change AttachStiffness of this SofaRayCaster type \sa m_stiffness
    public float AttachStiffness
    {
        get { return m_stiffness; }
        set
        {
            if (m_stiffness != value)
            {
                m_stiffness = value;
                if (m_sofaRC != null)
                {
                    m_sofaRC.setToolAttribute("stiffness", m_stiffness);
                }
            }
        }
    }


    ////////////////////////////////////////////
    //////     SofaRayCaster public API    /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will call \sa CreateSofaRayCaster() if startOnPlay is set to true
    void Awake()
    {
        if (!startOnPlay)
            return;

        CreateSofaRayCaster();
    }


    void Start()
    {
        // Call internal method that will create a ray caster in Sofa.
        CreateSofaRayCaster_impl();
    }

    /// Method to search for SofaContext if needed then call internal method \sa CreateSofaRayCaster_impl
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
    }


    /// Method to create the SofaRayCaster using a given SofaContext
    public void LoadSofaRayCaster(SofaUnity.SofaContext _context)
    {
        StopRay();
        m_sofaContext = _context;

        // Call internal method that will create a ray caster in Sofa.
        CreateSofaRayCaster_impl();
    }


    /// Method to unload the Sofacontext from this SofaRayCaster, Will call \sa StopRay()
    public void UnloadSofaRayCaster()
    {
        StopRay();
        m_sofaContext = null;
    }

    
    /// Public Method to stop this Ray casting \sa m_activateRay
    public override void StopRay()
    {
        Debug.Log("SofaRayCaster StopRay");
        if (m_sofaRC != null)
        {
            m_sofaRC.activateTool(false);
            m_sofaRC.Dispose();
            m_sofaRC = null;
        }
        base.StopRay();
    }

   
    /// Main method to propagate ray info from unity cast the ray in SOFA engine.
    public override bool CastRay()
    {
        if (automaticCast && m_sofaRC != null)
        {
            //int triId = -1;
            // get the id of the selected triangle. If < 0, no intersection
            if (m_isActivated)
            {
                Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
                Vector3 directionS = m_sofaContext.transform.InverseTransformDirection(m_direction);
                m_sofaRC.castRay(originS, directionS);
                //triId = m_sofaRC.castRay(originS, directionS);
                //if (triId >= 0)
                //    Debug.Log("origin: " + m_origin + " => originS: " + originS + " |  directionS: " + directionS + " | triId: " + triId);
            }
        }

        // Update the ray drawing
        if (m_drawRay)
            DrawRay();

        return false;
    }



    ////////////////////////////////////////////
    //////    SofaRayCaster internal API   /////
    ////////////////////////////////////////////

    /// Internal method called by ActiveTool setter to change ray internal parameters
    protected virtual void ActivateTool_impl(bool value)
    {
        m_isActivated = value;
        if (m_sofaRC != null)
        {
            m_sofaRC.activateTool(m_isActivated);

            if (m_rayType == SofaDefines.SRayInteraction.AttachTool)
                m_sofaRC.setToolAttribute("stiffness", m_stiffness);
        }

        if (m_rayRenderer)
        {
            if (value)
                m_rayRenderer.endColor = Color.red;
            else
                m_rayRenderer.endColor = m_rayColor;
        }
    }


    /// Internal Method called by \sa CreateSofaRayCaster or \sa LoadSofaRayCaster to create a SofaRayCasterAPI to interact with SOFA ray
    protected virtual void CreateSofaRayCaster_impl()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.GetSimuContext();
        if (_simu != IntPtr.Zero && m_sofaRC == null)
        {

            float raySofaLength = m_length * m_sofaContext.GetFactorUnityToSofa(1);
            if (m_rayType == SofaDefines.SRayInteraction.CuttingTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 0, base.name, raySofaLength);
                Debug.Log(this.name + " create SofaRayCaster CuttingTool with length: " + raySofaLength + " Under Name: " + base.name);
            }
            else if (m_rayType == SofaDefines.SRayInteraction.AttachTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 1, base.name, raySofaLength);                
                Debug.Log(this.name + " create SofaRayCaster AttachTool with length: " + raySofaLength + " Under Name: " + base.name);
            }
            else if (m_rayType == SofaDefines.SRayInteraction.FixTool)
            {
                m_sofaRC = new SofaRayCasterAPI(_simu, 2, base.name, raySofaLength);
                Debug.Log(this.name + " create SofaRayCaster FixTool with length: " + raySofaLength + " Under Name: " + base.name);
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
            if (m_sofaContext.AsyncSimulation == true)
                m_sofaContext.RegisterRayCaster(this);
            else
                automaticCast = true;
        }
    }



    ////////////////////////////////////////////
    //////    SofaRayCaster drawing API    /////
    ////////////////////////////////////////////

    /// Internal method to drawRay, more for debug info
    protected void DrawRay()
    {
        if (m_rayRenderer == null)
            InitialiseRayRenderer();

        Vector3 end = m_origin + m_direction * m_length;
        m_rayRenderer.SetPosition(0, m_origin);
        m_rayRenderer.SetPosition(1, end);        
    }

    /// Internal method to initialize ray renderer
    protected void InitialiseRayRenderer()
    {
        m_rayRenderer = this.gameObject.AddComponent<LineRenderer>();

        m_rayRenderer.material = Resources.Load("Materials/laser") as Material;
        m_rayRenderer.startWidth = m_rayWidth;
        m_rayRenderer.endWidth = m_rayWidth;
    }

    /// Internal method to udpate the ray drawing parameter
    protected void UpdateRayRenderer()
    {
        m_rayRenderer.startColor = m_rayColor;
        m_rayRenderer.endColor = m_rayColor;
        m_rayRenderer.startWidth = m_rayWidth;
        m_rayRenderer.endWidth = m_rayWidth;
    }
}
