using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SofaUnity
{
    /// <summary>
    /// Real time planar reflection for a flat surface (the room floor). A hidden camera renders the scene
    /// mirrored by the plane of this object into a render texture, published as the global texture
    /// _SofaPlanarReflectionTex and read in screen space by the SofaUnity/PlanarReflectionOverlay shader.
    /// The reflection follows the given camera only (Game view), other cameras see no reflection.
    /// </summary>
    [ExecuteAlways]
    public class SofaPlanarReflection : MonoBehaviour
    {
        /// Camera seen in the reflection. If null, Camera.main is used.
        public Camera m_targetCamera = null;

        /// Reflection texture size relative to the target camera size.
        [Range(0.25f, 1f)]
        public float m_resolutionScale = 0.5f;

        /// Offset of the clip plane along the normal, avoids the surface reflecting itself.
        public float m_clipPlaneOffset = 0.05f;

        /// Layers rendered in the reflection.
        public LayerMask m_reflectLayers = -1;

        private static readonly int s_reflectionTexId = Shader.PropertyToID("_SofaPlanarReflectionTex");
        private static readonly int s_reflectionWeightId = Shader.PropertyToID("_SofaPlanarReflectionWeight");

        private Camera m_reflectionCamera = null;
        private RenderTexture m_reflectionTexture = null;

        void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
            SetReflection(null);

            if (m_reflectionCamera != null)
                DestroyImmediate(m_reflectionCamera.gameObject);
            if (m_reflectionTexture != null)
                DestroyImmediate(m_reflectionTexture);
            m_reflectionCamera = null;
            m_reflectionTexture = null;
        }

        void LateUpdate()
        {
            Camera target = TargetCamera();
            if (target == null)
                return;

            UpdateResources(target);
            // render just before the target camera
            m_reflectionCamera.depth = target.depth - 1;
        }

        private Camera TargetCamera()
        {
            return m_targetCamera != null ? m_targetCamera : Camera.main;
        }

        private void UpdateResources(Camera target)
        {
            int width = Mathf.Max(16, (int)(target.pixelWidth * m_resolutionScale));
            int height = Mathf.Max(16, (int)(target.pixelHeight * m_resolutionScale));

            if (m_reflectionTexture == null || m_reflectionTexture.width != width || m_reflectionTexture.height != height)
            {
                if (m_reflectionTexture != null)
                    DestroyImmediate(m_reflectionTexture);

                m_reflectionTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGBHalf);
                m_reflectionTexture.name = "SofaPlanarReflection";
                m_reflectionTexture.useMipMap = true;
                m_reflectionTexture.autoGenerateMips = true;
                m_reflectionTexture.hideFlags = HideFlags.HideAndDontSave;
                m_reflectionTexture.Create();
            }

            if (m_reflectionCamera == null)
            {
                GameObject go = new GameObject("SofaPlanarReflectionCamera");
                go.hideFlags = HideFlags.HideAndDontSave;
                m_reflectionCamera = go.AddComponent<Camera>();

                UniversalAdditionalCameraData data = m_reflectionCamera.GetUniversalAdditionalCameraData();
                data.renderPostProcessing = false;
                data.renderShadows = true;
                data.antialiasing = AntialiasingMode.None;
                data.requiresDepthOption = CameraOverrideOption.Off;
                data.requiresColorOption = CameraOverrideOption.Off;
                data.volumeLayerMask = 0;
            }

            m_reflectionCamera.CopyFrom(target);
            m_reflectionCamera.cullingMask = m_reflectLayers;
            m_reflectionCamera.targetTexture = m_reflectionTexture;
            m_reflectionCamera.allowHDR = true;
            m_reflectionCamera.allowMSAA = false;
            m_reflectionCamera.enabled = true;
        }

        private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (m_reflectionCamera != null && camera == m_reflectionCamera)
            {
                // the reflected view must not read the texture it is writing
                SetReflection(null);
                UpdateReflectionMatrices(TargetCamera());
                GL.invertCulling = true;
            }
            else if (camera == TargetCamera() && m_reflectionTexture != null)
            {
                SetReflection(m_reflectionTexture);
            }
            else
            {
                SetReflection(null);
            }
        }

        private static void SetReflection(Texture texture)
        {
            Shader.SetGlobalTexture(s_reflectionTexId, texture != null ? texture : Texture2D.blackTexture);
            Shader.SetGlobalFloat(s_reflectionWeightId, texture != null ? 1f : 0f);
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (m_reflectionCamera != null && camera == m_reflectionCamera)
                GL.invertCulling = false;
        }

        private void UpdateReflectionMatrices(Camera target)
        {
            if (target == null)
                return;

            Vector3 normal = transform.up;
            Vector3 pos = transform.position;
            float d = -Vector3.Dot(normal, pos) - m_clipPlaneOffset;
            Matrix4x4 reflection = ReflectionMatrix(new Vector4(normal.x, normal.y, normal.z, d));

            m_reflectionCamera.transform.position = reflection.MultiplyPoint(target.transform.position);
            m_reflectionCamera.worldToCameraMatrix = target.worldToCameraMatrix * reflection;

            // oblique near plane = mirror plane, so nothing under the surface is reflected
            Vector4 clipPlane = CameraSpacePlane(m_reflectionCamera, pos, normal);
            m_reflectionCamera.projectionMatrix = target.CalculateObliqueMatrix(clipPlane);
        }

        private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal)
        {
            Vector3 offsetPos = pos + normal * m_clipPlaneOffset;
            Matrix4x4 m = cam.worldToCameraMatrix;
            Vector3 cpos = m.MultiplyPoint(offsetPos);
            Vector3 cnormal = m.MultiplyVector(normal).normalized;
            return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
        }

        private static Matrix4x4 ReflectionMatrix(Vector4 plane)
        {
            Matrix4x4 m = Matrix4x4.identity;
            m.m00 = 1f - 2f * plane.x * plane.x;
            m.m01 = -2f * plane.x * plane.y;
            m.m02 = -2f * plane.x * plane.z;
            m.m03 = -2f * plane.w * plane.x;

            m.m10 = -2f * plane.y * plane.x;
            m.m11 = 1f - 2f * plane.y * plane.y;
            m.m12 = -2f * plane.y * plane.z;
            m.m13 = -2f * plane.w * plane.y;

            m.m20 = -2f * plane.z * plane.x;
            m.m21 = -2f * plane.z * plane.y;
            m.m22 = 1f - 2f * plane.z * plane.z;
            m.m23 = -2f * plane.w * plane.z;
            return m;
        }
    }
}
