using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Abstract class for Sofa Data representation which is Serializable to store value in Unity scenes.
    /// </summary>
    [System.Serializable]
    public class SofaBaseData
    {
        ////////////////////////////////////////////
        //////      SofaBaseData members       /////
        ////////////////////////////////////////////

        /// Pointer to the Sofa component owner of this Data
        [SerializeField]
        protected SofaBaseComponent m_owner;

        /// Name of the Data
        [SerializeField]
        protected string m_dataName = "";

        /// Type of the Data
        [SerializeField]
        protected string m_dataType = "";

        /// Bool to store info if Data can be edited or not.
        [SerializeField]
        protected bool m_isReadOnly = false;

        /// Bool to store if Data has been changed inside Unity Editor when starting or opening scene.
        [SerializeField]
        protected bool m_isEdited = false;

        /// Bool to store info if Data has been edited and change need to be propagated.
        [SerializeField]
        protected bool m_isDirty = false;



        ////////////////////////////////////////////
        //////     SofaBaseData Accessors      /////
        ////////////////////////////////////////////

        /// Default constructor taking the Sofa component owner @sa m_owner, its name @sa m_dataName and its type @sa m_dataType as argument.
        public SofaBaseData(SofaBaseComponent owner, string nameID, string type)
        {
            m_owner = owner;
            m_dataName = nameID;
            m_dataType = type;
        }

        /// Getter of @sa m_dataName . No Setter available, only constructor can set that parameter.
        public string DataName
        {
            get { return m_dataName; }
        }

        /// Getter of @sa m_dataType . No Setter available, only constructor can set that parameter.
        public string DataType
        {
            get { return m_dataType; }
        }

        /// Getter of @sa m_isReadOnly . No Setter available,.
        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
            set { m_isReadOnly = value; }
        }

        /// TODO: work in progess method not yet used
        public bool SetValueIfEdited()
        {
            if (m_isEdited == true)
                return SetValueImpl();
            else
                return false;
        }


        ////////////////////////////////////////////
        //////        SofaBaseData API         /////
        ////////////////////////////////////////////

        /// Internal method to really set the value from Sofa inside this container. This method should be overriden by children
        protected virtual bool SetValueImpl()
        {
            return false;
        }

        /// Internal method to really get the value from Sofa inside this container. This method should be overriden by children
        protected virtual bool GetValueImpl()
        {
            return false;
        }
    }





    public class old_SofaBaseData
    {
        protected string m_nameID = "";
        protected bool m_isReadOnly = false;
        private SofaBaseObject m_owner;

        public old_SofaBaseData(string nameID, SofaBaseObject owner)
        {
            m_nameID = nameID;
            m_owner = owner;
        }

        public string nameID
        {
            get { return m_nameID; }
        }

        public bool isReadOnly
        {
            get { return m_isReadOnly; }
        }

        public virtual string getType()
        {
            return "None";
            //Debug.Log("SofaBaseData::Type");
        }
    }
}
