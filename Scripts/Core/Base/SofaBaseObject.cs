using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Base class to map a Sofa3DObject with a Unity GameObject
    /// This class control the creation of the Sofa3DObject as well as the link to the SofaContext 
    /// </summary>
    [ExecuteInEditMode]
    public class SofaBaseObject : MonoBehaviour
    {
        /////////////////////////////////////////////
        //////    SofaBaseObject API members    /////
        /////////////////////////////////////////////

        /// Pointer to the Sofa context this GameObject belongs to.
        protected SofaContext m_sofaContext = null;

        /// Name of this object, should be unique and will be used for the DAGNode and component names
        [SerializeField]
        protected string m_uniqueNameId = "None";

        /// Name of the Node parent name
        [SerializeField]
        protected string m_parentName = "None";

        /// Parameter to activate logging of this SOFA GameObject
        public bool m_log = false;


        /// Parameter storing the fact that the gameObject has been created. Different from @sa m_isCreated
        protected bool m_isAwake = false;

        /// Paramter storing the fact that the SofaObject has been created.
        protected bool m_isCreated = false;


        /////////////////////////////////////////////
        //////   SofaBaseObject API accessors   /////
        /////////////////////////////////////////////

        /// Getter/Setter for \sa m_uniqueNameId
        public string UniqueNameId
        {
            get { return m_uniqueNameId; }
            set { m_uniqueNameId = value; }
        }

        /// Method to get the parent node name of this object.
        public virtual string ParentName()
        {
            return m_parentName;
        }

        /// Getter for the parameter @sa m_isCreated
        public bool IsCreated() { return m_isCreated; }

        /// Getter for the parameter @sa m_isAwake
        public bool IsAwake() { return m_isAwake; }


        /// Setter for SofaContext \sa m_sofaContext
        public void SetSofaContext(SofaContext sofacontext)
        {
            m_sofaContext = sofacontext;
            if (m_sofaContext == null)
            {
                SofaLog("SofaBaseObject::loadContext - GetComponent<SofaContext> failed.", 2);
                return;
            }
        }

        /////////////////////////////////////////////
        //////    SofaBaseObject public API     /////
        /////////////////////////////////////////////

        // priority 0 = debug, 1 = warning, 2 = error
        protected void SofaLog(string msg, int priority = 0, bool forceLog = false)
        {
            string mode = "PlayMode";
            if (!Application.isPlaying)
                mode = "EditorMode";

            if (priority == 0)
            {
                if (forceLog || m_log)
                    Debug.Log("## " + Time.fixedTime + " - " + mode + " ##    " + this.name + "   >>>>   " + msg);
            }
            else if (priority == 1)
                Debug.LogWarning("## " + Time.fixedTime + " - " + mode + "## " + this.name + "   >>>>   " + msg);
            else if (priority == 2)
                Debug.LogError("## " + Time.fixedTime + " - " + mode + "## " + this.name + "   >>>>   " + msg);
        }


        /// Method called at GameObject creation. Will call internal method @see awakePostProcess()
        void Awake()
        {
            SofaLog("Awake - " + m_uniqueNameId);

            // add specific preprocessing here?


            // Call a post process method for additional codes.
            if (AwakePostProcess())
                m_isAwake = true;
        }


        /// Method called at GameObject init (after creation or when starting play). To be implemented by child class.
        void Start()
        {
            SofaLog("Start - " + m_uniqueNameId);

            Init_impl();
        }


        /// Method called to update GameObject, called once per frame. To be implemented by child class.
        void Update()
        {
            //SofaLog("SofaBaseObject::Update " + this.name + " called.");

            //if (!Application.isPlaying)
            //{
            //    updateInEditor();
            //    return;
            //}

            //// Call internal method that can be overwritten. Only if dirty
            //if (m_isDirty)
            //{
            //    updateImpl();
            //    m_isDirty = false;
            //}
        }


        public void CreateObject(SofaContext sofacontext, string name, string parentName)
        {
            SofaLog("####### SofaBase::Create: " + UniqueNameId);
            UniqueNameId = name;
            m_parentName = parentName;
            SetSofaContext(sofacontext);

            if (m_sofaContext != null)
                Create_impl();
            else
                SofaLog("SofaBaseObject::CreateObject has a null m_sofaContext.", 2);
        }


        public void Reconnect(SofaContext sofacontext)
        {
            SofaLog("####### SofaBase::Reconnect: " + UniqueNameId);
            SetSofaContext(sofacontext);
            Reconnect_impl();
        }

        /////////////////////////////////////////////
        //////   SofaBaseObject internal API    /////
        /////////////////////////////////////////////

        /// called by @sa CreateObject method, once the sofaContext and all parameters have been set.  To be implemented by child class.
        protected virtual void Create_impl()
        {

        }


        /// Method called by @sa Reconnect() method from SofaContext. To be implemented by child class.
        protected virtual void Reconnect_impl()
        {

        }


        ///// Method called by @sa Awake() method. As post process method after creation. To be implemented by child class.
        protected virtual bool AwakePostProcess()
        {
            return true;
        }

        /// Method called by @sa Start() method. To be implemented by child class.
        protected virtual void Init_impl()
        {

        }


        ///// Method called by @sa Update() method. To be implemented by child class.
        //public virtual void updateImpl()
        //{

        //}


        ///// Method called by @sa Update() method. When Unity is not playing.
        //public virtual void updateInEditor()
        //{

        //}

    }
}
