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
public class SofaSphereCollisionModel : MonoBehaviour
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaCustomMeshAPI m_impl = null;


    /// Parameter to activate logging of this Sofa GameObject
    protected bool m_log = false;

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
    public bool m_startOnPlay = true;

    ////////////////////////////////////////////
    /////       Object creation API        /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        if (m_log)
            Debug.Log("UNITY_EDITOR - SofaSphereCollisionModel::Awake - " + this.name);

        if (!m_startOnPlay)
            return;

        // First load the Sofa context and create the object.
        loadContext();

        // Call a post process method for additional codes.
        awakePostProcess();
    }


    protected bool loadContext()
    {
        if (m_log)
            Debug.Log("UNITY_EDITOR - SofaSphereCollisionModel::loadContext");

        // Search for SofaContext
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_sofaContext = _contextObject.GetComponent<SofaContext>();

            if (m_sofaContext == null)
            {
                Debug.LogError("Error SofaSphereCollisionModel::loadContext: Context not found");
                return false;
            }

            if (m_log)
                Debug.Log("this.name : " + this.name);

            // Really Create the gameObject linked to sofaObject
            createObject();

            // Increment counter if objectis created from loading scene process
            m_sofaContext.countCreated();

            // Increment the context object counter for names.
            m_sofaContext.objectcpt = m_sofaContext.objectcpt + 1;

            return true;
        }
        else
        {
            Debug.LogError("SofaSphereCollisionModel::loadContext - No SofaContext found.");
            return false;
        }    
    }

    
    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
    protected virtual void createObject()
    {
        if (m_log)
            Debug.Log("UNITY_EDITOR - SofaSphereCollisionModel::createObject");

        // Get access to the sofaContext
        IntPtr _simu = m_sofaContext.getSimuContext();

        if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh
            m_impl = new SofaCustomMeshAPI(_simu, this.name);            

        if (m_impl == null || !m_impl.m_isCreated)
        {
            Debug.LogError("SofaCustomMesh:: Object creation failed: " + this.name);
            this.enabled = false;
        }
    }

    protected virtual void awakePostProcess()
    {
        Mesh m_mesh = this.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = m_mesh.vertices;
        m_keyVertices = new List<Vector3>();

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

        computeSphereCenters();
    }


    public void setSofaContext(SofaUnity.SofaContext _context)
    {
        m_sofaContext = _context;
        if (m_sofaContext == null)
            return;

        // Really Create the gameObject linked to sofaObject
        createObject();

        // Increment counter if objectis created from loading scene process
        m_sofaContext.countCreated();

        // Increment the context object counter for names.
        m_sofaContext.objectcpt = m_sofaContext.objectcpt + 1;

        // Call a post process method for additional codes.
        awakePostProcess();

        if (m_impl != null)
        {
            m_impl.setFloatValue("contactStiffness", m_stiffness);
            m_impl.setFloatValue("radius", m_radius * m_sofaContext.getFactorUnityToSofa(1));
        }
    }


    public void unloadSofaContext()
    {
        m_sofaContext = null;
    }


    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        if (m_impl != null)
        {
            m_impl.setFloatValue("contactStiffness", m_stiffness);
            m_impl.setFloatValue("radius", m_radius * m_sofaContext.getFactorUnityToSofa(1));
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (m_activated && m_impl != null)
        {
            m_impl.updateMesh(this.transform, m_centers, m_sofaContext.transform);
        }
    }

    /// Method to compute the centers according to the @see m_keyVertices and @sa m_factor
    protected void computeSphereCenters()
    {
        if (m_usePositionOnly)
        {
            m_centers = new Vector3[10];
            for (int i=0; i<10; i++)
                m_centers[i] = this.transform.localPosition;

            if (m_impl != null)
                m_impl.setNumberOfVertices(1);

            return;
        }


        if (m_keyVertices == null)
        {
            awakePostProcess();
            return;
        }

        //Debug.Log("keyVertices.Count: " + m_keyVertices.Count);
        Vector3[] buffer = m_keyVertices.ToArray();

        List<Vector3> bufferTotal = new List<Vector3>();
        int cpt = 0;

        float contextFactor = m_sofaContext.getFactorUnityToSofa();
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
            m_impl.setNumberOfVertices(bufferTotal.Count);
    }




    ////////////////////////////////////////////
    /////        Object members API        /////
    ////////////////////////////////////////////

    /// Getter/Setter of the parameter @see m_activated  
    public bool activated
    {
        get { return m_activated; }
        set { m_activated = value; }
    }

    /// Getter/Setter of the parameter @see m_usePositionOnly  
    public bool usePositionOnly
    {
        get { return m_usePositionOnly; }
        set { m_usePositionOnly = value; }
    }

    /// Getter/Setter of the parameter @see m_factor       
    public float factor
    {
        get { return m_factor; }
        set
        {
            if (value != m_factor)
            {
                m_factor = value;
                computeSphereCenters();
            }
            else
                m_factor = value;
        }
    }

    /// Getter/Setter of the parameter @see m_radius     
    public float radius
    {
        get { return m_radius; }
        set
        {
            if (value != m_radius)
            {
                m_radius = value;
                if (m_impl != null)
                    m_impl.setFloatValue("radius", m_radius * m_sofaContext.getFactorUnityToSofa(1));
            }
            else
                m_radius = value;
        }
    }

    /// Getter/Setter of the parameter @see m_stiffness     
    public float stiffness
    {
        get { return m_stiffness; }
        set
        {
            if (value != m_stiffness)
            {
                m_stiffness = value;
                if (m_impl != null)
                    m_impl.setFloatValue("contactStiffness", m_stiffness);
            }
            else
                m_stiffness = value;
        }
    }


    /// Get the number of spheres
    public int nbrSpheres
    {
        get
        {
            if (m_centers != null)
                return m_centers.Length;
            else
                return 0;
        }
    }

    /// Method to draw debug information like the vertex being grabed
    void OnDrawGizmosSelected()
    {
        if (m_centers == null || m_sofaContext == null)
            return;

        Gizmos.color = Color.yellow;
        //float factor = m_sofaContext.getFactorSofaToUnity();
        
        foreach (Vector3 vert in m_centers)
        {
            Gizmos.DrawSphere(this.transform.TransformPoint(vert), m_radius/**m_sofaContext.getFactorSofaToUnity(1)*/);
        }
    }
}

