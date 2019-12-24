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
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }
    }


    [System.Serializable]
    public class SofaIntData : SofaBaseData
    {
        [SerializeField]
        protected int m_value = int.MinValue;

        public SofaIntData(SofaBaseComponent owner, string nameID, int value)
            : base(owner, nameID, "int")
        {
            m_value = value;
        }

        public int Value
        {
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
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
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    m_isEdited = true;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
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
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    m_isEdited = true;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
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
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    m_isEdited = true;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }
    }




    [System.Serializable]
    public class SofaVec3fData : SofaBaseData
    {
        [SerializeField]
        protected Vector3 m_value;

        public SofaVec3fData(SofaBaseComponent owner, string nameID, Vector3 value)
            : base(owner, nameID, "Vec3f")
        {
            m_value = new Vector3(value.x, value.y, value.z);
        }

        public Vector3 Value
        {
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    m_isEdited = true;
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log(m_owner.UniqueNameId + "{" + m_dataType + "}: " + m_dataName + " => " + m_value);
        }
    }


    //public class SData<T> : SBaseData
    public class old_SofaData : old_SofaBaseData
    {
        public string m_type = "None";

        public old_SofaData(string nameID, string type, SBaseObject owner)
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
