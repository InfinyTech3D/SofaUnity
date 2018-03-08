using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;
using UnityEditor;

public class SSphereCollisionModel : MonoBehaviour
{
    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaContext m_context = null;

    [SerializeField]
    protected float m_radius = 1.0f;
    [SerializeField]
    protected float m_factor = 1.0f;
    [SerializeField]
    protected bool m_activated = true;

    [SerializeField]
    protected float m_stiffness = 100.0f;

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

    public float radius
    {
        get { return m_radius; }
        set
        {
            if (value != m_radius)
            {
                m_radius = value;
                if (m_impl != null)
                    m_impl.setFloatValue("radius", m_radius);
            }
            else
                m_radius = value;
        }
    }

    public bool activated
    {
        get { return m_activated; }
        set { m_activated = value; }
    }

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


    /// Pointer to the corresponding SOFA API object
    protected SofaCustomMesh m_impl = null;    

    protected List<Vector3> m_keyVertices = null;
    protected Vector3[] m_centers = null;
    public int nbrSpheres
    {
        get {
            if (m_centers != null)
                return m_centers.Length;
            else
                return 0;
        }
    }

    /// Parameter to activate logging of this Sofa GameObject
    protected bool m_log = false;

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        if (m_log)
            Debug.Log("UNITY_EDITOR - SBaseMesh::Awake - " + this.name);

        // First load the Sofa context and create the object.
        loadContext();

        // Call a post process method for additional codes.
        awakePostProcess();
    }

    protected bool loadContext()
    {
        if (m_log)
            Debug.Log("UNITY_EDITOR - SBaseObject::loadContext");

        // Search for SofaContext
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_context = _contextObject.GetComponent<SofaContext>();

            if (m_log)
                Debug.Log("this.name : " + this.name);

            // Really Create the gameObject linked to sofaObject
            createObject();

            // Increment counter if objectis created from loading scene process
            m_context.countCreated();

            // Increment the context object counter for names.
            m_context.objectcpt = m_context.objectcpt + 1;

            return true;
        }
        else
        {
            Debug.LogError("SSphereCollisionModel::loadContext - No SofaContext found.");
            return false;
        }    
    }

   

    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
    protected virtual void createObject()
    {
        // Get access to the sofaContext
        IntPtr _simu = m_context.getSimuContext();

        if (_simu != IntPtr.Zero) // Create the API object for Sofa Regular Grid Mesh
            m_impl = new SofaCustomMesh(_simu, this.name);            

        if (m_impl == null)
        {
            Debug.LogError("SofaCustomMesh:: Object creation failed.");
            return;
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


    // Use this for initialization
    void Start()
    {
        if (m_impl != null)
        {
            m_impl.setFloatValue("contactStiffness", m_stiffness);
            m_impl.setFloatValue("radius", m_radius);
        }
    }

    protected void computeSphereCenters()
    {
        //Debug.Log("keyVertices.Count: " + m_keyVertices.Count);
        if (m_keyVertices == null)
        {
            awakePostProcess();
            return;
        }

        Vector3[] buffer = m_keyVertices.ToArray();
        List<Vector3> bufferTotal = new List<Vector3>();
        for (int i=0; i< buffer.Length; ++i)
        {
            bufferTotal.Add(buffer[i]);
            Vector3 pointA = this.transform.TransformPoint(buffer[i]);
            for (int j = i+1; j < buffer.Length; ++j)
            {
                Vector3 pointB = this.transform.TransformPoint(buffer[j]);
                Vector3 dir = pointB - pointA;
                float dist = dir.magnitude;

                dist = dist * 10;
                int interpol = (int)Math.Floor(dist / m_factor);
                
                if (interpol > 1)
                {
                    float interval = (dist*0.1f) / interpol;
                    //Debug.Log("dist: " + dist + " | interpol: " + interpol + " | from " + dist / m_factor + " | interval: " + interval);
                    
                    dir.Normalize();
                    for (int k = 1; k < interpol; k++)
                    {                        
                        Vector3 newPoint = pointA + dir * interval * k;                        
                        bufferTotal.Add(this.transform.InverseTransformPoint(newPoint));
                    }
                }
            }
        }

        Debug.Log("bufferTotal.Count: " + bufferTotal.Count);
        m_centers = new Vector3[bufferTotal.Count];
        int cpt = 0;
        foreach (Vector3 vert in bufferTotal)
        {
            m_centers[cpt] = vert;
            cpt++;
        }

        if (m_impl != null)
            m_impl.setNumberOfVertices(bufferTotal.Count);
    }




    // Update is called once per frame
    void Update()
    {
        if (m_activated && m_impl != null)
        {
            m_impl.updateMesh(this.transform, m_centers, m_context.getScaleUnityToSofa());
        }
    }

    void OnDrawGizmosSelected()
    {
        if (m_centers == null || m_context == null)
            return;

        Gizmos.color = Color.yellow;
        float factor = m_context.getFactorSofaToUnity();
        foreach (Vector3 vert in m_centers)
        {
            Gizmos.DrawSphere(this.transform.TransformPoint(vert), m_radius * factor);
        }
    }
}

