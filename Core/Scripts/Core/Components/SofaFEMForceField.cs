using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using SofaUnityAPI;

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

        /// Setter to activate FEM display
        public void DisplayForcefield(bool value)
        {
            if (m_meshInit)
                m_renderer.enabled = value;
        }


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
            // check if real FEM
            if (!(m_componentType.Contains("FEMForceField") || m_componentType.Contains("Tetrahedral"))) {
                m_meshInit = false;
                return;
            }
            
            // Init mesh 
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
                    if (m_sofaMesh.HasTopology())
                        mf.mesh = m_sofaMesh.SofaMeshTopology.m_mesh;
                    m_trackedTopologyRevision = m_sofaMesh.GetTopologyRevision();
                }
            }

            // Init VonMises if available
            computeVonMisesDisplay();
        }


        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        
        /// Method called by @sa Update() method.        
        protected override void Update_impl()
        {
            if (!m_meshInit)
                return;

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

                updateVonMises();
            }
        }


        ////////////////////////////////////////////
        //////        FEM Von Mises API        /////
        ////////////////////////////////////////////

        SofaDataVectorDouble d_vonMisesPerNode = null;

        protected float[] m_vonMisesValues = null;

        protected bool m_vonMisesInit = false;
        protected Mesh m_mesh = null;
        
        protected Vector2[] m_uv = null;
        protected float m_minValue = float.MaxValue;
        protected float m_maxValue = float.MinValue;

        protected void computeVonMisesDisplay()
        {
            // Init Data to track von mises values
            if (d_vonMisesPerNode == null)
            {
                SofaDataVector dataPtr = this.m_dataArchiver.GetVectorData("vonMisesPerNode");
                if (dataPtr != null)
                    d_vonMisesPerNode = (SofaDataVectorDouble)dataPtr;
                else
                    d_vonMisesPerNode = null;
            }

            // if no Data vonMises, exit
            if (d_vonMisesPerNode == null)
                return;

            // init buffer to store vonMises values
            int nbrV = d_vonMisesPerNode.GetSize();
            if (m_vonMisesValues == null)
                m_vonMisesValues = new float[nbrV];

            m_vonMisesInit = true;
        }
        

        protected void updateVonMises()
        {
            if (!m_vonMisesInit)
                return;

            int nbrV = d_vonMisesPerNode.GetSize();
            int res = d_vonMisesPerNode.GetValues(m_vonMisesValues);
            if (res == -1)
                return;

            // Store pointer to this MeshFilter
            if (m_mesh == null)
            {
#if UNITY_EDITOR
                MeshFilter mf = gameObject.GetComponent<MeshFilter>();
                m_mesh = mf.sharedMesh;
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
#endif
            }

            // init buffer to store computed UV 
            int nbrTV = m_mesh.vertices.Length;
            if (m_uv == null)
                m_uv = new Vector2[nbrTV];

            m_minValue = float.MaxValue;
            m_maxValue = float.MinValue;

            // Get min and max values (need to check if we reinit them during simulation)
            for (int i = 0; i < nbrV; ++i)
            {
                if (m_vonMisesValues[i] > m_maxValue)
                    m_maxValue = m_vonMisesValues[i];
                if (m_vonMisesValues[i] < m_minValue)
                    m_minValue = m_vonMisesValues[i];
            }

            float scale = 1 / (m_maxValue - m_minValue);
            foreach (KeyValuePair<int, int> id in m_sofaMesh.SofaMeshTopology.mappingVertices)
            {
                m_uv[id.Key].x = (m_vonMisesValues[id.Value] - m_minValue )* scale;
                m_uv[id.Key].y = (m_vonMisesValues[id.Value] - m_minValue )* scale;
            }
            m_mesh.uv = m_uv;
        }
    }

} // namespace SofaUnity
