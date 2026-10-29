using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace SofaUnity
{
    /// <summary>
    /// Options of the realistic rendering setup. Serialized in EditorPrefs (scene references excepted).
    /// </summary>
    [System.Serializable]
    public class SofaRenderingSetupSettings
    {
        // scene references, auto detected when null
        [System.NonSerialized] public Camera m_camera = null;
        [System.NonSerialized] public GameObject m_roomRoot = null;

        public bool m_lights = true;
        public bool m_disableExistingLights = true;
        public float m_keyIntensity = 1.5f;
        public float m_fillIntensity = 0.35f;
        public float m_rimIntensity = 1.0f;

        public bool m_adjustShadowDistance = true;
        /// 0 = computed from the camera and the SOFA objects
        public float m_shadowDistance = 0f;

        public bool m_ambient = true;
        public bool m_reflectionProbe = true;
        public bool m_postProcessing = true;
        public bool m_depthOfField = false;

        public bool m_tangents = true;

        public bool m_room = true;
        public bool m_createRoomIfMissing = true;
        public float m_floorGap = 1.0f;
        public float m_floorSmoothness = 0.75f;
        public float m_wallSmoothness = 0.55f;
        public bool m_planarFloorReflection = true;
    }


    /// <summary>
    /// Editor tool upgrading the look of any SofaUnity scene (URP), whatever its SofaContext:
    /// 3 points lighting oriented from the camera, trilight ambient, box projected reflection probe fitted on the room,
    /// per scene post-processing profile, shadow distance fitted on the scene, tangent recomputation on SOFA meshes
    /// (SOFA materials are left untouched), glossy room with real time planar reflection on the floor.
    /// The room is detected (or created from demo_canvas) and fitted around the SOFA objects bounds.
    /// Room materials are stored in SofaUnity/Core/Resources/Materials, volume profiles in SofaUnity/Core/Scripts/Rendering/Profiles.
    /// </summary>
    public static class SofaRenderingSetup
    {
        public const string RootName = "Rendering Setup";
        private const string AssetFolder = "Assets/SofaUnity/Core/Scripts/Rendering";
        private const string MaterialFolder = "Assets/SofaUnity/Core/Resources/Materials";
        private const string ProfileFolder = AssetFolder + "/Profiles";
        private const string RoomPrefabPath = "Assets/SofaUnity/Core/Prefabs/demo_canvas.prefab";
        private const string RoomMaterialName = "Canva-backgroundSofaInf3D";
        private const string RoomGeneratedPrefix = "Room_";
        private const string RealisticSuffix = "_Realistic";

        public static void Apply(SofaRenderingSetupSettings settings)
        {
            EnsureFolder(MaterialFolder);
            EnsureFolder(ProfileFolder);

            Camera cam = settings.m_camera != null ? settings.m_camera : FindSceneCamera();
            if (cam == null)
            {
                Debug.LogError("[SofaRenderingSetup] No camera found in the scene.");
                return;
            }

            // Remove previous setup so the tool can be re-run
            GameObject previous = GameObject.Find(RootName);
            if (previous != null)
                Undo.DestroyObjectImmediate(previous);

            GameObject root = new GameObject(RootName);
            Undo.RegisterCreatedObjectUndo(root, "Apply SofaUnity realistic rendering");

            Bounds focus = ComputeSofaBounds();

            List<MeshRenderer> room = new List<MeshRenderer>();
            if (settings.m_room)
            {
                room = FindRoomRenderers(settings.m_roomRoot);
                if (room.Count == 0 && settings.m_createRoomIfMissing)
                    room = CreateRoom(focus, settings.m_floorGap);
            }
            MeshRenderer floor = FindFloor(room);
            Bounds roomBounds = ComputeRoomBounds(room, floor, focus);

            if (settings.m_disableExistingLights)
                DisableExistingLights(root);
            if (settings.m_lights)
                CreateLights(root.transform, cam, settings);
            if (settings.m_adjustShadowDistance)
                CreateShadowDistance(root.transform, cam, focus, floor, settings.m_shadowDistance);
            if (settings.m_ambient)
                SetupAmbient();
            if (settings.m_reflectionProbe)
                CreateReflectionProbe(root.transform, focus, roomBounds);
            if (settings.m_postProcessing)
                CreateVolume(root.transform, cam, focus, settings);
            SetupCamera(cam, settings.m_postProcessing);
            if (settings.m_tangents)
                AddTangentUpdaters();
            if (settings.m_room)
                UpgradeRoomMaterials(room, floor, cam, settings);

            EditorSceneManager.MarkSceneDirty(root.scene);
            AssetDatabase.SaveAssets();
            Debug.Log("[SofaRenderingSetup] Realistic rendering applied on '" + root.scene.name + "' (camera: " + cam.name
                + ", room renderers: " + room.Count + "). Save the scene to keep it.");
        }


        ////////////////////////////////////////////
        //////       Scene analysis            /////
        ////////////////////////////////////////////

        /// Example scenes cameras are not always tagged MainCamera: fallback on the first on-screen camera
        public static Camera FindSceneCamera()
        {
            if (Camera.main != null)
                return Camera.main;
            foreach (Camera cam in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (IsSceneCamera(cam))
                    return cam;
            }
            return null;
        }

        private static bool IsSceneCamera(Camera cam)
        {
            return cam.gameObject.hideFlags == HideFlags.None && cam.targetTexture == null;
        }

        /// Bounds of all SOFA visual meshes, or of the SofaContexts when the meshes are not created yet
        private static Bounds ComputeSofaBounds()
        {
            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 2f);

            foreach (SofaVisualModel visual in Object.FindObjectsByType<SofaVisualModel>(FindObjectsSortMode.None))
            {
                MeshFilter mf = visual.GetComponent<MeshFilter>();
                MeshRenderer mr = visual.GetComponent<MeshRenderer>();
                if (mf == null || mr == null || mf.sharedMesh == null || mf.sharedMesh.vertexCount == 0)
                    continue;
                Encapsulate(ref bounds, ref hasBounds, mr.bounds);
            }

            if (!hasBounds)
            {
                foreach (SofaContext context in Object.FindObjectsByType<SofaContext>(FindObjectsSortMode.None))
                    Encapsulate(ref bounds, ref hasBounds, new Bounds(context.transform.position, Vector3.one * 2f));
            }
            return bounds;
        }

        private static void Encapsulate(ref Bounds bounds, ref bool hasBounds, Bounds other)
        {
            if (hasBounds)
                bounds.Encapsulate(other);
            else
                bounds = other;
            hasBounds = true;
        }

        /// Room = children of the given root, or every renderer using the SofaUnity room material
        private static List<MeshRenderer> FindRoomRenderers(GameObject roomRoot)
        {
            List<MeshRenderer> room = new List<MeshRenderer>();
            MeshRenderer[] candidates = roomRoot != null
                ? roomRoot.GetComponentsInChildren<MeshRenderer>()
                : Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

            foreach (MeshRenderer renderer in candidates)
            {
                if (renderer.GetComponent<SofaVisualModel>() != null || renderer.sharedMaterial == null)
                    continue;

                string matName = renderer.sharedMaterial.name;
                if (roomRoot != null || matName == RoomMaterialName || matName.StartsWith(RoomGeneratedPrefix))
                    room.Add(renderer);
            }
            return room;
        }

        /// Instantiate the SofaUnity demo room, centered under the SOFA objects and large enough to contain them
        private static List<MeshRenderer> CreateRoom(Bounds focus, float floorGap)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(RoomPrefabPath);
            if (prefab == null)
            {
                Debug.LogWarning("[SofaRenderingSetup] Room prefab not found: " + RoomPrefabPath);
                return new List<MeshRenderer>();
            }

            GameObject room = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(room, "Create room");
            room.name = "Room";

            // demo room is 48 units wide: scale it so the objects take at most a quarter of it
            float extent = Mathf.Max(focus.size.x, focus.size.z);
            float scale = Mathf.Max(1f, extent * 4f / 48f);
            room.transform.localScale = Vector3.one * scale;

            List<MeshRenderer> renderers = new List<MeshRenderer>(room.GetComponentsInChildren<MeshRenderer>());
            MeshRenderer floor = FindFloor(renderers);
            float floorLocalY = floor != null ? floor.transform.position.y : 0f;
            room.transform.position = new Vector3(focus.center.x, focus.min.y - floorGap - floorLocalY, focus.center.z);
            return renderers;
        }

        /// Floor = the lowest horizontal facing up renderer of the room
        private static MeshRenderer FindFloor(List<MeshRenderer> room)
        {
            MeshRenderer floor = null;
            foreach (MeshRenderer renderer in room)
            {
                if (Vector3.Dot(renderer.transform.up, Vector3.up) < 0.9f)
                    continue;
                if (floor == null || renderer.bounds.center.y < floor.bounds.center.y)
                    floor = renderer;
            }
            return floor;
        }

        /// Room volume above the floor, or a box around the SOFA objects when there is no room
        private static Bounds ComputeRoomBounds(List<MeshRenderer> room, MeshRenderer floor, Bounds focus)
        {
            if (room.Count == 0)
                return new Bounds(focus.center, focus.size * 6f + Vector3.one);

            Bounds bounds = room[0].bounds;
            foreach (MeshRenderer renderer in room)
                bounds.Encapsulate(renderer.bounds);

            if (floor != null)
            {
                // walls usually go below the floor: cut the volume at the floor level
                Vector3 min = bounds.min;
                min.y = floor.bounds.max.y;
                bounds.SetMinMax(min, bounds.max);
            }
            return bounds;
        }


        ////////////////////////////////////////////
        //////            Lighting             /////
        ////////////////////////////////////////////

        private static void DisableExistingLights(GameObject root)
        {
            foreach (Light light in Object.FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                if (light.transform.IsChildOf(root.transform))
                    continue;
                Undo.RecordObject(light.gameObject, "Disable light");
                light.gameObject.SetActive(false);
            }
        }

        /// Lights are oriented relatively to the camera so the setup works whatever the scene framing
        private static void CreateLights(Transform parent, Camera cam, SofaRenderingSetupSettings settings)
        {
            float yaw = cam.transform.eulerAngles.y;

            // Key: warm, from upper front-left of the camera, casts the soft contact shadow on the floor
            Light key = CreateDirectional(parent, "Key Light", Quaternion.Euler(50f, yaw + 30f, 0f), new Color(1f, 0.95f, 0.88f), settings.m_keyIntensity);
            key.shadows = LightShadows.Soft;
            key.shadowStrength = 0.85f;
            key.shadowBias = 0.05f;
            key.shadowNormalBias = 0.3f;

            // Fill: cool and dim, from the right, opens the shadow side without flattening it
            CreateDirectional(parent, "Fill Light", Quaternion.Euler(15f, yaw - 45f, 0f), new Color(0.75f, 0.84f, 1f), settings.m_fillIntensity);

            // Rim: from behind, slightly pink to fake light scattering through the tissue edges
            // (rotation tuned on Example_01_SimpleLiver with a camera yaw of 0)
            Quaternion rimRotation = new Quaternion(-0.6021015f, 0.7378160f, -0.3046602f, 0.0168412f);
            CreateDirectional(parent, "Rim Light", Quaternion.Euler(0f, yaw, 0f) * rimRotation, new Color(1f, 0.82f, 0.78f), settings.m_rimIntensity);
        }

        private static Light CreateDirectional(Transform parent, string name, Quaternion rotation, Color color, float intensity)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = rotation;
            Light light = go.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = color;
            light.intensity = intensity;
            light.shadows = LightShadows.None;
            return light;
        }

        /// Shadows must reach the farthest SOFA object and its shadow on the floor
        private static void CreateShadowDistance(Transform parent, Camera cam, Bounds focus, MeshRenderer floor, float distance)
        {
            if (distance <= 0f)
            {
                Bounds shadowed = focus;
                shadowed.Expand(new Vector3(focus.size.x, 0f, focus.size.z));
                if (floor != null)
                    shadowed.Encapsulate(new Vector3(shadowed.center.x, floor.bounds.max.y, shadowed.center.z));

                Vector3 camPos = cam.transform.position;
                Vector3 min = shadowed.min;
                Vector3 max = shadowed.max;
                for (int i = 0; i < 8; i++)
                {
                    Vector3 corner = new Vector3((i & 1) == 0 ? min.x : max.x, (i & 2) == 0 ? min.y : max.y, (i & 4) == 0 ? min.z : max.z);
                    distance = Mathf.Max(distance, Vector3.Distance(camPos, corner));
                }
                distance = Mathf.Max(10f, distance * 1.2f);
            }

            GameObject go = new GameObject("Shadow Distance");
            go.transform.SetParent(parent, false);
            SofaShadowDistance shadow = go.AddComponent<SofaShadowDistance>();
            shadow.m_shadowDistance = distance;
        }

        private static void SetupAmbient()
        {
            // Enclosed room: a gradient matches the walls better than the default procedural sky
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.46f, 0.48f, 0.53f);
            RenderSettings.ambientEquatorColor = new Color(0.33f, 0.32f, 0.31f);
            RenderSettings.ambientGroundColor = new Color(0.16f, 0.14f, 0.13f);
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.reflectionIntensity = 1f;
        }

        /// Realtime probe (room walls are not static), captured above the SOFA objects, box fitted on the room
        private static void CreateReflectionProbe(Transform parent, Bounds focus, Bounds roomBounds)
        {
            GameObject go = new GameObject("Room Reflection Probe");
            go.transform.SetParent(parent, false);

            Vector3 capture = new Vector3(focus.center.x, focus.max.y + focus.extents.y + 1f, focus.center.z);
            capture.y = Mathf.Min(capture.y, roomBounds.max.y - 0.5f);
            go.transform.position = capture;

            ReflectionProbe probe = go.AddComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Realtime;
            probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
            probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
            probe.boxProjection = true;
            probe.center = roomBounds.center - capture;
            probe.size = roomBounds.size;
            probe.resolution = 256;
            probe.hdr = true;
            probe.intensity = 1f;
            probe.RenderProbe();
        }


        ////////////////////////////////////////////
        //////         Post-processing         /////
        ////////////////////////////////////////////

        /// One profile per scene, as the depth of field focus depends on the framing
        private static void CreateVolume(Transform parent, Camera cam, Bounds focus, SofaRenderingSetupSettings settings)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (string.IsNullOrEmpty(sceneName))
                sceneName = "Untitled";
            string path = ProfileFolder + "/" + sceneName + "_VolumeProfile.asset";

            VolumeProfile profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(profile, path);
            }

            Tonemapping tonemapping = GetOrAdd<Tonemapping>(profile);
            tonemapping.mode.Override(TonemappingMode.ACES);

            ColorAdjustments color = GetOrAdd<ColorAdjustments>(profile);
            color.postExposure.Override(0.35f);
            color.contrast.Override(12f);
            color.saturation.Override(6f);

            Bloom bloom = GetOrAdd<Bloom>(profile);
            bloom.threshold.Override(1.0f);
            bloom.intensity.Override(0.35f);
            bloom.scatter.Override(0.65f);
            bloom.highQualityFiltering.Override(true);

            Vignette vignette = GetOrAdd<Vignette>(profile);
            vignette.intensity.Override(0.22f);
            vignette.smoothness.Override(0.45f);

            // Shallow depth of field like a product shot, focused on the SOFA objects
            DepthOfField dof = GetOrAdd<DepthOfField>(profile);
            dof.active = settings.m_depthOfField;
            dof.mode.Override(DepthOfFieldMode.Bokeh);
            dof.focusDistance.Override(Vector3.Distance(cam.transform.position, focus.center));
            dof.focalLength.Override(60f);
            dof.aperture.Override(4f);

            EditorUtility.SetDirty(profile);

            GameObject go = new GameObject("Post Process Volume");
            go.transform.SetParent(parent, false);
            Volume volume = go.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 10f;
            volume.sharedProfile = profile;
        }

        private static T GetOrAdd<T>(VolumeProfile profile) where T : VolumeComponent
        {
            if (profile.TryGet(out T component))
                return component;
            component = profile.Add<T>();
            component.name = typeof(T).Name;
            AssetDatabase.AddObjectToAsset(component, profile);
            return component;
        }

        private static void SetupCamera(Camera cam, bool postProcessing)
        {
            Undo.RecordObject(cam, "Setup camera");
            cam.allowHDR = true;

            UniversalAdditionalCameraData data = cam.GetUniversalAdditionalCameraData();
            Undo.RecordObject(data, "Setup camera");
            data.renderPostProcessing = postProcessing;
            // SMAA rather than TAA: TAA ghosts on fast deforming SOFA meshes
            data.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            data.antialiasingQuality = AntialiasingQuality.High;
            data.renderShadows = true;
            EditorUtility.SetDirty(data);
        }


        ////////////////////////////////////////////
        //////      SOFA objects & room        /////
        ////////////////////////////////////////////

        /// SOFA visual materials are never modified: only tangents are added so their normal maps shade correctly
        private static void AddTangentUpdaters()
        {
            foreach (SofaVisualModel visual in Object.FindObjectsByType<SofaVisualModel>(FindObjectsSortMode.None))
            {
                if (visual.GetComponent<MeshFilter>() != null && visual.GetComponent<SofaMeshTangentUpdater>() == null)
                    Undo.AddComponent<SofaMeshTangentUpdater>(visual.gameObject);
            }
        }

        private static void UpgradeRoomMaterials(List<MeshRenderer> room, MeshRenderer floor, Camera cam, SofaRenderingSetupSettings settings)
        {
            Material floorMat = null;
            Material wallMat = null;
            Material reflectionMat = null;
            foreach (MeshRenderer renderer in room)
            {
                Material source = renderer.sharedMaterial;
                if (source == null)
                    continue;

                Undo.RecordObject(renderer, "Upgrade room material");
                renderer.receiveShadows = true;

                if (renderer == floor)
                {
                    // Polished floor: glossy Lit base + real time planar reflection overlay
                    if (floorMat == null)
                        floorMat = CreateRoomMaterial(source, RoomGeneratedPrefix + "Floor" + RealisticSuffix, settings.m_floorSmoothness);

                    SofaPlanarReflection planar = renderer.GetComponent<SofaPlanarReflection>();
                    if (settings.m_planarFloorReflection)
                    {
                        if (reflectionMat == null)
                            reflectionMat = CreateFloorReflectionMaterial(floorMat);
                        renderer.sharedMaterials = new Material[] { floorMat, reflectionMat };

                        if (planar == null)
                            planar = Undo.AddComponent<SofaPlanarReflection>(renderer.gameObject);
                        Undo.RecordObject(planar, "Setup planar reflection");
                        planar.m_targetCamera = cam;
                    }
                    else
                    {
                        renderer.sharedMaterials = new Material[] { floorMat };
                        if (planar != null)
                            Undo.DestroyObjectImmediate(planar);
                    }
                }
                else
                {
                    // Satin walls: glossy enough to pick the room and the SOFA objects from the box projected probe
                    if (wallMat == null)
                        wallMat = CreateRoomMaterial(source, RoomGeneratedPrefix + "Wall" + RealisticSuffix, settings.m_wallSmoothness);
                    renderer.sharedMaterials = new Material[] { wallMat };
                }
            }
        }

        private static Material CreateRoomMaterial(Material source, string name, float smoothness)
        {
            string path = MaterialFolder + "/" + name + ".mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(source);
                AssetDatabase.CreateAsset(mat, path);
            }
            // Non metallic: metallic on a white surface only greys it out
            mat.SetFloat("_Metallic", 0f);
            mat.SetFloat("_Smoothness", smoothness);
            mat.SetFloat("_SpecularHighlights", 1f);
            mat.SetFloat("_EnvironmentReflections", 1f);
            mat.SetColor("_BaseColor", new Color(0.92f, 0.91f, 0.9f));
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static Material CreateFloorReflectionMaterial(Material floorMat)
        {
            Shader shader = Shader.Find("SofaUnity/PlanarReflectionOverlay");
            string path = MaterialFolder + "/" + RoomGeneratedPrefix + "Floor_PlanarReflection.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.shader = shader;
            mat.SetFloat("_ReflectionStrength", 0.15f);
            mat.SetFloat("_FresnelStrength", 0.6f);
            mat.SetFloat("_FresnelPower", 4f);
            mat.SetFloat("_Blur", 1.5f);
            mat.SetFloat("_Distortion", 0.008f);
            if (floorMat.HasProperty("_BumpMap"))
            {
                mat.SetTexture("_BumpMap", floorMat.GetTexture("_BumpMap"));
                mat.SetTextureScale("_BumpMap", floorMat.GetTextureScale("_BumpMap"));
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder))
                return;
            string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(folder));
        }
    }


    /// <summary>
    /// Window to choose the options of the realistic rendering setup before applying it on the open scene.
    /// </summary>
    public class SofaRenderingSetupWindow : EditorWindow
    {
        private const string PrefsKey = "SofaUnity.RenderingSetup.Settings";

        private SofaRenderingSetupSettings m_settings = null;
        private Vector2 m_scroll = Vector2.zero;

        [MenuItem("Tools/SofaUnity/Realistic Rendering Setup")]
        public static void Open()
        {
            GetWindow<SofaRenderingSetupWindow>("SofaUnity Rendering");
        }

        private static SofaRenderingSetupSettings LoadSettings()
        {
            SofaRenderingSetupSettings settings = new SofaRenderingSetupSettings();
            string json = EditorPrefs.GetString(PrefsKey, "");
            if (!string.IsNullOrEmpty(json))
                JsonUtility.FromJsonOverwrite(json, settings);
            return settings;
        }

        void OnEnable()
        {
            m_settings = LoadSettings();
        }

        void OnGUI()
        {
            SofaRenderingSetupSettings s = m_settings;
            m_scroll = EditorGUILayout.BeginScrollView(m_scroll);

            EditorGUILayout.LabelField("Scene (auto detected when empty)", EditorStyles.boldLabel);
            s.m_camera = (Camera)EditorGUILayout.ObjectField("Camera", s.m_camera, typeof(Camera), true);
            s.m_roomRoot = (GameObject)EditorGUILayout.ObjectField("Room Root", s.m_roomRoot, typeof(GameObject), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);
            s.m_lights = EditorGUILayout.Toggle("3 Points Lights", s.m_lights);
            using (new EditorGUI.DisabledScope(!s.m_lights))
            {
                s.m_keyIntensity = EditorGUILayout.Slider("Key Intensity", s.m_keyIntensity, 0f, 4f);
                s.m_fillIntensity = EditorGUILayout.Slider("Fill Intensity", s.m_fillIntensity, 0f, 2f);
                s.m_rimIntensity = EditorGUILayout.Slider("Rim Intensity", s.m_rimIntensity, 0f, 3f);
            }
            s.m_disableExistingLights = EditorGUILayout.Toggle("Disable Existing Lights", s.m_disableExistingLights);
            s.m_adjustShadowDistance = EditorGUILayout.Toggle("Adjust Shadow Distance", s.m_adjustShadowDistance);
            using (new EditorGUI.DisabledScope(!s.m_adjustShadowDistance))
                s.m_shadowDistance = EditorGUILayout.FloatField(new GUIContent("Shadow Distance (0 = auto)",
                    "Distance from the camera at which shadows stop. Auto covers the SOFA objects and their shadow on the floor."), s.m_shadowDistance);
            s.m_ambient = EditorGUILayout.Toggle("Trilight Ambient", s.m_ambient);
            s.m_reflectionProbe = EditorGUILayout.Toggle("Room Reflection Probe", s.m_reflectionProbe);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Post-processing", EditorStyles.boldLabel);
            s.m_postProcessing = EditorGUILayout.Toggle("Post-processing", s.m_postProcessing);
            using (new EditorGUI.DisabledScope(!s.m_postProcessing))
                s.m_depthOfField = EditorGUILayout.Toggle("Depth Of Field", s.m_depthOfField);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("SOFA Objects", EditorStyles.boldLabel);
            s.m_tangents = EditorGUILayout.Toggle("Recompute Tangents", s.m_tangents);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Room", EditorStyles.boldLabel);
            s.m_room = EditorGUILayout.Toggle("Setup Room", s.m_room);
            using (new EditorGUI.DisabledScope(!s.m_room))
            {
                s.m_createRoomIfMissing = EditorGUILayout.Toggle("Create Room If Missing", s.m_createRoomIfMissing);
                s.m_floorGap = EditorGUILayout.FloatField("Floor Gap (new room)", s.m_floorGap);
                s.m_floorSmoothness = EditorGUILayout.Slider("Floor Smoothness", s.m_floorSmoothness, 0f, 1f);
                s.m_wallSmoothness = EditorGUILayout.Slider("Wall Smoothness", s.m_wallSmoothness, 0f, 1f);
                s.m_planarFloorReflection = EditorGUILayout.Toggle("Planar Floor Reflection", s.m_planarFloorReflection);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            if (GUI.changed)
                EditorPrefs.SetString(PrefsKey, JsonUtility.ToJson(s));

            if (GUILayout.Button("Apply To Open Scene", GUILayout.Height(30)))
                SofaRenderingSetup.Apply(s);
        }
    }
}
