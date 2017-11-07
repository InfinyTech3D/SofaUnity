using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Specialisation of SRayCaster class
/// Will comunicate with Sofa ray caster and allow several interaction using a ray:
/// Grasping and fixing pointes and deleting elements.
/// </summary>
public class SLaserRay : SRayCaster
{    
    /// Direction of the laser ray in local coordinate 
    public Vector3 m_axisDirection = new Vector3(1.0f, 0.0f, 0.0f);
    /// Translation of the origin of the laser ray from the origin of the GameObject in world coordinate
    public Vector3 m_translation = new Vector3(0.0f, 0.0f, 0.0f);

    /// Booleen to activate or not that tool
    public bool m_isActivated = false;

    /// Booleen to draw the effective ray sent to Sofa ray caster
    public bool drawRay = false;

    /// Enum that set the type of interaction to plug to this tool on sofa side
    public enum LaserType
    {
        CuttingTool,
        AttachTool,
        FixTool
    };
    public LaserType m_laserType;


    /// Laser object
    /// {
    /// Booleen to draw the laser object
    public bool drawlaser = true;
    /// Laser material
    public Material laserMat;
    /// Laser GameObject
    protected GameObject laser;
    
    /// Laser renderer
    protected LineRenderer lr;
    [SerializeField]
    public Color startColor = Color.green;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float width = 0.15f;

    // Light emitted by the laser origin
    protected GameObject lightSource;
    protected Light light;

    // Light Particle system following the lineRenderer
    protected ParticleSystem ps;
    protected bool psInitialized = false;
    public Material particleMat;
    /// }

    /// Protected method that will really create the Sofa ray caster
    protected override void createSofaRayCaster()
    {
        if (drawlaser)
        {
            // Create Laser
            laser = new GameObject("Laser");
            laser.transform.parent = this.transform;            
            laser.transform.localPosition = Vector3.zero;
            laser.transform.localRotation = Quaternion.identity;
            laser.transform.localScale = Vector3.one;

            // Create light source
            lightSource = new GameObject("Light");
            lightSource.transform.parent = laser.transform;
            lightSource.transform.localPosition = Vector3.zero;
            lightSource.transform.localRotation = Quaternion.identity;
            lightSource.transform.localScale = Vector3.one;

            initializeLaser();
        }
        
        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.getSimuContext();
        if (_simu != IntPtr.Zero)
        {   
            if (m_laserType == LaserType.CuttingTool)
                m_sofaRC = new SofaRayCaster(_simu, 0, base.name, length);
            else if (m_laserType == LaserType.AttachTool)
                m_sofaRC = new SofaRayCaster(_simu, 1, base.name, length);
            else
                m_sofaRC = new SofaRayCaster(_simu, 2, base.name, length);

            base.createSofaRayCaster();
        }
    }

    // Use this for initialization
    void Start()
    {
        m_axisDirection.Normalize();        
    }

    // Update is called once per frame
    void Update()
    {
        // compute the direction and origin of the ray by adding object transform + additional manual transform
        Vector3 transLocal = transform.TransformVector(m_translation);
        origin = transform.position + transLocal;
        direction = transform.forward * m_axisDirection[0] + transform.right * m_axisDirection[1] + transform.up * m_axisDirection[2];

        // update the light source
        if (drawlaser)
            lightSource.transform.position = origin + transLocal; 

        // initialise for the first time the particule system
        if (!psInitialized && drawlaser)
            initializeParticles();


        if (m_sofaRC != null)
        {
            // get the id of the selected triangle. If < 0, no intersection
            int triId = m_sofaRC.castRay(origin, direction);
            
            if (Input.GetKey(KeyCode.C))
            {
                this.activeTool(true);
            }
            else if (Input.GetKey(KeyCode.V))
            {
                this.activeTool(false);
            }
        }

        // Update the laser drawing
        if (drawlaser)
            this.draw(origin, origin + direction * length);
    }


    /// Internal method to activate or not the tool, will also update the rendering
    protected void activeTool(bool value)
    {
        m_isActivated = value;

        if (m_sofaRC != null)
            m_sofaRC.activateTool(m_isActivated);

        if (value)
            this.endColor = Color.red;
        else
            this.endColor = Color.green;

        if (drawlaser)
            this.updateLaser();
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

        if (drawRay)
        {
            //create linerenderer
            laser.AddComponent<LineRenderer>();
            lr = laser.GetComponent<LineRenderer>();
            lr.material = laserMat;
            lr.startWidth = width;
            lr.endWidth = width;

#if UNITY_5_5 || UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1
            lr.numPositions = 2;
#else
            lr.positionCount = 2;
#endif
        }
    }

    /// Internal method to create the laser particle system rendering
    private void initializeParticles()
    {
        //create particlesystem
        //TODO: add scaling/size with laser width
        ps = laser.AddComponent<ParticleSystem>();
        ps = laser.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.angle = 0;
        shape.radius = 0.2f;
        var em = ps.emission;
        em.rateOverTime = 1000;
        var psmain = ps.main;
        psmain.startSize = 1.0f;
        psmain.startLifetime = 0.11f;
        psmain.startSpeed = 100;
        psmain.maxParticles = 800;
        psmain.startColor = new Color(1, 1, 1, 0.25f);
        //var pscolor = ps.colorOverLifetime;
        //pscolor.color = new ParticleSystem.MinMaxGradient(startColor, endColor);
        var psrenderer = ps.GetComponent<ParticleSystemRenderer>();
        psrenderer.material = particleMat;

        psInitialized = true;
    }

    /// Method to update the position of the laser to render
    public void draw(Vector3 start, Vector3 end)
    {
        if (drawRay)
        {
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }
    }

    /// Method to update the laser rendering when tool status change
    public void updateLaser()
    {
        if (drawRay)
        {
            lr.startColor = startColor;
            lr.endColor = endColor;
            lr.startWidth = width;
            lr.endWidth = width;
        }

        ps = laser.GetComponent<ParticleSystem>();
        var psmain = ps.main;
        psmain.startColor = new Color(endColor.r, endColor.g, endColor.b, 0.25f); ;

        light.color = endColor;
        light.intensity = width * 100;
        light.bounceIntensity = width * 3;
        light.range = width / 2.5f;
    }

}
