using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaFEMForceField : SofaBaseComponent
    {
        private SofaMesh m_sofaMesh = null;
        private MeshRenderer m_renderer;
        private bool m_oldStatus = false;

        protected override void CreateSofaAPI_Impl()
        {            
            SofaLog("SofaFEMForceField::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId);

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
                m_oldStatus = false;
            }

            if (m_renderer.sharedMaterial == null)
            {
                m_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }

            m_meshInit = false;
        }

        bool m_meshInit = false;

        Mesh meshCopy = null;
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
                }
            }
        }

        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        
        /// Method called by @sa Update() method.        
        protected override void Update_impl()
        {
            if (m_renderer.enabled != m_oldStatus)
            {
                m_oldStatus = m_renderer.enabled;
                if (m_oldStatus)
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
                mf.mesh.vertices = m_sofaMesh.SofaMeshTopology.m_mesh.vertices;
                mf.mesh.normals = m_sofaMesh.SofaMeshTopology.m_mesh.normals;
            }
        }


        /// Method to draw objects for debug only
        void OnDrawGizmosSelected()
        {
            return;
            //if (m_hasCollisionSphere && m_showCollisionSphere)
            if (meshCopy)
            {

                Gizmos.color = Color.yellow;
                foreach (Vector3 vert in meshCopy.vertices)
                {
                    Gizmos.DrawSphere(this.transform.TransformPoint(vert), 1.0f);
                }
            }

        }
    }

} // namespace SofaUnity
