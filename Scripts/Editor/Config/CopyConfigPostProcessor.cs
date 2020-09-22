using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class CopyConfigPostProcessor
{
    /// <summary>
    /// Generate .ini file on load.
    /// </summary>
    static CopyConfigPostProcessor()
    {
        string sofaIniFile = Application.dataPath + "/SofaUnity/Plugins/Native/x64/sofa.ini";
        using (StreamWriter outputIniFile = new StreamWriter(sofaIniFile))
        {
            string SofaUnityDir = Application.dataPath + "/SofaUnity/scenes/SofaScenes";
            outputIniFile.WriteLine("SHARE_DIR=" + SofaUnityDir);
            outputIniFile.WriteLine("SHARE_DIR=C:/projects/sofa-src/share/");
            outputIniFile.WriteLine("EXAMPLES_DIR=" + SofaUnityDir);
            outputIniFile.WriteLine("LICENSE_DIR=" + Application.dataPath + "/SofaUnity/License/");
            outputIniFile.WriteLine("PYTHON_DIR=" + Application.dataPath + "/SofaUnity/Plugins/Native/x64/");

            Debug.Log("Generate " + sofaIniFile + " file.");
        }
    }

    private static void DirectoryCopy(
        string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the source directory does not exist, throw an exception.
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory does not exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }


        // Get the file contents of the directory to copy.
        FileInfo[] files = dir.GetFiles();

        foreach (FileInfo file in files)
        {
            // do not copy meta files
            if (file.Extension == ".meta" || file.Extension == "meta")
                continue;

            // do not copy unity scene file
            if (file.Extension == ".unity" || file.Extension == "unity")
                continue;


            // Create the path to the new copy of the file.
            string temppath = Path.Combine(destDirName, file.Name);

            // Copy the file.
            file.CopyTo(temppath, false);
        }

        // If copySubDirs is true, copy the subdirectories.
        if (copySubDirs)
        {

            foreach (DirectoryInfo subdir in dirs)
            {
                // Create the subdirectory.
                string temppath = Path.Combine(destDirName, subdir.Name);

                // Copy the subdirectories.
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    static string scenePath = "";

    [PostProcessSceneAttribute(2)]
    public static void OnPostprocessScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        scenePath = scene.path;        
    }


    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        System.IO.FileInfo info = new FileInfo(pathToBuiltProject);
        //string depPath = System.IO.Path.Combine(Application.dataPath, "Dependencies");
        Debug.Log("Path to built project: " + pathToBuiltProject);

        switch (target)
        {
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                {
                    string dataDir = string.Empty;
                    if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
                    {
                        dataDir = pathToBuiltProject.Replace(".exe", "") + "_Data";
                    }
                    else
                    {
                        dataDir = pathToBuiltProject + "/Contents/Resources/Data";
                    }

                    string SofaUnityDir = dataDir + "/SofaUnity/scenes/SofaScenes";
                    string currentSofaDir = System.IO.Path.Combine(Application.dataPath, "SofaUnity/scenes/SofaScenes");

                    Debug.Log("Copying: " + currentSofaDir + " to " + SofaUnityDir);
                    DirectoryCopy(currentSofaDir, SofaUnityDir, true);

                    DirectoryCopy(Application.dataPath + "/SofaUnity/License/", dataDir + "/License/", true);

                    string buildPathIniDir = dataDir + "/SofaUnity/Plugins/Native/x64/";
                    if (!Directory.Exists(buildPathIniDir))
                    {
                        Directory.CreateDirectory(buildPathIniDir);
                        Debug.Log("Create directory " + buildPathIniDir);
                    }

                    string outputIniPath = Path.Combine(buildPathIniDir, "sofa.ini");
                    using (StreamWriter outputIniFile = new StreamWriter(outputIniPath))
                    {
                        outputIniFile.WriteLine("SHARE_DIR=" + SofaUnityDir);
                        outputIniFile.WriteLine("EXAMPLES_DIR=" + SofaUnityDir);
                        outputIniFile.WriteLine("LICENSE_DIR=" + dataDir + "/License/");
                        Debug.Log("Generate " + outputIniPath + " file.");
                    }


                    Debug.Log("currentScene path: " + scenePath);
                    if (scenePath.Length != 0)
                    {
                        System.IO.FileInfo scenePathInfo = new FileInfo(scenePath);

                        string scenePathDir = dataDir + "/SofaUnity/scenes/";
                        DirectoryInfo dirInfo = scenePathInfo.Directory;
                        Debug.Log("scenePathInfo.Name: " + dirInfo.Parent.Name);
                        Debug.Log("Copying: " + scenePathInfo.DirectoryName + " to " + scenePathDir);
                        DirectoryCopy(scenePathInfo.DirectoryName, scenePathDir, true);
                    }

                    break;
                }

        }
    }
}
