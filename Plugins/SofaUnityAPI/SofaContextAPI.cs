using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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
        bool m_isDisposed = false;

        private bool m_isReady = false;

        public static string getResourcesPath()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Application.persistentDataPath;
#else
            return Application.dataPath;
#endif
        }

        void CopyAssetToPersistent()
        {
            string sofaUnityResourcesPath = getResourcesPath() + "/SofaUnity";
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR && TEST_PERSISTENT_WIN  // to test the zip/unzip mechanism on Windows
            // Merely a test to see if it is really doing its job
            Debug.Log("unzipping to persistent data path (windows)");
            Utility_SharpZipCommands.ExtractTGZ (Application.streamingAssetsPath + "/" + "Data.tgz",Application.persistentDataPath);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            // Prepare Data
            Debug.Log("data:" + Application.dataPath);
            Debug.Log("persistent:" + Application.persistentDataPath);
            Debug.Log("streaming:" + Application.streamingAssetsPath);

            //if stub file Resources.data doesn't exist, extract default data...
            if (File.Exists(sofaUnityResourcesPath + "/" + "Resources.data") == false)
            {
                Debug.Log("Resources.data doesn't exist, creating it for the first time.");
                //copy tgz to directory where we can extract it
                WWW www = new WWW(Application.streamingAssetsPath + "/Resources.tgz");
                while (!www.isDone) { }
                System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + "Resources.tgz", www.bytes);
                //extract it
                Utility_SharpZipCommands.ExtractTGZ(Application.persistentDataPath + "/" + "Resources.tgz", sofaUnityResourcesPath);
                //delete tgz
                File.Delete(Application.persistentDataPath + "/" + "Resources.tgz");
            }
            else
            {
                Debug.Log("Resources.data does exist, will not extract default data.");
            }
#endif
        }

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

            CopyAssetToPersistent();

            // load the sofaIni file
            string pathIni = Application.dataPath + "/SofaUnity/Plugins/Native/x64/sofa.ini";
            string sharePath = sofaPhysicsAPI_loadSofaIni(m_native, pathIni);
            
            if (sharePath.Contains("Error"))
            {
                Debug.LogError("SofaContextAPI: loadSofaIni method returns error code: " + sharePath);
            }

            // Create a simulation scene.
            int res = sofaPhysicsAPI_createScene(m_native);
            if (res == 0)
            {
                m_isReady = true;
            }
            else
            {
                Debug.LogError("SofaContextAPI scene creation return: " + SofaDefines.msg_error[res]);
                m_isReady = false;
            }
        }

        /// Destructor
        ~SofaContextAPI()
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
                    Debug.LogError("SofaContextAPI::loadScene method returns: " + SofaDefines.msg_error[res] + " for scene: " + filename);
            }
            else
                Debug.LogError("SofaContextAPI::loadScene can't load file: " + filename + "no sofaPhysicsAPI created!");
        }

        /// Method to stop the Sofa simulation
        public void unload()
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_unloadScene(m_native);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::unload method returns: " + SofaDefines.msg_error[res]);
            }
            else
            {
                Debug.LogError("SofaContextAPI::unload scene file not possible without a valid sofaPhysicsAPI created!");
            }
        }        

        /// Method to load the default list of SOFA plugin (default core and components plugins)
        public void loadDefaultPlugins(string pluginPath)
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_loadDefaultPlugins(m_native, pluginPath);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::loadDefaultPlugin method returns: " + SofaDefines.msg_error[res]);
            }
            else
                Debug.LogError("SofaContextAPI::loadDefaultPlugin can't load plugins, no sofaPhysicsAPI created!");
            
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


        public float GetTime()
        {
            if (m_isReady)
                return sofaPhysicsAPI_time(m_native);
            else
                return 0.0f;
        }

        public float GetSimulationFPS()
        {
            if (m_isReady)
                return sofaPhysicsAPI_getSimulationFPS(m_native);
            else
                return 666.0f;
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
                    Debug.LogError("SofaContextAPI::getGravity method returns: " + SofaDefines.msg_error[res]);
            }
            else
                Debug.LogError("SofaContextAPI::getGravity can't access Object Pointer m_native.");
            
            return gravity;
        }

        /// Get the number of object in the simulation scene
        /// Warning: This method has been depreciate.
        public int getNumberObjects()
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_getNumberObjects(m_native);

            return res;
        }

        /// Get Sofa object name (used as Id) by its position id in the creation order
        /// Warning: This method has been depreciate.
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
        /// Warning: This method has been depreciate.
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



        public int logSceneGraph()
        {
            int res = -9999;
            if (m_isReady)
                res = sofaPhysicsAPI_logSceneGraph(m_native);

            return res;
        }

        public int getNbrDAGNode()
        {
            int res = -9999;
            if (m_isReady)
                res = sofaPhysicsAPI_getNbrDAGNode(m_native);

            return res;
        }

        public string getBaseComponentTypes()
        {
            if (m_isReady)
            {
                string type = sofaPhysicsAPI_getBaseComponentTypes(m_native);
                return type;
            }
            else
                return "Error";
        }

        public string getDAGNodeName(int DAGNodeID)
        {
            if (m_isReady)
            {
                string str = sofaPhysicsAPI_getDAGNodeAPIName(m_native, DAGNodeID);
                return str;
            }
            else
                return "Error";
        }

        public string getDAGNodeDisplayName(int DAGNodeID)
        {
            if (m_isReady)
            {
                string str = sofaPhysicsAPI_getDAGNodeDisplayName(m_native, DAGNodeID);
                return str;
            }
            else
                return "Error";
        }


        public string getDAGNodeComponents(string nodeName)
        {
            if (m_isReady)
            {
                string type = sofaPhysicsAPI_getComponentsAsString(m_native, nodeName);
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


        public void SofaKeyPressEvent(int keyId)
        {
            int res = 0;
            if (m_isReady)
                sofaPhysicsAPI_createKeyPressEvent(m_native, keyId);

            if (res != 0)
                Debug.LogError("SofaContextAPI::SofaKeyPressEvent method returns: " + SofaDefines.msg_error[res]);
        }

        public void SofaKeyReleaseEvent(int keyId)
        {
            int res = 0;
            if (m_isReady)
                sofaPhysicsAPI_createKeyReleaseEvent(m_native, keyId);

            if (res != 0)
                Debug.LogError("SofaContextAPI::SofaKeyReleaseEvent method returns: " + SofaDefines.msg_error[res]);
        }



        /////////////////////////////////////////////////////////////////////////////////////////
        //////////          API to Communication with SofaAdvancePhysicsAPI         /////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        /// Bindings to the SofaAdvancePhysicsAPI creation/destruction methods
        [DllImport("SAPAPI")]
        public static extern IntPtr sofaPhysicsAPI_create(int nbrThread);

        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_delete(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_APIName(IntPtr obj);


        /// Bindings to create or load an existing simulation scene
        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_unloadScene(IntPtr obj);

        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_getStateMachine(IntPtr obj);

        /// Binding to load a plugin
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadDefaultPlugins(IntPtr obj, string pluginPath);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_loadSofaIni(IntPtr obj, string pathIni);


        /// Bindings to hangle glew creation/destruction
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_initGlutGlew(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_freeGlutGlew(IntPtr obj);

        
        /// Bindings to communicate with the simulation loop
        [DllImport("SAPAPI")]
        public static extern void sofaPhysicsAPI_start(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern void sofaPhysicsAPI_step(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern void sofaPhysicsAPI_stop(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern void sofaPhysicsAPI_reset(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern bool sofaPhysicsAPI_asyncStep(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern bool sofaPhysicsAPI_isAsyncStepCompleted(IntPtr obj);

        [DllImport("SAPAPI")]
        public static extern float sofaPhysicsAPI_getSimulationFPS(IntPtr obj);



        /// Bindings for generic environement of the simulation scene.
        [DllImport("SAPAPI")]
        public static extern void sofaPhysicsAPI_setTimeStep(IntPtr obj, double value);

        [DllImport("SAPAPI")]
        public static extern float sofaPhysicsAPI_timeStep(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern float sofaPhysicsAPI_time(IntPtr obj);
        

        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_getGravity(IntPtr obj, double[] values);
        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_setGravity(IntPtr obj, double[] values);



        /// Bindings tests
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int test_getAPI_ID(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void sofaPhysicsAPI_setTestName(IntPtr obj, string name);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_testName(IntPtr obj);


        /// logging api
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_activateMessageHandler(IntPtr obj, bool value);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getNbMessages(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getMessage(IntPtr obj, int messageId, int[] messageType);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_clearMessages(IntPtr obj);

        
        /// Bindings to communicate with the simulation tree
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getNbrDAGNode(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_logSceneGraph(IntPtr obj);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getDAGNodeAPIName(IntPtr obj, int DAGNodeID);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getDAGNodeDisplayName(IntPtr obj, int DAGNodeID);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getBaseComponentTypes(IntPtr obj);



        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getComponentsAsString(IntPtr obj, string nodeName);



        /// old binding API
        /// Warning: This method has been depreciate.
        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);

        /// Warning: This method has been depreciate.
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectName(IntPtr obj, int id);

        /// Warning: This method has been depreciate.
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_get3DObjectType(IntPtr obj, int id);


        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createKeyPressEvent(IntPtr obj, int keyId);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createKeyReleaseEvent(IntPtr obj, int keyId);
    }
}
