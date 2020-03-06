using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMeshTopology
    {
        protected TopologyObjectType m_topologyType = TopologyObjectType.NO_TOPOLOGY;

        public Mesh m_mesh = null;

        // Do we need dynamic or static buffer here??
        protected List<Vector3> m_vertices = null;
        protected List<Edge> m_edges = null;
        protected List<Triangle> m_triangles = null;
        protected List<Quad> m_quads = null;
        protected List<Tetrahedron> m_tetrahedra = null;
        protected List<Hexahedron> m_hexahedron = null;

        // real buffer sent to SOFA
        protected int m_nbVertices = 0;
        public float[] m_vertexBuffer = null;


        protected int nbTriangles = 0;
        protected int[] m_trianglesBuffer;


        /// Member: if tetrahedron is detected, will gather the number of element
        protected int nbTetra = 0;
        /// Member: if tetrahedron is detected, will store the tetrahedron topology
        protected int[] m_tetraBuffer;
        /// Member: if tetrahedron is detected, will store the vertex mapping between triangulation and tetrahedron topology
        public Dictionary<int, int> mappingVertices;




        public TopologyObjectType TopologyType
        {
            get { return m_topologyType; }
        }



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

        
        public void CreateVertexBuffer(int nbVertices)
        {
            m_nbVertices = nbVertices;
            m_vertexBuffer = new float[nbVertices * 3];
        }


        //public void UpdateVelocity()

        //public void addVertex(float x, float y, float z)
        //{
            
        //}


        public void ComputeMesh()
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


        protected void ComputeMeshFromHexahedron()
        {
            Debug.LogError("SofaMeshTopology::ComputeMeshFromHexahedron() method not yet implemented!");
        }


        protected void ComputeMeshFromTetrahedron()
        {
            nbTetra = m_tetrahedra.Count;
            int[] tris = new int[nbTetra * 12];

            Vector3[] verts = new Vector3[nbTetra * 4];
            Vector3[] norms = new Vector3[nbTetra * 4];
            Vector2[] uv = new Vector2[nbTetra * 4];

            mappingVertices = new Dictionary<int, int>();

            for (int i = 0; i < nbTetra; ++i)
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
                    uv[idTet + j].x = j / 4;
                    uv[idTet + j].y = uv[i * 4 + j].x;
                }


                tris[i * 12 + 0] = id[0];
                tris[i * 12 + 1] = id[2];
                tris[i * 12 + 2] = id[1];

                tris[i * 12 + 3] = id[1];
                tris[i * 12 + 4] = id[2];
                tris[i * 12 + 5] = id[3];

                tris[i * 12 + 6] = id[2];
                tris[i * 12 + 7] = id[0];
                tris[i * 12 + 8] = id[3];

                tris[i * 12 + 9] = id[0];
                tris[i * 12 + 10] = id[1];
                tris[i * 12 + 11] = id[3];
            }

            m_mesh.vertices = verts;
            m_mesh.normals = norms;
            m_mesh.uv = uv;
            m_mesh.triangles = tris;
        }


        protected void ComputeMeshFromQuad()
        {
            Debug.LogWarning("SofaMeshTopology::ComputeMeshFromQuad() method not yet implemented!");
        }


        protected void ComputeMeshFromTriangle()
        {
            m_mesh.triangles = m_trianglesBuffer;
        }


        protected void ComputeMeshFromEdge()
        {
            Debug.LogWarning("SofaMeshTopology::ComputeMeshFromEdge() method not yet implemented!");
        }


        /// Method to compute the TetrahedronFEM topology and store it as triangle in Unity Mesh, will store the vertex mapping into @see mappingVertices
        public int[] computeForceField()
        {
            

            return null;
        }


        /// Method to update the TetrahedronFEM topology using the vertex mapping.
        public void UpdateTetraMesh()
        {
            // Compute the barycenters of each tetra and update the vertices
            Vector3[] verts = m_mesh.vertices;
            for (int i = 0; i < nbTetra; ++i)
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
