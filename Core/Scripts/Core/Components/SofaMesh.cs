﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa MechanicalOBject and the Topology components in a single class 
    /// </summary>
    public class SofaMesh : SofaBaseComponent
    {
        ////////////////////////////////////////////
        //////        SofaMesh members         /////
        ////////////////////////////////////////////

        /// Pointer to a SofaMeshTopology class that hold the Mesh and method to compute it
        protected SofaMeshTopology m_topology = null;

        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMeshAPI m_sofaMeshAPI = null;

        /// counter of number of component relying on this mesh. Used to know if update is needed
        protected int m_listenerCounter = 0;


        /// real vertices buffer from SOFA (right handed)
        protected float[] m_vertexBuffer = null;
        protected float[] m_restVertexBuffer = null;
        protected Vector3[] m_unityVertices = null; // only used for NO_TOPOLOGY case

        /// number of points inside this mesh
        public int m_meshDim = 3;

        /// Number of vertices stored in this mesh
        protected int m_nbVertices = 0;
        /// Number of edges stored in this mesh
        protected int m_nbEdges = 0;
        /// Number of triangles stored in this mesh
        protected int m_nbTriangles = 0;
        /// Number of quads stored in this mesh
        protected int m_nbQuads = 0;
        /// Number of tetrahedra stored in this mesh
        protected int m_nbTetrahedra = 0;
        /// Number of hexahedra stored in this mesh
        protected int m_nbHexahedra = 0;


        ////////////////////////////////////////////
        //////        SofaMesh accessors       /////
        ////////////////////////////////////////////

        /// Getter to the topology class \sa SofaMeshTopology m_topology.
        public SofaMeshTopology SofaMeshTopology
        {
            get { return m_topology; }
        }

        /// Getter to check if Topology has already been created.
        public bool HasTopology()
        {
            if (m_topology != null)
                return true;
            else
                return false;
        }

        /// Method to find the type of the topology hold by this class.
        public TopologyObjectType TopologyType()
        {
            if (m_topology != null)
                return m_topology.TopologyType;
            else
                return TopologyObjectType.NO_TOPOLOGY;
        }

        /// Method to add a new listener to the counter \sa m_listenerCounter
        public void AddListener()
        {
            m_listenerCounter++;
        }

        /// Method to remove a listener to the counter \sa m_listenerCounter
        public void RemoveListener()
        {
            m_listenerCounter--;
        }


        /// Gette to the number of vertices
        public int NbVertices()
        {
            return m_nbVertices;
        }

        /// Gette to the number of edges
        public int NbEdges()
        {
            return m_nbEdges;
        }

        /// Gette to the number of triangles
        public int NbTriangles()
        {
            return m_nbTriangles;
        }

        /// Gette to the number of quads
        public int NbQuads()
        {
            return m_nbQuads;
        }

        /// Gette to the number of tetrahedra
        public int NbTetrahedra()
        {
            return m_nbTetrahedra;
        }

        /// Gette to the number of hexahedra
        public int NbHexahedra()
        {
            return m_nbHexahedra;
        }

        /// Getter to the inner topology revision
        public int GetTopologyRevision()
        {
            if (m_sofaMeshAPI == null)
                return 0;
            else
                return m_sofaMeshAPI.GetTopologyRevision();
        }

        /// Method to set new vertices position to this mesh
        public void SetPositions(Vector3[] vertices)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.SetPositions(vertices, m_sofaContext.transform);
        }

        /// Method to set new vertices velocity to this mesh
        public void SetVelocities(Vector3[] vels)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.SetVelocities(vels);
        }

        /// Method to set new vertices position to this mesh, using raw float buffer. 
        /// !! Be careful, no conversion from left-right coordinates system
        public void SetRawPositions(float[] vertices)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.SetRawPositions(vertices);
        }

        /// Method to set new vertices velocity to this mesh, using raw float buffer. 
        /// !! Be careful, no conversion from left-right coordinates system
        public void SetRawVelocities(float[] vels)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.SetRawVelocities(vels);
        }


        /// Method to set new vertices position to this mesh
        public void SetRawRestPositions(float[] vertices)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.SetRawRestPositions(vertices);
        }


        /// Method to return the position of the id th vertex inside the Mesh
        public Vector3 GetPosition(int id)
        {
            if (m_topology != null)
                return m_topology.m_mesh.vertices[id];
            else
                return m_unityVertices[id];
        }


        /// Method to set new vertices velocity to this mesh, using raw float buffer. 
        /// !! Be careful, no conversion from left-right coordinates system
        public void GetRawPositions(float[] vertices)
        {
            if (m_sofaMeshAPI != null)
                m_sofaMeshAPI.GetRawPositions(vertices);
        }

        ////////////////////////////////////////////
        //////          SofaMesh API           /////
        ////////////////////////////////////////////

        /// Method called by @sa CreateSofaAPI() method. To be implemented by child class if specific ComponentAPI has to be created.
        protected override void CreateSofaAPI_Impl()
        {
            SofaLog("SofaMesh::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_isCustom);

            this.gameObject.tag = "Player";

            InitBaseMeshAPI();
        }


        /// Method called by @sa Create_impl() method. to specify the type of component
        protected override void SetComponentType()
        {
            // overide name with current type
            m_componentType = m_impl.GetComponentType();
            this.gameObject.name = "SofaMesh" + "  -  " + m_componentType;
        }


        public bool m_forceUpdate = false;
        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            if (m_sofaMeshAPI == null)
                return;

            if (m_listenerCounter == 0)
                return;
            else if (m_listenerCounter < 0)
                Debug.LogError("SofaMesh has " + m_listenerCounter + " listerners, this should not be possible");

            if (this.TopologyType() != TopologyObjectType.NO_TOPOLOGY && m_sofaMeshAPI.HasTopologyChanged())
            {
                //Debug.Log("SofaMesh::updateImpl TopologyChanged");
                HandleTopologyChange();
            }

            UpdateTopology();            
        }



        ////////////////////////////////////////////
        //////      SofaMesh internal API      /////
        ////////////////////////////////////////////

        /// Method called by \sa CreateSofaAPI_Impl to init mesh at start
        protected void InitBaseMeshAPI()
        {
            if (m_sofaMeshAPI == null)
            {
                // Get access to the sofaContext
                IntPtr _simu = m_sofaContext.GetSimuContext();

                if (_simu == IntPtr.Zero)
                    return;

                // Create the API object for SofaMesh
                m_sofaMeshAPI = new SofaBaseMeshAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_isCustom);
                SofaLog("SofaMesh::InitBaseMeshAPI object created");

                InitTopology();
            }
        }


        /// Method called by \sa InitBaseMeshAPI() to init the topology
        protected void InitTopology()
        {
            if (m_sofaMeshAPI == null)
                return;

            // class to handle the topology of the mesh
            m_topology = new SofaMeshTopology();

            // get the number of vertices on the SOFA side
            m_nbVertices = m_sofaMeshAPI.getNbVertices();
            m_meshDim = m_sofaMeshAPI.GetMeshDimension();

            // create the vertex buffer and get buffer from SOFA
            CreateVertexBuffer();

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
                m_nbQuads = m_sofaMeshAPI.GetNbQuads();
                m_nbTriangles = m_sofaMeshAPI.GetNbTriangles();
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

            // For now Rigid type are handle as NO_TOPOLOGY
            if (m_meshDim > 3)
            {
                HasTopo = false;
            }

            if (HasTopo && m_nbVertices > 0)
            {
                m_sofaMeshAPI.GetRawPositions(m_vertexBuffer);
                m_topology.ComputeMesh(m_vertexBuffer, m_nbVertices, m_meshDim);
                UpdateTopology();
            }
            else
            {
                m_topology = null; // no topology, only points are stored here.
                CreateUnityVertices();
            }
        }


        /// Method to create a vertex static float buffer given the number of vertices and the dimension (2D or 3D)
        protected void CreateVertexBuffer()
        {
            m_vertexBuffer = null;
            m_vertexBuffer = new float[m_nbVertices * m_meshDim];
        }


        /// Method to create a vertex static float buffer of rest position (position at current step will be copied in this array)
        protected void CreateRestVertexBuffer()
        {
            m_restVertexBuffer = null;
            int nbrFloat = m_nbVertices * m_meshDim;
            m_restVertexBuffer = new float[nbrFloat];

            for (int i = 0; i < nbrFloat; i++)
            {
                m_restVertexBuffer[i] = m_vertexBuffer[i];
            }
        }

        /// For NO TOPOLOGY case, hande a array of Vector3 here
        protected void CreateUnityVertices()
        {
            if (m_nbVertices == 0)
                return;

            m_unityVertices = new Vector3[m_nbVertices];
            UpdateUnityVertices();
        }


        /// For NO TOPOLOGY case, hande a array of Vector3 here
        protected void UpdateUnityVertices()
        {
            if (m_nbVertices == 0)
                return;

            m_sofaMeshAPI.GetRawPositions(m_vertexBuffer);
            for (int i = 0; i < m_nbVertices; ++i)
            {
                if (m_meshDim > 2)
                    m_unityVertices[i] = new Vector3(-m_vertexBuffer[i * m_meshDim], m_vertexBuffer[i * m_meshDim + 1], m_vertexBuffer[i * m_meshDim + 2]);
                else if (m_meshDim > 1)
                    m_unityVertices[i] = new Vector3(-m_vertexBuffer[i * m_meshDim], m_vertexBuffer[i * m_meshDim + 1], 0.0f);
                else
                    m_unityVertices[i] = new Vector3(-m_vertexBuffer[i * m_meshDim], 0.0f, 0.0f);
            }
        }

        /// Method called by \sa update_impl() to recompute the topology if changed
        protected void HandleTopologyChange()
        {
            if (this.TopologyType() == TopologyObjectType.TRIANGLE)
            {
                Debug.LogError("HandleTopologyChange for TRIANGLE not implemented yet!");
            }
            else if (this.TopologyType() == TopologyObjectType.EDGE)
            {
                int _nbV = m_sofaMeshAPI.getNbVertices();
                if (m_nbVertices != _nbV)
                {
                    m_nbVertices = _nbV;
                    CreateVertexBuffer();
                }
            }
            else if (this.TopologyType() == TopologyObjectType.TETRAHEDRON)
            {
                m_nbTetrahedra = m_sofaMeshAPI.GetNbTetrahedra();
                if (m_nbTetrahedra > 0)
                {
                    m_topology.CreateTetrahedronBuffer(m_nbTetrahedra, m_sofaMeshAPI.GetTetrahedraArray(m_nbTetrahedra));
                    m_topology.ComputeMesh(m_vertexBuffer, m_nbVertices, m_meshDim);
                }
            }
        }


        /// Method called by \sa update_impl() to update the topology
        protected void UpdateTopology()
        {
            if (m_nbVertices == 0)
                return;

            if (this.TopologyType() == TopologyObjectType.TRIANGLE)
            {
                m_sofaMeshAPI.updateMesh(m_topology.m_mesh);
                //m_sofaMeshAPI.updateMeshVelocity(m_topology.m_mesh, m_sofaContext.TimeStep);
            }
            else if (this.TopologyType() == TopologyObjectType.EDGE)
            {
                m_sofaMeshAPI.updateMesh(m_topology.m_mesh);
            }
            else if (this.TopologyType() == TopologyObjectType.HEXAHEDRON)
            {
                m_sofaMeshAPI.updateVolumeMesh(m_topology.m_mesh, m_topology.mappingVertices);
            }
            else if (this.TopologyType() == TopologyObjectType.TETRAHEDRON)
            {
                m_sofaMeshAPI.updateVolumeMesh(m_topology.m_mesh, m_topology.mappingVertices);
                m_topology.ScaleVolumeMesh();
            }
            else if (this.TopologyType() == TopologyObjectType.NO_TOPOLOGY)
            {
                int _nbV = m_sofaMeshAPI.getNbVertices();
                if (m_nbVertices != _nbV)
                {
                    m_nbVertices = _nbV;
                    m_unityVertices = new Vector3[m_nbVertices];
                }

                UpdateUnityVertices();
            }

            if (drawForces)
            {
                m_sofaMeshAPI.GetRawForces(rawForces);
                for (int i = 0; i < m_nbVertices; i++)
                {
                    forces[i] = new Vector3(-rawForces[i * 3], rawForces[i * 3 + 1], rawForces[i * 3 + 2]);
                }
            }
        }


        private bool drawForces = false;
        float[] rawForces = null;
        Vector3[] forces = null;
        public bool DrawForces
        {
            get { return drawForces; }
            set
            {
                if (drawForces == value)
                    return;

                drawForces = value;
                if (drawForces)
                {
                    if (rawForces == null)
                    {
                        int _nbV = m_sofaMeshAPI.getNbVertices();
                        int meshDimension = m_sofaMeshAPI.GetMeshDimension();
                        rawForces = new float[_nbV * meshDimension];
                        forces = new Vector3[_nbV];
                    }

                    m_listenerCounter++;
                }
                else
                    m_listenerCounter--;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (!drawForces)
                return;

            if (m_topology == null|| m_topology.m_mesh == null)
                return;

            Gizmos.color = Color.red;
            for (int i=0; i< m_topology.m_mesh.vertexCount; i++)
            {
                Vector3 startPos = m_topology.m_mesh.vertices[i];
                Vector3 endPos = startPos + forces[i].normalized;
                Gizmos.DrawLine(startPos, endPos);
            }
            
        }

    }

} // namespace SofaUnity
