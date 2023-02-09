using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

/// <summary>
/// GameObject to control a GeomagicDevice on the SOFA simulation side. This component need to be linked with a geomagicDriver
/// SOFA component. @sa m_geomagicDriver
/// This component will update the GameObject transform it is assign to with the geomagic device information.
/// </summary>
[ExecuteInEditMode]
public class GeomagicController : MonoBehaviour
{
    ////////////////////////////////////////////
    /////    GeomagicController members    /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the GeomagicDriver SofaComponent it will control.
    [SerializeField]
    protected SofaComponent m_geomagicDriver = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaGeomagic m_sofaGeomagic = null;

    /// Audio source to be controlled by this Geomagic actionner
    public AudioSource m_audioSource = null;

    /// Bool to know if 1st button of the tool is pressed.
    protected bool m_deviceButton1 = false;
    /// Bool to know if 2nd button of the tool is pressed.
    protected bool m_deviceButton2 = false;

    /// Bool to store a toogle status value when pressing 1st button.
    protected bool m_statusButton1 = false;
    /// Bool to store a toogle status value when pressing 2nd button.
    protected bool m_statusButton2 = false;

    /// Bool to know if tool is currently in contact, sending some forcefeedback.
    protected bool m_toolInContact = false;

    public GameObject smoke = null;



    ////////////////////////////////////////////
    /////   GeomagicController accessors   /////
    ////////////////////////////////////////////

    /// Accessor to know if 1st button is pressed. @sa m_deviceButton1
    public bool IsButton1Pressed() { return m_deviceButton1; }

    /// Accessor to know if 2nd button is pressed. @sa m_deviceButton2
    public bool IsButton2Pressed() { return m_deviceButton2; }

    /// Accessor to know 1st button toogle status. @sa m_statusButton1
    public bool Button1Status() { return m_statusButton1; }
    /// Accessor to know 2nd button toogle status. @sa m_statusButton2
    public bool Button2Status() { return m_statusButton2; }

    /// Accessor to know if tool is currently in contact. @sa m_toolInContact
    public bool IsToolInContact() { return m_toolInContact; }


    /// Getter/setter to the GeomagicDriver SofaComponent object.
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
    /////   GeomagicController public API  /////
    ////////////////////////////////////////////

    /// Method call by Unity loop when simulation start. Will call @sa CreateDeviceController and @sa GeomagicDevice_init if possible
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


    /// Main method to create the controller on the SOFA side.
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
            m_sofaGeomagic = new SofaGeomagic(m_sofaContext.GetSimuContext(), m_geomagicDriver.DisplayName);
        }
    }


    /// Method called at each unity step. Will get information from the simulation and update the GAmeobject transform
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

                    if (m_audioSource)
                    {
                        if (m_statusButton1)
                            m_audioSource.Play();
                        else
                            m_audioSource.Stop();
                    }

                    if (smoke)
                    {
                        if (m_statusButton1)
                            smoke.SetActive(true);
                        else
                            smoke.SetActive(false);
                    }

                        m_deviceButton1 = true;
                }
                else
                {
                    m_deviceButton1 = false;
                }

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

        //if (Input.GetKeyDown(KeyCode.End))
        //{
        //    if (m_sofaGeomagic != null)
        //        m_sofaGeomagic.numberOfTools();
        //}
    }
}