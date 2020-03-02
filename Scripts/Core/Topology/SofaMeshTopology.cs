using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMeshTopology
    {

        protected TopologyObjectType m_topologyType;

        public Mesh m_mesh = null;

        // Do we need dynamic or static buffer here??
        protected List<Vector3> m_vertices = null;
        protected List<Edge> m_edges = null;
        protected List<Triangle> m_triangles = null;
        protected List<Quad> m_quads = null;
        protected List<Tetrahedron> m_tetrahedra = null;
        protected List<Hexahedron> m_hexahedron = null;

        public float[] m_vertexBuffer = null;



        /// Member: if tetrahedron is detected, will gather the number of element
        protected int nbTetra = 0;
        /// Member: if tetrahedron is detected, will store the tetrahedron topology
        protected int[] m_tetra;
        /// Member: if tetrahedron is detected, will store the vertex mapping between triangulation and tetrahedron topology
        protected Dictionary<int, int> mappingVertices;



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

            for (int i = 0; i < nbElem; ++i)
            {
                Tetrahedron tetra = new Tetrahedron(elems[i * 4], elems[i * 4 + 1], elems[i * 4 + 2], elems[i * 4 + 3]);
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

            for (int i = 0; i < nbElem; ++i)
            {
                Triangle tri = new Triangle(elems[i * 3], elems[i * 3 + 1], elems[i * 3 + 2]);

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


        public void CreateVertexBuffer()
        {
            m_vertices = new List<Vector3>();
        }

        public void addVertex(float x, float y, float z)
        {
            
        }


        /// Method to compute the TetrahedronFEM topology and store it as triangle in Unity Mesh, will store the vertex mapping into @see mappingVertices
        public int[] computeForceField()
        {
            //int[] tris = new int[nbTetra * 12];
            //Vector3[] verts = new Vector3[nbTetra * 4];//m_mesh.vertices;
            //Vector3[] norms = new Vector3[nbTetra * 4];//m_mesh.normals;
            //Vector2[] uv = new Vector2[nbTetra * 4];
            //mappingVertices = new Dictionary<int, int>();
            //nbVert = m_mesh.vertices.Length;

            //for (int i = 0; i < nbTetra; ++i)
            //{
            //    int[] id = new int[4];
            //    int[] old_id = new int[4];

            //    int idTet = i * 4;
            //    for (int j = 0; j < 4; ++j)
            //    {
            //        id[j] = idTet + j;
            //        old_id[j] = m_tetra[idTet + j];

            //        verts[id[j]] = m_mesh.vertices[old_id[j]];
            //        norms[id[j]] = m_mesh.normals[old_id[j]];
            //        mappingVertices.Add(id[j], old_id[j]);

            //        m_tetra[idTet + j] = id[j];
            //        uv[idTet + j].x = j / 4;
            //        uv[idTet + j].y = uv[i * 4 + j].x;
            //    }


            //    tris[i * 12 + 0] = id[0];
            //    tris[i * 12 + 1] = id[2];
            //    tris[i * 12 + 2] = id[1];

            //    tris[i * 12 + 3] = id[1];
            //    tris[i * 12 + 4] = id[2];
            //    tris[i * 12 + 5] = id[3];

            //    tris[i * 12 + 6] = id[2];
            //    tris[i * 12 + 7] = id[0];
            //    tris[i * 12 + 8] = id[3];

            //    tris[i * 12 + 9] = id[0];
            //    tris[i * 12 + 10] = id[1];
            //    tris[i * 12 + 11] = id[3];
            //}

            //m_mesh.vertices = verts;
            //m_mesh.normals = norms;
            //m_mesh.uv = uv;

            return null;
        }


        /// Method to update the TetrahedronFEM topology using the vertex mapping.
        //public void updateTetraMesh()
        //{
        //    // first update the vertices dissociated
        //    m_sofaMeshAPI.updateMeshTetra(m_mesh, mappingVertices);

        //    // Compute the barycenters of each tetra and update the vertices
        //    Vector3[] verts = m_mesh.vertices;
        //    for (int i = 0; i < nbTetra; ++i)
        //    {
        //        Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);
        //        int idI = i * 4;
        //        // compute tetra barycenter
        //        for (int j = 0; j < 4; ++j)
        //            bary += verts[m_tetra[idI + j]];
        //        bary /= 4;

        //        // reduce the tetra size according to the barycenter
        //        for (int j = 0; j < 4; ++j)
        //            verts[m_tetra[idI + j]] = bary + (verts[m_tetra[idI + j]] - bary) * 0.5f;
        //    }

        //    m_mesh.vertices = verts;
        //}
    }
}
