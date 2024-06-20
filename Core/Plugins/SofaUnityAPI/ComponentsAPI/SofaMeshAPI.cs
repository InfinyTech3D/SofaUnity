using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class used to handle bindings to the Sofa generic Mesh object.
/// </summary>
public class SofaMeshAPI : SofaBaseMeshAPI
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaMeshAPI(IntPtr simu, string nameID, bool isCustom = false)
        : base(simu, nameID, isCustom)
    {

    }

    /// Destructor
    ~SofaMeshAPI()
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
