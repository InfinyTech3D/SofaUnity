#define SofaUnityRenderer

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Main class that map the Sofa Context API to a Unity GameObject
    /// This class will control the simulation parameters and load Sofa scene creating a hiearachy between objects.
    /// </summary>
    [ExecuteInEditMode]
    public class SofaContext : MonoBehaviour
    {
        ////////////////////////////////////////////
        //////       SofaContext members       /////
        ////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        private SofaContextAPI m_impl = null;

#if SofaUnityEngine
        /// Pointer to the SofaDAGNodeManager which is used to recreate the SOFA node hierarchy
        private SofaDAGNodeManager m_nodeGraphMgr = null;

        /// Pointer to the PluginManager which hold the list of sofa plugin to be loaded
        [SerializeField]
        private PluginManagerInterface m_pluginMgr = null;

        private SofaGraphicAPI m_graphicAPI = null;

#elif SofaUnityRenderer
        /// Pointer to the Sofa Context API.
        //private SofaContextAPI m_impl = null;
        [SerializeField]
        private SofaUnityRenderer m_renderer = null;
#endif

        /// Pointer to the SceneFileManager which is used to check the file and hold the filename and paths.
        [SerializeField]
        private SceneFileManager m_sceneFileMgr = null;

        ////////////////////////////////////////////
        ////////          parameters         ///////
        ////////////////////////////////////////////

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = true;

        /// Booleen to activate sofa message handler
        public bool CatchSofaMessages = true;

        /// Booleen to start Sofa simulation on Play
        //public bool StartOnPlay = true;

        [SerializeField]
        protected bool m_asyncSimulation = false;

        /// Parameter: Vector representing the gravity force.
        [SerializeField]
        protected Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);

        /// Parameter: Float representing the simulation timestep to use.
        [SerializeField]
        protected float m_timeStep = 0.02f; // ~ 1/60


        ////////////////////////////////////////////
        //////      SofaContext accessors      /////
        ////////////////////////////////////////////

        /// Getter of current Sofa Context API, @see m_impl
        public SofaContextAPI GetSofaAPI()
        {
            if (m_impl == null)
                Init();

            if (m_impl == null) // still null
            {
                Debug.LogError("Error: SofaContext has not be created. method getSimuContext return IntPtr.Zero");
                return null;
            }
            return m_impl;
        }

#if SofaUnityEngine
        /// getter to the \sa PluginManager m_pluginMgr
        public PluginManagerInterface PluginManagerInterface
        {
            get { return m_pluginMgr; }
        }

        /// getter to the \sa SofaDAGNodeManager m_nodeGraphMgr
        public SofaDAGNodeManager NodeGraphMgr
        {
            get { return m_nodeGraphMgr; }
        }

        /// getter to the \sa SofaGraphicAPI m_graphicAPI
        public SofaGraphicAPI SofaGraphicAPI
        {
            get { return m_graphicAPI; }
        }
#endif

        /// getter to the \sa SceneFileManager m_sceneFileMgr
        public SceneFileManager SceneFileMgr
        {
            get { return m_sceneFileMgr; }
        }

        /// Getter/Setter of current gravity @see m_gravity
        public Vector3 Gravity
        {
            get { return m_gravity; }
            set
            {
                if (m_gravity != value)
                {
                    m_gravity = value;
                    if (m_impl != null)
                        m_impl.setGravity(m_gravity);
                }
            }
        }


        /// Getter/Setter of current timeStep @see m_timeStep
        public float TimeStep
        {
            get { return m_timeStep; }
            set
            {
                if (m_timeStep != value && value > 0.0f)
                {
                    m_timeStep = value;
                    if (m_impl != null)
                        m_impl.timeStep = m_timeStep;
                }
            }
        }


        public bool UnLoadScene
        {
            set
            {
                if (value && m_renderer != null)
                {
                    Debug.Log("UnLoadScene " + value);
                    ClearSofaScene();
                }
            }
        }

        
        ////////////////////////////////////////////
        ////////      scale conversions      ///////
        ////////////////////////////////////////////

        public Vector3 GetScaleSofaToUnity()
        {
            return new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }

        public Vector3 GetScaleUnityToSofa()
        {
            Vector3 scale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            for (int i = 0; i < 3; i++)
                if (scale[i] != 0)
                    scale[i] = 1 / scale[i];

            return scale;
        }

        public float GetFactorSofaToUnity(int dir = -1)
        {
            Vector3 scale = this.transform.localScale;
            float factor;
            if (dir == -1)
                factor = (Math.Abs(scale.x) + Math.Abs(scale.y) + Math.Abs(scale.z)) / 3;
            else
                factor = scale[dir];

            return factor;
        }

        public float GetFactorUnityToSofa(int dir = -1)
        {
            float factor = GetFactorSofaToUnity(dir);               
            if (factor != 0.0f) factor = 1 / factor;

            return factor;
        }


        ////////////////////////////////////////////
        ////////       Behavior methods      ///////
        ////////////////////////////////////////////

        /// Method called at GameObject creation.
        void Awake()
        {
            if (m_log)
            {
                if (Application.isPlaying)
                    Debug.Log("#### SofaContext is in Playing Mode.");
                else
                    Debug.Log("#### SofaContext is in Editor Mode.");
            }

            this.gameObject.tag = "GameController";

            StartSofa();
        }


        /// Method called at GameObject destruction.
        void OnDestroy()
        {
            if (m_log)
                Debug.Log("SofaContext::OnDestroy stop called.");

            if (m_impl != null)
            {
                if (m_log)
                    Debug.Log("SofaContext::OnDestroy stop now.");

                if (isMsgHandlerActivated)
                {
                    m_impl.activateMessageHandler(false);
                    isMsgHandlerActivated = false;
                }

                m_impl.stop();

                m_impl.unload();

                m_impl.Dispose();
            }
        }

        private void OnApplicationQuit()
        {
            // Todo
        }

        void StartSofa()
        {
            // Call the init method to create the Sofa Context
            Init();

            if (m_impl == null)
            {
                this.enabled = false;
                this.gameObject.SetActive(false);
                return;
            }
        }

        public void ResetSofa()
        {
            if (m_impl != null)
            {
                m_impl.reset();
            }
        }


        /// Internal Method to init the SofaContext object
        void Init()
        {
            if (m_log)
                Debug.Log("## SofaContext ## init ");

            // Check and get the Sofa context
            if (m_impl == null) {
                if (m_log)
                    Debug.Log("## create SofaContextAPI");
                m_impl = new SofaContextAPI();
            }

            if (m_impl == null)
            {
                Debug.LogError("Error while creating SofaContextAPI");
                return;
            }

            DoCatchSofaMessages();

#if SofaUnityEngine
            SofaComponentFactory.InitBaseFactoryType();

            // Create the NodeMgr
            if (m_nodeGraphMgr == null)
                m_nodeGraphMgr = new SofaDAGNodeManager(this, m_impl);
            else // TODO make this serializable might help for custom simulation in futur.
                Debug.LogWarning("## m_nodeGraphMgr already created...");

            // Create the GraphicAPI
            if (m_graphicAPI == null)
                m_graphicAPI = new SofaGraphicAPI();
            else 
                Debug.LogWarning("## m_graphicAPI already created...");

            // Craete Plugin Mgr. // TODO: Need to connect that with the scene loading
            if (m_pluginMgr == null)
                m_pluginMgr = new PluginManagerInterface(m_impl);
            else
                m_pluginMgr.SetSofaContextAPI(m_impl);

            // Load SOFA plugins
            m_pluginMgr.LoadPlugins();
#elif SofaUnityRenderer
            if (m_renderer == null)
            {
                Debug.Log("## create m_renderer");
                m_renderer = new SofaUnityRenderer(this);
            }
#endif
            // start sofa instance
            m_impl.start();

            // Create SOFA scene file manager
            if (m_sceneFileMgr == null)
                m_sceneFileMgr = new SceneFileManager(this);
            else
                m_sceneFileMgr.SetSofaContext(this);


#if SofaUnityEngine
            // If already has a scene, reconnect it in DAGNode mgr
            SofaDAGNode rootNode = this.gameObject.GetComponent<SofaDAGNode>();
            if (rootNode == null) // first creation
                m_nodeGraphMgr.LoadNodeGraph();
            else
            {
                // reconnect the full graph
                ReconnectSofaScene();
            }
#elif SofaUnityRenderer
            if (m_renderer.m_visualModels == null)
                m_renderer.createScene();
            else
                ReconnectSofaScene();
#endif

            // set gravity and timestep if changed in editor
            m_impl.timeStep = m_timeStep;
            m_impl.setGravity(m_gravity);

            // Check message form SOFA side at end of Init
            DoCatchSofaMessages();
        }


        float updateFPSRate = 4.0f;  // 4 updates per sec.
        float nextFPSUpdate = 0.25f;
        public float SimulationFPS = 0.0f;

        protected int countStep = 0;
        protected List<float> m_times = new List<float>();
        protected List<float> m_sofaTimes = new List<float>();

        // Update is called once per fix frame
        void Update()
        {
            // only if scene is playing or if sofa is running
            if (Application.isPlaying == false) return;

            if (m_asyncSimulation)
                UpdateImplASync();
            else
                UpdateImplSync();

            //if (Time.time > nextFPSUpdate && m_impl != null)
            //{
            //    nextFPSUpdate += 1.0f / updateFPSRate;
            //    SimulationFPS = m_impl.GetSimulationFPS();
            //}

            // log sofa messages
            DoCatchSofaMessages();
        }
                

        private float nextUpdate = 0.0f;

        protected void UpdateImplSync()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate += m_timeStep;

                m_impl.step();

#if SofaUnityEngine
                if (m_nodeGraphMgr != null)
                    m_nodeGraphMgr.PropagateSetDirty(true);
#elif SofaUnityRenderer
                m_renderer.step();
#endif
            }
        }

        protected void UpdateImplASync()
        {

        }


        /// Method to load a filename and create GameObject per Sofa object found.
        public void LoadSofaScene()
        {
            if (m_sceneFileMgr == null)
                return;

            if (m_log)
                Debug.Log("## SofaContext ## loadFilename: " + m_sceneFileMgr.AbsoluteFilename());

            // load scene file in SOFA
            if (m_impl == null)
            {
                Debug.LogError("m_impl is null");
                return;
            }

            m_impl.loadScene(m_sceneFileMgr.AbsoluteFilename());

#if SofaUnityEngine
            // recreate node hiearchy in unity
            m_nodeGraphMgr.LoadNodeGraph();
#elif SofaUnityRenderer
            // load all visual models
            m_renderer.createScene();
#endif

            // Retrieve current timestep and gravity
            m_timeStep = m_impl.timeStep;
            m_gravity = m_impl.getGravity();

            DoCatchSofaMessages();
        }

        protected void ReconnectSofaScene()
        {
            Debug.Log("ReconnectSofaScene");
            if (m_sceneFileMgr == null)
                return;

            // load scene file in SOFA
            if (m_sceneFileMgr.SceneFilename.Length != 0)
                m_impl.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Do not retrieve timestep of gravity in case it has been changed in editor

            // re-create objects after scene loading and before graph reconnection
#if SofaUnityEngine
            // reconnect node hiearchy in unity
            m_nodeGraphMgr.ReconnectNodeGraph();
#elif SofaUnityRenderer
            m_renderer.Reconnect();
#endif

            DoCatchSofaMessages();
        }

        public void ClearSofaScene()
        {
            m_impl.stop();
            m_impl.unload();
#if SofaUnityEngine
            m_nodeGraphMgr.LoadNodeGraph();
#elif SofaUnityRenderer

#endif
            m_impl.start();
        }

        private bool isMsgHandlerActivated = false;
        protected void DoCatchSofaMessages()
        {
            if (m_renderer == null)
                return;

            // first time activated
            if (CatchSofaMessages && !isMsgHandlerActivated)
            {
                m_impl.activateMessageHandler(true);
                isMsgHandlerActivated = true;
            }
            else if (!CatchSofaMessages && isMsgHandlerActivated)
            {
                m_impl.activateMessageHandler(false);
                isMsgHandlerActivated = false;
            }

            if (isMsgHandlerActivated)
            {
                m_impl.DisplayMessages();
            }
        }

    }
}
