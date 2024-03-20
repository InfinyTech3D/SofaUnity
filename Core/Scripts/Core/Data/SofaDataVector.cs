using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    /// <summary>
    /// Generic Data class inheriting from SofaBaseData to support Data <vector<> > 
    /// To be inherited by specialisation class to support Data< vector<type> >
    /// Vector size will be stored in @sa m_vecSize
    /// Vector type will be stored in @sa m_vecType
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

        /// Default constructor taking the component owner, the data name, the data type and the vector size.
        public SofaDataVector(SofaBaseComponent owner, string dataName, string dataType, string vecType)
            : base(owner, dataName, dataType)
        {
            m_vecType = vecType;
            m_isVector = true;
        }

        /// public method to get vector size
        public int GetSize() { return m_vecSize; }
    }



    /// <summary>
    /// Specialization of class SofaDataVector to handle SOFA Data<vector <int> >
    /// </summary>
    [System.Serializable]
    public class SofaDataVectorInt : SofaDataVector
    {
        ///////////////////////////////////////////////
        //////       SofaDataVectorInt API        /////
        ///////////////////////////////////////////////

        /// Default constructor taking the component owner, the data name and the vector size. Will set the type internally
        public SofaDataVectorInt(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "int")
        {
            m_vecSize = m_owner.m_impl.GetVectoriSize(m_dataName);
        }

        /// <summary>
        /// Method to get values from SOFA
        /// </summary>
        /// <param name="values">Raw array of int to store SOFA values</param>
        /// <returns>int code from SOFA communication</returns>
        public int GetValues(int[] values)
        {
            return m_owner.m_impl.GetVectoriValue(m_dataName, m_vecSize, values);
        }

        /// <summary>
        /// Method to set values to SOFA
        /// </summary>
        /// <param name="values">Raw array of int values to set into SOFA</param>
        /// <returns>int code from SOFA communication</returns>
        public int SetValue(int[] values, int vecSize)
        {
            m_vecSize = vecSize;
            return m_owner.m_impl.SetVectoriValue(m_dataName, m_vecSize, values);
        }
    }


    /// <summary>
    /// Specialization of class SofaDataVector to handle SOFA Data<vector <float> >
    /// </summary>
    [System.Serializable]
    public class SofaDataVectorFloat : SofaDataVector
    {
        /////////////////////////////////////////////////
        //////       SofaDataVectorFloat API        /////
        /////////////////////////////////////////////////

        /// Default constructor taking the component owner, the data name and the vector size. Will set the type internally
        public SofaDataVectorFloat(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "float")
        {
            m_vecSize = m_owner.m_impl.GetVectorfSize(m_dataName);
        }

        /// <summary>
        /// Method to get values from SOFA
        /// </summary>
        /// <param name="values">Raw array of float to store SOFA values</param>
        /// <returns>int code from SOFA communication</returns>
        public int GetValues(float[] values)
        {
            return m_owner.m_impl.GetVectorfValue(m_dataName, m_vecSize, values);
        }

        /// <summary>
        /// Method to set values to SOFA
        /// </summary>
        /// <param name="values">Raw array of float values to set into SOFA</param>
        /// <returns>int code from SOFA communication</returns>
        public int SetValue(float[] values, int vecSize)
        {
            m_vecSize = vecSize;
            return m_owner.m_impl.SetVectorfValue(m_dataName, m_vecSize, values);
        }
    }


    /// <summary>
    /// Specialization of class SofaDataVector to handle SOFA Data<vector <double> >
    /// </summary>
    [System.Serializable]
    public class SofaDataVectorDouble : SofaDataVector
    {
        //////////////////////////////////////////////////
        //////       SofaDataVectorDouble API        /////
        //////////////////////////////////////////////////

        /// Default constructor taking the component owner, the data name and the vector size. Will set the type internally
        public SofaDataVectorDouble(SofaBaseComponent owner, string dataName, string dataType)
            : base(owner, dataName, dataType, "double")
        {
            m_vecSize = m_owner.m_impl.GetVectordSize(m_dataName);
        }

        /// <summary>
        /// Method to get values from SOFA
        /// </summary>
        /// <param name="values">Raw array of float to store SOFA values</param>
        /// <returns>int code from SOFA communication</returns>
        public int GetValues(float[] values)
        {
            return m_owner.m_impl.GetVectordValue(m_dataName, m_vecSize, values);
        }

        /// <summary>
        /// Method to set values to SOFA
        /// </summary>
        /// <param name="values">Raw array of float values to set into SOFA</param>
        /// <returns>int code from SOFA communication</returns>
        public int SetValue(float[] values, int vecSize)
        {
            m_vecSize = vecSize;
            return m_owner.m_impl.SetVectordValue(m_dataName, m_vecSize, values);
        }
    }


    /// <summary>
    /// Specialization of class SofaDataVector to handle SOFA Data<vector <Vec2> >
    /// Can handle Vec2f or Vec2d, type info will be stored by @sa m_isDouble 
    /// </summary>
    [System.Serializable]
    public class SofaDataVectorVec2 : SofaDataVector
    {
        ////////////////////////////////////////////////
        //////       SofaDataVectorVec2 API        /////
        ////////////////////////////////////////////////

        [SerializeField]
        protected bool m_isDouble = false;

        /// Default constructor taking the component owner, the data name and the vector size. Will set the type internally
        public SofaDataVectorVec2(SofaBaseComponent owner, string dataName, string dataType, bool isDouble)
            : base(owner, dataName, dataType, "Vec2")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec2Size(m_dataName, m_isDouble);
        }

        /// <summary>
        /// Method to get values from SOFA. Array of Vector2 to raw float conversion will be performed
        /// </summary>
        /// <param name="values">Array of Vector2 to store SOFA values</param>
        /// <returns>int code from SOFA communication</returns>
        public int GetValues(Vector2[] values, bool updateSize = false)
        {
            if (updateSize)
                m_vecSize = m_owner.m_impl.GetVecofVec2Size(m_dataName, m_isDouble);

            float[] rawValues = new float[m_vecSize * 2];

            int res = m_owner.m_impl.GetVecofVec2Value(m_dataName, m_vecSize, rawValues, m_isDouble);
            for (int i = 0; i < m_vecSize; ++i)
            {
                values[i].x = rawValues[i * 2];
                values[i].y = rawValues[i * 2 + 1];
            }

            return res;
        }

        /// <summary>
        /// Method to set values to SOFA. Array of Vector2 to raw float conversion will be performed
        /// </summary>
        /// <param name="values">Array of Vector2 values to set into SOFA</param>
        /// <returns>int code from SOFA communication</returns>
        public int SetValues(Vector2[] values, int vecSize)
        {
            m_vecSize = vecSize;
            float[] rawValues = new float[m_vecSize * 2];
            for (int i = 0; i < m_vecSize; ++i)
            {
                rawValues[i * 2] = values[i].x;
                rawValues[i * 2 + 1] = values[i].y;
            }

            int res = m_owner.m_impl.SetVecofVec2Value(m_dataName, m_vecSize, rawValues, m_isDouble);

            return res;
        }
    }


    /// <summary>
    /// Specialization of class SofaDataVector to handle SOFA Data<vector <Vec3> >
    /// Can handle Vec3f or Vec3d, type info will be stored by @sa m_isDouble 
    /// </summary>
    [System.Serializable]
    public class SofaDataVectorVec3 : SofaDataVector
    {
        ////////////////////////////////////////////////
        //////       SofaDataVectorVec3 API        /////
        ////////////////////////////////////////////////

        [SerializeField]
        protected bool m_isDouble = false;

        /// Default constructor taking the component owner, the data name and the vector size. Will set the type internally
        public SofaDataVectorVec3(SofaBaseComponent owner, string dataName, string dataType, bool isDouble)
            : base(owner, dataName, dataType, "Vec3")
        {
            m_isDouble = isDouble;
            m_vecSize = m_owner.m_impl.GetVecofVec3Size(m_dataName, m_isDouble);            
        }

        /// <summary>
        /// Method to get values from SOFA. Array of Vector3 to raw float conversion will be performed
        /// </summary>
        /// <param name="values">Array of Vector3 to store SOFA values</param>
        /// <returns>int code from SOFA communication</returns>
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

        /// <summary>
        /// Method to set values to SOFA. Array of Vector3 to raw float conversion will be performed
        /// </summary>
        /// <param name="values">Array of Vector3 values to set into SOFA</param>
        /// <returns>int code from SOFA communication</returns>
        public int SetValues(Vector3[] values, int vecSize)
        {
            m_vecSize = vecSize;
            float[] rawValues = new float[m_vecSize * 3];
            for (int i = 0; i < m_vecSize; ++i)
            {
                 rawValues[i * 3] = values[i].x;
                 rawValues[i * 3 + 1] = values[i].y;
                 rawValues[i * 3 + 2] = values[i].z;
            }

            int res = m_owner.m_impl.SetVecofVec3Value(m_dataName, m_vecSize, rawValues, m_isDouble);

            return res;
        }
    }
}