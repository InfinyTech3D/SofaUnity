using UnityEngine;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Specific class for a Visual Mesh, inherite from SofaBaseMesh 
    /// This class will add a meshRenderer and create a SofaMesh API object to load the topology from Sofa Object.
    /// </summary>
    [ExecuteInEditMode]
    class SofaVisualMesh : SofaMeshObject
    {
        /////////////////////////////////////////////
        //////    SofaVisualMesh API members    /////
        /////////////////////////////////////////////



        /////////////////////////////////////////////
        //////   SofaVisualMesh API accessors   /////
        /////////////////////////////////////////////


        /////////////////////////////////////////////
        //////   SofaVisualMesh internal API    /////
        /////////////////////////////////////////////

        /// Method called by @sa Awake() method. As post process method after creation.
        //protected override void awakePostProcess()
        //{
        //    // Call SofaBaseMesh.awakePostProcess()
        //    base.awakePostProcess();

        //    //to see it, we have to add a renderer
        //    MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        //    if (mr == null)
        //        mr = gameObject.AddComponent<MeshRenderer>();

        //    if (mr.sharedMaterial == null)
        //    {
        //            mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
        //    }

        //    MeshCollider collid = gameObject.GetComponent<MeshCollider>();
        //    if (collid == null)
        //        gameObject.AddComponent<MeshCollider>();
        //}

        //Material m_currentMaterial = null;
        //public bool m_isWireframe = false;
        //public void ShowWireframe(bool value)
        //{
        //    Debug.Log("ShowWireframe: " + value);
        //    if (value)
        //    {
        //        Material wireMaterial = (Material)Resources.Load("Wireframe", typeof(Material));
        //        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        //        if (mr != null)
        //        {
        //            if (m_currentMaterial == null)
        //                m_currentMaterial = mr.sharedMaterial;
        //            mr.sharedMaterial = wireMaterial;
        //            m_isWireframe = true;
        //        }
        //    }
        //    else
        //    {
        //        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        //        if (mr != null)
        //        {
        //            if (m_currentMaterial != null)
        //                mr.sharedMaterial = m_currentMaterial;
        //            else
        //                mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
        //            m_isWireframe = false;
        //        }
        //    }
        //}

        //public bool m_isSelected = false;
        //private void OnTriggerEnter(Collider other)
        //{
        //   // if (enter)
        //    {
        //        m_isSelected = true;
        //    }
        //}

        
    }
}
