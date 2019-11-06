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

        private bool m_isReady = false;

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaContextAPI(bool async)
        {
            // Create the application
            if (async)
                m_native = sofaPhysicsAPI_create(2);
            else
                m_native = sofaPhysicsAPI_create(1);

            if (m_native == IntPtr.Zero)
            {
                Debug.LogError("Error no sofaAdvancePhysicsAPI found nor created!");
                m_isReady = false;
                return;
            }

            // Create a simulation scene.
            int res = sofaPhysicsAPI_createScene(m_native);
            if (res == 0)
            {
                m_isReady = true;

                // load the sofaIni file
                string pathIni = Application.dataPath + "/SofaUnity/Plugins/Native/x64/sofa.ini";
                string sharePath = sofaPhysicsAPI_loadSofaIni(m_native, pathIni);
                //Debug.Log("sharePath: " + sharePath);
            }
            else
                Debug.LogError("SofaContextAPI scene creation return: " + SofaDefines.msg_error[res]);
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
                    Debug.Log("SofaContextAPI::Dispose sofaPhysicsAPI_delete method returns: " + SofaDefines.msg_error[resDel]);
                if (resDel < 0)
                    Debug.LogError("SofaContextAPI::Dispose sofaPhysicsAPI_delete method returns: " + SofaDefines.msg_error[resDel]);
                m_isReady = false;
            }
        }

        /// Getter to the dispose parameter
        public bool isDisposed
        {
            get { return m_isDisposed; }
        }

        public int contextStatus()
        {
            if (m_isReady)
                return sofaPhysicsAPI_getStateMachine(m_native);
            else
                return -2;
        }

        /// Method to start the Sofa simulation
        public void start()
        {
            if (m_isReady)
                sofaPhysicsAPI_start(m_native);
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

        /// Method to perform one step of simulation in Sofa
        public void step()
        {
            if (m_isReady)
                sofaPhysicsAPI_step(m_native);
        }

        /// Method to perform one async step of simulation in Sofa
        public bool asyncStep()
        {
            if (m_isReady)
                return sofaPhysicsAPI_asyncStep(m_native);
            else
                return false;
        }

        /// Method to query the Sofa physics simulation for the asynch step completion
        public bool isAsyncStepCompleted()
        {
            if (m_isReady)
                return sofaPhysicsAPI_isAsyncStepCompleted(m_native);
            else
                return false;
        }        

        /// <summary> Load the Sofa scene given by name @param filename. </summary>
        /// <param name="filename"> Path to the filename. </param>
        public void loadScene(string filename)
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_loadScene(m_native, filename);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::loadScene method returns: " + SofaDefines.msg_error[res]);
            }
            else
                Debug.LogError("SofaContextAPI::loadScene can't load file: " + filename + "no sofaPhysicsAPI created!");
        }

        /// Method to stop the Sofa simulation
        public void unload()
        {
            if (m_isReady)
                sofaPhysicsAPI_unloadScene(m_native);
        }        

        /// <summary> Method to load a specific sofa plugin. </summary>
        /// <param name="pluginName"> Path to the plugin. </param>
        public void loadPlugin(string pluginName)
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_loadPlugin(m_native, pluginName);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::loadPlugin: " + pluginName + ", method returns: " + SofaDefines.msg_error[res]);
            }
            else
                Debug.LogError("SofaContextAPI::loadPlugin can't load plugin: " + pluginName + " no sofaPhysicsAPI created!");
        }


        /// Method to request load of glut and glew into Sofa software.
        public void initGlutGlew()
        {
            if (m_native != IntPtr.Zero)
            {
                int initGl = sofaPhysicsAPI_initGlutGlew(m_native);
                if (initGl != 0)
                    Debug.LogError("Error: loading glut/glew in Sofa. Method returns error: " + SofaDefines.msg_error[initGl]);
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
                    Debug.LogError("Error: free glut/glew in Sofa. Method returns error: " + SofaDefines.msg_error[freeGl]);
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
            get {
                if (m_isReady)
                    return (float)sofaPhysicsAPI_timeStep(m_native);
                else
                    return 0.01f;
            }
            set {
                if (m_isReady)
                    sofaPhysicsAPI_setTimeStep(m_native, value);                
            }
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
                int res = sofaPhysicsAPI_gravity(m_native, grav);

                if (res == 0)
                {
                    for (int i = 0; i < 3; ++i)
                        gravity[i] = (float)grav[i];
                }
                else
                    Debug.LogError("SofaContextAPI::getGravity method returns: " + SofaDefines.msg_error[res]);
            }
            else
                Debug.LogError("SofaContextAPI::getGravity can't access Object Pointer m_native.");
            
            return gravity;
        }

        /// Get the number of object in the simulation scene
        public int getNumberObjects()
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_getNumberObjects(m_native);

            return res;
        }

        /// Get Sofa object name (used as Id) by its position id in the creation order
        public string getObjectName(int id)
        {
            if (m_isReady)
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
            if (m_isReady)
            {
                string type = sofaPhysicsAPI_get3DObjectType(m_native, id);
                return type;
            }
            else
                return "Error";
        }


        public void activateMessageHandler(bool status)
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_activateMessageHandler(m_native, status);

            if (res != 0)
                Debug.LogError("SofaContextAPI::activateMessageHandler method returns: " + SofaDefines.msg_error[res]);
        }

        public void DisplayMessages()
        {
            if (!m_isReady)
                return;

            int res = 0;
            res = sofaPhysicsAPI_getNbMessages(m_native);

            if (res <= 0)
                return;

            int[] type = new int[1];
            type[0] = -1;
            for (int i=0; i<res; i++)
            {
                string message = sofaPhysicsAPI_getMessage(m_native, i, type);

                // check if message should be ignored given specific rules
                if (skipExceptionMessage(message))
                    continue;

                if (type[0] == -1)
                    continue;
                else if (type[0] == 3)
                    Debug.LogWarning("# Sofa Warning: " + message);
                else if (type[0] == 4)
                    Debug.LogError("# Sofa Error: " + message);
                else if (type[0] == 5)
                    Debug.LogError("<color=red># Sofa Fatal error:</color> " + message);
                else
                    Debug.Log("# Sofa Log: " + message);
            }

            res = sofaPhysicsAPI_clearMessages(m_native);
            if (res != 0)
                Debug.LogError("SofaContextAPI::clearMessages method returns: " + SofaDefines.msg_error[res]);
        }

        // check if message match given specific rules
        public bool skipExceptionMessage(string message)
        {
            if (message.Contains("Plugin not found"))
            {
                return true;
            }
            return false;
        }

        public int clearMessages()
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_clearMessages(m_native);

            return res;
        }


        /////////////////////////////////////////////////////////////////////////////////////////
        //////////          API to Communication with SofaAdvancePhysicsAPI         /////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        /// Bindings to the SofaAdvancePhysicsAPI creation/destruction methods
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern IntPtr sofaPhysicsAPI_create(int nbrThread);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_delete(IntPtr obj);


        /// Bindings to create or load an existing simulation scene
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_unloadScene(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getStateMachine(IntPtr obj);        

        /// Binding to load a plugin
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_loadSofaIni(IntPtr obj, string pathIni);


        /// Bindings to hangle glew creation/destruction
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_initGlutGlew(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_freeGlutGlew(IntPtr obj);


        /// Bindings to communicate with the simulation tree
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

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
        public static extern bool sofaPhysicsAPI_asyncStep(IntPtr obj);
        [DllImport("SofaAdvancePhysicsAPI")]
        public static extern bool sofaPhysicsAPI_isAsyncStepCompleted(IntPtr obj);
        

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


        /// logging api
        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_activateMessageHandler(IntPtr obj, bool value);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getNbMessages(IntPtr obj);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getMessage(IntPtr obj, int messageId, int[] messageType);

        [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_clearMessages(IntPtr obj);

    }
}
