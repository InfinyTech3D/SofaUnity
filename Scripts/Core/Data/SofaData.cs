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
    public class SofaBaseDataTest
    {
        [SerializeField]
        protected ComponentDataTest m_owner;
        [SerializeField]
        protected string m_dataName = "";
        [SerializeField]
        protected bool m_isReadOnly = false;

        public SofaBaseDataTest(ComponentDataTest owner, string nameID)
        {
            m_owner = owner;
            m_dataName = nameID;
        }

        public string DataName
        {
            get { return m_dataName; }
        }

        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
        }
    }


    [System.Serializable]
    public class SofaDataFloat : SofaBaseDataTest
    {
        [SerializeField]
        protected float m_value = float.MinValue;        

        public SofaDataFloat(ComponentDataTest owner, string nameID, float value)
            : base(owner, nameID)
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
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }

        public void Log()
        {
            Debug.Log("SofaDataFloat: " + m_dataName + " -> " + m_value + " owner: " + m_owner.nameId);
        }

        //public getEditor()
    }


    [System.Serializable]
    public class SofaDataVec3Float : SofaBaseDataTest
    {
        [SerializeField]
        protected Vector3 m_value;

        public SofaDataVec3Float(ComponentDataTest owner, string nameID, float value0, float value1, float value2)
            : base(owner, nameID)
        {
            m_value = new Vector3(value0, value1, value2);
        }

        public Vector3 Value
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
            Debug.Log("SofaDataFloat: " + m_dataName + " -> " + m_value + " owner: " + m_owner.nameId);
        }

        //public getEditor()
    }
}
