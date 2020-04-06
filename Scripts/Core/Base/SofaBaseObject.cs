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

        /// Parameter storing the fact that the object is fully init
        protected bool m_isAwake = false;

        /// Parameter to activate logging of this SOFA GameObject
        public bool m_log = false;

        /// Name of this object, should be unique
        [SerializeField]
        protected string m_uniqueNameId = "Not set";



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
            return "No impl";
        }

        public bool isAwake() { return m_isAwake; }


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


        /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
        void Awake()
        {
            SofaLog("Awake - " + m_uniqueNameId);

            // Call a post process method for additional codes.
//            awakePostProcess();

            // Store the fact that awake has finished.
            m_isAwake = true;

        }


        /// Method called at GameObject init (after creation or when starting play). To be implemented by child class.
        void Start()
        {
            SofaLog("Start - " + m_uniqueNameId);

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

        

        /////////////////////////////////////////////
        //////   SofaBaseObject internal API    /////
        /////////////////////////////////////////////

        /// called by @sa Awake method.
        protected virtual void CreateObject()
        {

        }


        /// 
        protected virtual void LoadSofaContext(SofaUnity.SofaContext _context)
        {

        }


        ///// Method called by @sa Awake() method. As post process method after creation. To be implemented by child class.
        //protected virtual void awakePostProcess()
        //{

        //}


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
