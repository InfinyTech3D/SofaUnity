using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SBaseGrid : SBaseObject
{

    //public struct IntVector3
    //{
    //    public IntVector3(x, y, z)
    //    {

    //    }
    //    int m_x;
    //    int m_y;
    //    int m_z;
    //}


    Vector3 m_gridSize = new Vector3(10.0f, 10.0f, 10.0f);
    public Vector3 gridSize
    {
        get { return m_gridSize; }
        set { m_gridSize = value; }
    }

    float m_mass = 1.0f;
    public float mass
    {
        get { return m_mass; }
        set { m_mass = value; }
    }

    float m_young = 300.0f;
    public float young
    {
        get { return m_young; }
        set { m_young = value; }
    }

    float m_poisson = 1.0f;
    public float poisson
    {
        get { return m_poisson; }
        set { m_poisson = value; }
    }
}
