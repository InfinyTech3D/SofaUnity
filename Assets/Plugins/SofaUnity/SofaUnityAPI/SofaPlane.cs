using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaPlane : SofaMeshObject
{
    public SofaPlane(IntPtr simu, int idObject, bool isRigid)
        : base(simu, idObject, isRigid)
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
