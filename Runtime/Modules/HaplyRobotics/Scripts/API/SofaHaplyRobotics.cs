using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnityAPI
{
    public class SofaHaplyRobotics : IDisposable
    {
        /// Name of the Sofa object mapped to this Object.
        protected string m_name;

        /// Pointer to the SofaPhysicsAPI 
        protected IntPtr m_simu = IntPtr.Zero;

        /// Parameter to activate internal logging
        protected bool log = true;
        protected bool m_isReady = false;
        public bool IsReady() { return m_isReady; }

        // TODO: check if needed
        bool m_isDisposed;

        /// Memory free method
        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

        /// Memory free method
        protected virtual void Dispose(bool disposing)
        {
            //if (!m_isDisposed)
            //{
            //    m_isDisposed = true;
            //}
        }

        public SofaHaplyRobotics(IntPtr simu, string deviceNameID)
        {
            m_simu = simu;
            m_name = deviceNameID;

            SofaDefines.msg_error[-730] = "SOFA HaplyRobotics Plugin has not been activated.";
            SofaDefines.msg_error[-731] = "SOFA HaplyRobotics manager creation failed.";
            SofaDefines.msg_error[-732] = "No SOFA HaplyRobotics Drivers found in the scene.";
            SofaDefines.msg_error[-733] = "SOFA HaplyRobotics Driver object is invalid.";
            SofaDefines.msg_error[-734] = "No SOFA HaplyRobotics Driver found with this name in the scene.";
            SofaDefines.msg_error[-735] = "SOFA HaplyRobotics Driver not registered in HaplyRobotics manager.";
            SofaDefines.msg_error[-738] = "SOFA HaplyRobotics manager can't access to simulation thread.";

            int res = -1;
            if (m_simu != IntPtr.Zero)
            {
                res = sofaPhysicsAPI_createInverse3Manager(m_simu, m_name);
                if (res != 0)
                {
                    Debug.LogError("SofaHaplyRobotics creation: " + m_name + " returns error: " + SofaDefines.msg_error[res]);
                    m_isReady = false;
                    return;
                }

            }
            else
                Debug.LogError("SofaHaplyRobotics creation: " + deviceNameID + " failed. Can't access Object Pointer simulation.");

            Debug.Log("SofaHaplyRobotics Creation returns: " + SofaDefines.msg_error[res]);
            m_isReady = true;
        }

        public int Inverse3_getPosition(float[] val)
        {
            int res = sofaPhysicsAPI_getInverse3Position(m_simu, m_name, val);
            return res;
        }

        public int Inverse3_getButtonStatus(int[] val)
        {
            int res = sofaPhysicsAPI_getInverse3ButtonStatus(m_simu, m_name, val);
            return res;
        }

        public int Inverse3_getStatus(int[] val)
        {
            int res = sofaPhysicsAPI_getInverse3Status(m_simu, m_name, val);
            return res;
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        ///////////      Communication API to set/get basic values into Sofa     ////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        [DllImport("SofaVerseAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createInverse3Manager(IntPtr obj, string deviceNameID);

        [DllImport("SofaVerseAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getInverse3Position(IntPtr obj, string deviceNameID, float[] values);

        [DllImport("SofaVerseAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getInverse3Status(IntPtr obj, string deviceNameID, int[] value);

        [DllImport("SofaVerseAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getInverse3ButtonStatus(IntPtr obj, string deviceNameID, int[] value);

    }
}
