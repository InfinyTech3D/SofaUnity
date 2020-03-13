using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaMouseInteractor : SofaRayCaster
{
    public Camera m_camera = null;
    protected int m_selectedTriID = -1;
    int[] m_selectedTri = new int[3];

    public SofaMesh m_sofaMesh = null;
    protected Mesh m_mesh;

    private MeshRenderer m_renderer;

    void Awake()
    {
        m_length = 1000.0f;
        m_stiffness = 1.0f;
        CreateSofaRayCaster();


        // Add a MeshFilter to the GameObject
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        //to see it, we have to add a renderer
        m_renderer = gameObject.GetComponent<MeshRenderer>();
        if (m_renderer == null)
        {
            m_renderer = gameObject.AddComponent<MeshRenderer>();
            m_renderer.enabled = true;
        }

        if (m_renderer.sharedMaterial == null)
        {
            m_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
        }

        m_mesh = mf.mesh = new Mesh();
        m_mesh.name = "SofaSelection";

        Vector3[] testVert = new Vector3[3];
        testVert[0] = new Vector3(0.0f, 0.0f, 0.0f);
        testVert[1] = new Vector3(0.0f, 10.0f, 10.0f);
        testVert[2] = new Vector3(10.0f, 0.0f, 10.0f);
        m_mesh.vertices = testVert;

        int[] triTest = new int[3];
        triTest[0] = 0;
        triTest[1] = 1;
        triTest[2] = 2;
        m_mesh.triangles = triTest;
    }


    void Start()
    {
        if (m_camera == null)
        {
            GameObject _cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (_cam != null)
            {
                // Get Sofa context
                m_camera = _cam.GetComponent<Camera>();
            }
            else
            {
                Debug.LogError("SofaMouseInteractor camera not found.");
                return;
            }
        }

        if (m_sofaMesh == null)
        {
            Debug.LogError("SofaMouseInteractor SofaMesh not found.");
            return;
        }

        m_sofaMesh.AddListener();
        StartRay();
    }

    /// Update is called once per frame in unity animation loop
    void Update()
    {
        if (!m_initialized)
            return;

        if (this.ActivateTool)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                this.ActivateTool = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            m_origin = ray.origin;
            m_direction = ray.direction;

            Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
            Vector3 directionS = m_sofaContext.transform.InverseTransformDirection(m_direction);

            if (Input.GetMouseButtonDown(0))
            {
                this.ActivateTool = true;
            }

            // cast ray here
            m_selectedTriID = -1;
            m_selectedTriID = m_sofaRC.castRay(originS, directionS);
            if (m_selectedTriID >= 0)
            {
                Debug.Log(this.gameObject.name + " || origin: " + m_origin + " => originS: " + originS + " |  direction : " + m_direction + " => directionS: " + directionS + " | triId: " + m_selectedTriID);

                m_selectedTri[0] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3];
                m_selectedTri[1] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3 + 1];
                m_selectedTri[2] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3 + 2];

                Vector3[] testVert = new Vector3[3];                
                testVert[0] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[0]]);
                testVert[1] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[1]]);
                testVert[2] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[2]]);
                m_mesh.vertices = testVert;
            }
        }
    }

}
