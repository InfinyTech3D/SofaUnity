using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Class to handle mouse interaction with SOFA objects.
/// This mouse interactor need to be linked with a Camera to get a viewpoint
/// </summary>
public class SofaMouseInteractor : SofaRayCaster
{
    ////////////////////////////////////////////
    //////   SofaMouseInteractor members   /////
    ////////////////////////////////////////////

    /// Pointer to the Unity camera Gameobject this interactor is linked to. If none, will take the MainCamera
    protected Camera m_camera = null;

    /// Bool parameter to draw or not the selected primitive
    [SerializeField]
    protected bool m_drawSelection = false;


    /// Pointer to the inner Mesh to render the selection. Will be linked to a SofaMesh
    protected Mesh m_mesh;

    /// Pointer to the MeshRenderer linked to the Mesh to draw the selection
    protected MeshRenderer m_renderer;


    /// Id of the selected Triangle
    private int m_selectedTriID = -1;
    /// Vertex Id of the of the selected Triangle
    private int[] m_selectedTri = new int[3];

    /// Pointer to the Sofa Mesh selected
    private SofaMesh m_sofaMesh = null;
    /// List of all SofaMesh in the current scene
    private List<SofaMesh> m_meshes;

    /// Parameter to store the first intersection between object and mouse ray
    private bool firstTouch = true;


    protected LineRenderer m_springRenderer = null;

    public Color c1 = Color.yellow;
    public Color c2 = Color.green;


    ////////////////////////////////////////////
    //////  SofaMouseInteractor accessors  /////
    ////////////////////////////////////////////

    /// Getter/setter for @sa m_drawSelection
    public bool DrawSelection
    {
        get { return m_drawSelection; }
        set {
            m_drawSelection = value;
            if (m_renderer)
                m_renderer.enabled = value;
        }
    }

    
    ////////////////////////////////////////////
    //////     SofaMouseInteractor API     /////
    ////////////////////////////////////////////

    /// Method called by Unity animation loop when object is created. Will init Meshfilter, mesh selection and the sofa ray caster.
    void Awake()
    {
        // create init sofaRay caster
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
            m_renderer.enabled = m_drawSelection;
        }

        if (m_renderer.sharedMaterial == null)
        {
            m_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
        }

        // create a single triangle mesh
        m_mesh = mf.mesh = new Mesh();
        m_mesh.name = "SofaSelection";

        Vector3[] initVert = new Vector3[3];
        int[] initTri = new int[3];
        for (int i=0; i<3; i++)
        {
            initVert[i] = new Vector3(0.0f, 0.0f, 0.0f);
            initTri[i] = i;
        }
        m_mesh.vertices = initVert;
        m_mesh.triangles = initTri;        

    }


    /// Method called by Unity animation loop when animation start. Will look for Camera if needed and SofaMesh.
    void Start()
    {
        // If not camera set, will search for the Maincamera
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
                m_initialized = false;
                return;
            }
        }

        /// Will store pointers to all SofaMesh found in the scene
        GameObject[] meshes = GameObject.FindGameObjectsWithTag("Player");
        int nbrM = meshes.Length;
        if (nbrM > 0)
            m_meshes = new List<SofaMesh>();

        foreach (GameObject obj in meshes)
        {
            SofaMesh mesh = obj.GetComponent<SofaMesh>();
            if (mesh)
                m_meshes.Add(mesh);
        }


        if (m_springRenderer == null)
        {
            m_springRenderer = gameObject.AddComponent<LineRenderer>();
            m_springRenderer.material = new Material(Shader.Find("Sprites/Default"));
            m_springRenderer.widthMultiplier = 0.2f;
            m_springRenderer.positionCount = 2;

            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            m_springRenderer.colorGradient = gradient;
        }


        /// Will create the real sofa Ray caster when simulation start.
        CreateSofaRayCaster_impl();
        /// automatically activate the ray.
        StartRay();
    }


    int m_foundTri = 0;

    /// Update is called once per frame in unity animation loop
    void Update()
    {
        if (!m_initialized)
            return;

        if (this.ActivateTool)
        {
            /// if already activated and release shift or mouse. Will unactive the interactor
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || Input.GetMouseButtonUp(0))
            {
                this.ActivateTool = false;
                UnTargetMesh();
                firstTouch = true;
            }
        }

        /// cast ray only if shift is hold
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            /// If left mouse button is clicked will activate interactor tool
            if (Input.GetMouseButtonDown(0))
            {
                this.ActivateTool = true;
            }

            /// compute 3D ray position and direction according to mouse position on screen
            Vector3 pos = Input.mousePosition;
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            m_origin = ray.origin;
            m_direction = ray.direction;

            /// Compute position in sofa world.
            Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
            Vector3 directionS = m_sofaContext.transform.InverseTransformDirection(m_direction);
            
            // cast ray here and get the selected indice
            m_selectedTriID = -1;
            m_selectedTriID = m_sofaRC.castRay(originS, directionS);
            if (m_selectedTriID >= 0)
            {
                if (firstTouch)
                {
                    string resMesh = m_sofaRC.getTouchedObjectName();
                    if (resMesh != "None")
                    {
                        TargetMesh(resMesh);
                        firstTouch = false;
                    }                    
                }

                if (m_selectedTriID != m_foundTri)
                {
                    //Debug.Log(this.gameObject.name + " || origin: " + m_origin + " => originS: " + originS + " |  direction : " + m_direction + " => directionS: " + directionS + " | triId: " + m_selectedTriID);
                    m_foundTri = m_selectedTriID;
                }

                /// Update selection mesh to render only if SofaMesh is set.
                if (m_sofaMesh != null)
                {
                    m_selectedTri[0] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3];
                    m_selectedTri[1] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3 + 1];
                    m_selectedTri[2] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_selectedTriID * 3 + 2];

                    Vector3[] testVert = new Vector3[3];                    
                    testVert[0] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[0]]);
                    testVert[1] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[1]]);
                    testVert[2] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[2]]);
                    m_mesh.vertices = testVert;

                    Vector3 bary = Vector3.zero;
                    bary = (testVert[0] + testVert[1] + testVert[2]) / 3;
                    UpdateSpringRenderer(m_origin, bary);
                }
            }
        }
    }



    /// Internal method to search for a SofaMesh and track it for rendering selection. Will AddListener to it.
    protected void TargetMesh(string meshName)
    {
        // nothing to do if no drawing
        if (!m_drawSelection)
            return;

        // need to go through UntargetMesh before selecting a new one.
        if (m_sofaMesh != null)
            return;

        foreach (SofaMesh mesh in m_meshes)
        {
            if (mesh.UniqueNameId == meshName)
            {
                m_sofaMesh = mesh;
                m_sofaMesh.AddListener();
            }
        }        
    }

    /// Internal method to release the target mesh and remove listener.
    protected void UnTargetMesh()
    {
        // nothing to do if no drawing
        if (!m_drawSelection)
            return;

        if (m_sofaMesh)
        {
            m_sofaMesh.RemoveListener();
            m_sofaMesh = null;
        }
    }


    protected void UpdateSpringRenderer(Vector3 origin, Vector3 end)
    {
        Debug.Log("origin: " + origin + " | end: " + end);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[2];
        points[0] = origin;
        points[1] = end;
        lineRenderer.SetPositions(points);
    }

}
