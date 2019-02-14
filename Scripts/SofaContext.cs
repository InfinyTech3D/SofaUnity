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
        /// Pointer to the Sofa Context API.
        SofaContextAPI m_impl;

        /// Parameter to activate logging of this Sofa GameObject
        public bool m_log = false;

        /// Parameter: Vector representing the gravity force.
        public Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);
        /// Parameter: Float representing the simulation timestep to use.
        public float m_timeStep = 0.02f; // ~ 1/60

        /// Parameter: String representing the Path to the Sofa scene.
        public string m_filename = "";

        /// Booleen to update sofa simulation
        public bool IsSofaUpdating = true;

        /// Booleen to activate sofa message handler
        public bool CatchSofaMessages = true;

        List<SRayCaster> m_casters = null;

        private SObjectHierarchy m_hierarchyPtr = null;


        /// Getter of current Sofa Context API, @see m_impl
        public IntPtr getSimuContext()
        {
            if (m_impl == null)
                init();

            if (m_impl == null) // still null
            {
                Debug.LogError("Error: SofaContext has not be created. method getSimuContext return IntPtr.Zero");
                return IntPtr.Zero;
            }
            return m_impl.getSimuContext();
        }

        /// Getter/Setter of current gravity @see m_gravity
        public Vector3 gravity
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
        public float timeStep
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

        /// Getter/Setter of current filename @see m_filename
        public string filename
        {
            get { return m_filename; }
            set
            {
                if (value != m_filename)
                {
                    if (File.Exists(Application.dataPath+value))
                    {
                        bool reload = false;
                        if (m_filename != "")
                            reload = true;

                        m_filename = value;

                        if (reload)
                            reloadFilename();
                        else
                            loadFilename();
                    }
                    else
                        Debug.LogError("Error file doesn't exist: " + Application.dataPath + value);
                }
            }
        }

        public Vector3 getScaleSofaToUnity()
        {
            return new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }

        public Vector3 getScaleUnityToSofa()
        {
            Vector3 scale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            for (int i = 0; i < 3; i++)
                if (scale[i] != 0)
                    scale[i] = 1 / scale[i];

            return scale;
        }

        public float getFactorSofaToUnity()
        {
            Vector3 scale = this.transform.localScale;
            float factor = (Math.Abs(scale.x) + Math.Abs(scale.y) + Math.Abs(scale.z)) / 3;
            return factor;
        }

        public float getFactorUnityToSofa()
        {
            float factor = getFactorSofaToUnity();
            if (factor != 0.0f) factor = 1 / factor;

            return factor;
        }

        public bool breakerActivated = false;
        private int cptBreaker = 0;
        private int countDownBreaker = 10;
        public void breakerProcedure()
        {
            breakerActivated = true;
            cptBreaker = 0;
        }

        /// Getter/Setter of current objectcpt @see m_objectCpt
        public int objectcpt
        {
            get {
                if (m_hierarchyPtr == null)
                    initHierarchy();

                return m_hierarchyPtr.m_objectCpt;
            }
            set {
                if (m_hierarchyPtr == null)
                    initHierarchy();
                m_hierarchyPtr.m_objectCpt = value;
            }
        }

        /// Getter to the number of object loaded from Sofa Scene.
        public int nbrObject
        {
            get {
                if (m_hierarchyPtr == null)
                    initHierarchy();
                return m_hierarchyPtr.m_nbrObject;
            }
        }

        public void registerObject(SBaseObject obj)
        {
            if (m_hierarchyPtr != null)
                m_hierarchyPtr.registerSObject(obj);
        }

        public void registerCaster(SRayCaster obj)
        {
            if (m_casters == null)
                m_casters = new List<SRayCaster>();
            m_casters.Add(obj);
        }

        /// Method called at GameObject creation.
        void Awake()
        {
            // Call the init method to create the Sofa Context
            init();

            if (m_impl == null)
            {
                this.enabled = false;
                this.gameObject.SetActive(false);
                return;
            }
        }

        // Use this for initialization
        void Start()
        {
            breakerActivated = false;
            cptBreaker = 0;            
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

                m_impl.stop();
                m_impl.Dispose();
            }
        }

        private void OnApplicationQuit()
        {
            if (m_casters != null)
            {
                foreach (SRayCaster child in m_casters)
                {
                    if (child != null)
                        child.stopRay();
                }
            }
        }


        void loadPlugins()
        {           
            string pluginPath = "";
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Plugins/Native/x64/";
            else
                pluginPath = "/Plugins/";
                      
            // Default plugin to be loaded
            m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaMiscCollision.dll");
            m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaSparseSolver.dll");
            
            m_impl.loadPlugin(Application.dataPath + pluginPath + "Geomagic.dll");
            m_impl.loadPlugin(Application.dataPath + pluginPath + "SofaCarving.dll");
        }

        public void initHierarchy()
        {
            if (m_hierarchyPtr == null)
                m_hierarchyPtr = new SObjectHierarchy(this);

            if (m_hierarchyPtr.m_objects == null)
                m_hierarchyPtr.m_objects = new List<SBaseObject>();
        }

        /// Internal Method to init the SofaContext object
        void init()
        {
            if (m_log)
                Debug.Log("## SofaContext ## init ");

            if (m_impl == null)
            {
                m_impl = new SofaContextAPI();

                if (m_hierarchyPtr == null)
                    initHierarchy();

                catchSofaMessages();
                loadPlugins();

                m_impl.start();

                if (m_filename != "")
                {
                    if (m_log)
                        Debug.Log("## SofaContext ## m_filename " + m_filename);

                    if (!File.Exists(Application.dataPath + m_filename))
                    {
                        int pos = m_filename.IndexOf("Assets", 0);
                        if (pos > 0)
                        {
                            m_filename = m_filename.Substring(pos + 6); // remove all path until Assets/ to make it relative
                        }

                        // Fix due to change of scene folder:
                        int pos2 = m_filename.IndexOf("SofaUnity", 0);
                        if (pos2 < 0)
                            m_filename = "/SofaUnity/" + m_filename;
                    }

                    if (!File.Exists(Application.dataPath + m_filename)) // still not found
                    {
                        Debug.LogError("Error file can't be found: " + Application.dataPath + m_filename);
                        return;
                    }

                    // load the file
                    m_impl.loadScene(Application.dataPath + m_filename);

                    // Set counter of object creation to 0
                    m_hierarchyPtr.cptCreated = 0;
                    m_hierarchyPtr.m_nbrObject = m_impl.getNumberObjects();

                    if (m_log)
                        Debug.Log("init - m_nbrObject: " + m_hierarchyPtr.m_nbrObject);

                    //m_timeStep = m_impl.timeStep;
                    //m_gravity = m_impl.getGravity();
                }
                //else
                //{
                m_impl.timeStep = m_timeStep;
                m_impl.setGravity(m_gravity);
                //}              
            }

            
        }
        
        // Update is called once per fix frame
        void FixedUpdate()
        {

        }

        private float nextUpdate = 0.0f;

        public bool testAsync = false;

        // Update is called once per frame
        void Update()
        {
            // only if scene is playing or if sofa is running
            if (IsSofaUpdating == false || Application.isPlaying == false) return; 

            if (testAsync)
                updateImplASync();
            else
                updateImplSync();

            // log sofa messages
            catchSofaMessages();

            // counter if need to freeze the simulation for several iterations
            cptBreaker++;
            if (cptBreaker == countDownBreaker)
            {
                cptBreaker = 0;
                breakerActivated = false;
            }

        }


        protected void updateImplSync()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate += m_timeStep;

                m_impl.step();

                if (m_hierarchyPtr.m_objects != null)
                {
                    // Set all objects to dirty to force and update.
                    foreach (SBaseObject child in m_hierarchyPtr.m_objects)
                    {
                        child.setDirty();
                    }
                }
            }
        }

        protected void updateImplASync()
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
                    if (m_hierarchyPtr.m_objects != null)
                    {
                        // Set all objects to dirty to force and update.
                        foreach (SBaseObject child in m_hierarchyPtr.m_objects)
                        {
                            //child.setDirty();
                            child.updateImpl();
                            //Debug.Log(child.name);
                        }
                    }

                    // update the ray casters
                    if (m_casters != null)
                    {
                        // Set all objects to dirty to force and update.
                        foreach (SRayCaster child in m_casters)
                        {
                            //child.setDirty();
                            child.updateImpl();
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
        protected void catchSofaMessages()
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

        protected void reloadFilename()
        {
            // stop simulation first
            m_impl.stop();
            m_impl.freeGlutGlew();

            // clear hierarchy
            //m_hierarchyPtr.clearHierarchy();
            List<GameObject> childToDestroy = new List<GameObject>();
            foreach (Transform child in this.transform)
            {
                SBaseObject obj = child.GetComponent<SBaseObject>();
                if (obj != null)
                {
                    childToDestroy.Add(child.gameObject);
                }
            }

            foreach (GameObject child in childToDestroy)
                DestroyImmediate(child);


            // destroy sofaContext
            m_impl.Dispose();

            // recreate hierarchy
            m_hierarchyPtr = new SObjectHierarchy(this);

            // recreate sofaContext
            m_impl = new SofaContextAPI();
            loadPlugins();
            m_impl.start();
            
            // loadFilename
            loadFilename();
        }


        /// Method to load a filename and create GameObject per Sofa object found.
        protected void loadFilename()
        {
            m_impl.loadScene(Application.dataPath + m_filename);
            m_hierarchyPtr.m_nbrObject = m_impl.getNumberObjects();

            if (m_log)
                Debug.Log("## SofaContext ## loadFilename - getNumberObjects: " + m_hierarchyPtr.m_nbrObject);

            // Add 1 fictive object to be sure this method reach the end of the object creation loop
            // before calling recreateHiearchy(); As the creation is asynchronous. call countCreated() at the end to compensate that +1.
            m_hierarchyPtr.m_nbrObject += 1; 
            for (int i = 0; i < m_hierarchyPtr.m_nbrObject; ++i)
            {
                string name = m_impl.getObjectName(i);
                string type = m_impl.getObjectType(i);

                GameObject go;
                if (type.Contains("SofaVisual"))
                {
                    go = new GameObject("SVisualMesh - " + name);
                    go.AddComponent<SVisualMesh>();
                }
                else if (type.Contains("SofaDeformable3DObject"))
                {
                    go = new GameObject("SMesh - " + name);
                    go.AddComponent<SDeformableMesh>();
                }
                else if (type.Contains("SofaRigid3DObject"))
                {
                    go = new GameObject("SMesh - " + name);
                    go.AddComponent<SRigidMesh>();
                }
                else if (type.Contains("SofaComponentObject"))
                {
                    go = new GameObject("SComponent - " + name);
                    go.AddComponent<SComponentObject>();
                }
                else
                    continue;

                go.transform.parent = this.gameObject.transform;
            }
            
            countCreated();
        }


        /// Count the number of object created, when all created, will move them to recreate Sofa hierarchy.
        public void countCreated()
        {
            m_hierarchyPtr.cptCreated++;
            
            if (m_hierarchyPtr.cptCreated == m_hierarchyPtr.m_nbrObject)
                m_hierarchyPtr.recreateHiearchy();
        }

        
    }
}
