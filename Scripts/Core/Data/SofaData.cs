using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using UnityEditor;

namespace SofaUnity
{
    /// <summary>
    /// Generic Data class inheriting from SofaBaseData. This generic Data do not handle get/set values.
    /// @sa m_isSupported will be false.
    /// </summary>
    [System.Serializable]
    public class SofaData : SofaBaseData
    {
        public SofaData(SofaBaseComponent owner, string dataName, string type)
            : base(owner, dataName, type)
        {
            m_isSupported = false;
        }
    }



    /// <summary>
    /// Specialization of class SofaBaseData to handle SOFA Data<bool>
    /// </summary>
    [System.Serializable]
    public class SofaBoolData : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected bool m_value = false;


        ////////////////////////////////////////////
        //////        SofaBoolData API         /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaBoolData(SofaBaseComponent owner, string dataName, bool value)
            : base(owner, dataName, "bool")
        {
            m_value = value;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public bool Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {                    
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }                     
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////    SofaBoolData internal API    /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetBoolValue(m_dataName, m_value);
            return true;
        }

        /// Internal Method to get value from Sofa simulation. Will update @sa m_value
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetBoolValue(m_dataName);
            m_isDirty = false;
            return true;
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<int>
    /// </summary>
    [System.Serializable]
    public class SofaIntData : SofaBaseData
    {
        /// Stored Int value from Sofa
        [SerializeField]
        protected int m_value = int.MinValue;

        /// Specific info to know if this Data is storing an Int or un Unsigned Int inside SOFA.
        [SerializeField]
        protected bool m_isUnsigned;


        ////////////////////////////////////////////
        //////        SofaIntData API          /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaIntData(SofaBaseComponent owner, string dataName, int value, bool isUnsigned = false)
            : base(owner, dataName, "int")
        {
            m_value = value;
            m_isUnsigned = isUnsigned;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public int Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    if (m_isUnsigned && value < 0)
                        return;

                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////    SofaIntData internal API     /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            if (m_isUnsigned)
                m_owner.m_impl.SetUIntValue(m_dataName, m_value);
            else
                m_owner.m_impl.SetIntValue(m_dataName, m_value);

            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            if (m_isUnsigned)
                m_value = m_owner.m_impl.GetUIntValue(m_dataName);
            else
                m_value = m_owner.m_impl.GetIntValue(m_dataName);

            m_isDirty = false;
            return true;
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<float>
    /// </summary>
    [System.Serializable]
    public class SofaFloatData : SofaBaseData
    {
        /// Stored Float value from Sofa
        [SerializeField]
        protected float m_value = float.MinValue;


        ////////////////////////////////////////////
        //////        SofaFloatData API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaFloatData(SofaBaseComponent owner, string dataName, float value)
            : base(owner, dataName, "float")
        {
            m_value = value;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public float Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {                    
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////    SofaFloatData internal API   /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetFloatValue(m_dataName, m_value);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetFloatValue(m_dataName);
            m_isDirty = false;
            return true;
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<double>
    /// </summary>
    [System.Serializable]
    public class SofaDoubleData : SofaBaseData
    {
        /// Stored float value from Sofa double
        [SerializeField]
        protected float m_value = float.MinValue;


        ////////////////////////////////////////////
        //////       SofaDoubleData API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDoubleData(SofaBaseComponent owner, string dataName, float value)
            : base(owner, dataName, "double")
        {
            m_value = value;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public float Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////   SofaDoubleData internal API   /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetDoubleValue(m_dataName, m_value);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetDoubleValue(m_dataName);
            m_isDirty = false;
            return true;
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<string>
    /// </summary>
    [System.Serializable]
    public class SofaStringData : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected string m_value = "";


        ////////////////////////////////////////////
        //////       SofaStringData API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaStringData(SofaBaseComponent owner, string dataName, string value)
            : base(owner, dataName, "string")
        {
            m_value = value;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public string Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {                    
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////   SofaStringData internal API   /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetStringValue(m_dataName, m_value);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.getStringValue(m_dataName);
            m_isDirty = false;
            return true;
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<Vec2f> and Data<Vec2d>
    /// float or double info will be stored @sa m_isDouble
    /// </summary>
    [System.Serializable]
    public class SofaVec2Data : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected Vector2 m_value;

        /// Internal info to know if storing float or double
        [SerializeField]
        protected bool m_isDouble;


        ////////////////////////////////////////////
        //////        SofaVec2Data API         /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVec2Data(SofaBaseComponent owner, string dataName, Vector2 value, bool isDouble)
            : base(owner, dataName, "Vec2")
        {
            m_value = new Vector2(value.x, value.y);
            m_isDouble = isDouble;
        }
        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public Vector2 Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////    SofaVec2Data internal API    /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector2Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector2Value(m_dataName, m_isDouble);
            m_isDirty = false;
            return true;
        }
    }


    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data< Vec2<int> > and Data<Vec2 <unsigned int> >
    /// int or unsigned int info will be stored @sa m_isUnsigned
    /// </summary>
    [System.Serializable]
    public class SofaVec2IntData : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected Vector2Int m_value;

        /// Internal info to know if storing float or double
        [SerializeField]
        protected bool m_isUnsigned;


        ////////////////////////////////////////////
        //////       SofaVec2IntData API       /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVec2IntData(SofaBaseComponent owner, string dataName, Vector2Int value, bool isUnsigned)
            : base(owner, dataName, "Vec2i")
        {
            m_value = new Vector2Int(value.x, value.y);
            m_isUnsigned = isUnsigned;
        }
        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public Vector2Int Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////   SofaVec2IntData internal API  /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector2iValue(m_dataName, m_value, m_isUnsigned);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector2iValue(m_dataName/*, m_isUnsigned*/);
            m_isDirty = false;
            return true;
        }
    }


    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<Vec3f> and Data<Vec3d>
    /// float or double info will be stored @sa m_isDouble
    /// </summary>
    [System.Serializable]
    public class SofaVec3Data : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected Vector3 m_value;

        /// Internal info to know if storing float or double
        [SerializeField]
        protected bool m_isDouble;


        ////////////////////////////////////////////
        //////        SofaVec3Data API         /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVec3Data(SofaBaseComponent owner, string dataName, Vector3 value, bool isDouble)
            : base(owner, dataName, "Vec3")
        {
            m_value = new Vector3(value.x, value.y, value.z);
            m_isDouble = isDouble;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public Vector3 Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////    SofaVec3Data internal API    /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector3Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector3Value(m_dataName, m_isDouble);
            m_isDirty = false;
            return true;
        }
    }


    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data< Vec3<int> > and Data<Vec3 <unsigned int> >
    /// int or unsigned int info will be stored @sa m_isUnsigned
    /// </summary>
    [System.Serializable]
    public class SofaVec3IntData : SofaBaseData
    {
        /// Stored Bool value from Sofa
        [SerializeField]
        protected Vector3Int m_value;

        /// Internal info to know if storing float or double
        [SerializeField]
        protected bool m_isUnsigned;


        ////////////////////////////////////////////
        //////      SofaVec3IntData API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVec3IntData(SofaBaseComponent owner, string dataName, Vector3Int value, bool isUnsigned)
            : base(owner, dataName, "Vec3i")
        {
            m_value = new Vector3Int(value.x, value.y, value.z);
            m_isUnsigned = isUnsigned;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public Vector3Int Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }


        ////////////////////////////////////////////
        //////   SofaVec3IntData internal API  /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector3iValue(m_dataName, m_value, m_isUnsigned);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector3iValue(m_dataName/*, m_isUnsigned*/);
            m_isDirty = false;
            return true;
        }
    }


    /// <summary>
    /// Specialization of the class SofaBaseData to handle SOFA Data<Vec4f> and Data<Vec4d>
    /// float or double info will be stored @sa m_isDouble
    /// </summary>
    [System.Serializable]
    public class SofaVec4Data : SofaBaseData
    {
        [SerializeField]
        protected Vector4 m_value;

        /// Internal info to know if storing float or double
        [SerializeField]
        protected bool m_isDouble;


        ////////////////////////////////////////////
        //////        SofaVec4Data API         /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVec4Data(SofaBaseComponent owner, string dataName, Vector3 value, bool isDouble)
            : base(owner, dataName, "Vec4")
        {
            m_value = new Vector3(value.x, value.y, value.z);
            m_isDouble = isDouble;
        }

        /// Getter/Setter of the @sa m_value. Will call @sa SetValueImpl and @sa GetValueImpl internally for Sofa communication
        public Vector4 Value
        {
            get
            {
                if (m_isDirty) // nothing to do
                    GetValueImpl();

                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    if (SetValueImpl())
                    {
                        m_owner.m_impl.ReinitComponent();
                        m_isEdited = true;
                    }
                }
            }
        }

        /// Log Method for Debug info
        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }
        

        ////////////////////////////////////////////
        //////    SofaVec4Data internal API    /////
        ////////////////////////////////////////////

        /// Internal Method to set value inside Sofa simulation
        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector4Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        /// Internal Method to get value from Sofa simulation
        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector4Value(m_dataName, m_isDouble);
            m_isDirty = false;
            return true;
        }
    }

}
