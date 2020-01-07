using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using UnityEditor;

namespace SofaUnity
{
    [System.Serializable]
    public class SofaData : SofaBaseData
    {
        public SofaData(SofaBaseComponent owner, string nameID, string type)
            : base(owner, nameID, type)
        {
            
        }
    }



    [System.Serializable]
    public class SofaBoolData : SofaBaseData
    {
        [SerializeField]
        protected bool m_value = false;

        public SofaBoolData(SofaBaseComponent owner, string nameID, bool value)
            : base(owner, nameID, "bool")
        {
            m_value = value;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }                     
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetBoolValue(m_dataName, m_value);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetBoolValue(m_dataName);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


    [System.Serializable]
    public class SofaIntData : SofaBaseData
    {
        [SerializeField]
        protected int m_value = int.MinValue;

        [SerializeField]
        protected bool m_isUnsigned;

        public SofaIntData(SofaBaseComponent owner, string nameID, int value, bool isUnsigned = false)
            : base(owner, nameID, "int")
        {
            m_value = value;
            m_isUnsigned = isUnsigned;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

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

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            if (m_isUnsigned)
                m_value = m_owner.m_impl.GetUIntValue(m_dataName);
            else
                m_value = m_owner.m_impl.GetIntValue(m_dataName);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


    [System.Serializable]
    public class SofaFloatData : SofaBaseData
    {
        [SerializeField]
        protected float m_value = float.MinValue;        

        public SofaFloatData(SofaBaseComponent owner, string nameID, float value)
            : base(owner, nameID, "float")
        {
            m_value = value;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetFloatValue(m_dataName, m_value);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetFloatValue(m_dataName);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


    [System.Serializable]
    public class SofaDoubleData : SofaBaseData
    {
        [SerializeField]
        protected float m_value = float.MinValue;

        public SofaDoubleData(SofaBaseComponent owner, string nameID, float value)
            : base(owner, nameID, "double")
        {
            m_value = value;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetDoubleValue(m_dataName, m_value);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetDoubleValue(m_dataName);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }



    [System.Serializable]
    public class SofaStringData : SofaBaseData
    {
        [SerializeField]
        protected string m_value = "";

        public SofaStringData(SofaBaseComponent owner, string nameID, string value)
            : base(owner, nameID, "string")
        {
            m_value = value;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetStringValue(m_dataName, m_value);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.getStringValue(m_dataName);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }



    [System.Serializable]
    public class SofaVec2Data : SofaBaseData
    {
        [SerializeField]
        protected Vector2 m_value;

        [SerializeField]
        protected bool m_isDouble;

        public SofaVec2Data(SofaBaseComponent owner, string nameID, Vector2 value, bool isDouble)
            : base(owner, nameID, "Vec2")
        {
            m_value = new Vector2(value.x, value.y);
            m_isDouble = isDouble;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector2Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector2Value(m_dataName, m_isDouble);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


    [System.Serializable]
    public class SofaVec3Data : SofaBaseData
    {
        [SerializeField]
        protected Vector3 m_value;

        [SerializeField]
        protected bool m_isDouble;

        public SofaVec3Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble)
            : base(owner, nameID, "Vec3")
        {
            m_value = new Vector3(value.x, value.y, value.z);
            m_isDouble = isDouble;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector3Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector3Value(m_dataName, m_isDouble);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


   [System.Serializable]
    public class SofaVec4Data : SofaBaseData
    {
        [SerializeField]
        protected Vector4 m_value;

        [SerializeField]
        protected bool m_isDouble;

        public SofaVec4Data(SofaBaseComponent owner, string nameID, Vector3 value, bool isDouble)
            : base(owner, nameID, "Vec4")
        {
            m_value = new Vector3(value.x, value.y, value.z);
            m_isDouble = isDouble;
        }

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
                        Debug.Log("Set value: " + m_dataName + " = " + m_value);
                    }
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }

        protected override bool SetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_owner.m_impl.SetVector4Value(m_dataName, m_value, m_isDouble);
            return true;
        }

        protected override bool GetValueImpl()
        {
            if (m_owner.m_impl == null)
                return false;

            m_value = m_owner.m_impl.GetVector4Value(m_dataName, m_isDouble);
            Debug.Log("Get value: " + m_dataName + " = " + m_value);
            m_isDirty = false;
            return true;
        }
    }


    [System.Serializable]
    public class SofaVectorData : SofaBaseData
    {
        [SerializeField]
        protected string m_vecType = "";

        [SerializeField]
        protected int m_vecSize = 0;

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
            //Type toto = typeof(T);
            //Debug.Log("Type: " + toto);
        }
    }
}
