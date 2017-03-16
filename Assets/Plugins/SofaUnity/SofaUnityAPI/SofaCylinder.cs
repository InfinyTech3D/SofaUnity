using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaCylinder : SofaMeshObject
{
    public SofaCylinder(IntPtr simu, int idObject)
        : base(simu, idObject)
    {

    }

    ~SofaCylinder()
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