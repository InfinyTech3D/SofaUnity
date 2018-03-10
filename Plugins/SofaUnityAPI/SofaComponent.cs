using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

public class SofaComponent : SofaBaseObject
{
    /// <summary>
    /// Default constructor to create a Sofa Mesh
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaComponent(IntPtr simu, string nameID, bool isRigid)
        : base(simu, nameID, isRigid)
    {

    }


    /// Implicit method load the object from the Sofa side.
    public override void loadObject()
    {
        //if (m_native != IntPtr.Zero)        
        if (m_native == IntPtr.Zero) // first time create object only
        {
            m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Component can't be found: " + m_name);
            else
            {
                //Debug.Log("Load Node Name: " + m_name);
                m_parent = sofaPhysicsAPI_getParentNodeName(m_simu, m_name);
            }

            
        }
    }

    public string loadAllData()
    {
        if (m_native != IntPtr.Zero)
            return sofaPhysics3DObject_getDataFields(m_simu, m_name);
        else
            return "None";
    }



    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysics3DObject_getDataFields(IntPtr obj, string name);
}

