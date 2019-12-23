using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SofaUnity
{

    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }




    /// <summary>
    /// Base class of SOFA Unity GameObject
    /// This class control the commun members and methods to all GameObject of SOFA-Unity
    /// </summary>
    public class SofaBase : MonoBehaviour
    {
        ////////////////////////////////////////////
        ////////        Base SOFA API        ///////
        ////////////////////////////////////////////

        /// Pointer to the Sofa context this GameObject belongs to.
        public SofaContext m_sofaContext = null;
        public void SetSofaContext(SofaContext sofacontext)
        {
            m_sofaContext = sofacontext;
            if (m_sofaContext == null)
            {
                SofaLog("SBaseObject::loadContext - GetComponent<SofaContext> failed.", 2);
                return;
            }
        }

        public List<string> ConvertStringToList(string valueString)
        {
            List<string> values = valueString.Split(',').ToList();
            return values;
        }

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = true;


        /// Parameter storing the fact that the object is fully init
        protected bool m_isReady = false;
        public bool IsReady() { return m_isReady; }


        /// Name of this object, should be unique
        [SerializeField]
        [ReadOnly]
        protected string m_uniqueNameId = "Not set";
        public string UniqueNameId
        {
            get { return m_uniqueNameId; }
            set { m_uniqueNameId = value; }
        }


        /// bool to store the status of this GameObject. Used to update the mesh if is dirty.
        protected bool m_isDirty = true;
        public void SetDirty() { m_isDirty = true; }


        // priority 0 = debug, 1 = warning, 2 = error
        protected void SofaLog(string msg, int priority = 0, bool forceLog = false)
        {
            string mode = "PlayMode";
            if (!Application.isPlaying)
                mode = "EditorMode";

            if (priority == 0)
            {
                if (forceLog || m_log)
                    Debug.Log("##" + Time.fixedTime + " - " + mode + "## " + this.name + " >> " + msg);
            }
            else if (priority == 1)
                Debug.LogWarning("## " + Time.fixedTime + " - " + mode + "## " + this.name + " >> " + msg);
            else if (priority == 2)
                Debug.LogError("## " + Time.fixedTime + " - " + mode + "## " + this.name + " >> " + msg);
        }


        ////////////////////////////////////////////
        /////        Object public API         /////
        ////////////////////////////////////////////

        //// Start is called before the first frame update
        void Awake()
        {

        }
                
        // call by a thrid party, should do the same as awake but here we directly give the pointer to sofaContext
        public void Init(SofaContext sofacontext, string name)
        {
            UniqueNameId = name;
            SetSofaContext(sofacontext);
            InitImpl();
        }

        public void Reconnect(SofaContext sofacontext)
        {
            SetSofaContext(sofacontext);
            ReconnectImpl();
        }        

        void Start()
        {
            SofaLog("####### SofaBase::Start: " + UniqueNameId);
            
        }

        private bool firstUpdate = true;
        // Update is called once per frame
        void Update()
        {
            if(firstUpdate)
            {
                SofaLog("####### SofaBase::Update: " + UniqueNameId);
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
                UpdateImpl();
                m_isDirty = false;
            }
        }


        ////////////////////////////////////////////
        /////        Object virtual API        /////
        ////////////////////////////////////////////

        protected virtual void AwakePostProcess()
        {

        }

        protected virtual void InitImpl()
        {

        }

        protected virtual void ReconnectImpl()
        {

        }
        

        /// Method called by @sa Update() method. To be implemented by child class.
        protected virtual void UpdateImpl()
        {

        }


        /// Method called by @sa Update() method. When Unity is not playing.
        protected virtual void UpdateInEditor()
        {

        }


    }
}
