using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SofaUnityAPI;


namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SofaContext : MonoBehaviour
    {
        SofaContextAPI m_impl;
        
        void Awake()
        {
            //Debug.Log("SofaContext::Awake called.");

            init();
        }

        // Use this for initialization
        void Start()
        {
            //Debug.Log("SofaContext::Start called.");
            //GL.wireframe = true;
            
            //this.transform.position = new Vector3(0, 10, 0);
        }


        void OnDestroy()
        {
#if !UNITY_EDITOR
            Debug.Log("SofaContext::OnDestroy stop called.");
            m_impl.stop();
            m_impl.Dispose();       
#endif
        }

        void OnPreRender()
        {
            GL.wireframe = true;
        }


        void init()
        {
            if (m_impl == null)
            {                
                m_impl = new SofaContextAPI();
                m_impl.start();
                if (m_filename != "")
                    m_impl.loadScene(m_filename);

                m_impl.setTimeStep(m_timeStep);
                m_impl.setGravity(m_gravity);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Debug.Log("SofaContext::Update called.");
            m_impl.step();           
        }

        int m_objectCpt = 0;
        public int objectcpt
        {
            get { return m_objectCpt; }
            set { m_objectCpt = value; }
        }

        public Vector3 m_gravity = new Vector3(0f, -9.8f, 0f);
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

        public float m_timeStep = 0.02f; // ~ 1/60
        public float timeStep
        {
            get
            {
                return m_timeStep;
            }
            set
            {
                if (m_timeStep != value)
                {
                    m_timeStep = value;
                    if (m_impl != null)
                        m_impl.setTimeStep(m_timeStep);
                }
                //if (lateUpdateHelper != null)
                //{
                //    lateUpdateHelper.m_fixedTimeStep = value;
                //}
                m_timeStep = value;
            }
        }

        public string m_filename = "";
        public string filename
        {
            get { return m_filename; }
            set
            {
                if (value != m_filename)
                {
                    if (File.Exists(value))
                    {
                        m_filename = value;
                        if (m_impl != null)
                        {
                            m_impl.loadScene(m_filename);
                            int res = m_impl.getNumberObjects();
                            Debug.Log("getNumberObjects: " + res);
                            for (int i=0; i<res; ++i)
                            {
                                string name = m_impl.getObjectName(i);
                                string type = m_impl.getObjectType(i);

                                GameObject go;
                                Debug.Log("add Object: " + i);
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

                            recreateHiearchy();
                        }
                    }
                    else
                        Debug.LogError("Error file doesn't exist.");
                }
            }
        }


        public IntPtr getSimuContext()
        {
            if (m_impl == null)
                init();
            return m_impl.getSimuContext();
        }

        protected Dictionary<string, List<string> > hierarchy;

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
                if (entry.Key != "root")
                    moveChildren(entry.Key);
            }

        }

        protected void moveChildren(string currentNode)
        {
            Debug.Log("parent: " + currentNode);
            List<string> children = hierarchy[currentNode];

            // get parent
            Transform parent = this.transform;
            foreach (Transform child in transform)
                if (child.name.Contains(currentNode))
                    parent = child;

            foreach (string childName in children)
            {
                Debug.Log("  - : " + childName);
                foreach (Transform child in transform)
                {
                    if (child.name.Contains(childName))
                        child.transform.parent = parent.transform;
                }
            }
        }

            
    }
}
