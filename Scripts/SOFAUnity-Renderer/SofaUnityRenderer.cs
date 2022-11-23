using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnity
{
    public class SofaUnityRenderer : IDisposable
    {
        /// Internal pointer to the SofaPhysicsAPI 
        internal IntPtr m_native;

        private bool m_isReady = false;

        public SofaContext m_sofaContext = null;

        public List<SofaVisualModel> m_visualModels = null;

        public IntPtr getImpl() { return m_native; }

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaUnityRenderer()
        {
            int test = test_getAPI_ID();
            Debug.Log("test_getAPI_ID: " + test);
            m_native = sofaPhysicsAPI_create();

            if (m_native == IntPtr.Zero)
            {
                Debug.LogError("Error no sofaPhysicsAPI found nor created!");
                m_isReady = false;
                return;
            }

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

                string pluginPath = "";
                if (Application.isEditor)
                    pluginPath = "/SofaUnity/Plugins/Native/x64/";
                else
                    pluginPath = "/Plugins/x86_64/";

                loadPlugin(Application.dataPath + pluginPath + "Sofa.Component.dll");
                loadPlugin(Application.dataPath + pluginPath + "Sofa.GL.Component.dll");
                loadPlugin(Application.dataPath + pluginPath + "Sofa.GUI.Component.dll");
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
            {
                sofaPhysicsAPI_step(m_native);

                foreach(SofaVisualModel visuM in m_visualModels)
                {
                    visuM.SetDirty(true);
                }
            }
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


        public void activateMessageHandler(bool status)
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_activateMessageHandler(m_native, status);

            if (res != 0)
                Debug.LogError("SofaContextAPI::activateMessageHandler method returns: " + res);
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
            for (int i = 0; i < res; i++)
            {
                string message = sofaPhysicsAPI_getMessage(m_native, i, type);

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
                Debug.LogError("SofaContextAPI::clearMessages method returns: " + res);
        }


        public int clearMessages()
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_clearMessages(m_native);

            return res;
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


        public void loadPlugin(string pluginPath)
        {
            if (m_isReady)
            {
                int res = sofaPhysicsAPI_loadPlugin(m_native, pluginPath);
                if (res != 0)
                    Debug.LogError("SofaContextAPI::loadPlugin method returns: " + res + " for scene: " + pluginPath);
            }
            else
                Debug.LogError("SofaContextAPI::loadPlugin can't load file: " + pluginPath + " no sofaPhysicsAPI created!");
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



        public void createScene()
        {
            Debug.Log("### SofaVisualModel:createScene");
            int nbrVM = sofaPhysicsAPI_getNbrVisualModel(m_native);
            Debug.Log("createScene: Nbr VisualM: " + nbrVM);

            if (m_visualModels == null)
                m_visualModels = new List<SofaVisualModel>();

            for (int i=0; i< nbrVM; i++)
            {
                string nameVM = sofaVisualModel_getName(m_native, i);
                GameObject obj = new GameObject("SofaVisualModel - " + nameVM);
                SofaVisualModel visuM = obj.AddComponent<SofaVisualModel>();
                visuM.m_uniqName = nameVM;
                visuM.m_renderer = this;
                obj.transform.parent = m_sofaContext.gameObject.transform;

                m_visualModels.Add(visuM);
            }
        }


        public void Reconnect()
        {
            Debug.Log("## SofaVisualModel:Reconnect ");

            if (m_visualModels == null)
                m_visualModels = new List<SofaVisualModel>();

            SofaVisualModel[] Vms = m_sofaContext.GetComponentsInChildren<SofaVisualModel>();

            foreach (SofaVisualModel visuM in Vms)
            {
                Debug.Log("Reconnect SofaVisualModel: " + visuM.m_uniqName);
                visuM.m_renderer = this;
                m_visualModels.Add(visuM);
            }
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


        /// logging api
        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_activateMessageHandler(IntPtr obj, bool value);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_getNbMessages(IntPtr obj);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_getMessage(IntPtr obj, int messageId, int[] messageType);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_clearMessages(IntPtr obj);



        // API for scene creation/loading
        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_unloadScene(IntPtr obj);


        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_loadSofaIni(IntPtr obj, string pathIni);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);




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



        [DllImport("SofaPhysicsAPI")]
        public static extern int sofaPhysicsAPI_getNbrVisualModel(IntPtr ptr);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaVisualModel_getName(IntPtr ptr, int VModelID);
    }
}