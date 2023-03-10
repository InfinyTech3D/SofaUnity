using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Gameobject to transform a Edge topology into a Beam mesh that can be used with a meshfilter in Unity.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaBeamModel : MonoBehaviour
    {
        ////////////////////////////////////////////
        //////      SofaBeamModel members      /////
        ////////////////////////////////////////////

        /// Pointer to the SofaMesh this Beam is related to.
        public SofaMesh m_sofaMesh = null;

        /// Member: Unity Mesh object of this GameObject
        protected Mesh m_mesh;

        /// Parameter bool to store information if vec3 or rigid are parsed.
        [SerializeField]
        public bool isRigidMesh = false;

        /// Parameter of this beam, it determnines the discretisation of the circonference around each center point of the beam.
        [SerializeField]
        protected int m_beamDiscretisation = 4;

        /// Parameter of this beam, it gives the radius of the circle around each center point of the beam.
        [SerializeField]
        protected float m_beamRadius = 0.5f;


        /// Parameter to activate the recomputation of the beam mesh.
        protected bool m_doComputeMesh = true;

        /// Buffer of the center points of the beam, corresponding to the points coming from the SofaMesh
        protected Vector3[] m_vertCenter = null;
        /// Vertices buffer computed to draw the beam surface, copy into MeshFilter.vertices
        protected Vector3[] m_vertices = null;
        /// Buffer of the normal computed around the beam center. Copy into MeshFilter.normals
        protected Vector3[] m_normals = null;



        ////////////////////////////////////////////
        //////     SofaBeamModel accessors     /////
        ////////////////////////////////////////////

        /// getter/setter of the \sa m_beamDiscretisation parameter. Changing this parameter will recompute the beam mesh structure.
        public int BeamDiscretisation
        {
            get { return m_beamDiscretisation; }
            set
            {
                int tmp = value;
                if (tmp <= 0)
                    tmp = 1;

                if (tmp != m_beamDiscretisation)
                {
                    m_beamDiscretisation = tmp;
                    m_doComputeMesh = true;
                }
            }
        }


        /// getter/setter of the \sa m_beamRadius parameter. Changing this parameter will udpate the beam mesh positions.
        public float BeamRadius
        {
            get { return m_beamRadius; }
            set
            {
                if (value != m_beamRadius)
                {
                    m_beamRadius = value;
                    UpdateBeamMesh();
                }
            }
        }



        ////////////////////////////////////////////
        //////        SofaBeamModel API        /////
        ////////////////////////////////////////////

        /// Method call by Unity animation loop when object is created
        void Awake()
        {
            // Add a MeshFilter to the GameObject
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();

            if (mr.sharedMaterial == null)
            {
                mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }
        }


        /// Start is called by Unity animation loop before the first frame update
        void Start()
        {
            //Debug.Log("BeamModel::Start");
#if UNITY_EDITOR
            //Only do this in the editor
            MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
                                                          //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
                                                          //Mesh meshCopy = new Mesh();
                                                          // m_mesh = ;// = meshCopy;                    //Assign the copy to the meshes
            m_mesh = mf.mesh = new Mesh();
#else
            //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
#endif

            if (m_doComputeMesh)
                CreateBeamMesh();
        }


        /// Update is called by Unity animation loop once per frame
        void Update()
        {
            if (m_doComputeMesh)
                CreateBeamMesh();

            UpdateBeamMesh();
        }



        ////////////////////////////////////////////
        //////    SofaBeamModel internal API   /////
        ////////////////////////////////////////////

        /// Main method to create the beam mesh according to the vertex buffer from SofaMesh and the beam parameters. Will call \sa CreateMeshTriangulation()
        protected virtual void CreateBeamMesh()
        {
            if (m_sofaMesh == null || m_sofaMesh.SofaMeshTopology == null)
                return;
            
            int nbrV = m_sofaMesh.NbVertices();
            
            // nothing to do
            if (nbrV < 2)
                return;

            m_mesh.Clear();
            float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;
            m_vertCenter = new Vector3[nbrV];

            int sizeDof = 3;
            if (isRigidMesh)
                sizeDof = 7;

            for (int i = 0; i < nbrV; i++)
            {
                m_vertCenter[i] = new Vector3(sofaVertices[i * sizeDof], sofaVertices[i * sizeDof + 1], sofaVertices[i * sizeDof + 2]);
            }

            int nbrPointPerCircle = 4 * m_beamDiscretisation + 1; // +1 to close cylinder UV
            int nbrP = nbrV * nbrPointPerCircle + 2; // +2 for the centers

            m_vertices = new Vector3[nbrP];
            m_normals = new Vector3[nbrP];


            // 1. update borders first
            m_vertices[0] = m_vertCenter[0];
            m_vertices[nbrP - 1] = m_vertCenter[nbrV - 1];
            m_normals[0] = m_vertCenter[0] - m_vertCenter[1];
            m_normals[nbrP - 1] = m_vertCenter[nbrV - 1] - m_vertCenter[nbrV - 2];

            // 2. update first circle:
            UpdateLine(m_vertCenter[0], m_vertCenter[1], 0);
            // 3. update intermediate points
            for (int i = 1; i < nbrV - 1; i++)
            {
                UpdateLine(m_vertCenter[i], m_vertCenter[i + 1], i);
            }

            // 4. update last circle
            UpdateLine(m_vertCenter[nbrV - 2], m_vertCenter[nbrV - 1], nbrV - 1);
            m_mesh.vertices = m_vertices;
            m_mesh.normals = m_normals;

            // 5. create triangles here
            CreateMeshTriangulation(nbrV - 1);

            // 6. create fake uv
            Vector2[] uv = new Vector2[nbrP];
            int nbrCircles = nbrV;
            uv[0] = Vector2.zero;
            uv[nbrP - 1] = Vector2.zero;

            float incrementU = (float)(1.0f / (nbrPointPerCircle - 1));
            float incrementV = (float)(1.0f / (nbrCircles - 1));

            for (int i = 0; i < nbrCircles; i++)
            {
                int incr = i * nbrPointPerCircle + 1;
                float vvalue = i * incrementV;
                for (int j = 0; j < nbrPointPerCircle; j++)
                {
                    uv[incr + j].x = j * incrementU;
                    uv[incr + j].y = vvalue;
                }
            }
            m_mesh.uv = uv;

            m_doComputeMesh = false;
            
            // 7. Add SofaMesh listener to ensure update of the mesh positions
            m_sofaMesh.AddListener();
        }


        /// Method called by \sa CreateBeamMesh() to compute the triangulation on top of the created vertex buffer
        protected virtual void CreateMeshTriangulation(int nbrCylinder)
        {
            int nbrEdgePerCircle = m_beamDiscretisation * 4;
            int nbrPointPerCircle = m_beamDiscretisation * 4 + 1; // Duplicate first/last vertex
            int nbrTriPerBorder = nbrEdgePerCircle;
            int nbrTriPerCylinder = nbrEdgePerCircle * 2;
            int[] tris = new int[(nbrCylinder * nbrTriPerCylinder + 2 * nbrTriPerBorder) * 3];

            //Debug.Log("nbrCylinder: " + nbrCylinder);
            //Debug.Log("nbrPointPerCircle: " + nbrPointPerCircle);
            //Debug.Log("tris: " + (nbrCylinder * nbrTriPerCylinder + 2 * nbrTriPerBorder)*3);

            int cptTri = 0;
            // create first border
            for (int i = 1; i < nbrEdgePerCircle + 1; i++)
            {
                tris[cptTri + 1] = 0;
                tris[cptTri] = i;
                tris[cptTri + 2] = i + 1;

                cptTri += 3;
            }

            // create cylinders
            for (int i = 0; i < nbrCylinder; i++)
            {
                int idC1 = i * nbrPointPerCircle + 1; // last +1 is always for first border center
                int idC2 = (i + 1) * nbrPointPerCircle + 1;

                for (int j = 0; j < nbrEdgePerCircle; ++j)
                {
                    tris[cptTri + 1] = idC1 + j;
                    tris[cptTri] = idC2 + j;
                    tris[cptTri + 2] = idC2 + j + 1;

                    tris[cptTri + 4] = idC1 + j;
                    tris[cptTri + 3] = idC2 + j + 1;
                    tris[cptTri + 5] = idC1 + j + 1;

                    cptTri += 6;
                }
            }

            // create last border
            int idP = 1 + nbrPointPerCircle * nbrCylinder;
            int idLast = 1 + nbrPointPerCircle * (nbrCylinder + 1);

            for (int i = 0; i < nbrEdgePerCircle; i++)
            {
                tris[cptTri] = idLast;
                tris[cptTri + 1] = idP + i;
                tris[cptTri + 2] = idP + i + 1;

                cptTri += 3;
            }

            m_mesh.triangles = tris;
        }


        /// Main Method to update the beam mesh position from SofaMesh center positions. Will call \sa UpdateLine() around each center point
        protected virtual void UpdateBeamMesh()
        {
            if (m_sofaMesh == null || m_sofaMesh.SofaMeshTopology == null)
                return;

            int nbrV = m_vertCenter.Length;

            // nothing to do
            if (nbrV < 2)
                return;

            int sizeDof = 3;
            if (isRigidMesh)
                sizeDof = 7;

            float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;
            for (int i = 0; i < nbrV; i++)
            {
                m_vertCenter[i] = new Vector3(sofaVertices[i * sizeDof], sofaVertices[i * sizeDof + 1], sofaVertices[i * sizeDof + 2]);
            }


            // update borders first
            int idLast = m_vertices.Length - 1;
            m_vertices[0] = m_vertCenter[0];
            m_vertices[idLast] = m_vertCenter[nbrV - 1];
            m_normals[0] = m_vertCenter[0] - m_vertCenter[1];
            m_normals[idLast] = m_vertCenter[nbrV - 1] - m_vertCenter[nbrV - 2];

            // update first:
            UpdateLine(m_vertCenter[0], m_vertCenter[1], 0, true);
            // update intermediate points
            for (int i = 1; i < nbrV - 1; i++)
            {
                UpdateLine(m_vertCenter[i], m_vertCenter[i + 1], i);
            }

            //update last
            UpdateLine(m_vertCenter[nbrV - 2], m_vertCenter[nbrV - 1], nbrV - 1);
            m_mesh.vertices = m_vertices;
            m_mesh.normals = m_normals;
            m_mesh.RecalculateBounds();
        }


        /// Method to update the beam position on the circle around a given center position
        protected virtual void UpdateLine(Vector3 pointA, Vector3 pointB, int nbrCyl, bool firstPoint = false)
        {
            Vector3 cyl_dir = pointB - pointA;
            
            Vector3 cyl_N1 = Vector3.Cross(cyl_dir, Vector3.up);
            cyl_N1.Normalize();
            Vector3 cyl_N2 = Vector3.Cross(cyl_dir, cyl_N1);
            cyl_N2.Normalize();

            Vector3 center = pointB;
            if (firstPoint)
                center = pointA;

            Vector3[] corners = new Vector3[4];
            corners[0] = center + cyl_N1 * m_beamRadius;
            corners[1] = center + cyl_N2 * m_beamRadius;
            corners[2] = center - cyl_N1 * m_beamRadius;
            corners[3] = center - cyl_N2 * m_beamRadius;

            int discretisation = 4 * m_beamDiscretisation + 1; // +1 to close cylinder UV
            int increment = nbrCyl * discretisation + 1; // +1 to skip first center point
            int firstId = increment;
            if (m_beamDiscretisation > 1)
            {
                float factor = 1.0f / m_beamDiscretisation;
                for (int i = 0; i < 4; i++)
                {
                    // add corner first
                    m_vertices[increment] = corners[i];
                    m_normals[increment] = corners[i] - center;
                    // tangente
                    Vector3 dirT = corners[(i + 1) % 4] - corners[i];
                    increment++;

                    // add subpoints
                    for (int j = 1; j < m_beamDiscretisation; j++)
                    {
                        // not at radius length
                        m_vertices[increment] = corners[i] + factor * j * dirT;

                        // apply radius
                        Vector3 dirPoint = m_vertices[increment] - center;
                        dirPoint.Normalize();
                        m_normals[increment] = dirPoint;
                        m_vertices[increment] = center + dirPoint * m_beamRadius;
                        increment++;
                    }
                }
            }
            else
            {
                // add corners only
                for (int i = 0; i < 4; i++)
                {
                    m_vertices[increment] = corners[i];
                    m_normals[increment] = corners[i] - center;
                    increment++;
                }
            }

            // repeat the last first point in as last point
            m_vertices[increment] = m_vertices[firstId];
            m_normals[increment] = m_normals[firstId];
        }

    }
}
