using System;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class to create a rigid Grid Mesh, inherite from SRigidMesh 
    /// This class will prepare the creation of specific Grid by adding texture and material to the renderer.
    /// </summary>
    public class SRigidGrid : SRigidMesh
    {
        /// Parameter to use texCoords and recompute them.
        protected bool m_useTex = true;

        /// Parameter to store the grid resolution in 3D
        public Vector3 m_gridSize = new Vector3(5, 5, 5);


        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void awakePostProcess()
        {
            // Call SRigidMesh.awakePostProcess(); to create MeshRenderer
            base.awakePostProcess();

            // Add default material to the MeshRenderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();

            // Check if one material has already been set to the grid, otherwise apply default one.
            if (mr.sharedMaterial == null)
            {
                if (this.m_useTex)
                    mr.sharedMaterial = Resources.Load("Materials/BoxSofa") as Material;
                else
                    mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }
        }

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            base.initMesh(false);

            // Change the Name
            m_mesh.name = "SofaRigidGrid";

            // Set the grid resolution and update texCoords.
            //m_impl.setGridResolution(m_gridSize);

            // Some grid need to recompute normals or topology
            m_impl.recomputeTopology(m_mesh);

            if (this.m_useTex)
                m_impl.recomputeTexCoords(m_mesh);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }

        /// Getter/Setter to the @see m_gridSize
        public virtual Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    //if (m_impl != null)
                    //    m_impl.setGridResolution(m_gridSize);
                }
            }
        }
    }
}
