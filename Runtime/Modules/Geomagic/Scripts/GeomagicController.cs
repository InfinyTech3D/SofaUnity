using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
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
            if (GeomagicDriver == null)
            {
                Debug.LogWarning("GeomagicController::Start - No GeomagicDriver assigned to GameObject: " + this.name + " Will assigned first found.");

                SofaComponent[] components = GameObject.FindObjectsByType<SofaComponent>(FindObjectsSortMode.None);
                foreach (SofaComponent comp in components)
                {
                    if (comp.m_componentType == "GeomagicDriver")
                    {
                        GeomagicDriver = comp;
                        break;
                    }
                }

                if (GeomagicDriver == null) { 
                    Debug.LogError("GeomagicController::Start - No GeomagicDriver found in the scene for GameObject: " + this.name);
                    return;
                }
            }


            if (GeomagicDriver != null)
                CreateDeviceController();

            if (Application.isPlaying)
            {
                if (m_sofaGeomagic != null && !m_sofaGeomagic.IsReady())
                    m_sofaGeomagic.GeomagicDevice_init();
            }

            if (smoke)
            {
                smoke.SetActive(false);
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
                    // Get raw values from SOFA, need to inverse left-right hand coordinate system
                    Vector3 worldPos = new Vector3(-val[0], val[1], val[2]);
                    // Project world position into SofaContext frame
                    Vector3 localPos = m_sofaContext.transform.TransformPoint(worldPos);
                    this.transform.localPosition = localPos;

                    // Get SOFA quaternion and inverse rotation
                    var rotation = new Quaternion(val[3], val[4], val[5], val[6]);
                    Vector3 angles = rotation.eulerAngles;
                    this.transform.localEulerAngles = new Vector3(angles[0], -angles[1], -angles[2]);

                    // Combine current rotation with SofaContext one
                    this.transform.rotation = m_sofaContext.transform.rotation * this.transform.rotation;
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
                            if (m_deviceButton1)
                                smoke.SetActive(true);
                            else
                                smoke.SetActive(false);
                        }

                        m_deviceButton1 = true;
                    }
                    else
                    {
                        m_deviceButton1 = false;
                        if (smoke)
                            smoke.SetActive(false);
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

        /// Setter and Getter for SofaGeomagic internal component @sa m_sofaGeomagic
        public SofaGeomagic SofaGeomagicRef
        {
            get => m_sofaGeomagic;
            set => m_sofaGeomagic = value;
        }

    }
}
