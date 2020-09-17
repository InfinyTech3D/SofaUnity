using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SofaComponent class that will be linked to a SofacomponentListener Object which allow to listen to all Data of a target component.
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaComponentObjectAPI : SofaBaseObjectAPI
{
    /// <summary> Default constructor to create a  </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaComponentObjectAPI(IntPtr simu, string nameID, string parentName, bool isRigid)
        : base(simu, nameID, parentName, isRigid)
    {

    }


    /// Implicit method load the object from the Sofa side.
    public override void loadObject()
    {
        //if (m_native != IntPtr.Zero)        
        if (m_hasObject == false) // first time create object only
        {
            //int res1 = sofaPhysicsAPI_has3DObject(m_simu, m_name);
            int res1 = -1;
            if (res1 == 0)
                m_hasObject = true;
            else
                Debug.LogError("SofaComponent::loadObject get3DObject method returns: " + SofaDefines.msg_error[res1]);           
        }
    }


    /// Method to get all data listen by this component as a json unique string.
    public string loadAllData()
    {
        if (m_hasObject)
            return sofaPhysics3DObject_getDataFields(m_simu, m_name);
        else
            return "None";
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern string sofaPhysics3DObject_getDataFields(IntPtr obj, string name);
}

