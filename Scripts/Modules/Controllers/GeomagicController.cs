using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

[ExecuteInEditMode]
public class GeomagicController : MonoBehaviour
{
    ////////////////////////////////////////////
    /////    GeomagicController members    /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    [SerializeField]
    protected SofaComponent m_geomagicDriver = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaGeomagic m_sofaGeomagic = null;

    public GameObject particles = null;
    private AudioSource source = null;



    protected bool m_deviceButton1 = false;
    protected bool m_deviceButton2 = false;
    
    protected bool m_statusButton1 = false;
    protected bool m_statusButton2 = false;
    protected bool m_toolInContact = false;

    public bool IsButton1Pressed()
    {
        return m_deviceButton1;
    }
    public bool IsButton2Pressed()
    {
        return m_deviceButton2;
    }

    public bool Button1Status()
    {
        return m_statusButton1;
    }
    public bool Button2Status()
    {
        return m_statusButton2;
    }

    public bool IsToolInContact()
    {
        return m_toolInContact;
    }

    public SofaComponent GeomagicDriver
    {
        get { return m_geomagicDriver; }
        set
        {
            if (value != m_geomagicDriver)
            {
                m_geomagicDriver = value;
                CreateDeviceController();
            }
        }
    }

    ////////////////////////////////////////////
    /////       Object creation API        /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        
        // get acces to the audio source object
        //source = GetComponent<AudioSource>();
    }
    

    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        if (GeomagicDriver != null)
            CreateDeviceController();

        if (Application.isPlaying)
        {
            if (m_sofaGeomagic != null && !m_sofaGeomagic.IsReady())
                m_sofaGeomagic.GeomagicDevice_init();
        }
    }


    protected void CreateDeviceController()
    {
        if (m_geomagicDriver == null)
            return;

        m_sofaContext = m_geomagicDriver.m_sofaContext;
        bool contextOk = true;
        if (m_sofaContext == null)
        {
            Debug.LogError("GeomagicController::loadContext - GetComponent<SofaUnity.SofaContext> failed.");
            contextOk = false;
        }

        if (contextOk)
        {
            m_sofaGeomagic = new SofaGeomagic(m_sofaContext.GetSimuContext(), m_geomagicDriver.UniqueNameId);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (m_sofaGeomagic != null && m_sofaGeomagic.IsReady())
        {
            float[] val = new float[7];
            int res = m_sofaGeomagic.GeomagicDevice_getPosition(val);
            if (res == 0)
            {
                //Debug.Log(val[0] + " " + val[1] + " " + val[2] + " " + val[3] + " " + val[4] + " " + val[5] + " " + val[6]);
                Vector3 sofaScale = m_sofaContext.GetScaleSofaToUnity();
                this.transform.localPosition = new Vector3(val[0] * sofaScale[0], val[1] * sofaScale[1], val[2] * sofaScale[2]);
                var rotation = new Quaternion(val[3], val[4], val[5], val[6]);
                Vector3 angles = rotation.eulerAngles;
                //Debug.Log("angles: " + angles);
                rotation.eulerAngles = new Vector3(angles[0], -angles[1], -angles[2]);
                this.transform.localRotation = rotation;// * Quaternion.Euler(90, 0, 0);
            }
            else
                Debug.Log("Error position returns : " + res);

            int[] contact = new int[1];
            int res3 = m_sofaGeomagic.GeomagicDevice_getStatus(contact);
            if (res3 == 0)
            {
                if (contact[0] == 1)
                    m_toolInContact = true;
                else
                    m_toolInContact = false;
            }
            else
                Debug.Log("Error status returns : " + res3);

            int[] status = new int[1];
            int res2 = m_sofaGeomagic.GeomagicDevice_getButton1Status(status);
            if (res2 == 0)
            {
                if (status[0] == 1)
                {
                    if (!m_deviceButton1) // switch status
                        m_statusButton1 = !m_statusButton1;

                    m_deviceButton1 = true;
                }
                else
                    m_deviceButton1 = false;

                if (particles)
                    particles.SetActive(m_deviceButton1);
            }
            else
                Debug.LogError("EntactManager::Update - No Geomagic found.");

            res2 = m_sofaGeomagic.GeomagicDevice_getButton2Status(status);
            if (res2 == 0)
            {
                if (status[0] == 1)
                {
                    if (!m_deviceButton2) // switch status
                        m_statusButton2 = !m_statusButton2;

                    m_deviceButton2 = true;
                }
                else
                    m_deviceButton2 = false;


            }
            else
                Debug.LogError("EntactManager::Update - No Geomagic found.");
        }
        else
        {

        }

        if (source)
        {
            if (m_deviceButton1 && !source.isPlaying)
                source.Play();
            else if (!m_deviceButton1 && source.isPlaying)
                source.Stop();
        }

        //if (Input.GetKeyDown(KeyCode.End))
        //{
        //    if (m_sofaGeomagic != null)
        //        m_sofaGeomagic.numberOfTools();
        //}
    }
}