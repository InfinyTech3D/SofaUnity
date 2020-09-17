using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SofaUnity
{
    /// <summary>
    /// Base class of SOFA Unity GameObject
    /// This is an abstract class to control the global behavior of all SOFA GameObject in Unity
    /// It contains generic methods to set SOFAContext, uniqueNameID, create and reconnect object to the sofaContext
    /// </summary>
    [ExecuteInEditMode]
    public class SofaBase : MonoBehaviour
    {
        ////////////////////////////////////////////
        //////      Base SOFA API members      /////
        ////////////////////////////////////////////

        /// Name of this object, should be unique
        [SerializeField]
        protected string m_uniqueNameId = "Not set";

        [SerializeField]
        protected string m_displayName = "Not set";

        /// Pointer to the Sofa context this GameObject belongs to.
        public SofaContext m_sofaContext = null;

        /// Parameter to activate logging of this SOFA GameObject
        public bool m_log = false;

        /// Parameter storing the fact that the object is fully init
        protected bool m_isReady = false;

        /// bool to store the status of this GameObject. Used to update the mesh if is dirty.
        protected bool m_isDirty = true;

        /// Parameter to store if this component has been created from Unity side
        protected bool m_isCustom = false;



        ////////////////////////////////////////////
        //////     Base SOFA API accessors     /////
        ////////////////////////////////////////////

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

        /// Getter returning \sa m_isReady value
        public bool IsReady() { return m_isReady; }

        /// Getter/Setter for \sa m_uniqueNameId
        public string UniqueNameId
        {
            get { return m_uniqueNameId; }
            set { m_uniqueNameId = value; }
        }

        /// Getter/Setter for \sa m_uniqueNameId
        public string DisplayName
        {
            get { return m_displayName; }
            set { m_displayName = value; }
        }

        /// Setter for \sa m_isDirty value   
        public void SetDirty(bool value) { m_isDirty = value; }



        ////////////////////////////////////////////
        /////        Object public API         /////
        ////////////////////////////////////////////

        //// Start is called before the first frame update
        void Awake()
        {
            
        }

        /// call by a thrid party, should do the same as awake but here we directly give the pointer to sofaContext. Will call \sa Create_impl()
        public void Create(SofaContext sofacontext, string name, string displayName, bool isCustom = false)
        {
            SofaLog("####### SofaBase::Create: " + UniqueNameId);
            UniqueNameId = name;
            DisplayName = displayName;
            m_isCustom = isCustom;
            SetSofaContext(sofacontext);
            Create_impl();
        }

        /// call by a thrid party, will reconnect the object to the right SofaContext without recreating the GameObject. Will call Reconnect_impl()
        public void Reconnect(SofaContext sofacontext)
        {
            SofaLog("####### SofaBase::Reconnect: " + UniqueNameId);
            SetSofaContext(sofacontext);
            Reconnect_impl();
        }        

        /// Call by Unity animation loop after all GameObjects have been created. Will call InitImpl()
        void Start()
        {            
            Init_impl();
        }

        private bool firstUpdate = true;
        /// Call by Unity animation loop at each step. Update is called once per frame. Will call \sa UpdateImpl() or UpdateInEditor
        void Update()
        {
            if(firstUpdate)
            {
                SofaLog("####### SofaBase::First Update: " + UniqueNameId);
                firstUpdate = false;
            }

            if (!Application.isPlaying)
            {
                UpdateInEditor();
                return;
            }

            // Call internal method that can be overwritten. Only if dirty
            if (m_isDirty)
            {
                Update_impl();
                m_isDirty = false;
            }
        }


        ////////////////////////////////////////////
        /////        Object virtual API        /////
        ////////////////////////////////////////////

        /// Method called by @sa Create() method. To be implemented by child class.
        protected virtual void Create_impl()
        {

        }

        /// Method called by @sa Reconnect() method. To be implemented by child class.
        protected virtual void Reconnect_impl()
        {

        }

        /// Method called by @sa Start() method. To be implemented by child class.
        protected virtual void Init_impl()
        {

        }

        /// Method called by @sa Update() method. To be implemented by child class.
        protected virtual void Update_impl()
        {

        }


        /// Method called by @sa Update() method. When Unity is not playing.
        protected virtual void UpdateInEditor()
        {

        }



        ////////////////////////////////////////////
        /////       Generic utils methods      /////
        ////////////////////////////////////////////

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


        /// convert a full string with comma into a list of strings
        public List<string> ConvertStringToList(string valueString)
        {
            List<string> values = valueString.Split(',').ToList();
            return values;
        }
    }
}
