using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{

    public enum TopologyObjectType
    {
        POINT,
        EDGE,
        TRIANGLE,
        QUAD,
        TETRAHEDRON,
        HEXAHEDRON
    };

    //TODO: find how to use template in C# .... :(
    public class Edge
    {
        public int a, b;
        public Edge(int _a, int _b)
        {
            a = _a;
            b = _b;
        }
    }

    public class Triangle
    {
        public int a, b, c;
        public Triangle(int _a, int _b, int _c)
        {
            a = _a;
            b = _b;
            c = _c;
        }
    }

    public class Quad
    {
        public int a, b, c, d;
        public Quad(int _a, int _b, int _c, int _d)
        {
            a = _a;
            b = _b;
            c = _c;
            d = _d;
        }
    }

    public class Tetrahedron
    {
        public int a, b, c, d;
        public Tetrahedron(int _a, int _b, int _c, int _d)
        {
            a = _a;
            b = _b;
            c = _c;
            d = _d;
        }
    }

    public class Hexahedron
    {
        public int a, b, c, d, e, f, g, h;
        public Hexahedron(int _a, int _b, int _c, int _d, int _e, int _f, int _g, int _h)
        {
            a = _a;
            b = _b;
            c = _c;
            d = _d;
            e = _e;
            f = _f;
            g = _g;
            h = _h;
        }
    }

    
}