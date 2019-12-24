using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using UnityEditor;

namespace SofaUnity
{
    //public class SData<T> : SBaseData
    public class SofaData : SofaBaseData
    {
        public string m_type = "None";

        public SofaData(string nameID, string type, SBaseObject owner)
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

    [System.Serializable]
    public class SofaDataFloat
    {
        [SerializeField]
        protected string m_nameID = "";
        [SerializeField]
        protected float m_value = float.MinValue;
        [SerializeField]
        protected ComponentDataTest m_owner;

        public SofaDataFloat(ComponentDataTest owner, string nameID, float value)
        {
            m_owner = owner;
            m_nameID = nameID;
            m_value = value;
        }

        public string nameID
        {
            get { return m_nameID; }
        }

        public float value
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
            Debug.Log("SofaDataFloat: " + m_nameID + " -> " + m_value + " owner: " + m_owner.nameId);
        }

        //public getEditor()
    }


    [System.Serializable]
    public class SofaDataVec3Float
    {
        [SerializeField]
        protected string m_nameID = "";
        [SerializeField]
        protected Vector3 m_value;
        [SerializeField]
        protected ComponentDataTest m_owner;

        public SofaDataVec3Float(ComponentDataTest owner, string nameID, float value0, float value1, float value2)
        {
            m_owner = owner;
            m_nameID = nameID;
            m_value = new Vector3(value0, value1, value2);
        }

        public string nameID
        {
            get { return m_nameID; }
        }

        public Vector3 value
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
            Debug.Log("SofaDataFloat: " + m_nameID + " -> " + m_value + " owner: " + m_owner.nameId);
        }

        //public getEditor()
    }
}
