using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SofaUnity
{
    public class SofaVisualModel : SofaBaseComponent
    {
        /// Member: Unity Mesh object of this GameObject
        protected Mesh m_mesh;
        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMeshAPI m_sofaMeshAPI = null;

        protected override void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaBaseComponent " + UniqueNameId + " already has a SofaBaseComponentAPI.");
                return;
            }

            if (m_sofaContext == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext is null", 1);
                return;
            }

            if (m_sofaContext.GetSimuContext() == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext.GetSimuContext() is null", 1);
                return;
            }

            SofaLog("SofaVisualModel::CreateSofaAPI: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaVisualModelAPI(m_sofaContext.GetSimuContext(), UniqueNameId);


            InitBaseMeshAPI();
        }


        protected void InitBaseMeshAPI()
        {
            if (m_sofaMeshAPI == null)
            {
                // Get access to the sofaContext
                IntPtr _simu = m_sofaContext.GetSimuContext();

                if (_simu == IntPtr.Zero)
                    return;
                
                // Create the API object for SofaMesh
                m_sofaMeshAPI = new SofaBaseMeshAPI(m_sofaContext.GetSimuContext(), UniqueNameId, false);
                SofaLog("SofaVisualModel::InitBaseMeshAPI object created");

                m_sofaMeshAPI.loadObject();

                // Add a MeshFilter to the GameObject
                MeshFilter mf = gameObject.GetComponent<MeshFilter>();
                if (mf == null)
                    gameObject.AddComponent<MeshFilter>();

                //to see it, we have to add a renderer
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr == null)
                    mr = gameObject.AddComponent<MeshRenderer>();

                if (mr.sharedMaterial == null)
                {
                    mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
                }

                //MeshCollider collid = gameObject.GetComponent<MeshCollider>();
                //if (collid == null)
                //    gameObject.AddComponent<MeshCollider>();

                initMesh(false);
            }
        }
        
        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void AwakePostProcess()
        {
            
        }



        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected void initMesh(bool toUpdate)
        {
            if (m_sofaMeshAPI == null)
                return;

#if UNITY_EDITOR
            //Only do this in the editor
            MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
            //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
            Mesh meshCopy = new Mesh();
            m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes
            
#else
            //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
            if (m_log)
                Debug.Log("SofaBox::Start play mode.");
#endif

            m_mesh.name = "SofaVisualMesh";
            m_mesh.vertices = new Vector3[0];
            m_sofaMeshAPI.updateMesh(m_mesh);
            m_mesh.triangles = m_sofaMeshAPI.createTriangulation();
            m_sofaMeshAPI.recomputeTexCoords(m_mesh);

            SofaLog("SofaVisualModel::initMesh ok: " + m_mesh.vertices.Length);
            //base.initMesh(false);

            if (toUpdate)
                m_sofaMeshAPI.updateMesh(m_mesh);
        }


        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            SofaLog("SofaVisualMesh::updateImpl called.");

            if (m_sofaMeshAPI != null)
            {
                if (false)//(m_sofaMeshAPI.hasTopologyChanged())
                {
                    m_mesh.triangles = m_sofaMeshAPI.createTriangulation();
                    //if (m_invertNormals)
                    //{
                    //    m_sofaMeshAPI.m_invertNormals = m_invertNormals;
                    //    invertMeshNormals();
                    //}
                    m_sofaMeshAPI.setTopologyChange(false);
                    m_sofaMeshAPI.updateMesh(m_mesh);
                    m_mesh.RecalculateNormals();
                }
                else
                {
                    //int res = m_sofaMeshAPI.updateMeshVelocity(m_mesh, m_sofaContext.timeStep);                    
                    //if (res == -1)
                    //    m_sofaContext.breakerProcedure();
                    m_sofaMeshAPI.updateMesh(m_mesh);
                }
                m_mesh.RecalculateBounds();
            }

        }

    }

} // namespace SofaUnity
