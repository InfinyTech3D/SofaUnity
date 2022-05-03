using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa FEM component 
    /// Only Tetrahedron and Triangle FEM handle yet.
    /// </summary>
    public class SofaFEMForceField : SofaBaseComponent
    {
        ////////////////////////////////////////////
        //////    SofaFEMForceField members    /////
        ////////////////////////////////////////////

        /// Pointer to the SofaMesh this FEM is related to.
        private SofaMesh m_sofaMesh = null;

        /// Pointer to the MeshRenderer to display the FEM structure.
        private MeshRenderer m_renderer;

        /// Bool to store the info is this mesh has been init.
        private bool m_meshInit = false;

        /// Bool to store the info if renderer was enabled or not.
        private bool m_oldRendererStatus = false;

        private int m_trackedTopologyRevision = 0;

        

        ////////////////////////////////////////////
        //////      SofaFEMForceField API      /////
        ////////////////////////////////////////////

        /// Method called by @sa SofaBaseComponent::CreateSofaAPI() method. to add more creation step
        protected override void CreateSofaAPI_Impl()
        {            
            SofaLog("SofaFEMForceField::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_isCustom);

            // Add a MeshFilter to the GameObject
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();

            //to see it, we have to add a renderer
            m_renderer = gameObject.GetComponent<MeshRenderer>();
            if (m_renderer == null)
            {
                m_renderer = gameObject.AddComponent<MeshRenderer>();
                m_renderer.enabled = false;
                m_oldRendererStatus = false;
            }

            if (m_renderer.sharedMaterial == null)
            {
                if (GraphicsSettings.defaultRenderPipeline)
                {
                    m_renderer.sharedMaterial = GraphicsSettings.defaultRenderPipeline.defaultMaterial;
                }
                else
                {
                    m_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
                }
            }

            m_meshInit = false;
        }



        /// Method called by @sa SofaBaseComponent::Start() method. to add more init steps
        protected override void Init_impl()
        {
            if (m_meshInit == false)
            {
                m_sofaMesh = m_ownerNode.GetSofaMesh();
                if (m_sofaMesh == null)
                    m_sofaMesh = m_ownerNode.FindSofaMesh();

                if (m_sofaMesh)
                {
                    m_meshInit = true;

                    //Only do this in the editor
                    MeshFilter mf = GetComponent<MeshFilter>();
                    mf.mesh = m_sofaMesh.SofaMeshTopology.m_mesh;
                    m_trackedTopologyRevision = m_sofaMesh.GetTopologyRevision();
                }
            }
        }


        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        
        /// Method called by @sa Update() method.        
        protected override void Update_impl()
        {
            if (m_renderer.enabled != m_oldRendererStatus)
            {
                m_oldRendererStatus = m_renderer.enabled;
                if (m_oldRendererStatus)
                {
                    m_sofaMesh.AddListener();
                }
                else
                {
                    m_sofaMesh.RemoveListener();
                }
            }


            if (m_renderer.enabled)
            {
                MeshFilter mf = GetComponent<MeshFilter>();
                int tmpRev = m_sofaMesh.GetTopologyRevision();
                if (m_trackedTopologyRevision != tmpRev)
                {
                    m_trackedTopologyRevision = tmpRev;
                    mf.mesh.triangles = m_sofaMesh.SofaMeshTopology.m_mesh.triangles;
                }

                mf.mesh.vertices = m_sofaMesh.SofaMeshTopology.m_mesh.vertices;
                mf.mesh.normals = m_sofaMesh.SofaMeshTopology.m_mesh.normals;
            }
        }

    }

} // namespace SofaUnity
