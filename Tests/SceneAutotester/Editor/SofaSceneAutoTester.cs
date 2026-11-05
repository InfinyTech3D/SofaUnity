using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using SofaUnity;

namespace SofaScripts
{
    /// <summary>
    /// Editor-side probe driven by run_scene_tests.py through -executeMethod.
    /// Opens a scene, enters play mode, checks that the SofaContext is simulating
    /// (SOFA FPS > min) and writes a JSON result before quitting the editor.
    /// Crashes and hangs are detected by the python script (no result file / timeout).
    /// </summary>
    [InitializeOnLoad]
    public static class SofaSceneAutoTester
    {
        const string ArgScene = "-sofaTestScene";
        const string ArgOutput = "-sofaTestOutput";
        const string ArgWarmup = "-sofaTestWarmup";
        const string ArgDuration = "-sofaTestDuration";
        const string ArgMinFps = "-sofaTestMinFps";
        const int MaxStoredErrors = 100;    // unique messages, all occurrences are still counted
        const int MaxMessageLength = 500;

        [Serializable]
        class ErrorEntry
        {
            public string message;
            public int count;
        }

        [Serializable]
        class TestResult
        {
            public string scene;
            public string status;   // PASS / FAIL
            public string reason;
            public bool contextFound;
            public bool contextWasDisabled;
            public bool contextWasPaused;
            public float fpsMin;
            public float fpsMax;
            public float fpsAvg;
            public int fpsSamples;
            public int errorCount;
            public List<ErrorEntry> errors = new List<ErrorEntry>();
        }

        static string s_output = null;
        static float s_warmup = 3.0f;
        static float s_duration = 5.0f;
        static float s_minFps = 0.0f;

        // State is reset by the domain reload when entering play mode, which is fine:
        // everything is measured during play mode only.
        static double s_playStart = -1.0;
        static bool s_finished = false;
        static SofaContext s_context = null;
        static TestResult s_result = new TestResult();
        static float s_fpsSum = 0.0f;

        /// Called on every domain reload. Only active when launched by the python runner.
        static SofaSceneAutoTester()
        {
            s_output = GetArg(ArgOutput);
            if (s_output == null)
                return;

            s_warmup = GetFloatArg(ArgWarmup, s_warmup);
            s_duration = GetFloatArg(ArgDuration, s_duration);
            s_minFps = GetFloatArg(ArgMinFps, s_minFps);
            s_result.scene = GetArg(ArgScene);
            s_result.fpsMin = float.MaxValue;

            Application.logMessageReceived += HandleLog;
            EditorApplication.update += Tick;
        }

        /// Entry point: Unity.exe -batchmode -executeMethod SofaScripts.SofaSceneAutoTester.Run -sofaTestScene ... -sofaTestOutput ...
        public static void Run()
        {
            string scene = GetArg(ArgScene);
            if (scene == null || GetArg(ArgOutput) == null)
            {
                Debug.LogError("[SofaSceneAutoTester] Missing " + ArgScene + " or " + ArgOutput + " argument.");
                EditorApplication.Exit(2);
                return;
            }

            Debug.Log("[SofaSceneAutoTester] Opening scene: " + scene);
            EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

        static void Tick()
        {
            if (s_finished || !EditorApplication.isPlaying)
                return;

            if (s_playStart < 0.0)
            {
                s_playStart = EditorApplication.timeSinceStartup;
                s_context = UnityEngine.Object.FindFirstObjectByType<SofaContext>(FindObjectsInactive.Include);
                s_result.contextFound = s_context != null;
                if (s_context == null)
                {
                    Finish("FAIL", "No SofaContext found in scene.");
                    return;
                }

                // Some scenes have the SofaContext disabled or paused, force it on for the test.
                s_result.contextWasDisabled = !s_context.isActiveAndEnabled;
                s_result.contextWasPaused = !s_context.IsSofaUpdating;
                for (Transform t = s_context.transform; t != null; t = t.parent)
                    t.gameObject.SetActive(true);
                s_context.enabled = true;
                s_context.IsSofaUpdating = true;
            }

            double elapsed = EditorApplication.timeSinceStartup - s_playStart;
            if (elapsed < s_warmup)
                return;

            if (s_context == null) // destroyed during play
            {
                Finish("FAIL", "SofaContext was destroyed during play.");
                return;
            }

            float fps = s_context.SimulationFPS;
            s_result.fpsMin = Mathf.Min(s_result.fpsMin, fps);
            s_result.fpsMax = Mathf.Max(s_result.fpsMax, fps);
            s_fpsSum += fps;
            s_result.fpsSamples++;

            if (elapsed < s_warmup + s_duration)
                return;

            s_result.fpsAvg = s_fpsSum / s_result.fpsSamples;
            if (s_result.fpsAvg > s_minFps)
                Finish("PASS", "SOFA is running.");
            else
                Finish("FAIL", "SOFA FPS average " + s_result.fpsAvg + " <= min " + s_minFps + " (simulation not running).");
        }

        static void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)
                return;

            s_result.errorCount++;
            string text = type + ": " + message;
            if (text.Length > MaxMessageLength)
                text = text.Substring(0, MaxMessageLength);

            ErrorEntry entry = s_result.errors.Find(e => e.message == text);
            if (entry != null)
                entry.count++;
            else if (s_result.errors.Count < MaxStoredErrors)
                s_result.errors.Add(new ErrorEntry { message = text, count = 1 });
        }

        static void Finish(string status, string reason)
        {
            s_finished = true;
            s_result.status = status;
            s_result.reason = reason;
            if (s_result.fpsSamples == 0)
                s_result.fpsMin = 0.0f;

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(s_output)));
            File.WriteAllText(s_output, JsonUtility.ToJson(s_result, true));
            Debug.Log("[SofaSceneAutoTester] " + status + ": " + reason + " -> " + s_output);

            EditorApplication.Exit(status == "PASS" ? 0 : 1);
        }

        static string GetArg(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == name)
                    return args[i + 1];
            }
            return null;
        }

        static float GetFloatArg(string name, float defaultValue)
        {
            string value = GetArg(name);
            float result;
            if (value != null && float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result))
                return result;
            return defaultValue;
        }
    }
}
