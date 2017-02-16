using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SBaseObject : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Debug.Log("SBaseObject::Start called.");
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("SBaseObject::Update called.");
        }

        void Awake()
        {
            Debug.Log("SBaseObject::Awake called.");
        }

        string m_nameId;
        public string nameId
        {
            get { return m_nameId; }
            set { m_nameId = value; }
        }
    }
}
