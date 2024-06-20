using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Set of classes to define the topology elements in SofaUnity. WIP!!
/// </summary>
namespace SofaUnity
{
    /// Enum to store the type of topology
    public enum TopologyObjectType
    {
        POINT,
        EDGE,
        TRIANGLE,
        QUAD,
        TETRAHEDRON,
        HEXAHEDRON,
        NO_TOPOLOGY
    };


    public class TopologyElement
    {
        protected int[] m_indices;
        protected int m_size;

        public int GetValue(int index) => m_indices[index];
        public int SetValue(int index, int value) => m_indices[index] = value;

        public int this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }       
    }


    /// Class to define the Edge structure
    public class Edge : TopologyElement
    {
        public Edge(int _a, int _b)
        {
            m_size = 2;
            m_indices = new int[2];
            m_indices[0] = _a;
            m_indices[1] = _b;
        }

        public string toStr()
        {
            return "[" + m_indices[0] + ", " + m_indices[1] + "]";
        }
    }


    /// Class to define the Triangle structure
    public class Triangle : TopologyElement
    {
        public Triangle(int _a, int _b, int _c)
        {
            m_size = 3;
            m_indices = new int[3];
            m_indices[0] = _a;
            m_indices[1] = _b;
            m_indices[2] = _c;
        }

        public string toStr()
        {
            return "[" + m_indices[0] + ", " + m_indices[1] + ", " + m_indices[2] + "]";
        }
    }


    /// Class to define the Quad structure
    public class Quad : TopologyElement
    {
        public Quad(int _a, int _b, int _c, int _d)
        {
            m_size = 4;
            m_indices = new int[4];
            m_indices[0] = _a;
            m_indices[1] = _b;
            m_indices[2] = _c;
            m_indices[3] = _d;
        }

        public string toStr()
        {
            return "[" + m_indices[0] + ", " + m_indices[1] + ", " + m_indices[2] + ", " + m_indices[3] + "]";
        }
    }


    /// Class to define the Tetrahedron structure
    public class Tetrahedron : TopologyElement
    {
        public Tetrahedron(int _a, int _b, int _c, int _d)
        {
            m_size = 4;
            m_indices = new int[4];
            m_indices[0] = _a;
            m_indices[1] = _b;
            m_indices[2] = _c;
            m_indices[3] = _d;
        }

        public string toStr()
        {
            return "[" + m_indices[0] + ", " + m_indices[1] + ", " + m_indices[2] + ", " + m_indices[3] + "]";
        }
    }


    /// Class to define the Hexahedron structure
    public class Hexahedron : TopologyElement
    {
        public Hexahedron(int _a, int _b, int _c, int _d, int _e, int _f, int _g, int _h)
        {
            m_size = 8;
            m_indices = new int[8];
            m_indices[0] = _a;
            m_indices[1] = _b;
            m_indices[2] = _c;
            m_indices[3] = _d;

            m_indices[4] = _e;
            m_indices[5] = _f;
            m_indices[6] = _g;
            m_indices[7] = _h;
        }

        public string toStr()
        {
            return "[" + m_indices[0] + ", " + m_indices[1] + ", " + m_indices[2] + ", " + m_indices[3] 
                + ", " + m_indices[4] + ", " + m_indices[5] + ", " + m_indices[6] + ", " + m_indices[7] + "]";
        }
    }
}