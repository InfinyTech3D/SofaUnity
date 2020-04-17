using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Class used to handle bindings to the Sofa collision pipeline components: I.e:
/// CollisionPipeline, BroadPhase, NarrowPhase and Collisionresponse
/// </summary>
public class SofaCollisionPipelineAPI : SofaBaseObjectAPI
{
    public SofaCollisionPipelineAPI(IntPtr simu, string nameID)
         : base(simu, nameID, "root", false)
    {

    }

    /// Destructor
    ~SofaCollisionPipelineAPI()
    {
        Dispose(false);
    }

    /// Implicit method to really create object and link to Sofa object. Called by SofaBaseObject constructor
    protected override bool createObject()
    {
        if (m_hasObject == false) // first time create object only
        {
            // Create the cube
            int res = sofaPhysicsAPI_addCollisionPipeline(m_simu);

            if (res != 0)
            {
                Debug.LogError("SofaCollisionPipelineAPI::createObject creation method return error: " + SofaDefines.msg_error[res] + " for object " + m_name);
                return false;
            }

            if (displayLog)
                Debug.Log("SofaCollisionPipeline Added! " + m_name);

            //// Set created object to native pointer            
            //int res1 = sofaPhysicsAPI_has3DObject(m_simu, m_name);
            //if (res == 0)
            //    m_hasObject = true;
            //else
            //{
            //    Debug.LogError("SofaBoxAPI::createObject get3DObject method returns: " + SofaDefines.msg_error[res1]);
            //    m_hasObject = false;
            //    return false;
            //}

            return true;
        }

        return false;
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    ////////////          Communication API to sofaPhysicsAdvanceAPI         ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_addCollisionPipeline(IntPtr obj);

}
