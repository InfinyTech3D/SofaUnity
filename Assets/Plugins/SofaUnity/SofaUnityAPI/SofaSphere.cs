using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaSphere : SofaMeshObject
{
    public SofaSphere(IntPtr simu, int idObject, bool isRigid)
        : base(simu, idObject, isRigid)
    {

    }

    ~SofaSphere()
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
