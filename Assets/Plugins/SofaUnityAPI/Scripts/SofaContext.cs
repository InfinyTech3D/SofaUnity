using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SofaUnity
{
    public class SofaContext : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Debug.Log("SofaContext::Start called.");
            GL.wireframe = true;
        }
        void OnPreRender()
        {
            GL.wireframe = true;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("SofaContext::Update called.");
        }

        void Awake()
        {
            Debug.Log("SofaContext::Awake called.");
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
    }
}
