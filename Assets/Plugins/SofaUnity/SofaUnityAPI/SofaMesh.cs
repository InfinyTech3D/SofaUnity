using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaMesh : SofaBaseMesh
{
    public SofaMesh(IntPtr simu, string nameID, bool isRigid)
        : base(simu, nameID, isRigid)
    {

    }

    ~SofaMesh()
    {
        Dispose(false);
    }

    public override int[] createTriangulation()
    {
        //return new int[0];
        return base.createTriangulation();
    }

    

}
