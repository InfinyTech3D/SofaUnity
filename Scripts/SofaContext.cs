using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

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
        //private SofaContextAPI m_impl = null;
        [SerializeField]
        private SofaUnityRenderer m_renderer = null;

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

        /// Parameter: Vector representing the gravity force.
        [SerializeField]
        protected Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);

        /// Parameter: Float representing the simulation timestep to use.
        [SerializeField]
        protected float m_timeStep = 0.02f; // ~ 1/60


        ////////////////////////////////////////////
        //////      SofaContext accessors      /////
        ////////////////////////////////////////////


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
                    if (m_renderer != null)
                        m_renderer.setGravity(m_gravity);
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
                    if (m_renderer != null)
                        m_renderer.timeStep = m_timeStep;
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
                    Debug.Log("#### SofaContext is playing ");
                else
                    Debug.Log("#### SofaContext is editor ");
            }

            this.gameObject.tag = "GameController";

            StartSofa();
        }


        /// Method called at GameObject destruction.
        void OnDestroy()
        {
            if (m_log)
                Debug.Log("SofaContext::OnDestroy stop called.");

            if (m_renderer != null)
            {
                m_renderer.stop();
                m_renderer.unload();
                m_renderer.Dispose();
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

            if (m_renderer == null)
            {
                this.enabled = false;
                this.gameObject.SetActive(false);
                return;
            }
        }


        /// Internal Method to init the SofaContext object
        void Init()
        {
            if (m_log)
                Debug.Log("## SofaContext ## init ");

            // Todo create context
            // Check and get the Sofa context
            if (m_renderer == null)
            {
                Debug.Log("## create m_renderer");
                m_renderer = new SofaUnityRenderer();
                m_renderer.m_sofaContext = this;
            }

            if (m_renderer == null)
            {
                Debug.LogError("Error while creating SofaUnityRenderer");
                return;
            }

            DoCatchSofaMessages();

            // start sofa instance
            m_renderer.start();

            // Create SOFA scene file manager
            if (m_sceneFileMgr == null)
                m_sceneFileMgr = new SceneFileManager(this);
            else
                m_sceneFileMgr.SetSofaContext(this);

            // Todo create SOFAVisualModel
            if (!Application.isPlaying)
                m_renderer.createScene();
            else
                ReconnectSofaScene();
            
            // set gravity and timestep if changed in editor
            m_renderer.timeStep = m_timeStep;
            m_renderer.setGravity(m_gravity);

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
            // log sofa messages
            //DoCatchSofaMessages();

            // only if scene is playing or if sofa is running
            if (Application.isPlaying == false) return;

            UpdateImplSync();

            //if (Time.time > nextFPSUpdate && m_impl != null)
            //{
            //    nextFPSUpdate += 1.0f / updateFPSRate;
            //    SimulationFPS = m_impl.GetSimulationFPS();
            //}

            DoCatchSofaMessages();
        }
                

        private float nextUpdate = 0.0f;

        protected void UpdateImplSync()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate += m_timeStep;

                m_renderer.step();
            }
        }

        

        /// Method to load a filename and create GameObject per Sofa object found.
        public void LoadSofaScene()
        {
            if (m_sceneFileMgr == null)
                return;

            if (m_log)
                Debug.Log("## SofaContext ## loadFilename: " + m_sceneFileMgr.AbsoluteFilename());

            // load scene file in SOFA
            if (m_renderer == null)
            {
                Debug.LogError("m_impl is null");
                return;
            }

            m_renderer.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Retrieve current timestep and gravity
            m_timeStep = m_renderer.timeStep;
            m_gravity = m_renderer.getGravity();

            // load all visual models
            m_renderer.createScene();

            DoCatchSofaMessages();
        }

        protected void ReconnectSofaScene()
        {
            Debug.Log("ReconnectSofaScene");
            if (m_sceneFileMgr == null)
                return;

            // load scene file in SOFA
            if (m_sceneFileMgr.SceneFilename.Length != 0)
                m_renderer.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Do not retrieve timestep of gravity in case it has been changed in editor

            // re-create objects after scene loading and before graph reconnection
            m_renderer.Reconnect();

            DoCatchSofaMessages();
        }

        public void ClearSofaScene()
        {
            m_renderer.stop();
            m_renderer.unload();
            m_renderer.start();
        }

        private bool isMsgHandlerActivated = false;
        protected void DoCatchSofaMessages()
        {
            if (m_renderer == null)
                return;

            // first time activated
            if (CatchSofaMessages && !isMsgHandlerActivated)
            {
                m_renderer.activateMessageHandler(true);
                isMsgHandlerActivated = true;
            }
            else if (!CatchSofaMessages && isMsgHandlerActivated)
            {
                m_renderer.activateMessageHandler(false);
                isMsgHandlerActivated = false;
            }

            if (isMsgHandlerActivated)
            {
                m_renderer.DisplayMessages();
            }
        }

    }
}
