using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

/// <summary>
/// Base class inherite from MonoBehavior that design allow to create a set of sphere collision models
/// This class is a work in progress. 
/// It allows from a Unity GameObject geometry to generate a set of sphere that approximate the object.
/// The spheres are mapped into collision models in Sofa
/// </summary>
[ExecuteInEditMode]
public class SofaSphereCollisionHand : SofaBaseObject
{
    /////////////////////////////////////////////////
    /////   SofaSphereCollisionObject members   /////
    /////////////////////////////////////////////////

    /// Collision sphere radius
    [SerializeField] protected float m_radius = 1.0f;

    /// List of unique vertex that discribe the GameObject geometry
    protected List<Vector3> m_keyVertices = null;

    [SerializeField]
    private List<GameObject> m_capsuleColliderList = new List<GameObject>();
    private List<Vector3> m_pointsList = new List<Vector3>();

    private SofaSphereCollision m_sofaSphereCollision = new SofaSphereCollision();

    /////////////////////////////////////////////////
    /////  SofaSphereCollisionObject public API /////
    /////////////////////////////////////////////////

    /// <summary>
    /// Reference to SofaSphereCollision : commun part of  SofaSphereCollisionHand and SofaSphereCollisionObject
    /// </summary>
    [SerializeField] public SofaSphereCollision SofaSphereCollision
    {
        get => m_sofaSphereCollision;
        set => m_sofaSphereCollision = value;
    }

    /// <summary>
    /// Reference of collider list 
    /// </summary>
    public List<GameObject> CapsuleColliderList
    {
        get => m_capsuleColliderList;
        set => m_capsuleColliderList = value;
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
                if (m_sofaSphereCollision.Impl != null)
                    m_sofaSphereCollision.Impl.SetFloatValue("radius", m_radius * m_sofaContext.GetFactorUnityToSofa(1));
            }
            else
                m_radius = value;
        }
    }




    private void Awake()
    {
        /// Make sure m_sofaSphereCollision is not null
        if (m_sofaSphereCollision == null)
        {
            print("set m_sofaSphereCollision");
            m_sofaSphereCollision = new SofaSphereCollision();
        }
    }

    //////////////////////////////////////////////////
    /////  SofaSphereCollisionObject public API  /////
    //////////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Clear the capsule collider list on start to avoid duplicate 
        m_capsuleColliderList.Clear();

        // Looking for Capsule collider in children
        CapsuleCollider[] colliders = gameObject.GetComponentsInChildren<CapsuleCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            m_capsuleColliderList.Add(colliders[i].gameObject);
        }

        Init_impl();


        if (m_sofaSphereCollision.Impl != null)
        {

            m_sofaSphereCollision.Impl.SetFloatValue("contactStiffness", m_sofaSphereCollision.Stiffness);
            m_sofaSphereCollision.Impl.SetFloatValue("radius", m_radius * m_sofaContext.GetFactorUnityToSofa(1));
        }

    }


    // Update is called once per frame
    void Update()
    {
        UpdatePoints();
        m_sofaSphereCollision.UpdateLoop(transform, m_sofaContext);
    }


    /// Method to draw debug information like the vertex being grabed
    void OnDrawGizmosSelected()
    {
        //for now let's assume that all sphere has the same size...
        m_sofaSphereCollision.DrawGizmos(m_radius, transform, m_sofaContext);

        // ...in case separate each sphere size in Sofa use this instead

        //for (int i = 0; i < m_centers.Length; i++)
        //{
        //    if (i % 2 == 0)
        //    {
        //        m_radius = m_capsuleColliderList[i / 2].GetComponent<CapsuleCollider>().radius;
        //    }

        //    Gizmos.DrawSphere(this.transform.TransformPoint(m_centers[i]), m_radius * 100/**m_sofaContext.GetFactorSofaToUnity(1)*/);
        //}
    }


    //////////////////////////////////////////////////
    ///// SofaSphereCollisionObject internal API /////
    //////////////////////////////////////////////////

    /// Method called by @sa CreateObject method to really create the MechanicalObject and the sphere collision model on SOFA side
    protected override void Create_impl()
    {
        //m_sofaSphereCollision.CreateImpl(m_uniqueNameId, m_parentName, m_sofaContext, transform, enabled, m_isCreated);

        SofaLog("####### SofaSphereCollisionObject::Create_impl: " + UniqueNameId);
        if (m_sofaSphereCollision.Impl == null)
        {
            m_sofaSphereCollision.Impl = new SofaCustomMeshAPI(m_sofaContext.GetSimuContext(), m_parentName, m_uniqueNameId);

            if (m_sofaSphereCollision.Impl == null || !m_sofaSphereCollision.Impl.m_isCreated)
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
                    else if (_col)
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

    /// <summary>
    /// Update list of Sphere position depending on the capsule collider  
    /// </summary>
    private void UpdatePoints()
    {
        int j = 0;
        for (int i = 0; i < m_capsuleColliderList.Count; i++)
        {
            var col = m_capsuleColliderList[i].GetComponent<CapsuleCollider>();

            var direction = new Vector3 { [col.direction] = 1 };
            var offset = col.height / 2 - col.radius;

            var localPoint0 = col.center - direction * offset;
            var localPoint1 = col.center + direction * offset;

            var point0 = m_capsuleColliderList[i].transform.TransformPoint(localPoint0);
            var point1 = m_capsuleColliderList[i].transform.TransformPoint(localPoint1);

            m_sofaSphereCollision.Centers[j] = transform.InverseTransformPoint(point0);
            j++;
            m_sofaSphereCollision.Centers[j] = transform.InverseTransformPoint(point1);
            j++;
        }
    }

    /// <summary>
    /// Define the sphere position for the first iteration ; 
    /// The list is empty on start.
    /// </summary>
    /// <returns></returns>
    private Vector3[] DefinePoints()
    {
        for (int i = 0; i < m_capsuleColliderList.Count; i++)
        {
            var col = m_capsuleColliderList[i].GetComponent<CapsuleCollider>();

            var direction = new Vector3 { [col.direction] = 1 };
            var offset = col.height / 2 - col.radius;

            var localPoint0 = col.center - direction * offset;
            var localPoint1 = col.center + direction * offset;

            var point0 = m_capsuleColliderList[i].transform.TransformPoint(localPoint0);
            var point1 = m_capsuleColliderList[i].transform.TransformPoint(localPoint1);

            m_pointsList.Add(point0);
            m_pointsList.Add(point1);
        }

        Vector3[] pointList = m_pointsList.ToArray();

        return pointList;
    }

    /// Method called by @sa Awake() method. As post process method after creation.
    protected override void Init_impl()
    {
        m_keyVertices = new List<Vector3>();

        Mesh m_mesh = this.GetComponent<MeshFilter>().sharedMesh;

        if (m_mesh == null) // look for a mesh in the current gameObject
        {
            Debug.LogError("SofaSphereCollisionObject::AwakePostProcess Error No valid Meshfilter found in current gameObject.");
            return;
        }

        Vector3[] vertices = DefinePoints();
        //Vector3[] vertices = m_mesh.vertices;
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
                m_keyVertices.Add(vertices[i]);
        }
        ComputeSphereCenters();
    }




    /// Method to compute the centers according to the @see m_keyVertices and @sa m_factor
    protected void ComputeSphereCenters()
    {
        if (m_keyVertices == null)
        {
            AwakePostProcess();
            return;
        }


        m_sofaSphereCollision.Centers = new Vector3[m_keyVertices.Count];
        int cpt = 0;
        foreach (Vector3 vert in m_keyVertices)
        {
            m_sofaSphereCollision.Centers[cpt] = transform.InverseTransformPoint(vert);
            cpt++;
        }

        if (cpt >= 1000) // too much spheres
        {
            Debug.LogWarning("This factor create too many spheres: " + cpt + " Change the factor.");
            return;
        }


        if (m_sofaSphereCollision.Impl != null)
            m_sofaSphereCollision.Impl.SetNumberOfVertices(m_keyVertices.Count);
    }



}


