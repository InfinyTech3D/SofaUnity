using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SBaseObject : MonoBehaviour
    {                
        protected bool m_log = false;

        protected SofaContext m_context = null;

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

        protected bool loadContext()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseObject::loadContext");

            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                // get Sofa context
                m_context = _contextObject.GetComponent<SofaContext>();

                // really Create the gameObject linked to sofaObject
                createObject();

                return true;
            }
            else
            {
                Debug.LogError("SMesh::No context.");
                return false;
            }
        }

        /// Method called by \sa loadContext() method. To be implemented by child class.
        protected virtual void createObject()
        {

        }
    }
}
