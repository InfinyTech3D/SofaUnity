using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


public class ScenesTestRunner : MonoBehaviour
{
    string m_logs;
    Queue m_logsQueue = new Queue();
    public int m_maxstack = 5000;

    List<EditorBuildSettingsScene> m_editorBuildSettingsScenes = null;

    int m_nbrExe = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Add log catching
        Application.logMessageReceived += HandleLog;

        // Create list of scene to add to build settings
        m_editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        // 1. Add basic examples
        string folderExamples = Application.dataPath + "/SofaUnity/Scenes/Examples/";
        m_nbrExe = AddUnitySceneFromFolder(folderExamples);

        // 2. Add Demos Interaction
        string folderInteraction = Application.dataPath + "/SofaUnity/Scenes/Demos/Interaction/";
        AddUnitySceneFromFolder(folderInteraction);

        // 3. Add Demos Endoscopy
        string folderEndoscopy = Application.dataPath + "/SofaUnity/Scenes/Demos/Endoscopy/";
        AddUnitySceneFromFolder(folderEndoscopy);

        // 4. Add Demos Fluoroscopy
        string folderFluoro = Application.dataPath + "/SofaUnity/Scenes/Demos/Imaging/Fluoroscopy/";
        AddUnitySceneFromFolder(folderFluoro);

        // 5. Add Demos Haptics
        string folderHaptics = Application.dataPath + "/SofaUnity/Scenes/Demos/Haptic/";
        AddUnitySceneFromFolder(folderHaptics);


        EditorBuildSettings.scenes = m_editorBuildSettingsScenes.ToArray();
        Debug.Log("Nbr scene Editor: " + EditorSceneManager.loadedSceneCount);
        Debug.Log("Nbr scene Play: " + SceneManager.sceneCount);
        Debug.Log("Nbr scene Build: " + SceneManager.sceneCountInBuildSettings);
    }


    int AddUnitySceneFromFolder(string folderPath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

        FileInfo[] info = dirInfo.GetFiles("*.unity");
        int cptScene = 0;
        foreach (FileInfo f in info)
        {
            string relativepath = f.FullName.Substring(Application.dataPath.Length);
            relativepath = "Assets/" + relativepath.Replace("\\", "/");
            Debug.Log(f.Name + " = " + relativepath);

            m_editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(relativepath, true));
            cptScene++;
        }

        return cptScene;
    }


    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    int m_cptInternal = 0;
    int m_testedLevel = 0; // Last being the scene checker in build settings

    // Update is called once per frame
    void Update()
    {        
        if (m_testedLevel >= SceneManager.sceneCountInBuildSettings - 1)
            return;

        if (m_cptInternal == 50)
        {
            testScene(m_testedLevel);
        }

        if (m_cptInternal == 300)
        {
            // reinit
            closeScene(m_testedLevel);
            m_testedLevel++;
            m_cptInternal = 0;
        }

        m_cptInternal++;
    }

    
    void testScene(int level)
    {
        Scene my_scene = SceneManager.GetSceneByBuildIndex(level);
        Debug.Log("#########  Load Level: " + level + " -> " + my_scene.path + "  #########");
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        Scene test = SceneManager.GetActiveScene();
    }

    bool closeScene(int level)
    {
        Scene my_scene = SceneManager.GetSceneByBuildIndex(level);
        Debug.Log("#########  Close Level: " + level + " -> " + my_scene.name + "  #########");
        takeScreenShot();
        bool res = SceneManager.UnloadScene(level);
        return res;
    }

    void takeScreenShot()
    {
        string folderPath = Directory.GetCurrentDirectory() + "/Assets/SofaUnity/Tests/logs/";
        string screenshotName = "snap_" + m_testedLevel + ".png";
        string fullPath = System.IO.Path.Combine(folderPath, screenshotName);
        ScreenCapture.CaptureScreenshot(fullPath);
        
        Debug.Log("Capture: " + fullPath);
        string logPath = System.IO.Path.Combine(folderPath, "All-Logs.txt");
        StreamWriter writer = new StreamWriter(logPath, false);
        writer.Write(m_logs);
        writer.Close();
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (m_logsQueue.Count > m_maxstack)
            m_logsQueue.Clear();

        m_logs = logString;
        string newString = "\n [" + type + "] : " + m_logs;
        m_logsQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            m_logsQueue.Enqueue(newString);
        }
        m_logs = string.Empty;
        foreach (string mylog in m_logsQueue)
        {
            m_logs += mylog;
        }
    }
}
