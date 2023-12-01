using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class CuttingManager : MonoBehaviour
{
    public SofaComponent m_sofaCuttingMgr = null;

    public Vector3 CutPointA = new Vector3(0, 0, 0);
    public Vector3 CutPointB = new Vector3(1, 0, 0);

    public Vector3 CutDirection = new Vector3(0, -1, 0);
    public float CutDepth = 1.0f;

    public bool processCut = false;

    private Mesh mesh;
    public void Start()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[12]
        {
            // lower left triangle
            0, 1, 2,
            // upper right triangle
            3, 2, 1,

            0, 2, 1,
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;

        if (m_sofaCuttingMgr)
        {
            SofaVec3Data pointA = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointA");
            SofaVec3Data pointB = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointB");
            SofaVec3Data dir = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutDir");
            SofaDoubleData depth = m_sofaCuttingMgr.m_dataArchiver.GetSofaDoubleData("cutDepth");

            CutPointA = pointA.Value;
            CutPointB = pointB.Value;
            CutDirection = dir.Value;
            CutDepth = depth.Value;
        }

    }

    protected bool cutDone = false;
    int cptDone = 0;
    void Update()
    {
        updateVertices();

        if (processCut && m_sofaCuttingMgr)
        {
            SofaVec3Data pointA = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointA");
            SofaVec3Data pointB = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutPointB");
            SofaVec3Data dir = m_sofaCuttingMgr.m_dataArchiver.GetSofaVec3Data("cutDir");
            SofaDoubleData depth = m_sofaCuttingMgr.m_dataArchiver.GetSofaDoubleData("cutDepth");

            pointA.Value = CutPointA;
            pointB.Value = CutPointB;
            dir.Value = CutDirection;
            depth.Value = CutDepth;

            Debug.Log("!!!!!!!processCut!!!!!!!!");
            m_sofaCuttingMgr.m_dataArchiver.GetSofaBoolData("performCut").Value = true;
            processCut = false;
            cutDone = true;
        }
        else if (cutDone)
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

    void updateVertices()
    {
        Vector3[] vertices = mesh.vertices;
        CutDirection.Normalize();
        vertices[0] = CutPointA;
        vertices[1] = CutPointB;
        vertices[2] = CutPointA + CutDirection * CutDepth;
        vertices[3] = CutPointB + CutDirection * CutDepth;
        mesh.vertices = vertices;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;
    }
     
}
