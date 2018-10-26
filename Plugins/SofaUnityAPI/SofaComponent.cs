using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SofaComponent class that will be linked to a SofacomponentListener Object which allow to listen to all Data of a target component.
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaComponent : SofaBaseObject
{
    /// <summary> Default constructor to create a  </summary>
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
            int[] res1 = new int[1];
            m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name, res1);

            if (res1[0] != 0)
                Debug.LogError("SofaComponent::loadObject get3DObject method returns: " + SofaDefines.msg_error[res1[0]]);


            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Component can't be found: " + m_name);
            else
            {
                //Debug.Log("Load Node Name: " + m_name);
                m_parent = sofaPhysicsAPI_getParentNodeName(m_simu, m_name);
            }

            
        }
    }


    /// Method to get all data listen by this component as a json unique string.
    public string loadAllData()
    {
        if (m_native != IntPtr.Zero)
            return sofaPhysics3DObject_getDataFields(m_simu, m_name);
        else
            return "None";
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysics3DObject_getDataFields(IntPtr obj, string name);
}

