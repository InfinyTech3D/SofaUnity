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
        m_name = "plane_" + m_idObject + "_node";

        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the plane
            int res = sofaPhysicsAPI_addPlane(m_simu, "plane_" + m_idObject, m_isRigid);

            if (res == 1) // plane added
            {
                Debug.Log("plane Added! " + m_name);

                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            //    m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1");

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error plane created can't be found!");
        }
    }

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addPlane(IntPtr obj, string name, bool isRigid);

}
