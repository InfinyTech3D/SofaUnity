using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SofaUnityAPI;


namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SofaContext : MonoBehaviour
    {
        SofaContextAPI m_impl;
        
        void Awake()
        {
            Debug.Log("SofaContext::Awake called.");
            init();
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("SofaContext::Start called.");
            GL.wireframe = true;
            
            //this.transform.position = new Vector3(0, 10, 0);
        }


        void OnDestroy()
        {
            Debug.Log("SofaContext::OnDestroy called.");
#if !UNITY_EDITOR
            Debug.Log("SofaContext::OnDestroy stop called.");
            m_impl.stop();
            m_impl.Dispose();       
#endif
        }

        void OnPreRender()
        {
            GL.wireframe = true;
        }


        void init()
        {
            Debug.Log("SofaContext::init called.");
            if (m_impl == null)
            {
                Debug.Log("SofaContext::init Ok.");
                m_impl = new SofaContextAPI();
                m_impl.start();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log("SofaContext::Update called.");
            m_impl.step();           
        }

        Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);
        public Vector3 gravity
        {
            get { return m_gravity; }
            set
            {
                
                //if (_ddWorld != null)
                //{
                //    BulletSharp.Math.Vector3 grav = value.ToBullet();
                //    _ddWorld.SetGravity(ref grav);
                //}
                m_gravity = value;
            }
        }

        float m_timeStep = 0.02f; // ~ 1/60
        public float timeStep
        {
            get
            {
                return m_timeStep;
            }
            set
            {
                //if (lateUpdateHelper != null)
                //{
                //    lateUpdateHelper.m_fixedTimeStep = value;
                //}
                m_timeStep = value;
            }
        }

        public IntPtr getSimuContext()
        {
            if (m_impl == null)
                init();
            return m_impl.getSimuContext();
        }
    }
}
