using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace SofaUnityAPI
{

    public class SofaContextAPI : IDisposable
    {
        internal IntPtr m_impl;
        bool m_isDisposed;

        SofaContextAPI()
        {
            m_impl = sofaPhysicsAPI_create();
            if (m_impl == null)
                Debug.LogError("Error not sofaPhysicsAPI created!");

            sofaPhysicsAPI_createScene(m_impl);
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

        public void animate()
        {
            sofaPhysicsAPI_start(m_impl);
        }

        public void stop()
        {
            sofaPhysicsAPI_stop(m_impl);
        }

        public void step()
        {
            sofaPhysicsAPI_step(m_impl);
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
    }
}
