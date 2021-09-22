using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class TextureInterpolationModel : MonoBehaviour
    {
        ////////////////////////////////////////////////////////
        //////      TextureInterpolationModel members      /////
        ////////////////////////////////////////////////////////

        /// Member: Unity Mesh object of this GameObject
        protected Mesh m_mesh = null;
        protected int m_nbrV = 0;

        protected Vector2[] m_uv = null;

        /// Pointer to the SofaMesh holding the data field.
        public SofaMesh m_meshValues = null;
        public float m_minValue = -0.1f;
        public float m_maxValue = 10.0f;

        protected bool m_isReady = false;

        void Awake()
        {
            Debug.Log("TextureInterpolationModel::Awake");

#if UNITY_EDITOR
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            m_mesh = mf.sharedMesh;
#else
             //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
#endif

            if (m_mesh == null)
            {
                Debug.LogError("No valid MeshFilter found in this GameObject. Engine won't work correctly.");
                m_isReady = false;
                return;
            }

            m_nbrV = m_mesh.vertices.Length;
            m_isReady = true;
        }


        // Start is called before the first frame update
        void Start()
        {
            if (m_isReady == false)
                return;


#if UNITY_EDITOR
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            m_mesh = mf.sharedMesh;
#else
             //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
#endif

            if (m_meshValues == null) // no input values
            {
                if (m_uv == null)
                    m_uv = new Vector2[m_nbrV];

                for (int i = 0; i < m_nbrV; ++i)
                {
                    m_uv[i].x = 0.0f;
                    m_uv[i].y = 0.0f;
                }
                m_mesh.uv = m_uv;
            }
        }

        bool firstTime = true;
        // Update is called once per frame
        void Update()
        {          
            if (m_meshValues != null)
            {
                if (firstTime)
                {
                    m_meshValues.AddListener();
                    firstTime = false;
                    // get the mesh again, just in case...
#if UNITY_EDITOR
                    m_mesh = GetComponent<MeshFilter>().sharedMesh;
#else
                    m_mesh = GetComponent<MeshFilter>().mesh; 
#endif
                }

                int nbrV = m_meshValues.NbVertices();

                if (nbrV != m_nbrV)
                {
                    Debug.LogError("Mesh size: " + m_nbrV + " is different to the Data buffer size: " + nbrV);
                    return;
                }

                if (m_uv == null)
                    m_uv = new Vector2[m_nbrV];

                float[] dataValues = m_meshValues.SofaMeshTopology.m_vertexBuffer;
                //float min = 100.0f;
                //float max = -100.0f;

                int meshDim = m_meshValues.SofaMeshTopology.m_meshDim;

                //for (int i = 0; i < nbrV; i++)
                //{
                //    float val = dataValues[i * meshDim];
                //    if (val < min)
                //        min = val;

                //    if (val > max)
                //        max = val;
                //}

                //max += 0.01f;
                //min -= 0.01f;

                float scale = 1 / (m_maxValue - m_minValue);

                for (int i = 0; i < nbrV; i++)
                {
                    float val = dataValues[i* meshDim];

                    float tex = (val - m_minValue) * scale;

                    m_uv[i].x = tex;
                    m_uv[i].y = tex;
                }
                
                m_mesh.uv = m_uv;
            }
            else
                InterpolationTest();
        }


        protected void InterpolationTest()
        {
            float time = Time.time*3;
            if (m_uv == null)
                m_uv = new Vector2[m_nbrV];

            for (int i = 0; i < m_nbrV; i++)
            {
                if (time > i)
                {
                    m_uv[i].x = 0.99f;
                    m_uv[i].y = 0.99f;
                }
                else
                {
                    m_uv[i].x = 0.01f;
                    m_uv[i].y = 0.01f;
                }
                
            }

#if UNITY_EDITOR
            m_mesh = GetComponent<MeshFilter>().sharedMesh;
#else
            m_mesh = GetComponent<MeshFilter>().mesh; 
#endif

            if (m_mesh)
                m_mesh.uv = m_uv;

        }
    }

}
