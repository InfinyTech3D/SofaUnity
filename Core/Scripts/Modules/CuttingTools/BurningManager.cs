using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

//[ExecuteInEditMode]
public class BurningManager : MonoBehaviour
{
    /// Member: Unity Mesh object of this GameObject
    protected Mesh m_mesh = null;
    protected int m_nbrV = 0;
    protected Vector2[] m_uv2 = null;

    protected bool m_isReady = false;

    public GameObject m_burner = null;
    public float m_burnDistance = 2.0f;

    public SofaComponent m_carving = null;

    void Awake()
    {
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

        if (m_uv2 == null)
            m_uv2 = new Vector2[m_nbrV];

        for (int i = 0; i < m_nbrV; i++)
        {
            m_uv2[i].x = 0.0f;
            m_uv2[i].y = 0.0f;
        }
        m_mesh.uv2 = m_uv2;


        //Debug.Log("Start: " + m_nbrV);
        //InvokeRepeating("computeBurnTissus", 0.0f, 1.0f);
    }

    int texCpt = 0;

    void Update()
    {
        if (m_carving)
        {
            var bData = m_carving.m_dataArchiver.GetBaseData("texcoords");
            int counter = bData.GetDataCounter();

            if (counter == texCpt)
                return;

            bool updateSize = false;
            if (m_mesh.vertices.Length != m_nbrV) // carving in process
            {
                m_nbrV = m_mesh.vertices.Length;
                m_uv2 = new Vector2[m_nbrV];
                updateSize = true;
            }                       

            texCpt = counter;
            if (bData != null)
            {
                bool vector = bData.IsVector();
                SofaDataVectorVec2 dataV = (SofaDataVectorVec2)(bData);

                dataV.GetValues(m_uv2, updateSize);
                m_mesh.uv2 = m_uv2;
            }
            //vModel.UpdateTexCoords();
        }
    }

    //Vector3 firstVertex;

    void computeBurnTissus()
    {
        if (m_isReady == false)
            return;

        Vector3 pos = m_burner.transform.position;
        bool someChange = false;
        //firstVertex = this.transform.TransformPoint(m_mesh.vertices[0]);

        for (int i = 0; i < m_nbrV; i++)
        {
            Vector3 worldPos = this.transform.TransformPoint(m_mesh.vertices[i]);
            float dist = (pos - worldPos).magnitude;
            if (dist < m_burnDistance) // in distance scope
            {
                float coef = (m_burnDistance - dist) / m_burnDistance;
                //Debug.Log("dist: " + dist + " | coef: " + coef);
                if (m_uv2[i].x < coef)
                {
                    m_uv2[i].x = coef;
                    someChange = true;
                }
            }
        }
        if (someChange)
            m_mesh.uv2 = m_uv2;
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (!m_isReady)
    //        return;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(m_burner.transform.position, 0.005f);
    //    Gizmos.DrawLine(firstVertex, m_burner.transform.position);
    //}
}
