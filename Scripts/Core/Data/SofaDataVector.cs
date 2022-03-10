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
        public SofaDataVector(SofaBaseComponent owner, string nameID, string dataType, string vecType)
            : base(owner, nameID, dataType)
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
        public SofaDataVectorInt(SofaBaseComponent owner, string nameID, string dataType)
            : base(owner, nameID, dataType, "int")
        {
            m_vecSize = m_owner.m_impl.GetVectoriSize(m_dataName);
            Debug.Log(m_dataName + " -> " + m_vecSize);
        }
    }


    [System.Serializable]
    public class SofaDataVectorFloat : SofaDataVector
    {
        /////////////////////////////////////////////////
        //////       SofaDataVectorFloat API        /////
        /////////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorFloat(SofaBaseComponent owner, string nameID, string dataType)
            : base(owner, nameID, dataType, "float")
        {
            m_vecSize = m_owner.m_impl.GetVectorfSize(m_dataName);
            Debug.Log(m_dataName + " -> " + m_vecSize);
        }
    }


    [System.Serializable]
    public class SofaDataVectorDouble : SofaDataVector
    {
        //////////////////////////////////////////////////
        //////       SofaDataVectorDouble API        /////
        //////////////////////////////////////////////////

        /// Default constructor taking the value, the component owner and its name. Will set the type internally
        public SofaDataVectorDouble(SofaBaseComponent owner, string nameID, string dataType)
            : base(owner, nameID, dataType, "double")
        {
            m_vecSize = m_owner.m_impl.GetVectordSize(m_dataName);
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
        public SofaDataVectorVec2(SofaBaseComponent owner, string nameID, string dataType, bool isDouble)
            : base(owner, nameID, dataType, "Vec2")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec2Size(m_dataName, m_isDouble);            
            Debug.Log(m_dataName + " -> " + m_vecSize);
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
        public SofaDataVectorVec3(SofaBaseComponent owner, string nameID, string dataType, bool isDouble)
            : base(owner, nameID, dataType, "Vec3")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec3Size(m_dataName, m_isDouble);            
            Debug.Log(m_dataName + " -> " + m_vecSize);
        }
    }
}