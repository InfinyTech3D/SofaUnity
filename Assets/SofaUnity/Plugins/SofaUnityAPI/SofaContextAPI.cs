using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace SofaUnityAPI
{
    /// <summary>
    /// Main class of the Sofa plugin. This class handle all bindings to manage the Sofa simulation scene.
    /// It will connect to the SofaPhysicsAPI. Get the list of object or start/stop the simulation.
    /// </summary>
    public class SofaContextAPI : IDisposable
    {
        /// Internal pointer to the SofaPhysicsAPI 
        internal IntPtr m_native;

        // TODO: check if needed
        bool m_isDisposed;

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaContextAPI()
        {
            // Create the application
            m_native = sofaPhysicsAPI_create();
            if (m_native == IntPtr.Zero)
                Debug.LogError("Error no sofaPhysicsAPI found and created!");

            // Create a simulation scene.
            sofaPhysicsAPI_createScene(m_native);
        }

        /// Destructor
        ~SofaContextAPI()
        {
            Dispose(false);
        }


        /// Dispose method to release the object
        public void Dispose()
        {
            Debug.Log("SofaContextAPI::Dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_native != IntPtr.Zero)
            {
                //int resDel = sofaPhysicsAPI_delete(m_native);
                //m_native = IntPtr.Zero;
                //if (resDel < 0)
                //    Debug.LogError("SofaContextAPI::Dispose sofaPhysicsAPI_delete returns: " + resDel);
            }
            //  int freeGl = sofaPhysicsAPI_freeGlutGlew(m_native);
            //Debug.Log("Glut free: " + freeGl);

            if (!m_isDisposed)
            {
                m_isDisposed = true;

                // TODO: should call delete method in SofaPAPI first

                // Free the App
                GCHandle.FromIntPtr(m_native).Free();
            }
        }

        /// Getter to the dispose parameter
        public bool isDisposed
        {
            get { return m_isDisposed; }
        }


        /// Method to start the Sofa simulation
        public void start()
        {
            sofaPhysicsAPI_start(m_native);
        }

        /// Method to stop the Sofa simulation
        public void stop()
        {
            sofaPhysicsAPI_stop(m_native);
        }

        /// Method to perform one steop the Sofa simulation
        public void step()
        {
            sofaPhysicsAPI_step(m_native);
        }

        /// Load the Sofa scene given by name @param filename
        public void loadScene(string filename)
        {
            if (m_native != IntPtr.Zero)
            {
                int res = sofaPhysicsAPI_loadScene(m_native, filename);
            }
            else
                Debug.LogError("Error can't load file: " + filename + "no sofaPhysicsAPI created!");
        }

        public void loadPlugin(string pluginName)
        {
            if (m_native != IntPtr.Zero)
            {
                int initGl = sofaPhysicsAPI_initGlutGlew(m_native);
                Debug.Log("Glut init: " + initGl);
                int res = sofaPhysicsAPI_loadPlugin(m_native, pluginName);
                Debug.Log("Plugin loaded: " + pluginName + "| res: " + res);
            }
            else
                Debug.LogError("Error can't load plugin: " + pluginName + "no sofaPhysicsAPI created!");
        }


        /// Get the Sofa simulation context
        public IntPtr getSimuContext()
        {
            return m_native;
        }

        /// Getter/Setter of timestep
        public double timeStep
        {
            get { return sofaPhysicsAPI_timeStep(m_native); }
            set { sofaPhysicsAPI_setTimeStep(m_native, value); }
        }
        
        /// Setter of gravity vector
        public void setGravity(Vector3 gravity)
        {
            double[] grav = new double[3];
            for (int i = 0; i < 3; ++i)
                grav[i] = (double)gravity[i];
            sofaPhysicsAPI_setGravity(m_native, grav);
        }
        
        /// Get the number of object in the simulation scene
        public int getNumberObjects()
        {
            int res = 0;
            if (m_native != IntPtr.Zero)
                res = sofaPhysicsAPI_getNumberObjects(m_native);

            return res;
        }

        /// Get Sofa object name (used as Id) by its position id in the creation order
        public string getObjectName(int id)
        {
            if (m_native != IntPtr.Zero)
            {
                string name = sofaPhysicsAPI_get3DObjectName(m_native, id);
                return name;
            }
            else
                return "Error";
        }

        /// Get Sofa object type by its position id in the creation order
        public string getObjectType(int id)
        {
            if (m_native != IntPtr.Zero)
            {
                string type = sofaPhysicsAPI_get3DObjectType(m_native, id);
                return type;
            }
            else
                return "Error";
        }


        /// Bindings to the SofaAdvancePhysicsAPI methods
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern IntPtr sofaPhysicsAPI_create();
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_createScene(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_delete(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_initGlutGlew(IntPtr obj);

      //  [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
      //  public static extern int sofaPhysicsAPI_freeGlutGlew(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNumberMeshes(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectType(IntPtr obj, int id);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_start(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_step(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_stop(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_reset(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_setTimeStep(IntPtr obj, double value);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern double sofaPhysicsAPI_timeStep(IntPtr obj);


        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_setGravity(IntPtr obj, double[] values);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_gravity(IntPtr obj, double[] values);
        


        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void sofaPhysicsAPI_setTestName(IntPtr obj, string name);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_testName(IntPtr obj);

    }
}
