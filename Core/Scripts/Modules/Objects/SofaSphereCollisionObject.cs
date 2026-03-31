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

        [SerializeField]
        public bool m_startOnPlay = true;

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
            if (m_sofaSphereCollision.Impl != null)
            {
                Init_impl();

                m_sofaSphereCollision.Impl.SetFloatValue("contactStiffness", m_sofaSphereCollision.Stiffness);
                m_sofaSphereCollision.Impl.SetFloatValue("radius", m_sofaSphereCollision.Radius);
            }

        }


        // Update is called once per frame
        void Update()
        {
            if (!m_isCreated)
                return;

            // Update local key vertices position from unity mesh
            UpdateKeyVertices();
            // send world key vertices position to sofa (transform in sofa will be done in SofaCustomMeshAPI.UpdateMesh() method)
            m_sofaSphereCollision.UpdateLoop(transform, m_sofaContext.transform);
        }


        /// Method to draw debug information like the vertex being grabed
        void OnDrawGizmosSelected()
        {
            m_sofaSphereCollision.DrawGizmos(m_sofaSphereCollision.Radius, transform);
        }


        //////////////////////////////////////////////////
        ///// SofaSphereCollisionObject internal API /////
        //////////////////////////////////////////////////

        /// Method called by @sa CreateObject method to really create the MechanicalObject and the sphere collision model on SOFA side
        protected override void Create_impl()
        {
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
                            m_sofaSphereCollision.Impl.SetMeshNameID(_mesh.UniqueNameId);
                        }
                        if (_col)
                        {
                            m_sofaSphereCollision.Impl.SetCollisionNameID(_col.UniqueNameId);
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
            Debug.Log("####### SofaSphereCollisionObject::Init_impl: " + UniqueNameId);
            // First compute unity meshfilter unique position into m_keyVertices.
            CreateKeyVertices();

            // Then copy them into m_centers
            m_sofaSphereCollision.CreateSphereCenters(m_keyVertices.ToArray());
        }


        private void CreateKeyVertices()
        {
            m_keyVertices.Clear();

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
                m_sofaSphereCollision.Centers[i] = /*this.transform.TransformPoint*/(vertices[m_keyVerticesIndex[i]]);
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
