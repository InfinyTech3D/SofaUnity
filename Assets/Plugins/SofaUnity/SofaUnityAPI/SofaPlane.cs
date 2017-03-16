using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaPlane : SofaMeshObject
{
    public SofaPlane(IntPtr simu, int idObject)
        : base(simu, idObject)
    {

    }

    ~SofaPlane()
    {
        Dispose(false);
    }

    public override int[] createTriangulation()
    {
        return base.createTriangulation();
    }

    protected override void createObject()
    {

    }
}
