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
           // Dispose(false);
        }


        /// Dispose method to release the object
        public void Dispose()
        {
            if (m_native != IntPtr.Zero)
            {
                int resDel = sofaPhysicsAPI_delete(m_native);
                m_native = IntPtr.Zero;
                if (false)
                    Debug.Log("Error: SofaContextAPI::Dispose sofaPhysicsAPI_delete method returns: " + resDel);
                if (resDel < 0)
                    Debug.LogError("Error: SofaContextAPI::Dispose sofaPhysicsAPI_delete method returns: " + resDel);
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

        /// Method to perform one step of simulation in Sofa
        public void step()
        {
            sofaPhysicsAPI_step(m_native);
        }

        /// <summary> Load the Sofa scene given by name @param filename. </summary>
        /// <param name="filename"> Path to the filename. </param>
        public void loadScene(string filename)
        {
            if (m_native != IntPtr.Zero)
            {
                int res = sofaPhysicsAPI_loadScene(m_native, filename);
            }
            else
                Debug.LogError("Error can't load file: " + filename + "no sofaPhysicsAPI created!");
        }

        /// <summary> Method to load a specific sofa plugin. </summary>
        /// <param name="pluginName"> Path to the plugin. </param>
        public void loadPlugin(string pluginName)
        {
            if (m_native != IntPtr.Zero)
            {
                int res = sofaPhysicsAPI_loadPlugin(m_native, pluginName);
                if (res < 0)
                    Debug.LogError("Plugin loaded: " + pluginName + " method returns error: " + res);
            }
            else
                Debug.LogError("Error: can't load plugin: " + pluginName + "no sofaPhysicsAPI created!");
        }


        /// Method to request load of glut and glew into Sofa software.
        public void initGlutGlew()
        {
            if (m_native != IntPtr.Zero)
            {
                int initGl = sofaPhysicsAPI_initGlutGlew(m_native);
                if (initGl < 0)
                    Debug.LogError("Error: loading glut/glew in Sofa. Method returns error: " + initGl);
            }
            else
                Debug.LogError("Error can't load glut/glew no sofaPhysicsAPI created!");
        }


        /// Method to request load of glut and glew into Sofa software.
        public void freeGlutGlew()
        {
            if (m_native != IntPtr.Zero)
            {
                int freeGl = sofaPhysicsAPI_freeGlutGlew(m_native);
                if (freeGl < 0)
                    Debug.LogError("Error: free glut/glew in Sofa. Method returns error: " + freeGl);
            }
            else
                Debug.LogError("Error can't free glut/glew no sofaPhysicsAPI found!");
        }

        
        /// Get the Sofa simulation context
        public IntPtr getSimuContext()
        {
            return m_native;
        }

        /// Getter/Setter of timestep
        public float timeStep
        {
            get { return (float)sofaPhysicsAPI_timeStep(m_native); }
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

        public Vector3 getGravity()
        {
            Vector3 gravity = new Vector3(0.0f, 0.0f, 0.0f);
            if (m_native != IntPtr.Zero)
            {
                double[] grav = new double[3];
                int res = sofaPhysicsAPI_gravity(m_native, grav);

                if (res >= 0)
                {
                    for (int i = 0; i < 3; ++i)
                        gravity[i] = (float)grav[i];
                }
                else
                    Debug.LogError("Error in method getGravity(). Method Return: " + res);
            }
            else
                Debug.LogError("Error in method getGravity(). Can't access Object Pointer m_native.");
            
            return gravity;
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


        /////////////////////////////////////////////////////////////////////////////////////////
        //////////          API to Communication with SofaAdvancePhysicsAPI         /////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        /// Bindings to the SofaAdvancePhysicsAPI creation/destruction methods
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern IntPtr sofaPhysicsAPI_create();

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_delete(IntPtr obj);


        /// Bindings to create or load an existing simulation scene
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern void sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);


        /// Binding to load a plugin
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);


        /// Bindings to hangle glew creation/destruction
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_initGlutGlew(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_freeGlutGlew(IntPtr obj);


        /// Bindings to communicate with the simulation tree
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNumberMeshes(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectType(IntPtr obj, int id);


        /// Bindings to communicate with the simulation loop
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
        

        /// Bindings tests
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_APIName(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void sofaPhysicsAPI_setTestName(IntPtr obj, string name);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_testName(IntPtr obj);

    }
}
