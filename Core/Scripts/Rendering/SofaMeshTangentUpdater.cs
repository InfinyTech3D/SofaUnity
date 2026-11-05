using UnityEngine;
using UnityEngine.Rendering;

namespace SofaUnity
{
    /// <summary>
    /// SofaVisualModel only updates vertices/normals, never tangents. Without tangents a normal map
    /// is shaded in a wrong tangent frame (grainy, view dependent look). This component recomputes the
    /// tangents of the deformed mesh after SOFA has updated it.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    public class SofaMeshTangentUpdater : MonoBehaviour
    {
        /// Recompute every frame (needed for deformable meshes). If false, only computed once.
        public bool m_updateEveryFrame = true;

        private MeshFilter m_meshFilter = null;
        private bool m_done = false;

        void LateUpdate()
        {
            if (!m_updateEveryFrame && m_done)
                return;

            if (m_meshFilter == null)
                m_meshFilter = GetComponent<MeshFilter>();

            Mesh mesh = m_meshFilter.sharedMesh;
            if (mesh == null || mesh.vertexCount == 0 || !mesh.isReadable)
                return;

            if (!mesh.HasVertexAttribute(VertexAttribute.TexCoord0) || !mesh.HasVertexAttribute(VertexAttribute.Normal))
                return;

            mesh.RecalculateTangents();
            m_done = true;
        }
    }
}
