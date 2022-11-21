using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using SofaUnity;
using System;
using System.Runtime.InteropServices;

[ExecuteInEditMode]
public class SofaVisualModel : MonoBehaviour
{
    /// Member: Unity Mesh object of this GameObject
    protected Mesh m_mesh;
    public string m_uniqName = "Node";
    public SofaUnityRenderer m_renderer = null;


    void Start()
    {
        Debug.Log("Start: " + m_uniqName);
        // Add a MeshFilter to the GameObject
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        //to see it, we have to add a renderer
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();

        if (mr.sharedMaterial == null)
        {
            if (GraphicsSettings.defaultRenderPipeline)
            {
                mr.sharedMaterial = GraphicsSettings.defaultRenderPipeline.defaultMaterial;
            }
            else
            {
                mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }
        }

#if UNITY_EDITOR
        m_mesh = mf.mesh = new Mesh();
#else
        //do this in play mode
        m_mesh = GetComponent<MeshFilter>().mesh;
        if (m_log)
            Debug.Log("SofaBox::Start play mode.");
#endif

        m_mesh.name = m_uniqName;
        int nbrV = sofaVisualModel_getNbVertices(m_renderer.getImpl(), m_uniqName);
        int nbrT = sofaVisualModel_getNbTriangles(m_renderer.getImpl(), m_uniqName);
        Debug.Log("nbrV: " + nbrV);
        Debug.Log("nbrT: " + nbrT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    ///////////////////////////////////////////////////////////
    //////////          API Communication         /////////////
    ///////////////////////////////////////////////////////////

    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getNbVertices(IntPtr obj, string name);

    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getVertices(IntPtr obj, string name, float[] buffer);
    
    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getNormals(IntPtr obj, string name, float[] buffer);
    
    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getTexCoords(IntPtr obj, string name, float[] buffer);
    
    
    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getNbTriangles(IntPtr obj, string name);
    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getTriangles(IntPtr obj, string name, int[] buffer);

    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getNbQuads(IntPtr obj, string name);
    [DllImport("SofaPhysicsAPI")]
    public static extern int sofaVisualModel_getQuads(IntPtr obj, string name, int[] buffer);


}
