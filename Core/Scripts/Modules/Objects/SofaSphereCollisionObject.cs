using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Base class inherite from MonoBehavior that design allow to create a set of sphere collision models
    /// This class is a work in progress. 
    /// It allows from a Unity GameObject geometry to generate a set of sphere that approximate the object.
    /// The spheres are mapped into collision models in Sofa
    /// </summary>
    [ExecuteInEditMode]
    public class SofaSphereCollisionObject : SofaBaseObject
    {
        /////////////////////////////////////////////////
        /////   SofaSphereCollisionObject members   /////
        /////////////////////////////////////////////////

        private SofaSphereCollision m_sofaSphereCollision = new SofaSphereCollision();

        /// <summary>
        /// Reference to SofaSphereCollision : commun part of  SofaSphereCollisionHand and SofaSphereCollisionObject
        /// </summary>
        [SerializeField]
        public SofaSphereCollision SofaSphereCollision
        {
            get => m_sofaSphereCollision;
            set => m_sofaSphereCollision = value;
        }
        
        /// Discretisation factor to compute the number of sphere to create on the object.
        [SerializeField] protected float m_factor = 50.0f;


        /// List of unique vertex that discribe the GameObject geometry
        protected List<Vector3> m_keyVertices = new List<Vector3>();
        protected List<int> m_keyVerticesIndex = new List<int>();

        private Mesh m_mesh = null;
        public SofaMesh m_sofaMesh = null;
        public string m_sofaMeshName = ""; // to automatically find it TODO
        public SofaCollisionModel m_sphereModel = null;

        /// Parameter bool to store information if vec3 or rigid are parsed.
        private bool m_ready = false;

        /////////////////////////////////////////////////
        /////  SofaSphereCollisionObject public API /////
        /////////////////////////////////////////////////

        /// Getter/Setter of the parameter @see m_factor       
        public float Factor
        {
            get { return m_factor; }
            set
            {
                if (value != m_factor)
                {
                    m_factor = value;
                    //ComputeSphereCenters();
                }
                else
                    m_factor = value;
            }
        }

        //////////////////////////////////////////////////
        /////  SofaSphereCollisionObject public API  /////
        //////////////////////////////////////////////////

        // Use this for initialization
        void Start()
        {
            Init_impl();
        }

        void OnDestroy()
        {
            if (m_sofaSphereCollision.m_mecaObj != null)
            {
                Debug.Log("SofaSphereCollisionObject::OnDestroy, remove listener for: " + m_sofaSphereCollision.m_mecaObj.UniqueNameId);
                m_sofaSphereCollision.m_mecaObj.RemoveListener();
            }

            //if (m_sofaSphereCollision.Impl != null)
            //{
            //    m_sofaSphereCollision.Impl.Destroy();
            //}
        }


        // Update is called once per frame
        void Update()
        {
            if (!m_ready)
                return;

            // Update local key vertices position from unity mesh
            UpdateKeyVertices();
            // send world key vertices position to sofa (transform in sofa will be done in SofaCustomMeshAPI.UpdateMesh() method)
            //m_sofaSphereCollision.UpdateLoop(transform, m_sofaContext.transform);

            m_sofaSphereCollision.UpdateLoop();
        }


        /// Method to draw debug information like the vertex being grabed
        void OnDrawGizmosSelected()
        {
            m_sofaSphereCollision.DrawSphereGizmos();
        }


        //////////////////////////////////////////////////
        ///// SofaSphereCollisionObject internal API /////
        //////////////////////////////////////////////////

        /// Method called by @sa CreateObject method to really create the MechanicalObject and the sphere collision model on SOFA side
        protected override void Create_impl()
        {
            // Do not use this method for now.
            return;


            SofaLog("####### SofaSphereCollisionObject::Create_impl: " + UniqueNameId);
            
            if (!m_sofaSphereCollision.isCreated())
            {
                bool resCreation = m_sofaSphereCollision.CreateSofaSphereCollisionObject(m_sofaContext, m_parentName, m_uniqueNameId); 
                if (!resCreation)
                {
                    SofaLog("SofaSphereCollisionObject:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                    return;
                }                
                else
                {
                    m_isCreated = true;
                    foreach (Transform child in this.transform)
                    {
                        SofaMesh _mesh = child.gameObject.GetComponent<SofaMesh>();
                        SofaCollisionModel _col = child.gameObject.GetComponent<SofaCollisionModel>();
                        if (_mesh)
                        {
                            Debug.Log("SofaMesh set: " + _mesh.DisplayName);
                            m_sofaSphereCollision.Impl.SetMeshNameID(_mesh.UniqueNameId);
                            m_sofaSphereCollision.m_mecaObj = _mesh;
                            m_sofaSphereCollision.m_mecaObj.AddListener();
                        }
                        if (_col)
                        {
                            Debug.Log("SofaCollisionModel set: " + _col.DisplayName);
                            m_sofaSphereCollision.Impl.SetCollisionNameID(_col.UniqueNameId);
                            m_sofaSphereCollision.m_sphereModel = _col;
                        }
                    }
                }
            }
            else
                SofaLog("SofaSphereCollisionObject::Create_impl, SofaCustomMeshAPI already created: " + UniqueNameId, 1);
        }

        /// Method called by @sa Reconnect() method from SofaContext when scene is resctructed/reloaded.
        protected override void Reconnect_impl()
        {
            // nothing different.
            Create_impl();
        }


        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void Init_impl()
        {
            if (m_sofaMeshName.Length > 0)
            {
                SofaMesh[] meshes = GameObject.FindObjectsByType<SofaMesh>(FindObjectsSortMode.None);
                Debug.Log("Nbr Mesh: " + meshes.Length);
                foreach (SofaMesh mesh in meshes)
                {
                    if (mesh.UniqueNameId.Contains(m_sofaMeshName))
                        m_sofaMesh = mesh;
                }
            }

            if (m_sofaMesh == null)
            {
                Debug.LogError("m_sofaMesh is not set.");
                m_ready = false;
                return;
            }

            if (m_sphereModel == null)
            {
                Debug.LogError("m_sphereModel is not set.");
                m_ready = false;
                return;
            }


            // First compute unity meshfilter unique position into m_keyVertices.
            CreateKeyVertices();

            // Link to existing Mesh and CollisionModel in Sofa scene
            m_sofaSphereCollision.LinkSofaSphereCollisionObject(m_sofaMesh, m_sphereModel);

            // First time define the list of center and will check if SOFA buffer is correctly allocated
            m_sofaSphereCollision.CreateSphereCenters(m_keyVertices.ToArray()); // store spheres center in world coordinates
            m_isCreated = true;
            m_ready = true;
        }


        private void CreateKeyVertices()
        {
            m_keyVertices.Clear();
            m_keyVerticesIndex.Clear();

            m_mesh = this.GetComponent<MeshFilter>().sharedMesh;

            if (m_mesh == null) // look for a mesh in the current gameObject
            {
                Debug.LogError("SofaSphereCollisionObject::Init_impl Error No valid Meshfilter found in current gameObject.");
                return;
            }

            Vector3[] vertices = m_mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                bool found = false;
                foreach (Vector3 vert in m_keyVertices)
                {
                    if (vert == vertices[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    m_keyVertices.Add(vertices[i]);
                    m_keyVerticesIndex.Add(i);
                }
            }

            // convert to world position
            //for (int i = 0; i < m_keyVertices.Count; i++)
            //{
            //    m_keyVertices[i] = this.transform.TransformPoint(m_keyVertices[i]);
            //}
        }

        private void UpdateKeyVertices()
        {
            if (!m_mesh)
                return;

            
            Vector3[] vertices = m_mesh.vertices;
            for (int i = 0; i < m_keyVerticesIndex.Count; i++)
            {
                m_sofaSphereCollision.Centers[i] = this.transform.TransformPoint(vertices[m_keyVerticesIndex[i]]);
            }
        }



        /// Method to compute the centers according to the @see m_keyVertices and @sa m_factor
    //    protected void ComputeUnityPositions()
    //    {
    //        if (m_keyVertices == null)
    //        {
    //            AwakePostProcess();
    //            return;
    //        }

    //        //Debug.Log("keyVertices.Count: " + m_keyVertices.Count);
    //        Vector3[] buffer = m_keyVertices.ToArray();

    //        List<Vector3> bufferTotal = new List<Vector3>();
    //        int cpt = 0;

    //        float contextFactor = m_sofaContext.GetFactorUnityToSofa();
    //        for (int i = 0; i < buffer.Length; ++i)
    //        {
    //            bufferTotal.Add(buffer[i]);
    //            cpt++;
    //            Vector3 pointA = this.transform.TransformPoint(buffer[i]);
    //            for (int j = i + 1; j < buffer.Length; ++j)
    //            {
    //                Vector3 pointB = this.transform.TransformPoint(buffer[j]);
    //                Vector3 dir = pointB - pointA;
    //                float dist = dir.magnitude;

    //                dist = dist * 10;

    //                int interpol = (int)Math.Floor((dist * contextFactor) / m_factor);

    //                if (interpol > 1)
    //                {
    //                    float interval = (dist * 0.1f) / interpol;
    //                    //Debug.Log("dist: " + dist + " | interpol: " + interpol + " | from " + dist / m_factor + " | interval: " + interval);

    //                    dir.Normalize();
    //                    for (int k = 1; k < interpol; k++)
    //                    {
    //                        Vector3 newPoint = pointA + dir * interval * k;

    //                        if (cpt >= 1000)
    //                            break;

    //                        bufferTotal.Add(this.transform.InverseTransformPoint(newPoint));
    //                        cpt++;
    //                    }
    //                }

    //                if (cpt >= 1000)
    //                    break;
    //            }

    //            if (cpt >= 1000)
    //                break;
    //        }

    //        if (m_log)
    //            Debug.Log("bufferTotal.Count: " + bufferTotal.Count);

    //        m_centers = new Vector3[bufferTotal.Count];
    //        cpt = 0;
    //        foreach (Vector3 vert in bufferTotal)
    //        {
    //            m_centers[cpt] = vert;
    //            cpt++;
    //        }

    //        if (cpt >= 1000) // too much spheres
    //        {
    //            Debug.LogWarning("This factor create too many spheres: " + cpt + " Change the factor.");
    //            return;
    //        }


    //        if (m_impl != null)
    //            m_impl.SetNumberOfVertices(bufferTotal.Count);
    //    }

    }

}
