using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCutter : TriangleCutter
{

    // Use this for initialization
    void Start()
    {
        m_length = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        //get ray from current mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        m_origin = ray.origin;
        m_direction = ray.direction;

        CastRay();

        if (Input.GetMouseButton(2))
        {
            cutTriangles();
        }

        resetCollider();
    }
}
