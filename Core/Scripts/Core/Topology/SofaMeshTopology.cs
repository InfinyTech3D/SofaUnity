using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// WIP Class to store a serie of List of the different topology elements. 
    /// This class should be used internally inside a component dealing with topology like a SofaMesh.
    /// This class will convert SOFA topology architecture into a Unity mesh
    /// </summary>
    public class SofaMeshTopology
    {
        ////////////////////////////////////////////
        //////     SofaMeshTopology members    /////
        ////////////////////////////////////////////

        /// Higher level of topology handle in this class
        protected TopologyObjectType m_topologyType = TopologyObjectType.NO_TOPOLOGY;

        /// Pointer to the Unity Mesh structure
        public Mesh m_mesh = null;

        // Do we need dynamic or static buffer here??
        protected List<Vector3> m_vertices = null;
        protected List<Edge> m_edges = null;
        protected List<Triangle> m_triangles = null;
        protected List<Quad> m_quads = null;
        protected List<Tetrahedron> m_tetrahedra = null;
        protected List<Hexahedron> m_hexahedron = null;
        
        /// number of points inside this mesh
        protected int m_nbVertices = 0;
        /// number of points inside this mesh
        public int m_meshDim = 3;

        /// real buffer sent to SOFA
        public float[] m_vertexBuffer = null;
        public float[] m_restVertexBuffer = null;

        /// number of triangles inside this mesh
        protected int nbTriangles = 0;
        /// real buffer sent to SOFA
        protected int[] m_trianglesBuffer;


        /// Member: if tetrahedron is detected, will gather the number of element
        protected int nbElemVol = 0;
        /// Member: if tetrahedron is detected, will store the tetrahedron topology
        protected int[] m_tetraBuffer;
        /// Member: if tetrahedron is detected, will store the vertex mapping between triangulation and tetrahedron topology
        public Dictionary<int, int> mappingVertices;


        ////////////////////////////////////////////
        //////    SofaMeshTopology accessors   /////
        ////////////////////////////////////////////

        /// Getter of the higher topology type stored in this class.
        public TopologyObjectType TopologyType
        {
            get { return m_topologyType; }
        }


        /// Method to create a Hexahedron static buffer given the number of elements
        public void CreateHexahedronBuffer(int nbElem, int[] elems)
        {
            m_hexahedron = new List<Hexahedron>
            {
                Capacity = nbElem
            };

            for (int i = 0; i < nbElem; ++i)
            {
                Hexahedron hexa = new Hexahedron(elems[i * 8], elems[i * 8 + 1], elems[i * 8 + 2], elems[i * 8 + 3],
                    elems[i * 8 + 4], elems[i * 8 + 5], elems[i * 8 + 6], elems[i * 8 + 7]);
                m_hexahedron.Add(hexa);
            }

            m_topologyType = TopologyObjectType.HEXAHEDRON;
        }


        /// Method to create a Tetrahedron static buffer given the number of elements
        public void CreateTetrahedronBuffer(int nbElem, int[] elems)
        {
            m_tetrahedra = new List<Tetrahedron>
            {
                Capacity = nbElem
            };

            m_tetraBuffer = new int[nbElem * 4];
            for (int i = 0; i < nbElem; ++i)
            {
                Tetrahedron tetra = new Tetrahedron(elems[i * 4], elems[i * 4 + 1], elems[i * 4 + 2], elems[i * 4 + 3]);
                m_tetraBuffer[i * 4] = elems[i * 4];
                m_tetraBuffer[i * 4 + 1] = elems[i * 4 + 1];
                m_tetraBuffer[i * 4 + 2] = elems[i * 4 + 2];
                m_tetraBuffer[i * 4 + 3] = elems[i * 4 + 3];
                
                m_tetrahedra.Add(tetra);
            }

            m_topologyType = TopologyObjectType.TETRAHEDRON;
        }


        /// Method to create a Quad static buffer given the number of elements
        public void CreateQuadBuffer(int nbElem, int[] elems)
        {
            m_quads = new List<Quad>
            {
                Capacity = nbElem
            };

            for (int i = 0; i < nbElem; ++i)
            {
                Quad quad = new Quad(elems[i * 4], elems[i * 4 + 1], elems[i * 4 + 2], elems[i * 4 + 3]);

                m_quads.Add(quad);
            }

            m_topologyType = TopologyObjectType.QUAD;
        }


        /// Method to create a Triangle static buffer given the number of elements
        public void CreateTriangleBuffer(int nbElem, int[] elems)
        {
            m_triangles = new List<Triangle>
            {
                Capacity = nbElem
            };

            m_trianglesBuffer = new int[nbElem * 3];
            for (int i = 0; i < nbElem; ++i)
            {
                Triangle tri = new Triangle(elems[i * 3], elems[i * 3 + 1], elems[i * 3 + 2]);
                m_trianglesBuffer[i * 3] = elems[i * 3];
                m_trianglesBuffer[i * 3 + 1] = elems[i * 3 + 1];
                m_trianglesBuffer[i * 3 + 2] = elems[i * 3 + 2];

                m_triangles.Add(tri);
            }

            m_topologyType = TopologyObjectType.TRIANGLE;
        }


        /// Method to create a Edge static buffer given the number of elements
        public void CreateEdgeBuffer(int nbElem, int[] elems)
        {
            m_edges = new List<Edge>
            {
                Capacity = nbElem
            };

            for (int i = 0; i < nbElem; ++i)
            {
                Edge edge = new Edge(elems[i * 2], elems[i * 2 + 1]);

                m_edges.Add(edge);
            }

            m_topologyType = TopologyObjectType.EDGE;
        }


        /// Method to create a vertex static float buffer given the number of vertices
        public void CreateVertexBuffer(int nbVertices, int meshDimension)
        {
            m_nbVertices = nbVertices;
            m_meshDim = meshDimension;
            m_vertexBuffer = new float[nbVertices * m_meshDim];
        }

        public void CreateRestVertexBuffer()
        {
            int nbrFloat = m_nbVertices * m_meshDim;
            m_restVertexBuffer = new float[nbrFloat];

            for (int i=0; i< nbrFloat; i++)
            {
                m_restVertexBuffer[i] = m_vertexBuffer[i];
            }
        }

        
        /// Main method to compute the mesh given its topology type and static buffer. Will call internal method according to the type.
        public void ComputeMesh()
        {
            // compute mesh in fonction of its dimension
            if (m_meshDim == 3)
            {
                Compute3DMesh();
            }
            else if (m_meshDim == 2)
            {
                Compute2DMesh();
            }
            else
            {
                // nothing to do for a 1D mesh. Just keep the list of Float values.
                // Other size than 2 or 3D are not yet supported.
                return;
            }


            // compute the mesh topology on top of the vertex buffer
            if (m_topologyType == TopologyObjectType.HEXAHEDRON)
            {
                ComputeMeshFromHexahedron();
            }
            else if (m_topologyType == TopologyObjectType.TETRAHEDRON)
            {
                ComputeMeshFromTetrahedron();
            }
            else if (m_topologyType == TopologyObjectType.QUAD)
            {
                ComputeMeshFromQuad();
            }
            else if (m_topologyType == TopologyObjectType.TRIANGLE)
            {
                ComputeMeshFromTriangle();
            }
            else if (m_topologyType == TopologyObjectType.EDGE)
            {
                ComputeMeshFromEdge();
            }
        }


        protected void Compute3DMesh()
        {
            m_mesh = new Mesh();
            m_mesh.name = "SofaMesh";
            Vector3[] unityVertices = new Vector3[m_nbVertices];
            for (int i = 0; i < m_nbVertices; ++i)
            {
                unityVertices[i].x = m_vertexBuffer[i * 3];
                unityVertices[i].y = m_vertexBuffer[i * 3 + 1];
                unityVertices[i].z = m_vertexBuffer[i * 3 + 2];
            }
            m_mesh.vertices = unityVertices;
            m_mesh.normals = new Vector3[m_nbVertices];
            m_mesh.uv = new Vector2[m_nbVertices];
        }


        protected void Compute2DMesh()
        {
            m_mesh = new Mesh();
            m_mesh.name = "SofaMesh";
            Vector3[] unityVertices = new Vector3[m_nbVertices];
            for (int i = 0; i < m_nbVertices; ++i)
            {
                unityVertices[i].x = m_vertexBuffer[i * 2];
                unityVertices[i].y = m_vertexBuffer[i * 2 + 1];
                unityVertices[i].z = 0.0f;
            }
            m_mesh.vertices = unityVertices;
            m_mesh.normals = new Vector3[m_nbVertices];
            m_mesh.uv = new Vector2[m_nbVertices];
        }


        ////////////////////////////////////////////
        //////  SofaMeshTopology internal API  /////
        ////////////////////////////////////////////

        /// Internal method to create the unity Mesh structure given a Hexahedron topology. Called by @sa ComputeMesh()
        protected void ComputeMeshFromHexahedron()
        {
            nbElemVol = m_hexahedron.Count;
            int[] tris = new int[nbElemVol * 12 * 3];

            Vector3[] verts = new Vector3[nbElemVol * 8];
            Vector3[] norms = new Vector3[nbElemVol * 8];
            Vector2[] uv = new Vector2[nbElemVol * 8];

            mappingVertices = new Dictionary<int, int>();

            for (int i = 0; i < nbElemVol; ++i)
            {
                Hexahedron hexa = m_hexahedron[i];
                int[] id = new int[8];
                int idHexa = i * 8;
                for (int j = 0; j < 8; ++j)
                {
                    id[j] = idHexa + j;
                    int vertId = hexa[j];

                    verts[id[j]] = m_mesh.vertices[vertId];
                    norms[id[j]] = m_mesh.normals[vertId];
                    mappingVertices.Add(id[j], vertId);

                    uv[id[j]].x = (float)j/8;
                    uv[id[j]].y = (float)j /8;
                }


                // face back
                int triVId = i * 12 * 3;
                tris[triVId + 0] = id[0];
                tris[triVId + 1] = id[2];
                tris[triVId + 2] = id[1];

                tris[triVId + 3] = id[0];
                tris[triVId + 4] = id[3];
                tris[triVId + 5] = id[2];

                // face front
                tris[triVId + 6] = id[4];
                tris[triVId + 7] = id[5];
                tris[triVId + 8] = id[6];

                tris[triVId + 9] = id[4];
                tris[triVId + 10] = id[6];
                tris[triVId + 11] = id[7];

                // face right
                tris[triVId + 12] = id[1];
                tris[triVId + 13] = id[2];
                tris[triVId + 14] = id[6];

                tris[triVId + 15] = id[5];
                tris[triVId + 16] = id[1];
                tris[triVId + 17] = id[6];

                // face left
                tris[triVId + 18] = id[0];
                tris[triVId + 19] = id[7];
                tris[triVId + 20] = id[3];

                tris[triVId + 21] = id[0];
                tris[triVId + 22] = id[4];
                tris[triVId + 23] = id[7];

                // face up
                tris[triVId + 24] = id[2];
                tris[triVId + 25] = id[3];
                tris[triVId + 26] = id[6];

                tris[triVId + 27] = id[3];
                tris[triVId + 28] = id[7];
                tris[triVId + 29] = id[6];

                // face down
                tris[triVId + 30] = id[0];
                tris[triVId + 31] = id[1];
                tris[triVId + 32] = id[5];

                tris[triVId + 33] = id[0];
                tris[triVId + 34] = id[5];
                tris[triVId + 35] = id[4];
            }

            m_mesh.vertices = verts;
            m_mesh.normals = norms;
            m_mesh.uv = uv;
            m_mesh.triangles = tris;
        }


        /// Internal method to create the unity Mesh structure given a Tetrahedron topology. Called by @sa ComputeMesh()
        protected void ComputeMeshFromTetrahedron()
        {
            nbElemVol = m_tetrahedra.Count;
            int[] tris = new int[nbElemVol * 12];

            Vector3[] verts = new Vector3[nbElemVol * 4];
            Vector3[] norms = new Vector3[nbElemVol * 4];
            Vector2[] uv = new Vector2[nbElemVol * 4];

            mappingVertices = new Dictionary<int, int>();

            for (int i = 0; i < nbElemVol; ++i)
            {
                int[] id = new int[4];
                int[] old_id = new int[4];

                int idTet = i * 4;
                for (int j = 0; j < 4; ++j)
                {
                    id[j] = idTet + j;
                    old_id[j] = m_tetraBuffer[idTet + j];

                    verts[id[j]] = m_mesh.vertices[old_id[j]];
                    norms[id[j]] = m_mesh.normals[old_id[j]];
                    mappingVertices.Add(id[j], old_id[j]);

                    m_tetraBuffer[idTet + j] = id[j];
                    uv[idTet + j].x = (float)j / 4;
                    uv[idTet + j].y = (float)j / 4;
                }

                // face 0
                tris[i * 12 + 0] = id[0];
                tris[i * 12 + 1] = id[2];
                tris[i * 12 + 2] = id[1];

                // face 1
                tris[i * 12 + 3] = id[1];
                tris[i * 12 + 4] = id[2];
                tris[i * 12 + 5] = id[3];

                // face 2
                tris[i * 12 + 6] = id[2];
                tris[i * 12 + 7] = id[0];
                tris[i * 12 + 8] = id[3];

                // face 3
                tris[i * 12 + 9] = id[0];
                tris[i * 12 + 10] = id[1];
                tris[i * 12 + 11] = id[3];
            }

            m_mesh.vertices = verts;
            m_mesh.normals = norms;
            m_mesh.uv = uv;
            m_mesh.triangles = tris;
        }


        /// Internal method to create the unity Mesh structure given a Quad topology. Called by @sa ComputeMesh()
        protected void ComputeMeshFromQuad()
        {
            Debug.LogWarning("SofaMeshTopology::ComputeMeshFromQuad() method not yet implemented!");
        }


        /// Internal method to create the unity Mesh structure given a Triangle topology. Called by @sa ComputeMesh()
        protected void ComputeMeshFromTriangle()
        {
            m_mesh.triangles = m_trianglesBuffer;
        }


        /// Internal method to create the unity Mesh structure given a Edge topology. Called by @sa ComputeMesh()
        protected void ComputeMeshFromEdge()
        {
            //Debug.LogWarning("SofaMeshTopology::ComputeMeshFromEdge() method not yet implemented!");
        }



        /// Method to update the tetrahedron topology using the vertex mapping.
        /// TODO: like ComputeMesh and handle the different cases
        public void ScaleVolumeMesh()
        {
            // Compute the barycenters of each tetra and update the vertices
            Vector3[] verts = m_mesh.vertices;
            for (int i = 0; i < nbElemVol; ++i)
            {
                Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);
                int idI = i * 4;
                // compute tetra barycenter
                for (int j = 0; j < 4; ++j)
                    bary += verts[m_tetraBuffer[idI + j]];
                bary /= 4;

                // reduce the tetra size according to the barycenter
                for (int j = 0; j < 4; ++j)
                    verts[m_tetraBuffer[idI + j]] = bary + (verts[m_tetraBuffer[idI + j]] - bary) * 0.5f;
            }

            m_mesh.vertices = verts;
        }
    }
}
