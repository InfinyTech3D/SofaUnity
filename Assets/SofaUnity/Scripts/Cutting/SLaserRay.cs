using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

public class SLaserRay : SRayCaster
{
    public bool m_isCutting = false;
    public Vector3 m_axisDirection = new Vector3(1.0f, 0.0f, 0.0f);
    public Vector3 m_translation = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 transLocal;
    bool logController = false;

    public bool drawlaser = true;

    // Laser object
    public Material laserMat;
    private GameObject laser;
    private LineRenderer lr;

    [SerializeField]
    public Color startColor = Color.green;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float width = 0.15f;

    // Light
    private GameObject lightSource;
    private Light light;

    // Light Particle system
    private ParticleSystem ps;
    private bool psInitialized = false;
    public Material particleMat;

    // Smoke Particle system
    private GameObject smokeObject;
    private ParticleSystem smoke;


    public enum LaserType
    {
        CuttingTool,
        AttachTool,
        FixTool
    };

    public enum ButtonType
    {
        Trigger,
        Grip
    };

    void OnValidate()
    {
        //startWidth = Mathf.Max(0, startWidth);
        //endWidth = Mathf.Max(0, endWidth);
        width = Mathf.Max(0, width);
    }

    public LaserType m_laserType;
    public ButtonType m_actionButton;

    protected override void createSofaRayCaster()
    {
        if (drawlaser)
        {
            // Create Laser
            laser = new GameObject("Laser");
            laser.transform.parent = this.transform;
            laser.transform.localPosition = new Vector3(-0.035f, -0.005f, 0.005f);
            laser.transform.localRotation = Quaternion.identity;
            laser.transform.localScale = Vector3.one;

            // Create light source
            lightSource = new GameObject("Light");
            lightSource.transform.parent = laser.transform;
            lightSource.transform.localPosition = Vector3.zero;
            lightSource.transform.localRotation = Quaternion.identity;
            lightSource.transform.localScale = Vector3.one;

            initializeLaser();

            // Add smoke to tool
            //smokeObject = new GameObject("Smoke");
            //smokeObject.transform.parent = this.transform;
            //smokeObject.AddComponent<ParticleSystem>();
            //smoke = smokeObject.GetComponent<ParticleSystem>();
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

    public override bool castRay()
    {
        return false;
    }

    void Start()
    {
        m_axisDirection.Normalize();

        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "SLaserRay", "VRTK_ControllerEvents", "the same"));
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

        GetComponent<VRTK_ControllerEvents>().GripClicked += new ControllerInteractionEventHandler(DoGripClicked);
        GetComponent<VRTK_ControllerEvents>().GripUnclicked += new ControllerInteractionEventHandler(DoGripUnclicked);
    }

    void Update()
    {                
        transLocal = transform.TransformVector(m_translation);
        origin = transform.position + transLocal;
        direction = transform.forward * m_axisDirection[0] + transform.right * m_axisDirection[1] + transform.up * m_axisDirection[2];
        //direction = transform.forward;

        //set light position a bit back to have better lighting
        //lightSource.transform.position = end - (distance / 100) * transform.forward;
        if (drawlaser)
            lightSource.transform.position = origin + transLocal; 

        if (!psInitialized && drawlaser)
            initializeParticles();

        if (m_sofaRC != null)
        {
            int triId = m_sofaRC.castRay(origin, direction);
            //if (triId < 10000)
            //    Debug.Log("Sofa Collision triId " + triId);

            if (Input.GetKey(KeyCode.C))
            {
                this.activeTool(true);
            }
            else if (Input.GetKey(KeyCode.V))
            {
                this.activeTool(false);
            }
        }

        if (drawlaser)
            this.draw(origin, origin + direction * length);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        if(logController)
            VRTK_Logger.Info("SLaserRay::Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("SLaserRay TRIGGER pressed");
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "pressed", e);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "released", e);
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "clicked", e);
        if (m_actionButton == ButtonType.Trigger)
        {
            activeTool(true);
        }
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "unclicked", e);
        if (m_actionButton == ButtonType.Trigger)
        {
            activeTool(false);
        }
    }

    private void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "clicked", e);
        if (m_actionButton == ButtonType.Grip)
        {
            activeTool(true);
        }
    }

    private void DoGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "unclicked", e);
        if (m_actionButton == ButtonType.Grip)
        {
            activeTool(false);
        }
    }

    private void activeTool(bool value)
    {
        m_isCutting = value;
        m_sofaRC.activateTool(m_isCutting);

        if (value)
            this.endColor = Color.red;
        else
            this.endColor = Color.green;

        if (drawlaser)
            this.updateLaser();
    }

    private void initializeLaser()
    {
        //create light
        light = lightSource.AddComponent<Light>();
        light = lightSource.GetComponent<Light>();
        light.intensity = width * 10;
        light.bounceIntensity = width * 10;
        light.range = width / 4;
        light.color = endColor;

        /*
        //create linerenderer
        laser.AddComponent<LineRenderer>();
        lr = laser.GetComponent<LineRenderer>();
        //lr.useWorldSpace = false;
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.material = laserMat;
        lr.startWidth = width;
        lr.endWidth = width;

#if UNITY_5_5 || UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1
        lr.numPositions = 2;
#else
        lr.positionCount = 2;
#endif
*/
    }

    private void initializeParticles()
    {
        //create particlesystem
        //TODO: add scaling/size with laser width
        ps = laser.AddComponent<ParticleSystem>();
        ps = laser.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.angle = 0;
        shape.radius = 0.1f;
        var em = ps.emission;
        em.rateOverTime = 8000;
        var psmain = ps.main;
        psmain.startSize = 0.8f;
        psmain.startLifetime = 0.11f;
        psmain.startSpeed = 100;
        psmain.maxParticles = 1000;
        psmain.startColor = new Color(1, 1, 1, 0.25f);
        //var pscolor = ps.colorOverLifetime;
        //pscolor.color = new ParticleSystem.MinMaxGradient(startColor, endColor);
        var psrenderer = ps.GetComponent<ParticleSystemRenderer>();
        psrenderer.material = particleMat;

        psInitialized = true;
    }

    public void draw(Vector3 start, Vector3 end)
    {
        //lr.SetPosition(0, start);
        //lr.SetPosition(1, end);
    }

    public void updateLaser()
    {
        //lr.startColor = startColor;
        //lr.endColor = endColor;
        //lr.startWidth = width;
        //lr.endWidth = width;

        ps = laser.GetComponent<ParticleSystem>();
        var psmain = ps.main;
        psmain.startColor = new Color(endColor.r, endColor.g, endColor.b, 0.25f); ;

        light.color = endColor;
        light.intensity = width * 100;
        light.bounceIntensity = width * 3;
        light.range = width / 2.5f;
    }

}
