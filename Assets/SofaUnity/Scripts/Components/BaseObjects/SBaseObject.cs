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
       
        protected string m_nameId;
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

                this.transform.parent = _contextObject.gameObject.transform;

                Debug.Log("this.name : " + this.name);
                int pos = this.name.IndexOf("-");
                if (pos != -1)
                    m_nameId = this.name.Substring(pos + 2, this.name.Length - (pos + 2)); // remove the space
                else
                {
                    m_nameId = this.name;
                    m_nameId += "_" + m_context.objectcpt;
                }

                Debug.Log("m_nameId: " + m_nameId);

                // really Create the gameObject linked to sofaObject
                createObject();

                // increment counter if loading scene
                m_context.countCreated();

                m_context.objectcpt = m_context.objectcpt + 1;
                Debug.Log("m_context.objectcpt: " + m_context.objectcpt);

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
