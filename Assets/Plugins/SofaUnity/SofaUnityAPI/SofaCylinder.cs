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
        m_name = "cylinder_" + m_idObject + "_node";

        if (m_native == IntPtr.Zero) // first time create object only
        {
            // Create the cylinder
            int res = sofaPhysicsAPI_addCylinder(m_simu, "cylinder_" + m_idObject, false);
            if (res == 1) // cylinder added
            {
                Debug.Log("cylinder Added! " + m_name);

                // Set created object to native pointer
                m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);
            }

            //    m_native = sofaPhysicsAPI_get3DObject(m_simu, "truc1");

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error cylinder created can't be found!");
        }
    }

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addCylinder(IntPtr obj, string name, bool isRigid);
}