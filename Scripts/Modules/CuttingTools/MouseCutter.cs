using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCutter : TriangleCutter
{

    // Use this for initialization
    void Start()
    {
        length = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        //get ray from current mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        origin = ray.origin;
        direction = ray.direction;

        castRay();

        if (Input.GetMouseButton(2))
        {
            cutTriangles();
        }

        resetCollider();
    }
}
