using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Genereric class to factorise commun part on SofaSphereCollisionHand and SofaSphereCollisionObject
    /// </summary>
    public class SofaSphereCollision
    {
        /// Pointer to the corresponding SOFA API object
        protected SofaCustomMeshAPI m_impl = null;

        /// Booleen to activate/unactivate the collision
        [SerializeField] protected bool m_activated = true;

        /// Collision sphere radius
        [SerializeField] protected float m_radius = 1.0f;
        /// Collision sphere contact stiffness
        [SerializeField] protected float m_stiffness = 1000.0f;

        [SerializeField]
        private GameObject parentT = null;

        [SerializeField]
        private bool m_startOnPlay = true;

        private SofaContext m_sofaContext = null;
        public SofaMesh m_mecaObj = null;
        public SofaCollisionModel m_sphereModel = null;

        /// array of vertex corresponding to the sphere centers
        protected Vector3[] m_centers = null;

        /// <summary>
        /// Getter / Setter of sofa implementation 
        /// </summary>
        public SofaCustomMeshAPI Impl
        {
            get => m_impl;
            set => m_impl = value;
        }

        /// <summary>
        /// Getter / Setter of the parent 
        /// </summary>
        public GameObject ParentT
        {
            get => parentT;
            set => parentT = value;
        }

        /// <summary>
        /// Getter / Setter of StartOnPlay attibute
        /// </summary>
        public bool StartOnPlay
        {
            get => m_startOnPlay;
            set => m_startOnPlay = value;
        }

        /// <summary>
        /// Getter / setter for list of points
        /// </summary>
        public Vector3[] Centers
        {
            get => m_centers;
            set => m_centers = value;
        }

        /// Getter/Setter of the parameter @see m_activated  
        public bool Activated
        {
            get { return m_activated; }
            set { m_activated = value; }
        }

        /// Method to know if the SofaCustomMeshAPI for spheres has been created or not
        public bool isCreated()
        {
            return m_impl != null;
        }

        /// Getter/Setter of the parameter @see m_radius     
        public float Radius
        {
            get { return m_radius; }
            set
            {
                if (value != m_radius)
                {
                    m_radius = value;
                    if (m_impl != null)
                        m_impl.SetFloatValue("radius", m_radius);
                }
                else
                    m_radius = value;
            }
        }

        /// Getter/Setter of the parameter @see m_stiffness     
        public float Stiffness
        {
            get { return m_stiffness; }
            set
            {
                if (value != m_stiffness)
                {
                    m_stiffness = value;
                    if (m_impl != null)
                        m_impl.SetFloatValue("contactStiffness", m_stiffness);
                }
                else
                    m_stiffness = value;
            }
        }

        /// Get the number of spheres
        public int NbrSpheres
        {
            get
            {
                if (m_centers != null)
                    return m_centers.Length;
                else
                    return 0;
            }
        }


        public bool CreateSofaSphereCollisionObject(SofaContext simu, string parentName, string uniqNameId)
        {
            if (m_impl == null)
            {
                m_impl = new SofaCustomMeshAPI(simu.GetSimuContext(), parentName, uniqNameId);
                m_sofaContext = simu;
            }

            if (m_impl == null || !m_impl.m_isCreated)
            {
                Debug.LogError("SofaSphereCollisionObject:: Object creation failed: " + uniqNameId);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CreateSphereCenters(Vector3[] values)
        {
            Debug.Log("CreateSphereCenters: " + values.Length);
            m_centers = values;
            //if (m_impl != null)
            //{
            //    m_impl.SetNumberOfVertices(m_centers.Length);
            //    SofaDataVectorDouble radii = (SofaDataVectorDouble)m_sphereModel.m_dataArchiver.GetVectorData("listRadius");
            //    Debug.Log("CreateSphereCenters: sofa data listRadius size: " + radii.GetSize());
            //    float[] raddiValues = new float[m_centers.Length];
            //    for (int i = 0; i < m_centers.Length; ++i)
            //    {
            //        raddiValues[i] = m_radius;
            //    }
            //    radii.SetValue(raddiValues, m_centers.Length);
            //}
        }

        /// <summary>
        /// Update position of sphere depending on parent position 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="ctxt"></param>
        public void UpdateLoop(Transform sphereObjTransform, Transform sofaCtxtTransform)
        {
            if (m_impl == null)
            {
                Debug.LogError("SofaSphereCollisionObject:: UpdateLoop failed. SofaCustomMeshAPI for spheres not created yet.");
                return; // SofaCustomMeshAPI for spheres not created yet 
            }

            if (parentT != null)
            {
                sphereObjTransform.position = parentT.transform.position;
            }

            if (m_activated && m_centers != null)
            {
                m_impl.UpdateMesh(sphereObjTransform, m_centers, sofaCtxtTransform);
            }
        }



        /// <summary>
        /// Draw spheres on unity side using Gizmo to know where collision happend 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="transform"></param>
        /// <param name="ctxt"></param>
        public void DrawGizmos(float radius, Transform transform)
        {
            if (m_centers == null)
                return;

            Gizmos.color = Color.yellow;

            foreach (Vector3 vert in m_centers)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vert), radius);
            }
        }

    }
}
