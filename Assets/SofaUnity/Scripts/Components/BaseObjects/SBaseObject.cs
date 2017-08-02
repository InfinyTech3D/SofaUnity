using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Base class to map a Sofa Object with a Unity GameObject
    /// This class control the creation of the object as well as the link to the SofaContext 
    /// </summary>
    public class SBaseObject : MonoBehaviour
    {
        /// Pointer to the Sofa context this GameObject belongs to.
        protected SofaContext m_context = null;

        /// Name of this GameObject
        protected string m_nameId;
        public string nameId
        {
            get { return m_nameId; }
            set { m_nameId = value; }
        }

        /// Parameter to activate logging of this Sofa GameObject
        protected bool m_log = false;


        /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
        void Awake()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::Awake - " + m_nameId);

            loadContext();

            awakePostProcess();
        }

        /// Method called at GameObject init (after creation or when starting play). To be implemented by child class.
        void Start()
        {
            Debug.Log("SBaseObject::Start called.");
        }

        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        void Update()
        {
            Debug.Log("SBaseObject::Update called.");
        }


        /// Method called to update GameObject, called once per frame. To be implemented by child class.
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

                int pos = this.name.IndexOf("-");
                if (pos != -1)
                    m_nameId = this.name.Substring(pos + 2, this.name.Length - (pos + 2)); // remove the space
                else
                {
                    m_nameId = this.name;
                    m_nameId += "_" + m_context.objectcpt;
                }

                if (m_log)
                    Debug.Log("this.name : " + this.name + " - m_nameId: " + m_nameId);

                // really Create the gameObject linked to sofaObject
                createObject();

                // increment counter if loading scene
                m_context.countCreated();

                m_context.objectcpt = m_context.objectcpt + 1;

                return true;
            }
            else
            {
                Debug.LogError("SMesh::No context.");
                return false;
            }
        }

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
        protected virtual void createObject()
        {

        }

        /// Method called by @sa Awake() method. As post process method after creation. To be implemented by child class.
        protected virtual void awakePostProcess()
        {

        }
    }
}
