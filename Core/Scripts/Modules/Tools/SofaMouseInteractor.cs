using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
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
        [SerializeField]
        protected bool m_drawSpring = false;

        // Draws a triangle that covers the middle of the screen
        public Material m_selectionMaterial = null;
        public Material m_springMaterial = null;


        /// Id of the selected Triangle
        private int m_selectedTriID = -1;
        /// Vertex Id of the of the selected Triangle
        private int[] m_selectedTri = new int[3];
        private Vector3[] m_selectedVertices = null;
        private Vector3[] m_springVertices = null;

        /// Pointer to the Sofa Mesh selected
        private SofaMesh m_sofaMesh = null;
        /// List of all SofaMesh in the current scene
        private List<SofaMesh> m_meshes;

        /// Parameter to store the first intersection between object and mouse ray
        private bool firstTouch = true;


        protected bool isCastingRay = false;




        ////////////////////////////////////////////
        //////  SofaMouseInteractor accessors  /////
        ////////////////////////////////////////////

        /// Getter/setter for @sa m_drawSelection
        public bool DrawSelection
        {
            get { return m_drawSelection; }
            set
            {
                m_drawSelection = value;
            }
        }

        public bool DrawSpring
        {
            get { return m_drawSpring; }
            set
            {
                m_drawSpring = value;
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

            if (m_selectedVertices == null)
                m_selectedVertices = new Vector3[3];

            if (m_springVertices == null)
                m_springVertices = new Vector3[2];
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


            /// Will create the real sofa Ray caster when simulation start.
            CreateSofaRayCaster_impl();
            /// automatically activate the ray.
            StartRay();
        }


        int m_foundTri = -1;

        /// Update is called once per frame in unity animation loop
        void Update()
        {
            if (!m_initialized)
                return;

            // As soon as shift is press we start sending ray but the tool is not active == no interaction
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isCastingRay = true;
                firstTouch = true;
                //TargetMeshes();
            }

            // When shift is release, we stop casting ray, we unactive tool and set tri selected to -1
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                isCastingRay = false;
                m_foundTri = -1;
                this.ActivateTool = false;
                UnTargetMesh();
            }


            // Mouse button control the activation of tool
            if (Input.GetMouseButtonDown(0))
            {
                this.ActivateTool = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.ActivateTool = false;
            }

            if (isCastingRay)
            {
                /// compute 3D ray position and direction according to mouse position on screen
                Vector3 pos = Input.mousePosition;
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                m_origin = ray.origin;
                m_direction = ray.direction;

                /// Compute position in sofa world.
                Vector3 originS = m_sofaContext.transform.InverseTransformPoint(m_origin);
                Vector3 directionS = m_sofaContext.transform.InverseTransformPoint(m_direction);

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

                    //Debug.Log(this.gameObject.name + " || origin: " + m_origin + " => originS: " + originS + " |  direction : " + m_direction + " => directionS: " + directionS + " | triId: " + m_selectedTriID);
                    if (!this.ActivateTool)
                    {
                        m_foundTri = m_selectedTriID;
                    }
                }

                /// Update selection mesh to render only if SofaMesh is set and option are set.
                if ((m_sofaMesh != null) && (m_foundTri != -1) && (m_drawSelection || m_drawSpring))
                {
                    // verticesIds in Unity world
                    m_selectedTri[0] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_foundTri * 3];
                    m_selectedTri[1] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_foundTri * 3 + 1];
                    m_selectedTri[2] = m_sofaMesh.SofaMeshTopology.m_mesh.triangles[m_foundTri * 3 + 2];


                    //m_selectedVertices[0] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[0]]);
                    //m_selectedVertices[1] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[1]]);
                    //m_selectedVertices[2] = this.transform.InverseTransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[2]]);

                    m_selectedVertices[0] = m_sofaContext.transform.TransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[0]]);
                    m_selectedVertices[1] = m_sofaContext.transform.TransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[1]]);
                    m_selectedVertices[2] = m_sofaContext.transform.TransformPoint(m_sofaMesh.SofaMeshTopology.m_mesh.vertices[m_selectedTri[2]]);


                    m_springVertices[0] = (m_selectedVertices[0] + m_selectedVertices[1] + m_selectedVertices[2]) / 3;
                    float length = (m_origin - m_springVertices[0]).magnitude;
                    m_springVertices[1] = m_origin + m_direction * length;
                }
            }
        }




        /// Internal method to search for a SofaMesh and track it for rendering selection. Will AddListener to it.
        protected void TargetMesh(string meshName)
        {
            // need to go through UntargetMesh before selecting a new one.
            if (m_sofaMesh != null)
                return;

            // Hack as the name of the Mesh is not the name of MechanicalObject. Work only for one mesh.
            if (m_meshes.Count == 1)
            {
                m_sofaMesh = m_meshes[0];
                m_sofaMesh.AddListener();
            }

            //foreach (SofaMesh mesh in m_meshes)
            //{
            //    Debug.Log("mesh: " + mesh.UniqueNameId + " looking for: " + meshName);
            //    if (mesh.UniqueNameId == meshName)
            //    {
            //        m_sofaMesh = mesh;
            //        m_sofaMesh.AddListener();
            //    }
            //}        
        }


        protected void TargetMeshes()
        {
            foreach (SofaMesh mesh in m_meshes)
            {
                mesh.AddListener();
            }
        }


        /// Internal method to release the target mesh and remove listener.
        protected void UnTargetMesh()
        {
            if (m_sofaMesh)
            {
                m_sofaMesh.RemoveListener();
                m_sofaMesh = null;
            }
        }

        protected void UnTargetMeshes()
        {
            foreach (SofaMesh mesh in m_meshes)
            {
                mesh.RemoveListener();
            }
        }


        void OnPostRender()
        {
            if (m_drawSelection)
                RenderSelection();

            if (m_drawSpring)
                RenderSpring();
        }

        void OnDrawGizmos()
        {
            if (m_drawSelection)
                RenderSelection();
        }


        protected void RenderSelection()
        {
            if (m_foundTri < 0)
                return;

            if (!m_selectionMaterial)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }

            m_selectionMaterial.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            GL.Vertex3(m_selectedVertices[0].x, m_selectedVertices[0].y, m_selectedVertices[0].z);
            GL.Vertex3(m_selectedVertices[1].x, m_selectedVertices[1].y, m_selectedVertices[1].z);
            GL.Vertex3(m_selectedVertices[2].x, m_selectedVertices[2].y, m_selectedVertices[2].z);

            GL.Vertex3(m_selectedVertices[0].x, m_selectedVertices[0].y, m_selectedVertices[0].z);
            GL.Vertex3(m_selectedVertices[2].x, m_selectedVertices[2].y, m_selectedVertices[2].z);
            GL.Vertex3(m_selectedVertices[1].x, m_selectedVertices[1].y, m_selectedVertices[1].z);
            GL.End();
        }


        protected void RenderSpring()
        {
            if (m_foundTri < 0 || !this.ActivateTool)
                return;

            GL.Begin(GL.LINES);
            m_springMaterial.SetPass(0);
            GL.Vertex3(m_springVertices[0].x, m_springVertices[0].y, m_springVertices[0].z);
            GL.Vertex3(m_springVertices[1].x, m_springVertices[1].y, m_springVertices[1].z);
            GL.End();
        }

    }
}
