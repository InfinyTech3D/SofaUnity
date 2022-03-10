using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    /// <summary>
    /// Generic Data class inheriting from SofaBaseData
    /// </summary>
    [System.Serializable]
    public class SofaDataVector : SofaBaseData
    {
        /// Internal string type stored in this Data
        [SerializeField]
        protected string m_vecType = "None";

        /// Size of the stored vector
        [SerializeField]
        protected int m_vecSize = 0;

        ////////////////////////////////////////////
        //////       SofaDataVector API        /////
        ////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVector(SofaBaseComponent owner, string dataName, string dataType, string vecType)
            : base(owner, dataName, dataType)
        {
            m_vecType = vecType;
            m_isVector = true;
            //m_vecSize = size;
        }

        public int GetSize() { return m_vecSize; }
    }

    
    [System.Serializable]
    public class SofaDataVectorInt : SofaDataVector
    {
        ///////////////////////////////////////////////
        //////       SofaDataVectorInt API        /////
        ///////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorInt(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "int")
        {
            m_vecSize = m_owner.m_impl.GetVectoriSize(m_dataName);
        }

        public int GetValues(int[] values)
        {
            return m_owner.m_impl.GetVectoriValue(m_dataName, m_vecSize, values);
        }
    }


    [System.Serializable]
    public class SofaDataVectorFloat : SofaDataVector
    {
        /////////////////////////////////////////////////
        //////       SofaDataVectorFloat API        /////
        /////////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorFloat(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "float")
        {
            m_vecSize = m_owner.m_impl.GetVectorfSize(m_dataName);
        }

        public int GetValues(float[] values)
        {
            return m_owner.m_impl.GetVectorfValue(m_dataName, m_vecSize, values);
        }
    }


    [System.Serializable]
    public class SofaDataVectorDouble : SofaDataVector
    {
        //////////////////////////////////////////////////
        //////       SofaDataVectorDouble API        /////
        //////////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorDouble(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "double")
        {
            m_vecSize = m_owner.m_impl.GetVectordSize(m_dataName);
        }

        public int GetValues(float[] values)
        {
            return m_owner.m_impl.GetVectordValue(m_dataName, m_vecSize, values);
        }
    }


    [System.Serializable]
    public class SofaDataVectorVec2 : SofaDataVector
    {
        ////////////////////////////////////////////////
        //////       SofaDataVectorVec2 API        /////
        ////////////////////////////////////////////////

        [SerializeField]
        protected bool m_isDouble = false;

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorVec2(SofaBaseComponent owner, string dataName, string dataType, bool isDouble)
            : base(owner, dataName, dataType, "Vec2")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec2Size(m_dataName, m_isDouble);            
        }

        public int GetValues(Vector2[] values)
        {
            float[] rawValues = new float[m_vecSize * 2];

            int res = m_owner.m_impl.GetVecofVec2Value(m_dataName, m_vecSize, rawValues, m_isDouble);
            for (int i = 0; i < m_vecSize; ++i)
            {
                values[i].x = rawValues[i * 2];
                values[i].y = rawValues[i * 2 + 1];
            }

            return res;
        }
    }


    [System.Serializable]
    public class SofaDataVectorVec3 : SofaDataVector
    {
        ////////////////////////////////////////////////
        //////       SofaDataVectorVec3 API        /////
        ////////////////////////////////////////////////

        [SerializeField]
        protected bool m_isDouble = false;

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorVec3(SofaBaseComponent owner, string dataName, string dataType, bool isDouble)
            : base(owner, dataName, dataType, "Vec3")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec3Size(m_dataName, m_isDouble);            
        }

        public int GetValues(Vector3[] values)
        {
            float[] rawValues = new float[m_vecSize * 3];

            int res = m_owner.m_impl.GetVecofVec3Value(m_dataName, m_vecSize, rawValues, m_isDouble);
            for (int i = 0; i < m_vecSize; ++i)
            {
                values[i].x = rawValues[i * 3];
                values[i].y = rawValues[i * 3 + 1];
                values[i].z = rawValues[i * 3 + 2];
            }

            return res;
        }
    }
}