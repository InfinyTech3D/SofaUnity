using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnity
{

    public class SofaUnityRenderer
    {
        /// Internal pointer to the SofaPhysicsAPI 
        internal IntPtr m_native;

        private bool m_isReady = false;

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaUnityRenderer()
        {
            int test = test_getAPI_ID();
            Debug.Log("test_getAPI_ID" + test);
            m_native = sofaPhysicsAPI_create();

            if (m_native == IntPtr.Zero)
            {
                Debug.LogError("Error no sofaPhysicsAPI found nor created!");
                m_isReady = false;
                return;
            }



            // Create a simulation scene.
            int res = sofaPhysicsAPI_createScene(m_native);
            if (res == 0)
            {
                m_isReady = true;
            }
            else
            {
                Debug.LogError("SofaContextAPI scene creation return: " + res);
                m_isReady = false;
            }
        }

        /// Destructor
        ~SofaUnityRenderer()
        {
            // Dispose(false);
            Dispose();
        }


        /// Dispose method to release the object
        public void Dispose()
        {
            if (m_native != IntPtr.Zero)
            {
                stop();

                unload();

                int resDel = sofaPhysicsAPI_delete(m_native);
                m_native = IntPtr.Zero;
                if (resDel < 0)
                    Debug.LogError("SofaUnityRenderer::Dispose sofaPhysicsAPI_delete method returns: " + resDel);
                m_isReady = false;
            }
        }


        /// Method to start the Sofa simulation
        public void start()
        {
            if (m_isReady)
                sofaPhysicsAPI_start(m_native);
        }

        /// Method to perform one step of simulation in Sofa
        public void step()
        {
            if (m_isReady)
                sofaPhysicsAPI_step(m_native);
        }

        /// Method to stop the Sofa simulation
        public void stop()
        {
            if (m_isReady)
                sofaPhysicsAPI_stop(m_native);
        }

        /// Method to reset the Sofa simulation
        public void reset()
        {
            if (m_isReady)
                sofaPhysicsAPI_reset(m_native);
        }


        /// <summary> Load the Sofa scene given by name @param filename. </summary>
        /// <param name="filename"> Path to the filename. </param>
        public void loadScene(string filename)
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_loadScene(m_native, filename);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::loadScene method returns: " + res + " for scene: " + filename);
            }
            else
                Debug.LogError("SofaContextAPI::loadScene can't load file: " + filename + " no sofaPhysicsAPI created!");
        }

        /// Method to stop the Sofa simulation
        public void unload()
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_unloadScene(m_native);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::unload method returns: " + res);
            }
            else
            {
                Debug.LogError("SofaContextAPI::unload scene file not possible without a valid sofaPhysicsAPI created!");
            }
        }



        /// Getter/Setter of timestep
        public float timeStep
        {
            get
            {
                if (m_isReady)
                    return (float)sofaPhysicsAPI_timeStep(m_native);
                else
                    return 0.01f;
            }
            set
            {
                if (m_isReady)
                    sofaPhysicsAPI_setTimeStep(m_native, value);
            }
        }


        public float GetTime()
        {
            if (m_isReady)
                return sofaPhysicsAPI_time(m_native);
            else
                return 0.0f;
        }


        /// Setter of gravity vector
        public void setGravity(Vector3 gravity)
        {
            double[] grav = new double[3];
            for (int i = 0; i < 3; ++i)
                grav[i] = (double)gravity[i];

            if (m_isReady)
                sofaPhysicsAPI_setGravity(m_native, grav);
        }

        public Vector3 getGravity()
        {
            Vector3 gravity = new Vector3(0.0f, 0.0f, 0.0f);
            if (m_isReady)
            {
                double[] grav = new double[3];
                int res = sofaPhysicsAPI_getGravity(m_native, grav);

                if (res == 0)
                {
                    for (int i = 0; i < 3; ++i)
                        gravity[i] = (float)grav[i];
                }
                else
                    Debug.LogError("SofaContextAPI::getGravity method returns: " + res);
            }
            else
                Debug.LogError("SofaContextAPI::getGravity can't access Object Pointer m_native.");

            return gravity;
        }


        ///////////////////////////////////////////////////////////
        //////////          API Communication         /////////////
        ///////////////////////////////////////////////////////////
        [DllImport("SofaPhysicsAPI")]
        public static extern int test_getAPI_ID();

        [DllImport("SofaPhysicsAPI")]
        public static extern IntPtr sofaPhysicsAPI_create();
    
        [DllImport("SofaPhysicsAPI")]
        public static extern int sofaPhysicsAPI_delete(IntPtr obj);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_APIName(IntPtr obj);


        // API for scene creation/loading
        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_unloadScene(IntPtr obj);


        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_loadSofaIni(IntPtr obj, string pathIni);



        // API for animation loop
        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_start(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_stop(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_step(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_reset(IntPtr ptr);


        [DllImport("SofaPhysicsAPI")]
        public static extern float sofaPhysicsAPI_time(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern float sofaPhysicsAPI_timeStep(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_setTimeStep(IntPtr ptr, float value);


        [DllImport("SofaPhysicsAPI")]
        public static extern int sofaPhysicsAPI_getGravity(IntPtr ptr, double[] values);

        [DllImport("SofaPhysicsAPI")]
        public static extern int sofaPhysicsAPI_setGravity(IntPtr ptr, double[] values);

    }
}