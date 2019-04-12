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
        public bool m_log = false;

        protected bool directChild = true;

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

        protected static int cptLifeCycle = 0;

        // priority 0 = debug, 1 = warning, 2 = error
        protected void sofaLog(string msg, int priority = 0, bool forceLog = false)
        {
            string mode = "PlayMode";
            if (!Application.isPlaying)
                mode = "EditorMode";

            if (priority == 0)
            {
                if (forceLog || m_log)
                    Debug.Log("## " + Time.fixedTime + " " + mode + "## " + this.name + " >> " + msg);
            }
            else if (priority == 1)
                Debug.LogWarning("## " + Time.fixedTime + " " + mode + "## " + this.name + " >> " + msg);
            else if (priority == 2)
                Debug.LogError("## " + Time.fixedTime + " " + mode + "## " + this.name + " >> " + msg);
        }


        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()

        void Awake()
        {
            sofaLog("Awake - " + m_nameId);

            // First load the Sofa context and create the object.
            loadContext();

            // init the parameters from the sofa object
            if (!Application.isPlaying)
                initParameters();
            else
                initParametersOnPlay();

            // Call a post process method for additional codes.
            awakePostProcess();

            // Store the fact that awake has finished.
            m_isAwake = true;

            // Increment life cycle
            cptLifeCycle++;
        }

            
        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        protected bool loadContext()
        {
            sofaLog("Awake - loadContext");

            // Search for SofaContext
            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                // Get Sofa context
                m_context = _contextObject.GetComponent<SofaContext>();

                if (m_context == null)
                {
                    sofaLog("SBaseObject::loadContext - GetComponent<SofaContext> failed.", 2);
                    return false;
                }

                // By default place this object as child of SofaContext
                if (this.directChild)
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

                sofaLog("this.name : " + this.name + " - m_nameId: " + m_nameId);

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
                sofaLog("SBaseObject::loadContext - No SofaContext found.", 2);
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

        /// called by @sa Awake method. To store value from sofa object at runTime
        protected virtual void initParametersOnPlay()
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
            sofaLog("SBaseObject::Start called.");

            // Increment life cycle
            cptLifeCycle++;
        }
        

        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        void Update()
        {
            //sofaLog("SBaseObject::Update " + this.name + " called.");

            if (!Application.isPlaying)
            {
                updateInEditor();
                return;
            }

            // Call internal method that can be overwritten. Only if dirty
            if (m_isDirty)
            {
                updateImpl();
                m_isDirty = false;
            }
        }


        /// Method called by @sa Update() method. To be implemented by child class.
        public virtual void updateImpl()
        {

        }


        /// Method called by @sa Update() method. When Unity is not playing.
        public virtual void updateInEditor()
        {

        }


        public virtual void activateObject()
        {

        }

        public virtual void deactivateObject()
        {

        }


        /// Unity callback method OnEnable, will check cptLifeCycle to detect when animation will start.
        protected void OnEnable()
        {
            // if static int cptLifeCycle ==0 , this means animation is going to start.
            if (!Application.isPlaying && cptLifeCycle == 0)
            {
                copyDataForAnimation();
            }

            // Increment life cycle
            cptLifeCycle++;

            activateObject();
        }

        protected void OnDisable()
        {

            deactivateObject();
        }


        /// Method called just before animation start by OnEnable. To be able to copy init parameters to current one.
        protected virtual void copyDataForAnimation()
        {

        }
    }
}
