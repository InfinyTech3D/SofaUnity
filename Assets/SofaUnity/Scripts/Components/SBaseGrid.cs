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

    

    
}
