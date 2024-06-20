using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [Flags]
    public enum DataFlagsEnum
    {
        FLAG_NONE = 0,      ///< Means "no flag" when a value is required.
        FLAG_READONLY = 1 << 0, ///< The Data will be read-only in GUIs.
        FLAG_DISPLAYED = 1 << 1, ///< The Data will be displayed in GUIs.
        FLAG_PERSISTENT = 1 << 2, ///< The Data contains persistent information.
        FLAG_AUTOLINK = 1 << 3, ///< The Data should be autolinked when using the src="..." syntax.
        FLAG_REQUIRED = 1 << 4, ///< True if the Data has to be set for the owner component to be valid (a warning is displayed at init otherwise)
        FLAG_ANIMATION_INSTANCE = 1 << 10,
        FLAG_VISUAL_INSTANCE = 1 << 11,
        FLAG_HAPTICS_INSTANCE = 1 << 12,
    };

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

        [SerializeField]
        protected int m_counter = 0;

        [SerializeField]
        protected DataFlagsEnum m_flag = DataFlagsEnum.FLAG_NONE;

        [SerializeField]
        protected bool m_isSupported = true;

        [SerializeField]
        protected bool m_isVector = false;

        ////////////////////////////////////////////
        //////     SofaBaseData Accessors      /////
        ////////////////////////////////////////////

        /// Default constructor taking the Sofa component owner @sa m_owner, its name @sa m_dataName and its type @sa m_dataType as argument.
        public SofaBaseData(SofaBaseComponent owner, string nameID, string type)
        {
            m_owner = owner;
            m_dataName = nameID;
            m_dataType = type;

            GetDataFlagImpl();
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
        //public bool IsReadOnly
        //{
        //    get { return m_isReadOnly; }
        //    set { m_isReadOnly = value; }
        //}

        /// TODO: work in progess method not yet used
        public bool SetValueIfEdited()
        {
            if (m_isEdited == true)
                return SetValueImpl();
            else
                return false;
        }

        public bool CheckIfDirty()
        {
            // check SOFA Data counter
            int counter = m_owner.m_impl.GetDataCounter(m_dataName);
            if (m_counter != counter)
            {
                m_counter = counter;
                m_isDirty = true;
            }
            else
                m_isDirty = false;

            return m_isDirty;
        }

        public bool IsSupported()
        {
            return m_isSupported;
        }

        public bool IsVector()
        {
            return m_isVector;
        }

        public bool IsReadOnly()
        {
            return m_flag.HasFlag(DataFlagsEnum.FLAG_READONLY);
        }

        public bool IsDisplayed()
        {
            return m_flag.HasFlag(DataFlagsEnum.FLAG_DISPLAYED);
        }

        public bool IsRequired()
        {
            return m_flag.HasFlag(DataFlagsEnum.FLAG_REQUIRED);
        }

        public virtual int GetDataCounter()
        {
            m_counter = m_owner.m_impl.GetDataCounter(m_dataName);
            return m_counter;
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

        protected virtual void GetDataFlagImpl()
        {
            int res = m_owner.m_impl.GetDataFlags(m_dataName);
            m_flag = (DataFlagsEnum)(res);
        }

    }

}
