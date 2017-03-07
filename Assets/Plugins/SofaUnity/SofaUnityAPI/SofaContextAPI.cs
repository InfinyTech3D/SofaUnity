using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace SofaUnityAPI
{

    public class SofaContextAPI : IDisposable
    {
        internal IntPtr m_native;
        bool m_isDisposed;

        public SofaContextAPI()
        {
            m_native = sofaPhysicsAPI_create();
            if (m_native == IntPtr.Zero)
                Debug.LogError("Error not sofaPhysicsAPI created!");

            sofaPhysicsAPI_createScene(m_native);

            string name = sofaPhysicsAPI_APIName(m_native);
            //Debug.Log("API Name: " + name);

            Debug.Log("API TEST Name: " + sofaPhysicsAPI_testName(m_native));
            sofaPhysicsAPI_setTestName(m_native, "PROUT");
            Debug.Log("API TEST Name after: " + sofaPhysicsAPI_testName(m_native));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                m_isDisposed = true;

                //if (!_preventDelete)
                //{
                //    IntPtr userPtr = btCollisionShape_getUserPointer(_native);
                //    GCHandle.FromIntPtr(userPtr).Free();

                //    btCollisionShape_delete(_native);
                //}
            }
        }

        ~SofaContextAPI()
        {
            Dispose(false);
        }

        public bool isDisposed
        {
            get { return m_isDisposed; }
        }

        public void start()
        {
        //    Debug.Log("-- sofaPhysicsAPI_start called.");
            sofaPhysicsAPI_start(m_native);
        }

        public void stop()
        {
            //Debug.Log("-- sofaPhysicsAPI_stop called.");
            sofaPhysicsAPI_stop(m_native);
        }

        public void step()
        {
            Debug.Log("-- sofaPhysicsAPI_step called.");
            sofaPhysicsAPI_step(m_native);
        }

        public double timeStep()
        {
            return sofaPhysicsAPI_timeStep(m_native);
        }

        public void setTimeStep(double value)
        {
            sofaPhysicsAPI_setTimeStep(m_native, value);
        }

        public IntPtr getSimuContext()
        {
            return m_native;
        }


        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern IntPtr sofaPhysicsAPI_create();
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_start(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_step(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_stop(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_setTimeStep(IntPtr obj, double value);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern double sofaPhysicsAPI_timeStep(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
//        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void sofaPhysicsAPI_setTestName(IntPtr obj, string name);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_testName(IntPtr obj);

    }
}
