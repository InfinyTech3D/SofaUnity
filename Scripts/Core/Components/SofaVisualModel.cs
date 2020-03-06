using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SofaUnity
{
    public class SofaVisualModel : SofaBaseComponent
    {
        /// Member: Unity Mesh object of this GameObject
        protected Mesh m_mesh;
        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMeshAPI m_sofaMeshAPI = null;

        protected override void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaBaseComponent " + UniqueNameId + " already has a SofaBaseComponentAPI.");
                return;
            }

            if (m_sofaContext == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext is null", 1);
                return;
            }

            if (m_sofaContext.GetSimuContext() == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext.GetSimuContext() is null", 1);
                return;
            }

            SofaLog("SofaVisualModel::CreateSofaAPI: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaVisualModelAPI(m_sofaContext.GetSimuContext(), UniqueNameId);


            InitBaseMeshAPI();
        }


        protected void InitBaseMeshAPI()
        {
            if (m_sofaMeshAPI == null)
            {
                // Get access to the sofaContext
                IntPtr _simu = m_sofaContext.GetSimuContext();

                if (_simu == IntPtr.Zero)
                    return;
                
                // Create the API object for SofaMesh
                m_sofaMeshAPI = new SofaBaseMeshAPI(m_sofaContext.GetSimuContext(), UniqueNameId, false);
                SofaLog("SofaVisualModel::InitBaseMeshAPI object created");

                m_sofaMeshAPI.loadObject();

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

                //MeshCollider collid = gameObject.GetComponent<MeshCollider>();
                //if (collid == null)
                //    gameObject.AddComponent<MeshCollider>();

                initMesh(false);
            }
        }
        
        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void AwakePostProcess()
        {
            
        }



        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////
        protected Vector3[] m_vertCenter = null;
        protected Vector3[] m_vertices = null;
        protected Vector3[] m_normals = null;
        protected bool m_linearMesh = false;

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected void initMesh(bool toUpdate)
        {
            if (m_sofaMeshAPI == null)
                return;

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
            if (m_log)
                Debug.Log("SofaBox::Start play mode.");
#endif

            m_mesh.name = "SofaVisualMesh";
            m_mesh.vertices = new Vector3[0];
            m_sofaMeshAPI.updateMesh(m_mesh);

            int nbrF = m_sofaMeshAPI.GetNumberOfFaces();
            if (nbrF == 0)
            {
                SofaLog("SofaVisualModel::initMesh EdgeSetMesh", 0, true);
                m_linearMesh = true;
                CreateLinearMesh();
                return;
            }

            m_mesh.triangles = m_sofaMeshAPI.createTriangulation();            

            m_sofaMeshAPI.recomputeTexCoords(m_mesh);

            SofaLog("SofaVisualModel::initMesh ok: " + m_mesh.vertices.Length);
            //base.initMesh(false);

            if (toUpdate)
                m_sofaMeshAPI.updateMesh(m_mesh);

            
        }



        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            SofaLog("SofaVisualMesh::updateImpl called.");

            if (m_sofaMeshAPI != null)
            {
                if (false)//(m_sofaMeshAPI.hasTopologyChanged())
                {
                    m_mesh.triangles = m_sofaMeshAPI.createTriangulation();
                    //if (m_invertNormals)
                    //{
                    //    m_sofaMeshAPI.m_invertNormals = m_invertNormals;
                    //    invertMeshNormals();
                    //}
                    m_sofaMeshAPI.setTopologyChange(false);
                    m_sofaMeshAPI.updateMesh(m_mesh);
                    m_mesh.RecalculateNormals();
                }
                else if (m_linearMesh == true)
                {
                    m_sofaMeshAPI.updateVertices(m_vertCenter);
                    UpdateLinearMesh();
                }
                else
                {
                    //int res = m_sofaMeshAPI.updateMeshVelocity(m_mesh, m_sofaContext.timeStep);                    
                    //if (res == -1)
                    //    m_sofaContext.breakerProcedure();
                    m_sofaMeshAPI.updateMesh(m_mesh);
                }
                m_mesh.RecalculateBounds();
            }

        }

        protected int BeamDiscretisation = 4;
        protected float Beamradius = 0.5f;
        
        protected void CreateLinearMesh()
        {
            int nbrV = m_mesh.vertices.Length;

            // nothing to do
            if (nbrV < 2)
                return;

            m_vertCenter = new Vector3[m_mesh.vertices.Length];
            for (int i = 0; i < nbrV; i++)
                m_vertCenter[i] = m_mesh.vertices[i];

            int nbrP = nbrV * 4 * BeamDiscretisation + 2; // +2 for the centers
            //Debug.Log("nbrV: " + nbrV);
            //Debug.Log("nbrP: " + nbrP);

            m_vertices = new Vector3[nbrP];
            m_normals = new Vector3[nbrP];

            //Debug.Log("m_vertices: " + m_vertices.Length);
            //Debug.Log("m_vertCenter: " + m_vertCenter.Length);

            // update borders first
            m_vertices[0] = m_vertCenter[0];
            m_vertices[nbrP - 1] = m_vertCenter[nbrV - 1];
            m_normals[0] = m_vertCenter[0] - m_vertCenter[1];
            m_normals[nbrP - 1] = m_vertCenter[nbrV - 1] - m_vertCenter[nbrV - 2];

            // update first circle:
            UpdateLine(m_vertCenter[0], m_vertCenter[1], 0);
            // update intermediate points
            for (int i = 1; i < nbrV - 1; i++)
            {
                UpdateLine(m_vertCenter[i], m_vertCenter[i + 1], i);
            }

            //update last circle
            UpdateLine(m_vertCenter[nbrV - 2], m_vertCenter[nbrV - 1], nbrV - 1);
            m_mesh.vertices = m_vertices;
            m_mesh.normals = m_normals;

            // create triangles here
            CreateLinearMeshTriangulation(nbrV-1);

            // create fake uv
            Vector2[] uv = new Vector2[nbrP];
            int nbrPointPerCircle = BeamDiscretisation * 4;
            int nbrCircles = nbrV;
            uv[0] = Vector2.zero;
            uv[nbrP-1] = Vector2.zero;

            float incrementU = (float)(1.0f / (nbrPointPerCircle-1));
            float incrementV = (float)(1.0f / (nbrCircles-1));

            for (int i=0; i< nbrCircles; i++)
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
        }


        protected void UpdateLinearMesh()
        {
            int nbrV = m_vertCenter.Length;

            // nothing to do
            if (nbrV < 2)
                return;

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
            UpdateLine(m_vertCenter[nbrV - 2], m_vertCenter[nbrV - 1], nbrV-1);
            m_mesh.vertices = m_vertices;
            m_mesh.normals = m_normals;
        }

        protected void UpdateLine(Vector3 pointA, Vector3 pointB, int nbrCyl, bool firstPoint = false)
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
            corners[0] = center + cyl_N1 * Beamradius;
            corners[1] = center + cyl_N2 * Beamradius;
            corners[2] = center - cyl_N1 * Beamradius;
            corners[3] = center - cyl_N2 * Beamradius;

            int increment = nbrCyl * BeamDiscretisation * 4 + 1; // to skip first center point
            if (BeamDiscretisation > 1)
            {
                float factor = 1.0f / BeamDiscretisation;
                for (int i = 0; i < 4; i++)
                {
                    // add corner first
                    m_vertices[increment] = corners[i];
                    m_normals[increment] = corners[i] - center;
                    // tangente
                    Vector3 dirT = corners[(i + 1) % 4] - corners[i];
                    increment++;

                    // add subpoints
                    for (int j = 1; j < BeamDiscretisation; j++)
                    {
                        // not at radius length
                        m_vertices[increment] = corners[i] + factor * j * dirT;

                        // apply radius
                        Vector3 dirPoint = m_vertices[increment] - center;
                        dirPoint.Normalize();
                        m_normals[increment] = dirPoint;
                        m_vertices[increment] = center + dirPoint * Beamradius;
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
        }


        protected void CreateLinearMeshTriangulation(int nbrCylinder)
        {
            int nbrPointPerCircle = BeamDiscretisation * 4;
            int nbrTriPerBorder = nbrPointPerCircle;
            int nbrTriPerCylinder = nbrPointPerCircle * 2;
            int[] tris = new int[(nbrCylinder * nbrTriPerCylinder + 2 * nbrTriPerBorder)*3];

            //Debug.Log("nbrPointPerCircle: " + nbrPointPerCircle);
            //Debug.Log("tris: " + (nbrCylinder * nbrTriPerCylinder + 2 * nbrTriPerBorder)*3);

            int cptTri = 0;
            // create first border
            for (int i=0; i< nbrPointPerCircle; i++)
            {
                tris[cptTri + 1] = 0;
                tris[cptTri] = i + 1;
                tris[cptTri + 2] = (i+1)% nbrPointPerCircle + 1;

                cptTri += 3;
            }
            
            // create cylinders
            for (int i=0; i< nbrCylinder; i++)
            {
                int idC1 = i * nbrPointPerCircle + 1;
                int idC2 = (i + 1)* nbrPointPerCircle + 1;

                for (int j=0; j< nbrPointPerCircle; ++j)
                {
                    tris[cptTri + 1] = idC1 + j;
                    tris[cptTri] = idC2 + j;
                    tris[cptTri + 2] = idC2 + (j + 1) % nbrPointPerCircle;

                    tris[cptTri + 4] = idC1 + j;
                    tris[cptTri + 3] = idC2 + (j + 1) % nbrPointPerCircle;
                    tris[cptTri + 5] = idC1 + (j + 1) % nbrPointPerCircle;

                    cptTri += 6;
                }
            }

            // create last border
            int idP = 1 + nbrPointPerCircle * nbrCylinder;
            int idLast = 1 + nbrPointPerCircle * (nbrCylinder + 1);

            for (int i = 0; i < nbrPointPerCircle; i++)
            {
                tris[cptTri] = idLast;
                tris[cptTri + 1] = idP + i;
                tris[cptTri + 2] = idP + (i + 1) % nbrPointPerCircle;

                cptTri += 3;
            }

            m_mesh.triangles = tris;
        }

        /// Method to draw debug information like the vertex being grabed
        //void OnDrawGizmosSelected()
        //{
        //    if (m_sofaMeshAPI == null || m_sofaContext == null)
        //        return;



        //    Gizmos.color = Color.yellow;
        //    //float factor = m_sofaContext.GetFactorSofaToUnity();

        //    foreach (Vector3 vert in m_mesh.vertices)
        //    {
        //        Gizmos.DrawSphere(this.transform.TransformPoint(vert), 0.05f);
        //    }

        //    //foreach (Vector3 vert in m_verts)
        //    //{
        //    //    Gizmos.DrawSphere(this.transform.TransformPoint(vert), 0.01f);
        //    //}
        //}
    }

} // namespace SofaUnity
