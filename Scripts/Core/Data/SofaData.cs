using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using UnityEditor;

namespace SofaUnity
{
    /// <summary>
    /// Generic Data class inheriting from SofaBaseData
    /// </summary>
    [System.Serializable]
    public class SofaData : SofaBaseData
    {
        public SofaData(SofaBaseComponent owner, string nameID, string type)
            : base(owner, nameID, type)
        {
            
        }
    }



    /// <summary>
    /// Specialization of the class SofaBaseData for Bool data
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
        public SofaBoolData(SofaBaseComponent owner, string nameID, bool value)
            : base(owner, nameID, "bool")
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

        /// Internal Method to get value from Sofa simulation
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
    /// Specialization of the class SofaBaseData for Int data
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
        public SofaIntData(SofaBaseComponent owner, string nameID, int value, bool isUnsigned = false)
            : base(owner, nameID, "int")
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
    /// Specialization of the class SofaBaseData for Float data
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
        public SofaFloatData(SofaBaseComponent owner, string nameID, float value)
            : base(owner, nameID, "float")
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
    /// Specialization of the class SofaBaseData for Double data
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
        public SofaDoubleData(SofaBaseComponent owner, string nameID, float value)
            : base(owner, nameID, "double")
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
    /// Specialization of the class SofaBaseData for String data
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
        public SofaStringData(SofaBaseComponent owner, string nameID, string value)
            : base(owner, nameID, "string")
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
    /// Specialization of the class SofaBaseData for Vec2<float> data
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
        public SofaVec2Data(SofaBaseComponent owner, string nameID, Vector2 value, bool isDouble)
            : base(owner, nameID, "Vec2")
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
    /// Specialization of the class SofaBaseData for Vec2<int> data
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
        public SofaVec2IntData(SofaBaseComponent owner, string nameID, Vector2Int value, bool isUnsigned)
            : base(owner, nameID, "Vec2i")
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
    /// Specialization of the class SofaBaseData for Vec3<float> data
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
        public SofaVec3Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble)
            : base(owner, nameID, "Vec3")
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
    /// Specialization of the class SofaBaseData for Vec3<int> data
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
        public SofaVec3IntData(SofaBaseComponent owner, string nameID, Vector3Int value, bool isUnsigned)
            : base(owner, nameID, "Vec3i")
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
    /// Specialization of the class SofaBaseData for Vec4<float> data
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
        public SofaVec4Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble)
            : base(owner, nameID, "Vec4")
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


    /// <summary>
    /// Specialization of the class SofaBaseData for Vector<float> data
    /// Not yet handled.
    /// </summary>
    [System.Serializable]
    public class SofaVectorData : SofaBaseData
    {
        /// Internal string type stored in this Data
        [SerializeField]
        protected string m_vecType = "";

        /// Size of the stored vector
        [SerializeField]
        protected int m_vecSize = 0;

        ////////////////////////////////////////////
        //////       SofaVectorData API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaVectorData(SofaBaseComponent owner, string nameID, string dataType, string vecType, int size)
            : base(owner, nameID, dataType)
        {
            m_vecType = vecType;
            m_vecSize = size;
        }
    }


    //public class SData<T> : SBaseData
    public class old_SofaData : old_SofaBaseData
    {
        public string m_type = "None";

        public old_SofaData(string nameID, string type, SofaBaseObject owner)
            : base(nameID, owner)
        {
            m_type = type;
        }

        //readonly int m_size;
        //T[] values;

        public int dataSize = 0;

        public void init()
        {

        }

        public override string getType()
        {
            return m_type;
        }
    }
}
