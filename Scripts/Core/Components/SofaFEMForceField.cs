using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaFEMForceField : SofaBaseComponent
    {
        protected override void CreateSofaAPI_Impl()
        {            
            SofaLog("SofaFEMForceField::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

            // Add a MeshFilter to the GameObject
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
            {
                mr = gameObject.AddComponent<MeshRenderer>();
                mr.enabled = false;
            }

            if (mr.sharedMaterial == null)
            {
                mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
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

            foreach (Transform child in DAGNode.transform)
            {
                Debug.Log("Sibling name: " + child.name);
            }

            return false;
        }


        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            if (m_meshInit == false)
            {
                FindSofaMesh();
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
