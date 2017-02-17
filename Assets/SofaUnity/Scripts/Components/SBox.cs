using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    public class SBox : SBaseGrid
    {
        internal SofaBox m_impl;
        public GameObject m_context;

        void Awake()
        {
            Debug.Log("SBox::Awake called.");
            m_context = GameObject.Find("SofaContext");
            if (m_context != null)
            {
                Debug.Log("SBox::Has context.");
                SofaContext context = m_context.GetComponent<SofaContext>();
                IntPtr _simu = context.getSimuContext();
                if (_simu != IntPtr.Zero)
                {
                    Debug.Log("SBox::Has simu.");
                    m_impl = new SofaBox(_simu);
                }
            }
            else
                Debug.Log("SBox::No context.");
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("SBox::Start called.");
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("SBox::Update called.");

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (m_impl != null)
                    m_impl.test();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                if (m_impl != null)
                    m_impl.addCube();
            }

        }


    }
}
