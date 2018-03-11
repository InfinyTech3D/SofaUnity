using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Base class to map a Sofa3DObject with a Unity GameObject
    /// This class control the creation of the Sofa3DObject as well as the link to the SofaContext 
    /// </summary>
    public class SBaseObject : MonoBehaviour
    {
        ////////////////////////////////////////////
        /////        Object members API        /////
        ////////////////////////////////////////////

        /// Pointer to the Sofa context this GameObject belongs to.
        protected SofaContext m_context = null;

        /// Parameter to activate logging of this Sofa GameObject
        protected bool m_log = false;

        /// Parameter storing the fact that the object is fully init
        protected bool m_isAwake = false;
        public bool isAwake() { return m_isAwake; }

        /// Name of the Sofa3DObject mapped to this Unity GameObject
        protected string m_nameId;
        public string nameId
        {
            get { return m_nameId; }
            set { m_nameId = value; }
        }

        /// bool to store the status of this GameObject. Used to update the mesh if is dirty.
        protected bool m_isDirty = true;
        public void setDirty() { m_isDirty = true; }

        /// Method to get the parent node name of this object.
        public virtual string parentName()
        {
            return "No impl";
        }



        
        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
        void Awake()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::Awake - " + m_nameId);

            // First load the Sofa context and create the object.
            loadContext();

            // init the parameters from the sofa object
            initParameters();

            // Call a post process method for additional codes.
            awakePostProcess();

            // Store the fact that awake has finished.
            m_isAwake = true;
        }

            
        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        protected bool loadContext()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseObject::loadContext");

            // Search for SofaContext
            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                // Get Sofa context
                m_context = _contextObject.GetComponent<SofaContext>();

                if (m_context == null)
                {
                    Debug.LogError("SBaseObject::loadContext - GetComponent<SofaContext> failed.");
                    return false;
                }

                // By default place this object as child of SofaContext
                this.transform.parent = _contextObject.gameObject.transform;

                // Look for node a name. Remove unneeded parts of the name (like _Node)
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

                // Really Create the gameObject linked to sofaObject
                createObject();

                // Increment counter if objectis created from loading scene process
                m_context.countCreated();

                // Increment the context object counter for names.
                m_context.objectcpt = m_context.objectcpt + 1;
                m_context.registerObject(this);

                return true;
            }
            else
            {
                Debug.LogError("SBaseObject::loadContext - No SofaContext found.");
                return false;
            }
        }


        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
        protected virtual void createObject()
        {

        }


        /// called by @sa Awake method. To store value from sofa object
        protected virtual void initParameters()
        {

        }


        /// Method called by @sa Awake() method. As post process method after creation. To be implemented by child class.
        protected virtual void awakePostProcess()
        {

        }

        


        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called at GameObject init (after creation or when starting play). To be implemented by child class.
        void Start()
        {
            if (m_log)
                Debug.Log("SBaseObject::Start called.");
        }
        

        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        void Update()
        {
            if (m_log)
                Debug.Log("SBaseObject::Update called.");

            // Call internal method that can be overwritten. Only if dirty
            if (m_isDirty)
            {
                updateImpl();
                m_isDirty = false;
            }
        }


        /// Method called by @sa Update() method. To be implemented by child class.
        protected virtual void updateImpl()
        {

        }        
    }
}
