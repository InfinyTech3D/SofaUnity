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
        protected bool m_log = false;

        /// Parameter: Vector representing the gravity force.
        public Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);
        /// Parameter: Float representing the simulation timestep to use.
        public float m_timeStep = 0.02f; // ~ 1/60
        /// Parameter: String representing the Path to the Sofa scene.
        public string m_filename = "";

        
        /// Parameter: Int number of objects created in this context. Used to add count in object name created from Unity.
        int m_objectCpt = 0;

        /// Parameter: Int number of object to be loaded from a Sofa scene.
        protected int m_nbrObject = 0;
        /// Parameter: Internal counter to the number of object created from a Sofa scene.
        int cptCreated = 0;

        /// Dictionary storing the hierarchy of Sofa objects. Key = parent name, value = List of children names.
        protected Dictionary<string, List<string> > hierarchy;


        /// Getter of current Sofa Context API, @see m_impl
        public IntPtr getSimuContext()
        {
            if (m_impl == null)
                init();
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
                        m_filename = value;
                        if (m_impl != null)
                            loadFilename();
                    }
                    else
                        Debug.LogError("Error file doesn't exist.");
                }
            }
        }

        /// Getter/Setter of current objectcpt @see m_objectCpt
        public int objectcpt
        {
            get { return m_objectCpt; }
            set { m_objectCpt = value; }
        }

        /// Getter to the number of object loaded from Sofa Scene.
        public int nbrObject
        {
            get { return m_nbrObject; }
        }



        /// Method called at GameObject creation.
        void Awake()
        {
            // Call the init method to create the Sofa Context
            init();
        }

        // Use this for initialization
        void Start()
        {

        }

        /// Method called at GameObject destruction.
        void OnDestroy()
        {
            if(m_log)
                Debug.Log("SofaContext::OnDestroy stop called.");

            foreach (Transform child in transform)
            {
                //GameObject.Destroy(child.gameObject);
            }
#if !UNITY_EDITOR
            if(m_log)
                Debug.Log("SofaContext::OnDestroy stop called.");

            m_impl.stop();
            m_impl.Dispose();       
#endif
        }

        /// Internal Method to init the SofaContext object
        void init()
        {
            if (m_impl == null)
            {
#if UNITY_EDITOR
                m_impl = new SofaContextAPI();
                m_impl.start();
                if (m_filename != "")
                {
                    if (!File.Exists(Application.dataPath + m_filename))
                    {
                        int pos = m_filename.IndexOf("Assets", 0);
                        if (pos > 0)
                        {
                            m_filename = m_filename.Substring(pos + 6); // remove all path until Assets/
                        }
                    }
                        // load the file
                    m_impl.loadScene(Application.dataPath + m_filename);

                    // Set counter of object creation to 0
                    cptCreated = 0;
                    m_nbrObject = m_impl.getNumberObjects();

                    if (m_log)
                        Debug.Log("init - m_nbrObject: " + m_nbrObject);
                }

                m_impl.timeStep = m_timeStep;
                m_impl.setGravity(m_gravity);
#endif
            }
        }

        // Update is called once per fix frame
        void FixedUpdate()
        {            
                
        }

        void Update()
        {
            m_impl.step();
        }


        /// Method to load a filename and create GameObject per Sofa object found.
        protected void loadFilename()
        {
            m_impl.loadScene(Application.dataPath + m_filename);
            m_nbrObject = m_impl.getNumberObjects();

            if (m_log)
                Debug.Log("loadFilename - getNumberObjects: " + m_nbrObject);

            // Add 1 fictive object to be sure this method reach the end of the object creation loop
            // before calling recreateHiearchy(); As the creation is asynchronous. call countCreated() at the end to compensate that +1.
            m_nbrObject += 1; 
            for (int i = 0; i < m_nbrObject; ++i)
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
                else
                    continue;

                go.transform.parent = this.gameObject.transform;
            }
            
            countCreated();
        }


        /// Count the number of object created, when all created, will move them to recreate Sofa hierarchy.
        public void countCreated()
        {            
            cptCreated++;
            
            if (cptCreated == m_nbrObject)
                recreateHiearchy();
        }

        /// Method to compute the hierarchy of SofaObject using the parent name. Will move all children to recreate the Sofa hierarchy.
        protected void recreateHiearchy()
        {
            if (m_impl == null)
                return;

            hierarchy = new Dictionary<string, List<string> >();
            foreach (Transform child in transform)
            {
                SBaseMesh obj = child.GetComponent<SBaseMesh>();
                if (hierarchy.ContainsKey(obj.parentName()))               
                    hierarchy[obj.parentName()].Add(child.name);
                else
                {
                    List<string> children = new List<string>();
                    children.Add(child.name);
                    hierarchy.Add(obj.parentName(), children);
                }
            }

            foreach (KeyValuePair<string, List<string> > entry in hierarchy)
            {
                List<string> children = entry.Value;

                if (m_log)
                {
                    foreach (string childName in children)
                        Debug.Log("#### Hierarchy: parent: " + entry.Key + " - child: " + childName);
                }

                if (entry.Key != "root")
                moveChildren(entry.Key);
            }
            hierarchy.Clear();
        }


        /// Method to move each children according to its parent name
        protected void moveChildren(string parentName)
        {
            List<string> children = hierarchy[parentName];

            // get parent
            Transform parent = this.transform;
            bool found = false;
            foreach (Transform child in transform)
                if (child.name.Contains(parentName))
                {
                    parent = child.transform;
                    found = true;
                    break;
                }

            if (!found)
                Debug.LogError("Sofacontext::moveChildren parent node not found: " + parentName);


            foreach (string childName in children)
            {
                foreach (Transform child in transform)
                {
                    if (m_log)
                        Debug.Log("name found: " + child.name + " current parent: " + child.transform.parent.name);

                    if (child.name.Contains(childName))
                    {
                        child.transform.parent = parent.transform;
                        break;
                    }
                }
            }
        }
    }
}
