using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Base class of SOFA Unity GameObject
    /// This class control the commun members and methods to all GameObject of SOFA-Unity
    /// </summary>
    public class SBase : MonoBehaviour
    {
        ////////////////////////////////////////////
        ////////        Base SOFA API        ///////
        ////////////////////////////////////////////

        /// Pointer to the Sofa context this GameObject belongs to.
        protected SofaContext m_context = null;

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = false;


        /// Parameter storing the fact that the object is fully init
        protected bool m_isReady = false;
        public bool isReady() { return m_isReady; }


        /// Name of this object, should be unique
        protected string m_uniqueNameId = "Not set";
        public string uniqueNameId
        {
            get { return m_uniqueNameId; }
            set { m_uniqueNameId = value; }
        }


        /// bool to store the status of this GameObject. Used to update the mesh if is dirty.
        protected bool m_isDirty = true;
        public void setDirty() { m_isDirty = true; }


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

        //// Start is called before the first frame update
        //void Awake()
        //{
        //    sofaLog("Awake - " + m_nameId);
        //}

        //void Start()
        //{
            


        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}
