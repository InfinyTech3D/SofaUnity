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
    }
}
