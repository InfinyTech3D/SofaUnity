using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class CuttingManager : MonoBehaviour
{
    /// Link to the cutting manager on the SOFA side
    public SofaComponent m_sofaCuttingMgr = null;
    
    /// Pointers to sphere objects
    public GameObject m_pointA = null;
    public GameObject m_pointB = null;
    public GameObject m_pointC = null;

    [SerializeField]
    private Vector3 m_cutPointA = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 m_cutPointB = new Vector3(1, 0, 0);
    [SerializeField]
    private Vector3 m_cutDirection = new Vector3(0, -1, 0);
    [SerializeField]
    private float m_cutDepth = 1.0f;

    [SerializeField]
    public Vector3 CutPointA
    {
        get { return m_cutPointA; }
        set { if (value != m_cutPointA) {
                m_cutPointA = value;
                if (m_pointA) 
                    m_pointA.transform.position = value;
            }
        }
            
    }

    public Vector3 CutPointB
    {
        get { return m_cutPointB; }
        set
        {
            if (value != m_cutPointB)
            {
                m_cutPointB = value;
                if (m_pointB)
                    m_pointB.transform.position = value;
            }
        }
    }

    public Vector3 CutDirection
    {
        get { return m_cutDirection; }
        set
        {
            if (value != m_cutDirection)
            {
                m_cutDirection = value;
                m_cutDirection.Normalize();
                if (m_pointC)
                {
                    Vector3 pointC = (m_cutPointA + m_cutPointB) * 0.5f;
                    pointC = pointC + m_cutDirection * m_cutDepth;
                    m_pointC.transform.position = pointC;
                }
            }
        }
    }

    public float CutDepth
    {
        get { return m_cutDepth; }
        set
        {
            if (value != m_cutDepth && value > 0.0f)
            {
                m_cutDepth = value;
                if (m_pointC)
                {
                    Vector3 pointC = (m_cutPointA + m_cutPointB) * 0.5f;
                    pointC = pointC + m_cutDirection * m_cutDepth;
                    m_pointC.transform.position = pointC;
                }
            }
        }
    }
    

    /// bool to store the current status of the component to avoid complex check in update method
    private bool m_isReady = false;

    /// Internal mesh to display the square of cut
    private Mesh m_mesh;

    public void Start()
    {
        // check inputs
        if (m_pointA == null || m_pointA == null || m_pointA == null)
        {
            Debug.LogError("3 Point Gameobject are needed to create the cutting plan");
            m_isReady = false;
            return;
        }

        // create cutting square to be displayed
        createCuttingSquare();

        // If cutting manager is setretrieve SOFA square info
        if (m_sofaCuttingMgr)
        {
            SofaVec3Data pointA = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointA");
            SofaVec3Data pointB = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointB");
            SofaVec3Data dir = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutDir");
            SofaDoubleData depth = m_sofaCuttingMgr.m_dataArchiver.GetSofaDoubleData("cutDepth");

            
            m_cutPointA = pointA.Value;
            m_cutPointB = pointB.Value;
            m_cutDirection = dir.Value;
            m_cutDepth = depth.Value;

            Vector3 pointC = (m_cutPointA + m_cutPointB) * 0.5f;
            pointC = pointC + m_cutDirection * m_cutDepth;

            m_pointA.transform.position = m_cutPointA;
            m_pointB.transform.position = m_cutPointB;
            m_pointC.transform.position = pointC;

            m_isReady = true;
        }

    }

    private bool cutDone = false;
    private int cptDone = 0;
    void Update()
    {
        if (!m_isReady)
            return;

        updateCuttingPlanFromObjects();

        if (cutDone)
        {
            cptDone++;
            if (cptDone == 50)
            {
                m_sofaCuttingMgr.m_dataArchiver.GetSofaBoolData("performCut").Value = false;
                cutDone = false;
                cptDone = 0;
            }
        }
    }


    private void createCuttingSquare()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        m_mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        m_mesh.vertices = vertices;

        int[] tris = new int[12]
        {
            // lower left triangle
            0, 1, 2,
            // upper right triangle
            3, 2, 1,

            0, 2, 1,
            2, 3, 1
        };
        m_mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        m_mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        m_mesh.uv = uv;

        meshFilter.mesh = m_mesh;
    }


    void updateCuttingPlanFromObjects()
    {
        m_cutPointA = m_pointA.transform.position;
        m_cutPointB = m_pointB.transform.position;

        Vector3 tmp = (m_cutPointA + m_cutPointB) * 0.5f;
        Vector3 dir = m_pointC.transform.position - tmp;
        m_cutDepth = dir.magnitude;
        m_cutDirection = dir / m_cutDepth; 


        Vector3[] vertices = m_mesh.vertices;
        m_cutDirection.Normalize();
        vertices[0] = m_cutPointA;
        vertices[1] = m_cutPointB;
        vertices[2] = m_cutPointA + m_cutDirection * m_cutDepth;
        vertices[3] = m_cutPointB + m_cutDirection * m_cutDepth;
        m_mesh.vertices = vertices;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        m_mesh.normals = normals;
    }

    public void performCut()
    {
        SofaVec3Data pointA = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointA");
        SofaVec3Data pointB = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointB");
        SofaVec3Data dir = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutDir");
        SofaDoubleData depth = m_sofaCuttingMgr.m_dataArchiver.GetSofaDoubleData("cutDepth");

        pointA.Value = m_cutPointA;
        pointB.Value = m_cutPointB;
        dir.Value = m_cutDirection;
        depth.Value = m_cutDepth;

        Debug.Log("!!!!!!!processCut!!!!!!!!");
        m_sofaCuttingMgr.m_dataArchiver.GetSofaBoolData("performCut").Value = true;

        cutDone = true;
    }


}
