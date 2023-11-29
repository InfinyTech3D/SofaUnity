using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Specialisation of SofaRayCaster class into a laserModel object
/// Will comunicate with Sofa ray caster and allow several interaction using a ray:
/// Grasping and fixing pointes and deleting elements.
/// </summary>
public class SofaLaserModel : SofaRayCaster
{
    ////////////////////////////////////////////
    //////      SofaLaserModel members     /////
    ////////////////////////////////////////////

    /// Booleen to draw the laser object
    [SerializeField]
    protected bool m_drawLaser = false;
    [SerializeField]
    protected bool m_drawLight = false;


    /// Laser width parameter
    [SerializeField]
    protected float m_laserWidth = 0.5f;

    /// Laser color parameter at origin
    [SerializeField]
    protected Color m_laserStartColor = Color.green;
    /// Laser color parameter at extremity
    [SerializeField]
    protected Color m_laserEndColor = Color.green;



    /// Parameter to store status of rendering laser if init
    [SerializeField]
    protected bool m_renderingInit = false;

    /// Light Gameobject to set the origine of the light emitted by the laser
    [SerializeField]
    protected GameObject m_lightSource = null;

    /// light emitted by the laser at origin
    [SerializeField]
    protected Light m_light = null;

    // Light Particle system to draw the laser
    [SerializeField]
    protected ParticleSystem m_ps = null;

    /// Material used by the particle system
    public Material m_particleMat = null;



    ////////////////////////////////////////////
    //////     SofaLaserModel accessors    /////
    ////////////////////////////////////////////

    /// Getter/setter for laser width rendering \sa m_laserWidth
    public bool DrawLaser
    {
        get { return m_drawLaser; }
        set
        {
            if (m_drawLaser != value)
            {
                m_drawLaser = value;
                UpdateLaserRendering();
            }
        }
    }

    /// Getter/setter for laser width rendering \sa m_laserWidth
    public float LaserWidth
    {
        get { return m_laserWidth; }
        set
        {
            if (m_laserWidth != value)
            {
                m_laserWidth = value;
                UpdateLaserRendering();
            }
        }
    }

    /// Getter/setter for laser color at origin \sa m_laserStartColor
    public Color LaserStartColor
    {
        get { return m_laserStartColor; }
        set
        {
            if (m_laserStartColor != value)
            {
                m_laserStartColor = value;
                UpdateLaserRendering();
            }
        }
    }


    /// Getter/setter for laser color at end \sa m_laserEndColor
    public Color LaserEndColor
    {
        get { return m_laserEndColor; }
        set
        {
            if (m_laserEndColor != value)
            {
                m_laserEndColor = value;
                UpdateLaserRendering();
            }
        }
    }


    /// Getter/setter for laser width rendering \sa m_laserWidth
    public bool DrawLight
    {
        get { return m_drawLight; }
        set
        {
            if (m_drawLight != value)
            {
                m_drawLight = value;
                UpdateLaserRendering();
            }
        }
    }


    ////////////////////////////////////////////
    //////    SofaLaserModel public API    /////
    ////////////////////////////////////////////

    /// Update is called once per frame in unity animation loop
    void Update()
    {
        if (m_initialized && m_activateRay)
        {
            UpdateImpl();
        }
    }


    /// 
    public void UpdateImpl()
    {
        // update ray transform from this gameObject transform
        m_origin = transform.position;
        m_direction = transform.forward;

        // update the light source
        if (m_drawLaser && m_lightSource)
            m_lightSource.transform.position = m_origin;


        // cast ray here
        CastRay();
    }


    /// Internal method to activate or not the tool, will also update the rendering
    protected override void ActivateTool_impl(bool value)
    {
        base.ActivateTool_impl(value);

        if (m_drawLaser || m_drawLight)
            this.UpdateLaserRendering();
    }

    
    
    ////////////////////////////////////////////
    //////    SofaLaserModel internal API  /////
    ////////////////////////////////////////////

    /// Protected method that will really create the Sofa ray caster
    protected override void CreateSofaRayCaster_impl()
    {
        if (m_drawLaser)
        {
            if (!m_renderingInit)
                InitializeLaserRendering();
        }

        base.CreateSofaRayCaster_impl();
    }


    /// Internal method to create the laser line renderer and light
    private void InitializeLaserRendering()
    {
        if (m_lightSource == null)
        {
            // Create light source
            m_lightSource = new GameObject("Light");
            m_lightSource.transform.parent = this.transform;
            m_lightSource.transform.localPosition = Vector3.zero;
            m_lightSource.transform.localRotation = Quaternion.identity;
            m_lightSource.transform.localScale = Vector3.one;
        }

        //create light
        if (m_light == null)
        {
            m_light = m_lightSource.AddComponent<Light>();
            m_light.intensity = m_laserWidth * 10;
            m_light.bounceIntensity = m_laserWidth * 10;
            m_light.range = m_laserWidth / 4;
            m_light.color = m_laserEndColor;
            m_light.enabled = true;
        }

        // create particle system
        if (m_ps == null)
        {
            m_ps = this.gameObject.AddComponent<ParticleSystem>();
            var shape = m_ps.shape;
            shape.angle = 0;
            shape.radius = 0.2f;
            var em = m_ps.emission;
            em.rateOverTime = 1000;
            var psmain = m_ps.main;
            psmain.startSize = 1.0f;
            psmain.startLifetime = m_length;// * 0.1f;
            psmain.startSpeed = 100;
            psmain.maxParticles = 800;
            psmain.startColor = new Color(1, 1, 1, 0.25f);
            //var pscolor = ps.colorOverLifetime;
            //pscolor.color = new ParticleSystem.MinMaxGradient(startColor, endColor);

            ParticleSystemRenderer psrenderer = m_ps.GetComponent<ParticleSystemRenderer>();
            if (m_particleMat == null)
                m_particleMat = Resources.Load("Materials/laser") as Material;

            psrenderer.material = m_particleMat;
        }

        m_renderingInit = true;
    }

    
    /// Method to update the laser rendering when tool status change
    protected void UpdateLaserRendering()
    {
        if (!m_renderingInit)
            InitializeLaserRendering();

        // update light
        m_lightSource.SetActive(m_drawLight);

        // update particles
        ParticleSystemRenderer psrenderer = m_ps.GetComponent<ParticleSystemRenderer>();
        psrenderer.enabled = m_drawLaser;

        // update laser color and parameters
        if (m_drawLaser)
        {
            var psmain = m_ps.main;
            if (m_isActivated)
                psmain.startColor = new Color(1.0f, 0.0f, 0.0f, 0.25f);
            else
                psmain.startColor = new Color(m_laserEndColor.r, m_laserEndColor.g, m_laserEndColor.b, 0.25f); ;
        }

        // update llight color and parameters
        if (m_drawLight)
        {
            //m_light.intensity = m_length * 100;
            //m_light.bounceIntensity = m_rayWidth * 3;
            //m_light.range = m_rayWidth;
            if (m_isActivated)
                m_light.color = Color.red;
            else
                m_light.color = m_laserEndColor;
        }

    }
}
