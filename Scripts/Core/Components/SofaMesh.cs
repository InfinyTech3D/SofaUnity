using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SofaUnity
{
    public class SofaMesh : SofaBaseComponent
    {
        /// Member: Unity Mesh object of this GameObject
        protected SofaMeshTopology m_topology = null;

        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMeshAPI m_sofaMeshAPI = null;

        protected int m_nbVertices = 0;
        protected int m_nbEdges = 0;
        protected int m_nbTriangles = 0;
        protected int m_nbQuads = 0;
        protected int m_nbTetrahedra = 0;
        protected int m_nbHexahedra = 0;

        protected override void CreateSofaAPI_Impl()
        {
            SofaLog("SofaVisualModel::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaVisualModelAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

            InitBaseMeshAPI();
        }


        protected override void SetComponentType()
        {
            // overide name with current type
            m_componentType = m_impl.GetComponentType();
            this.gameObject.name = "SofaMesh" + "  -  " + m_uniqueNameId;
        }


        public bool HasTopology()
        {
            if (m_topology != null)
                return true;
            else
                return false;
        }

        public SofaMeshTopology SofaMeshTopology
        {
            get { return m_topology; }
        }


        public TopologyObjectType TopologyType()
        {
            if (m_topology != null)
                return m_topology.TopologyType;
            else
                return TopologyObjectType.NO_TOPOLOGY;
        }


        ///// public method that return the number of vertices, override base method by returning potentially the number of vertices from tetra topology.
        public int NbVertices()
        {
            return m_nbVertices;
        }

        public int NbEdges()
        {
            return m_nbEdges;
        }

        public int NbTriangles()
        {
            return m_nbTriangles;
        }

        public int NbQuads()
        {
            return m_nbQuads;
        }

        public int NbTetrahedra()
        {
            return m_nbTetrahedra;
        }

        public int NbHexahedra()
        {
            return m_nbHexahedra;
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

                InitTopology();
            }
        }


        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected void InitTopology()
        {
            if (m_sofaMeshAPI == null)
                return;

            m_topology = new SofaMeshTopology();

            m_nbVertices = m_sofaMeshAPI.getNbVertices();
            m_topology.CreateVertexBuffer(m_nbVertices);

            m_sofaMeshAPI.GetVertices(m_topology.m_vertexBuffer);

            // here get topology type
            // createTopologyBuffer depending of the type set the type to sofaMeshTopology
            // Filltopology(int`[] fromm_sofaMeshAPI)
            // compute final mapping

            // updateMesh normally(float[] velocities frim m_sofaMeshAPI
            //update mesh from SofaMeshTopology knows how to update Mesh structure using mapping or not)

            bool HasTopo = false;
            m_nbHexahedra = m_sofaMeshAPI.GetNbHexahedra();
            if (m_nbHexahedra > 0)
            {
                m_topology.CreateHexahedronBuffer(m_nbHexahedra, m_sofaMeshAPI.GetHexahedraArray(m_nbHexahedra));
                HasTopo = true;
            }

            if (!HasTopo)
            {
                m_nbTetrahedra = m_sofaMeshAPI.GetNbTetrahedra();
                if (m_nbTetrahedra > 0)
                {
                    m_topology.CreateTetrahedronBuffer(m_nbTetrahedra, m_sofaMeshAPI.GetTetrahedraArray(m_nbTetrahedra));
                    HasTopo = true;
                }
            }

            if (!HasTopo)
            {
                m_nbQuads = m_sofaMeshAPI.GetNbTriangles();
                m_nbTriangles = m_sofaMeshAPI.GetNbQuads();
                if (m_nbQuads > 0)
                {
                    m_topology.CreateQuadBuffer(m_nbQuads, m_sofaMeshAPI.GetQuadsArray(m_nbQuads));
                    HasTopo = true;
                }
                if (m_nbTriangles > 0)
                {
                    m_topology.CreateTriangleBuffer(m_nbTriangles, m_sofaMeshAPI.GetTrianglesArray(m_nbTriangles));
                    HasTopo = true;
                }
            }

            if (!HasTopo)
            {
                m_nbEdges = m_sofaMeshAPI.GetNbEdges();
                if (m_nbEdges > 0)
                {
                    m_topology.CreateEdgeBuffer(m_nbEdges, m_sofaMeshAPI.GetEdgesArray(m_nbEdges));
                    HasTopo = true;
                }
            }

            if (HasTopo)
            {
                m_topology.ComputeMesh();
            }
                
        }

        public bool m_forceUpdate = false;
        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            Debug.Log("SofaMesh UpdateImpl");
            // TODO: for the moment the recompute of tetra is too expensive. Only update the number of vertices and tetra
            // Need to find another solution.
            //if (m_impl.hasTopologyChanged())
            //{
            //    m_impl.setTopologyChange(false);

            //    if (nbTetra > 0)
            //        updateTetraMesh();
            //    else
            //        m_impl.updateMesh(m_mesh);
            //}

            if (m_sofaMeshAPI != null && m_forceUpdate)
            {  
                if (this.TopologyType() == TopologyObjectType.TRIANGLE)
                {
                    m_sofaMeshAPI.updateMeshVelocity(m_topology.m_mesh, m_sofaContext.TimeStep);
                }
                else if (this.TopologyType() == TopologyObjectType.EDGE)
                {

                }
                else if (this.TopologyType() == TopologyObjectType.TETRAHEDRON)
                {
                    m_sofaMeshAPI.updateMeshTetra(m_topology.m_mesh, m_topology.mappingVertices);
                    //else // pass from false to true.
                    //{
                    //    m_sofaMeshAPI.updateMesh(m_mesh);
                    //}
                }
            }
        }

    }

} // namespace SofaUnity
