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
        private SofaContextAPI m_impl;

        /// Pointer to the SofaDAGNodeManager which is used to recreate the SOFA node hierarchy
        private SofaDAGNodeManager m_nodeGraphMgr = null;

        /// Pointer to the PluginManager which hold the list of sofa plugin to be loaded
        [SerializeField]
        private PluginManager m_pluginMgr = null;

        /// Pointer to the SceneFileManager which is used to check the file and hold the filename and paths.
        [SerializeField]
        private SceneFileManager m_sceneFileMgr = null;


        List<SofaRayCaster> m_casters = null;

        ////////////////////////////////////////////
        ////////          parameters         ///////
        ////////////////////////////////////////////

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = false;

        /// Booleen to update sofa simulation
        public bool IsSofaUpdating = true;

        /// Booleen to activate sofa message handler
        public bool CatchSofaMessages = true;

        /// Booleen to start Sofa simulation on Play
        //public bool StartOnPlay = true;

        public bool StepbyStep = false;

        public bool testAsync = false;

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
        public IntPtr GetSimuContext()
        {
            if (m_impl == null)
                Init();

            if (m_impl == null) // still null
            {
                Debug.LogError("Error: SofaContext has not be created. method getSimuContext return IntPtr.Zero");
                return IntPtr.Zero;
            }
            return m_impl.getSimuContext();
        }

        /// getter to the \sa PluginManager m_pluginMgr
        public PluginManager PluginManager
        {
            get { return m_pluginMgr; }
        }

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
                if (m_timeStep != value)
                {
                    m_timeStep = value;
                    if (m_impl != null)
                        m_impl.timeStep = m_timeStep;
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

        /*
        public bool breakerActivated = false;
        private int cptBreaker = 0;
        private int countDownBreaker = 10;
        public void breakerProcedure()
        {
            breakerActivated = true;
            cptBreaker = 0;
        }

     */
        public void RegisterRayCaster(SofaRayCaster obj)
        {
            if (m_casters == null)
                m_casters = new List<SofaRayCaster>();
            m_casters.Add(obj);
        }
       

        /// Method called at GameObject creation.
        void Awake()
        {
            if (Application.isPlaying)
                Debug.Log("#### SofaContext is playing | StartOnPlay: " + IsSofaUpdating);
            else
                Debug.Log("#### SofaContext is editor | StartOnPlay: " + IsSofaUpdating);

            //if (Application.isPlaying && StartOnPlay == false)
            //    return;

            this.gameObject.tag = "GameController";

            StartSofa();
        }


        /// Method called at GameObject destruction.
        void OnDestroy()
        {
            if(m_log)
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

                if (m_log)
                    Debug.Log("## SofaContext status before stop: " + m_impl.contextStatus());

                m_impl.stop();

                if (m_log)
                    Debug.Log("## SofaContext status after stop: " + m_impl.contextStatus());

                m_impl.unload();

                if (m_log)
                    Debug.Log("## SofaContext status after unload: " + m_impl.contextStatus());

                m_impl.Dispose();
            }
        }

        private void OnApplicationQuit()
        {
            if (m_casters != null)
            {
                foreach (SofaRayCaster child in m_casters)
                {
                    if (child != null)
                        child.StopRay();
                }
            }
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

            //breakerActivated = false;
            //cptBreaker = 0;
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
            //if (this.transform.localScale.x > 0)
            //{
            //    Vector3 scale = this.transform.localScale;
            //    //scale.x *= -1;
            //    this.transform.localScale = scale;
            //}

            if (m_log)
                Debug.Log("## SofaContext ## init ");

            if (m_impl == null)
            {
                m_impl = new SofaContextAPI(testAsync);

                if (m_nodeGraphMgr == null)
                {
                    m_nodeGraphMgr = new SofaDAGNodeManager(this, m_impl);
                }
                else
                {
                    // TODO make this serializable might help for custom simulation in futur.
                    Debug.Log("## m_nodeGraphMgr already created...");
                }

                // handle sofa plugins
                if (m_pluginMgr == null)
                    m_pluginMgr = new PluginManager(m_impl);
                else
                    m_pluginMgr.SetSofaContextAPI(m_impl);

                m_pluginMgr.LoadPlugins();

                // start sofa instance
                if (m_log)
                    Debug.Log("## SofaContext status before start: " + m_impl.contextStatus());

                m_impl.start();

                if (m_log)
                    Debug.Log("## SofaContext status after start: " + m_impl.contextStatus());

                // handle SOFA scene file
                if (m_sceneFileMgr == null)
                    m_sceneFileMgr = new SceneFileManager(this);
                else
                    m_sceneFileMgr.SetSofaContext(this);

                if (m_sceneFileMgr.HasScene)
                {
                    //m_sceneFileMgr.LoadFilename();
                    ReconnectSofaScene();
                }

                DoCatchSofaMessages();
                if (m_log)
                    Debug.Log("## SofaContext status end init: " + m_impl.contextStatus());

                // set gravity and timestep if changed in editor
                m_impl.timeStep = m_timeStep;
                m_impl.setGravity(m_gravity);
            }
            else
            {
                Debug.LogError("### SofaContext init No Impl");
            }
        }
        
        // Update is called once per fix frame
        void FixedUpdate()
        {

        }
        
        // Update is called once per frame
        void Update()
        {
            // only if scene is playing or if sofa is running
            if (IsSofaUpdating == false || Application.isPlaying == false) return; 

            if (testAsync)
                UpdateImplASync();
            else
                UpdateImplSync();

            // log sofa messages
            DoCatchSofaMessages();

            // counter if need to freeze the simulation for several iterations
            //cptBreaker++;
            //if (cptBreaker == countDownBreaker)
            //{
            //    cptBreaker = 0;
            //    breakerActivated = false;
            //}

            if (StepbyStep)
            {
                IsSofaUpdating = false;
                StepbyStep = false;
            }
        }
        

        private float nextUpdate = 0.0f;

        protected void UpdateImplSync()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate += m_timeStep;

                m_impl.step();

                if (m_nodeGraphMgr != null)
                    m_nodeGraphMgr.PropagateSetDirty(true);
            }
        }

        protected void UpdateImplASync()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate += m_timeStep;

                //Debug.Log(Time.deltaTime);

                // if physics simulation async step is still running do not wait and return the control to Unity
                if (m_impl.isAsyncStepCompleted())
                {
                   // Debug.Log("isAsyncStepCompleted: YES ");
                    
                    // physics simulation step completed and is not running
                    // perform data synchronization safely (no need of synchronization locks)                        
                    //if (m_hierarchyPtr.m_objects != null)
                    //{
                    //    // Set all objects to dirty to force and update.
                    //    foreach (SofaBaseObject child in m_hierarchyPtr.m_objects)
                    //    {
                    //        //child.setDirty();
                    //        child.updateImpl();
                    //        //Debug.Log(child.name);
                    //    }
                    //}

                    // update the ray casters
                    if (m_casters != null)
                    {
                        // Set all objects to dirty to force and update.
                        foreach (SofaRayCaster child in m_casters)
                        {
                            //child.setDirty();
                            child.CastRay();
                            //Debug.Log(child.name);
                        }
                    }

                    //m_impl.step();
                    // run a new physics simulation async step
                    m_impl.asyncStep();
                }
                //else
                //{
                //    Debug.Log("isAsyncStepCompleted: NO ");
                //}
            }
        }

        private bool isMsgHandlerActivated = false;
        protected void DoCatchSofaMessages()
        {
            // first time activated
            if (CatchSofaMessages && !isMsgHandlerActivated)
            {
                m_impl.activateMessageHandler(true);
                isMsgHandlerActivated = true;
            }
            else if(!CatchSofaMessages && isMsgHandlerActivated)
            {
                m_impl.activateMessageHandler(false);
                isMsgHandlerActivated = false;
            }

            if (isMsgHandlerActivated)
            {
                 m_impl.DisplayMessages();
            }
        }

        

        /// Method to load a filename and create GameObject per Sofa object found.
        public void LoadSofaScene()
        {
            if (m_sceneFileMgr == null)
                return;

            Debug.Log("## SofaContext ## loadFilename: " + m_sceneFileMgr.AbsoluteFilename());
            // load scene file in SOFA
            if (m_impl == null)
                Debug.LogError("m_impl is null");
            m_impl.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Retrieve current timestep and gravity
            m_timeStep = m_impl.timeStep;
            m_gravity = m_impl.getGravity();

            // recreate node hiearchy in unity
            m_nodeGraphMgr.LoadNodeGraph();

            int nbrObj = m_impl.getNumberObjects();
            Debug.Log("######### nbr Objects: " + nbrObj);
            for (int i = 0; i < nbrObj; i++)
            {
                Debug.Log(i + " -> " + m_impl.getObjectName(i));
            }
        }

        protected void ReconnectSofaScene()
        {
            if (m_sceneFileMgr == null)
                return;

            Debug.Log("## SofaContext ## ReconnectSofaScene: " + m_sceneFileMgr.AbsoluteFilename());
            // load scene file in SOFA
            m_impl.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Do not retrieve timestep of gravity in case it has been changed in editor

            // reconnect node hiearchy in unity
            m_nodeGraphMgr.ReconnectNodeGraph();
        }

        public void ClearSofaScene()
        {
            m_impl.stop();
            m_impl.unload();
            //m_nodeGraphMgr.clear();
            m_impl.start();
        }

    }
}
