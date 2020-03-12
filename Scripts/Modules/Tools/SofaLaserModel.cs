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



    /// Laser object
    /// {    
    /// Laser material
    public Material laserMat = null;
    /// Laser GameObject
    protected GameObject laser = null;

    /// Booleen to draw the laser object
    public bool drawLaserParticles = false;

    [SerializeField]
    public Color startColor = Color.green;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float width = 0.15f;

    // Light emitted by the laser origin
    protected GameObject lightSource = null;
    protected Light light;

    // Light Particle system following the lineRenderer
    protected ParticleSystem ps;
    protected bool psInitialized = false;
    public Material particleMat;
    /// }
    protected float m_startSpeed = 100;


    /// Protected method that will really create the Sofa ray caster
    public override void CreateSofaRayCaster()
    {
        // Create Laser
        if (laser == null)
        {
            laser = new GameObject("Laser");
            laser.transform.parent = this.transform;
            laser.transform.localPosition = Vector3.zero;
            laser.transform.localRotation = Quaternion.identity;
            laser.transform.localScale = Vector3.one * 0.1f;
        }

        if (drawLaserParticles && lightSource == null)
        {
            // Create light source
            lightSource = new GameObject("Light");
            lightSource.transform.parent = laser.transform;
            lightSource.transform.localPosition = Vector3.zero;
            lightSource.transform.localRotation = Quaternion.identity;
            lightSource.transform.localScale = Vector3.one;

            initializeLaser();

            // initialise for the first time the particule system
            if (psInitialized == false)
                initializeParticles();            
        }

        //this.activeTool(false);

        base.CreateSofaRayCaster();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_initialized && m_activateRay)
        {
            UpdateImpl();
        }
    }


    public void UpdateImpl()
    {
        // update ray transform from this gameObject transform
        m_origin = transform.position;
        m_direction = transform.forward;

        // update the light source
        if (drawLaserParticles && lightSource)
            lightSource.transform.position = m_origin;


        // cast ray here
        CastRay();
    }


    /// Internal method to activate or not the tool, will also update the rendering
    protected override void ActivateTool_impl(bool value)
    {
        if (drawLaserParticles || m_drawRay)
            this.updateLaser();

        base.ActivateTool_impl(value);
    }

    /// Internal method to create the laser line renderer and light
    private void initializeLaser()
    {
        //create light
        light = lightSource.AddComponent<Light>();
        light = lightSource.GetComponent<Light>();
        light.intensity = width * 10;
        light.bounceIntensity = width * 10;
        light.range = width / 4;
        light.color = endColor;
    }

    /// Internal method to create the laser particle system rendering
    private void initializeParticles()
    {
        psInitialized = true;
        //create particlesystem
        //TODO: add scaling/size with laser width
        if (drawLaserParticles)
        {
            ps = laser.AddComponent<ParticleSystem>();
            var shape = ps.shape;
            shape.angle = 0;
            shape.radius = 0.2f;
            var em = ps.emission;
            em.rateOverTime = 1000;
            var psmain = ps.main;
            psmain.startSize = 1.0f;
            psmain.startLifetime = m_length * 0.1f;
            psmain.startSpeed = 100;
            psmain.maxParticles = 800;
            psmain.startColor = new Color(1, 1, 1, 0.25f);
            //var pscolor = ps.colorOverLifetime;
            //pscolor.color = new ParticleSystem.MinMaxGradient(startColor, endColor);
        
            var psrenderer = ps.GetComponent<ParticleSystemRenderer>();
            if (particleMat == null)
                particleMat = new Material(Shader.Find("Particles/Default-Particle"));

            psrenderer.material = particleMat;
        }
    }


    /// Method to update the laser rendering when tool status change
    public void updateLaser()
    {
        if (drawLaserParticles)
        {
            ps = laser.GetComponent<ParticleSystem>();
            var psmain = ps.main;
            psmain.startColor = new Color(endColor.r, endColor.g, endColor.b, 0.25f); ;
        
            light.color = endColor;
            light.intensity = width * 100;
            light.bounceIntensity = width * 3;
            light.range = width / 2.5f;
        }
    }

}
