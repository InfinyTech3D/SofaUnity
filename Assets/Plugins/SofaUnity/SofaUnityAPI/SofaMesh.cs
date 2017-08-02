using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class used to handle bindings to the Sofa generic Mesh object.
/// </summary>
public class SofaMesh : SofaBaseMesh
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaMesh(IntPtr simu, string nameID, bool isRigid)
        : base(simu, nameID, isRigid)
    {

    }

    /// Destructor
    ~SofaMesh()
    {
        Dispose(false);
    }

    /// Method to create the triangulation from Sofa topology to Unity buffers
    public override int[] createTriangulation()
    {
        //return new int[0];
        return base.createTriangulation();
    }
       

}
