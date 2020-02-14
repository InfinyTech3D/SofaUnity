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

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected void initMesh(bool toUpdate)
        {
            if (m_sofaMeshAPI == null)
                return;

#if UNITY_EDITOR
            //Only do this in the editor
            MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
            //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
            Mesh meshCopy = new Mesh();
            m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes
            
#else
            //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
            if (m_log)
                Debug.Log("SofaBox::Start play mode.");
#endif

            m_mesh.name = "SofaVisualMesh";
            m_mesh.vertices = new Vector3[0];
            m_sofaMeshAPI.updateMesh(m_mesh);

            int nbrF = m_sofaMeshAPI.getNumberOfFaces();
            if (nbrF == 0)
            {
                SofaLog("SofaVisualModel::initMesh EdgeSetMesh", 0, true);
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
        protected override void UpdateImpl()
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
                else
                {
                    //int res = m_sofaMeshAPI.updateMeshVelocity(m_mesh, m_sofaContext.timeStep);                    
                    //if (res == -1)
                    //    m_sofaContext.breakerProcedure();
                    m_sofaMeshAPI.updateMesh(m_mesh);
                    UpdateLinearMesh();
                }
                m_mesh.RecalculateBounds();
            }

        }

        protected int BeamDiscretisation = 1;
        protected float Beamradius = 0.5f;
        protected Vector3[] m_verts = null;
        protected void CreateLinearMesh()
        {
            int nbrV = m_mesh.vertices.Length;

            // nothing to do
            if (nbrV < 2)
                return;

            m_verts = new Vector3[nbrV * BeamDiscretisation * 4];


            // update first:
            UpdateLine(m_mesh.vertices[0], m_mesh.vertices[1], 0);
            // update intermediate points
            for (int i = 1; i < nbrV - 1; i++)
            {
                UpdateLine(m_mesh.vertices[i], m_mesh.vertices[i + 1], i);
            }

            //update last
            UpdateLine(m_mesh.vertices[nbrV - 2], m_mesh.vertices[nbrV - 1], nbrV - 1);
        }


        protected void UpdateLinearMesh()
        {
            int nbrV = m_mesh.vertices.Length;

            // nothing to do
            if (nbrV < 2)
                return;

            // update first:
            UpdateLine(m_mesh.vertices[0], m_mesh.vertices[1], 0, true);
            // update intermediate points
            for (int i = 1; i < nbrV - 1; i++)
            {
                UpdateLine(m_mesh.vertices[i], m_mesh.vertices[i + 1], i);
            }

            //update last
            UpdateLine(m_mesh.vertices[nbrV - 2], m_mesh.vertices[nbrV - 1], nbrV-1);
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

            int increment = nbrCyl * BeamDiscretisation * 4;
            if (BeamDiscretisation > 1)
            {
                float factor = 1.0f / BeamDiscretisation;
                for (int i = 0; i < 4; i++)
                {
                    // add corner first
                    m_verts[increment] = corners[i];
                    // tangente
                    Vector3 dirT = corners[(i + 1) % 4] - corners[i];
                    dirT.Normalize();
                    increment++;

                    // add subpoints
                    for (int j = 1; j < BeamDiscretisation; j++)
                    {
                        // not at radius length
                        m_verts[increment] = corners[i] + factor * j * dirT;

                        // apply radius
                        Vector3 dirPoint = m_verts[increment] - center;
                        dirPoint.Normalize();
                        m_verts[increment] = center + dirPoint * Beamradius;
                        increment++;
                    }
                }
            }
            else
            {
                // add corners only
                for (int i = 0; i < 4; i++)
                {
                    m_verts[increment] = corners[i];
                    increment++;
                }
            }
        }

        /// Method to draw debug information like the vertex being grabed
        void OnDrawGizmosSelected()
        {
            if (m_sofaMeshAPI == null || m_sofaContext == null)
                return;

            

            Gizmos.color = Color.yellow;
            //float factor = m_sofaContext.GetFactorSofaToUnity();

            foreach (Vector3 vert in m_mesh.vertices)
            {
                Gizmos.DrawSphere(this.transform.TransformPoint(vert), 0.05f);
            }

            foreach (Vector3 vert in m_verts)
            {
                Gizmos.DrawSphere(this.transform.TransformPoint(vert), 0.01f);
            }
        }
    }

} // namespace SofaUnity
