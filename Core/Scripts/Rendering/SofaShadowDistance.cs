using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SofaUnity
{
    /// <summary>
    /// Per scene shadow distance. URP stores the shadow distance in the pipeline asset (shared by all scenes),
    /// this component overrides it while the scene is loaded and restores the original value when disabled,
    /// so large SOFA objects get shadows without changing the project settings for the other scenes.
    /// </summary>
    [ExecuteAlways]
    public class SofaShadowDistance : MonoBehaviour
    {
        /// Max distance from the camera at which shadows are rendered.
        public float m_shadowDistance = 50f;

        private UniversalRenderPipelineAsset m_asset = null;
        private float m_previousDistance = -1f;

        void OnEnable()
        {
            Apply();
        }

        void OnDisable()
        {
            Restore();
        }

        void OnValidate()
        {
            if (isActiveAndEnabled)
                Apply();
        }

        void LateUpdate()
        {
            // quality level change switches the pipeline asset
            if (GraphicsSettings.currentRenderPipeline != m_asset || (m_asset != null && m_asset.shadowDistance != m_shadowDistance))
                Apply();
        }

        private void Apply()
        {
            UniversalRenderPipelineAsset asset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (asset != m_asset)
            {
                Restore();
                if (asset == null)
                    return;
                m_asset = asset;
                m_previousDistance = asset.shadowDistance;
            }

            if (m_asset != null)
                m_asset.shadowDistance = Mathf.Max(0f, m_shadowDistance);
        }

        private void Restore()
        {
            if (m_asset != null && m_previousDistance >= 0f)
                m_asset.shadowDistance = m_previousDistance;
            m_asset = null;
            m_previousDistance = -1f;
        }
    }
}
