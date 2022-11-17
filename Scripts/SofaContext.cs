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

 
        ////////////////////////////////////////////
        ////////          parameters         ///////
        ////////////////////////////////////////////

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = false;

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

        /// Getter/Setter of current gravity @see m_gravity
        public Vector3 Gravity
        {
            get { return m_gravity; }
            set
            {
                if (m_gravity != value)
                {
                    m_gravity = value;
                    //if (m_impl != null)
                    //    m_impl.setGravity(m_gravity);
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
                    //if (m_impl != null)
                    //    m_impl.timeStep = m_timeStep;
                }
            }
        }


        public bool UnLoadScene
        {
            set
            {
                //if (value && m_impl != null)
                //{
                //    Debug.Log("UnLoadScene " + value);
                //    ClearSofaScene();
                //}
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
            StartSofa();
        }


        /// Method called at GameObject destruction.
        void OnDestroy()
        {
            if(m_log)
                Debug.Log("SofaContext::OnDestroy stop called.");

            // Todo
        }

        private void OnApplicationQuit()
        {
            // Todo
        }

        void StartSofa()
        {
            // Call the init method to create the Sofa Context
            Init();

            // Todo

        }


        /// Internal Method to init the SofaContext object
        void Init()
        {
            if (m_log)
                Debug.Log("## SofaContext ## init ");

            // Todo create context

            // start sofa instance

            //m_impl.start();

            // Create SOFA scene file manager
            //if (m_sceneFileMgr == null)
            //    m_sceneFileMgr = new SceneFileManager(this);
            //else
            //    m_sceneFileMgr.SetSofaContext(this);

            // Todo create SOFAVisualModel


            // set gravity and timestep if changed in editor
            //m_impl.timeStep = m_timeStep;
            //m_impl.setGravity(m_gravity);

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

            UpdateImplSync();

            //if (Time.time > nextFPSUpdate && m_impl != null)
            //{
            //    nextFPSUpdate += 1.0f / updateFPSRate;
            //    SimulationFPS = m_impl.GetSimulationFPS();
            //}
        }
                

        private float nextUpdate = 0.0f;

        protected void UpdateImplSync()
        {
            //if (Time.time >= nextUpdate)
            //{
            //    nextUpdate += m_timeStep;

            //    m_impl.step();

            //}
        }

        

        /// Method to load a filename and create GameObject per Sofa object found.
        public void LoadSofaScene()
        {
            //if (m_sceneFileMgr == null)
            //    return;

            //if (m_log)
            //    Debug.Log("## SofaContext ## loadFilename: " + m_sceneFileMgr.AbsoluteFilename());

            //// load scene file in SOFA
            //if (m_impl == null)
            //{
            //    Debug.LogError("m_impl is null");
            //    return;
            //}
            
            //m_impl.loadScene(m_sceneFileMgr.AbsoluteFilename());

            // Retrieve current timestep and gravity
            //m_timeStep = m_impl.timeStep;
            //m_gravity = m_impl.getGravity();
        }

        public void ClearSofaScene()
        {
            //m_impl.stop();
            //m_impl.unload();
            //m_impl.start();
        }


    }
}
