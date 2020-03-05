using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaFEMForceField : SofaBaseComponent
    {
        private SofaMesh m_sofaMesh = null;
        private MeshRenderer m_renderer;
        private bool m_oldStatus = false;

        protected override void CreateSofaAPI_Impl()
        {            
            SofaLog("SofaFEMForceField::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

            // Add a MeshFilter to the GameObject
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();

            //to see it, we have to add a renderer
            m_renderer = gameObject.GetComponent<MeshRenderer>();
            if (m_renderer == null)
            {
                m_renderer = gameObject.AddComponent<MeshRenderer>();
                m_renderer.enabled = false;
                m_oldStatus = false;
            }

            if (m_renderer.sharedMaterial == null)
            {
                m_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }

            //MeshCollider collid = gameObject.GetComponent<MeshCollider>();
            //if (collid == null)
            //    gameObject.AddComponent<MeshCollider>();

            // FindSofaMesh();
            m_meshInit = false;
        }

        bool m_meshInit = false;

        protected bool FindSofaMesh()
        {
            Debug.Log("SofaFEMForceField FindSofaMesh in: " + m_ownerNode.UniqueNameId);
            GameObject DAGNode = m_ownerNode.gameObject;

            bool found = false;
            foreach (Transform child in DAGNode.transform)
            {                
                SofaMesh sofaMesh = child.GetComponent<SofaMesh>();
                if (sofaMesh != null)
                {
                    Debug.Log("Sofa Mesh: " + child.name);
                    m_sofaMesh = sofaMesh;
                    found = true;
                }
            }

            return found;
        }

        protected override void Init_impl()
        {
            if (m_meshInit == false)
            {
                if (FindSofaMesh())
                {
                    m_meshInit = true;

                    //Only do this in the editor
                    MeshFilter mf = GetComponent<MeshFilter>();
                    Debug.Log("Sofa Mesh: " + mf.name);
                    mf.mesh = m_sofaMesh.SofaMeshTopology.m_mesh;
                    Debug.Log("SofaMeshTopology.m_mesh: " + m_sofaMesh.SofaMeshTopology.m_mesh.vertices.Length);
                    Debug.Log("mf.mesh vertices: " + mf.mesh.vertices.Length);
                    Debug.Log("mf.mesh triangles: " + mf.mesh.triangles.Length);

                    //for (int i=0; i< mf.mesh.vertices.Length; ++i)
                    //{
                    //    Debug.Log("-> " + mf.mesh.vertices[i]);
                    //}
                }
            }
        }

        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        
        /// Method called by @sa Update() method.        
        protected override void Update_impl()
        {
            if (m_renderer.enabled != m_oldStatus)
            {
                m_oldStatus = m_renderer.enabled;
                if (m_oldStatus)
                {
                    m_sofaMesh.AddListener();
                }
                else
                {
                    m_sofaMesh.RemoveListener();
                }
            }


            if (m_renderer.enabled)
            {
                MeshFilter mf = GetComponent<MeshFilter>();
                Debug.Log("mf.mesh vertices: " + mf.mesh.vertices.Length);
                Debug.Log("mf.mesh triangles: " + mf.mesh.triangles.Length);
                Debug.Log("tri: " + mf.mesh.triangles[0] + " " + mf.mesh.triangles[1] + " " + mf.mesh.triangles[2]);
                Debug.Log("tri: " + mf.mesh.vertices[0] + " " + mf.mesh.vertices[2] + " " + mf.mesh.vertices[1]);
            }
        }
        

//        protected void initMesh(bool toUpdate)
//        {
//            if (m_sofaMeshAPI == null)
//                return;



//            m_mesh.name = "SofaMesh";
//            m_mesh.vertices = new Vector3[0];
//            m_sofaMeshAPI.updateMesh(m_mesh);

//            // here get topology type
//            // createTopologyBuffer depending of the type set the type to sofaMeshTopology
//            // Filltopology(int`[] fromm_sofaMeshAPI)
//            // compute final mapping

//            // updateMesh normally(float[] velocities frim m_sofaMeshAPI
//            //update mesh from SofaMeshTopology knows how to update Mesh structure using mapping or not)

//            int nbrHexa = m_sofaMeshAPI.GetNbHexahedra();
//            if (nbrHexa > 0)
//            {
//                m_topology.CreateHexahedronBuffer(nbrHexa, m_sofaMeshAPI.GetHexahedraArray(nbrHexa));
//                return;
//            }

//            int nbrTetra = m_sofaMeshAPI.GetNbTetrahedra();
//            if (nbrTetra > 0)
//            {
//                m_topology.CreateTetrahedronBuffer(nbrTetra, m_sofaMeshAPI.GetTetrahedraArray(nbrTetra));

//            }

//            int nbrQuads = m_sofaMeshAPI.GetNbTriangles();
//            int nbrTris = m_sofaMeshAPI.GetNbQuads();
//            if (nbrQuads > 0)
//            {
//                m_topology.CreateQuadBuffer(nbrQuads, m_sofaMeshAPI.GetQuadsArray(nbrQuads));

//            }
//            if (nbrTris > 0)
//            {
//                m_topology.CreateTriangleBuffer(nbrTris, m_sofaMeshAPI.GetTrianglesArray(nbrTris));

//            }

//            int nbrEdges = m_sofaMeshAPI.GetNbEdges();
//            if (nbrEdges > 0)
//            {
//                m_topology.CreateEdgeBuffer(nbrEdges, m_sofaMeshAPI.GetEdgesArray(nbrEdges));

//            }

//            // Special part for tetra
//            //if (nbTetra == 0)
//            //{
//            //    nbTetra = m_sofaMeshAPI.GetNbTetrahedra();
//            //    if (nbTetra > 0)
//            //    {
//            //        SofaLog("Tetra found! Number: " + nbTetra, 1, true);
//            //        m_tetra = new int[nbTetra * 4];

//            //        m_sofaMeshAPI.getTetrahedra(m_tetra);
//            //        m_mesh.triangles = this.computeForceField();
//            //    }
//            //    else
//            //        m_mesh.triangles = m_sofaMeshAPI.createTriangulation();
//            //}

//            //SofaLog("SofaVisualModel::initMesh ok: " + m_mesh.vertices.Length);
//            ////base.initMesh(false);

//            if (toUpdate)
//            {
//                //if (nbTetra > 0)
//                //    updateTetraMesh();
//                //else
//                //    m_sofaMeshAPI.updateMesh(m_mesh);
//            }
        }

} // namespace SofaUnity
