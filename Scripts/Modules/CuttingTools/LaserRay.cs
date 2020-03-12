using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserRay : RayCaster {

    public Material laserMat;
    public Material particleMat;
    private GameObject laser;
    private GameObject lightSource;
    private LineRenderer lr;
    private Light light;
    private ParticleSystem ps;

    public bool useParticleSystem = true;
    private bool psInitialized = false;

    [SerializeField]
    public Color startColor = Color.red;
    [SerializeField]
    public Color endColor = Color.green;
    [SerializeField]
    public float width = 0.15f;
    //[SerializeField]
    //public float startWidth = 0.15f;
    //[SerializeField]
    //public float endWidth = 0.15f;

    
    

    void OnValidate()
    {
        //startWidth = Mathf.Max(0, startWidth);
        //endWidth = Mathf.Max(0, endWidth);
        width = Mathf.Max(0, width);
    }

    // Use this for initialization
    void Start () {
        laser = new GameObject("Laser");
        laser.transform.parent = this.transform;
        laser.transform.localPosition = Vector3.zero;
        laser.transform.localRotation = Quaternion.identity;
        lightSource = new GameObject("Light");
        lightSource.transform.parent = laser.transform;
        lightSource.transform.localPosition = Vector3.zero;
        lightSource.transform.localRotation = Quaternion.identity;
        initializeLaser();
    }
	
	// Update is called once per frame
	void Update () {

        m_origin = transform.position;
        m_direction = transform.forward;

        updateLaser();

        CastRay();

        if (useParticleSystem && !psInitialized)
            initializeParticles();
        if (!useParticleSystem && psInitialized)
        {
            Destroy(laser.GetComponent<ParticleSystem>());
            psInitialized = false;
        }

        if (gotHit)
        {
            draw(transform.position, hit.point);
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }
    }

    private void draw(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        float distance = Vector3.Distance(start, end);

        //set light position a bit back to have better lighting
        lightSource.transform.position = end - (distance/100) * transform.forward;

        ////change light intensity and range according to distance and laser width
        //light.intensity = (endWidth * 30)/distance;
        //light.bounceIntensity = light.intensity / 1.5f;
        //light.range = endWidth * distance;


        if (useParticleSystem && laser.GetComponent<ParticleSystem>() != null)
        {
            //change particle distance adccording to hitpoint
            var psmain = ps.main;
            psmain.startLifetime = distance / 100;
        }
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

    private void initializeParticles()
    {
        //create particlesystem
        //TODO: add scaling/size with laser width
        ps = laser.AddComponent<ParticleSystem>();
        ps = laser.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.angle = 0;
        shape.radius = 0.01f;
        var em = ps.emission;
        em.rateOverTime = 8000;
        var psmain = ps.main;
        psmain.startSize = 0.1f;
        psmain.startLifetime = 0.1f;
        psmain.startSpeed = 100;
        psmain.maxParticles = 5000;
        psmain.startColor = new Color(1, 1, 1, 0.25f);
        //var pscolor = ps.colorOverLifetime;
        //pscolor.color = new ParticleSystem.MinMaxGradient(startColor, endColor);
        var psrenderer = ps.GetComponent<ParticleSystemRenderer>();
        psrenderer.material = particleMat;

        psInitialized = true;
    }

    private void updateLaser()
    {
        lr.startWidth = width;
        lr.endWidth = width;
        lr.startColor = startColor;
        lr.endColor = endColor;
        light.color = endColor;
        light.intensity = width * 10;
        light.bounceIntensity = width * 3;
        light.range = width / 2.5f;
    }
}
