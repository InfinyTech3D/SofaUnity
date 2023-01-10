/*****************************************************************************
 *                 - Copyright (C) - 2022 - InfinyTech3D -                   *
 *                                                                           *
 * This file is part of the SofaUnity-Renderer asset from InfinyTech3D       *
 *                                                                           *
 * GNU General Public License Usage:                                         *
 * This file may be used under the terms of the GNU General                  *
 * Public License version 3. The licenses are as published by the Free       *
 * Software Foundation and appearing in the file LICENSE.GPL3 included in    *
 * the packaging of this file. Please review the following information to    *
 * ensure the GNU General Public License requirements will be met:           *
 * https://www.gnu.org/licenses/gpl-3.0.html.                                *
 *                                                                           *
 * Commercial License Usage:                                                 *
 * Licensees holding valid commercial license from InfinyTech3D may use this *
 * file in accordance with the commercial license agreement provided with    *
 * the Software or, alternatively, in accordance with the terms contained in *
 * a written agreement between you and InfinyTech3D. For further information *
 * on the licensing terms and conditions, contact: contact@infinytech3d.com  *
 *                                                                           *
 * Authors: see Authors.txt                                                  *
 * Further information: https://infinytech3d.com                             *
 ****************************************************************************/

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
        bool m_isDisposed = false;

        private bool m_isReady = false;

        /// Default constructor, will create the pointer to SofaPhysicsAPI
        public SofaContextAPI(bool async = false)
        {
            // Create the application
#if SofaUnityEngine
            if (async)
                m_native = sofaPhysicsAPI_create(2);
            else
#endif
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


        /// Get the Sofa simulation context
        public IntPtr getSimuContext()
        {
            return m_native;
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

#if SofaUnityEngine
        public int contextStatus()
        {
            if (m_isReady)
                return sofaPhysicsAPI_getStateMachine(m_native);
            else
                return -2;
        }
#endif

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

#if SofaUnityEngine
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
#endif

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

#if SofaUnityEngine
        public float GetSimulationFPS()
        {
            if (m_isReady)
                return sofaPhysicsAPI_getSimulationFPS(m_native);
            else
                return 666.0f;
        }
#endif

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


#if SofaUnityEngine
        public int logSceneGraph()
        {
            int res = -9999;
            if (m_isReady)
                res = sofaPhysicsAPI_logSceneGraph(m_native);

            return res;
        }
#endif

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
            return false;
        }

        public int clearMessages()
        {
            int res = 0;
            if (m_isReady)
                res = sofaPhysicsAPI_clearMessages(m_native);

            return res;
        }


#if SofaUnityEngine
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
#endif


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


        /// Bindings to create or load an existing simulation scene
        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createScene(IntPtr obj);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadScene(IntPtr obj, string filename);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_unloadScene(IntPtr obj);

#if SofaUnityEngine
        [DllImport("SAPAPI")]
        public static extern int sofaPhysicsAPI_getStateMachine(IntPtr obj);
#endif

        /// Binding to load a plugin
        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string sofaPhysicsAPI_loadSofaIni(IntPtr obj, string pathIni);

        [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_loadPlugin(IntPtr obj, string pluginName);


        /// Bindings to communicate with the simulation loop
        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_start(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_stop(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_step(IntPtr ptr);

        [DllImport("SofaPhysicsAPI")]
        public static extern void sofaPhysicsAPI_reset(IntPtr ptr);

#if SofaUnityEngine
        [DllImport("SAPAPI")]
        public static extern bool sofaPhysicsAPI_asyncStep(IntPtr obj);
        [DllImport("SAPAPI")]
        public static extern bool sofaPhysicsAPI_isAsyncStepCompleted(IntPtr obj);

        [DllImport("SAPAPI")]
        public static extern float sofaPhysicsAPI_getSimulationFPS(IntPtr obj);
#endif


        /// Bindings for generic environement of the simulation scene.
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

#if SofaUnityEngine
        /// Bindings to communicate with the simulation tree
        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_logSceneGraph(IntPtr obj);       

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createKeyPressEvent(IntPtr obj, int keyId);

        [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int sofaPhysicsAPI_createKeyReleaseEvent(IntPtr obj, int keyId);
#endif
    }
}
