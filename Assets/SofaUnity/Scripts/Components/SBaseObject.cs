using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SBaseObject : MonoBehaviour
    {
        /// Mesh of this object
		protected Mesh m_mesh;
        /// Mesh renderer of this object
        //private MeshRenderer m_meshRenderer;
        protected bool m_log = false;

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
            //gameObject.AddComponent<MeshFilter>();
            //gameObject.AddComponent<MeshRenderer>();

            //m_mesh = gameObject.GetComponent<MeshFilter>().mesh;
            //m_meshRenderer = gameObject.GetComponent<MeshRenderer>();

          //  gameObject.transform.position = new Vector3(1, 1, 1); ;
        }        

        string m_nameId;
        public string nameId
        {
            get { return m_nameId; }
            set { m_nameId = value; }
        }
    }
}
