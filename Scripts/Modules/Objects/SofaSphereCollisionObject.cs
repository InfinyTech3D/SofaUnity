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
public class SofaSphereCollisionObject : SofaBaseObject
{
    /////////////////////////////////////////////////
    /////   SofaSphereCollisionObject members   /////
    /////////////////////////////////////////////////

    /// Pointer to the corresponding SOFA API object
    protected SofaCustomMeshAPI m_impl = null;

    /// Booleen to activate/unactivate the collision
    [SerializeField] protected bool m_activated = true;
    /// Booleen to activate/unactivate the factor or use unique position
    [SerializeField]
    protected bool m_usePositionOnly = true;
    /// Discretisation factor to compute the number of sphere to create on the object.
    [SerializeField] protected float m_factor = 50.0f;

    /// Collision sphere radius
    [SerializeField] protected float m_radius = 1.0f;
    /// Collision sphere contact stiffness
    [SerializeField] protected float m_stiffness = 100.0f;

    
    /// List of unique vertex that discribe the GameObject geometry
    protected List<Vector3> m_keyVertices = null;

    /// array of vertex corresponding to the sphere centers
    protected Vector3[] m_centers = null;

    [SerializeField]    
    public GameObject parentT = null;

    [SerializeField]
    public bool m_startOnPlay = true;

    /////////////////////////////////////////////////
    /////  SofaSphereCollisionObject public API /////
    /////////////////////////////////////////////////

    /// Getter/Setter of the parameter @see m_activated  
    public bool Activated
    {
        get { return m_activated; }
        set { m_activated = value; }
    }

    /// Getter/Setter of the parameter @see m_usePositionOnly  
    public bool UsePositionOnly
    {
        get { return m_usePositionOnly; }
        set { m_usePositionOnly = value; }
    }

    /// Getter/Setter of the parameter @see m_factor       
    public float Factor
    {
        get { return m_factor; }
        set
        {
            if (value != m_factor)
            {
                m_factor = value;
                ComputeSphereCenters();
            }
            else
                m_factor = value;
        }
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
                    m_impl.SetFloatValue("radius", m_radius * m_sofaContext.GetFactorUnityToSofa(1));
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


    //////////////////////////////////////////////////
    /////  SofaSphereCollisionObject public API  /////
    //////////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        if (m_impl != null)
        {
            Init_impl();

            m_impl.SetFloatValue("contactStiffness", m_stiffness);
            m_impl.SetFloatValue("radius", m_radius * m_sofaContext.GetFactorUnityToSofa(1));
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (parentT != null)
        {
            this.transform.position = parentT.transform.position;
        }

        if (m_activated && m_centers != null)
        {
            m_impl.UpdateMesh(this.transform, m_centers, m_sofaContext.transform);
        }
    }


    /// Method to draw debug information like the vertex being grabed
    void OnDrawGizmosSelected()
    {
        if (m_centers == null || m_sofaContext == null)
            return;

        Gizmos.color = Color.yellow;
        //float factor = m_sofaContext.GetFactorSofaToUnity();

        foreach (Vector3 vert in m_centers)
        {
            Gizmos.DrawSphere(this.transform.TransformPoint(vert), m_radius/**m_sofaContext.GetFactorSofaToUnity(1)*/);
        }
    }


    //////////////////////////////////////////////////
    ///// SofaSphereCollisionObject internal API /////
    //////////////////////////////////////////////////

    /// Method called by @sa CreateObject method to really create the MechanicalObject and the sphere collision model on SOFA side
    protected override void Create_impl()
    {
        SofaLog("####### SofaSphereCollisionObject::Create_impl: " + UniqueNameId);
        if (m_impl == null)
        {
            m_impl = new SofaCustomMeshAPI(m_sofaContext.GetSimuContext(), m_parentName, m_uniqueNameId);

            if (m_impl == null || !m_impl.m_isCreated)
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
                        m_impl.SetMeshNameID(_mesh.UniqueNameId);                        
                    }
                    else if(_col)
                    {
                        m_impl.SetCollisionNameID(_col.UniqueNameId);
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
        m_keyVertices = new List<Vector3>();

        Mesh m_mesh = this.GetComponent<MeshFilter>().sharedMesh;

        if (m_mesh == null) // look for a mesh in the current gameObject
        {
            Debug.LogError("SofaSphereCollisionObject::AwakePostProcess Error No valid Meshfilter found in current gameObject.");
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
                m_keyVertices.Add(vertices[i]);
        }

        ComputeSphereCenters();
    }

    
    

    /// Method to compute the centers according to the @see m_keyVertices and @sa m_factor
    protected void ComputeSphereCenters()
    {
        if (m_usePositionOnly)
        {
            m_centers = new Vector3[10];
            for (int i=0; i<10; i++)
                m_centers[i] = this.transform.InverseTransformPoint(this.transform.localPosition);

            if (m_impl != null)
                m_impl.SetNumberOfVertices(1);

            return;
        }


        if (m_keyVertices == null)
        {
            AwakePostProcess();
            return;
        }

        //Debug.Log("keyVertices.Count: " + m_keyVertices.Count);
        Vector3[] buffer = m_keyVertices.ToArray();

        List<Vector3> bufferTotal = new List<Vector3>();
        int cpt = 0;

        float contextFactor = m_sofaContext.GetFactorUnityToSofa();
        for (int i = 0; i < buffer.Length; ++i)
        {
            bufferTotal.Add(buffer[i]);
            cpt++;
            Vector3 pointA = this.transform.TransformPoint(buffer[i]);
            for (int j = i + 1; j < buffer.Length; ++j)
            {
                Vector3 pointB = this.transform.TransformPoint(buffer[j]);
                Vector3 dir = pointB - pointA;
                float dist = dir.magnitude;

                dist = dist * 10;

                int interpol = (int)Math.Floor((dist * contextFactor) / m_factor);

                if (interpol > 1)
                {
                    float interval = (dist * 0.1f) / interpol;
                    //Debug.Log("dist: " + dist + " | interpol: " + interpol + " | from " + dist / m_factor + " | interval: " + interval);

                    dir.Normalize();
                    for (int k = 1; k < interpol; k++)
                    {
                        Vector3 newPoint = pointA + dir * interval * k;

                        if (cpt >= 1000)
                            break;

                        bufferTotal.Add(this.transform.InverseTransformPoint(newPoint));
                        cpt++;
                    }
                }

                if (cpt >= 1000)
                    break;
            }

            if (cpt >= 1000)
                break;
        }

        if (m_log)
            Debug.Log("bufferTotal.Count: " + bufferTotal.Count);

        m_centers = new Vector3[bufferTotal.Count];
        cpt = 0;
        foreach (Vector3 vert in bufferTotal)
        {
            m_centers[cpt] = vert;
            cpt++;
        }

        if (cpt >= 1000) // too much spheres
        {
            Debug.LogWarning("This factor create too many spheres: " + cpt + " Change the factor.");
            return;
        }


        if (m_impl != null)
            m_impl.SetNumberOfVertices(bufferTotal.Count);
    }

}

