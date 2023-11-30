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
        //System.IO.FileInfo info = new FileInfo(pathToBuiltProject);
        //string depPath = System.IO.Path.Combine(Application.dataPath, "Dependencies");
        Debug.Log("Path to built project: " + pathToBuiltProject);

        switch (target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                {
                    // Get root data path
                    string rootBuildPath = string.Empty;
                    if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
                    {
                        rootBuildPath = pathToBuiltProject.Replace(".exe", "") + "_Data";
                    }
                    else
                    {
                        rootBuildPath = pathToBuiltProject + "/Contents/Resources/Data";
                    }

                    // Copy current License
                    DirectoryCopy(Application.dataPath + "/SofaUnity/License/", rootBuildPath + "/License/", true);


                    // Copy SOFA ini file
                    //{
                    // Create bin build path
                    string binBuildPath = rootBuildPath + "/SofaUnity/Plugins/Native/x64/";
                    string dataBuildPath = rootBuildPath + "/SofaUnity/scenes/SofaScenes";
                    if (!Directory.Exists(binBuildPath))
                    {
                        Directory.CreateDirectory(binBuildPath);
                        Debug.Log("Create directory " + binBuildPath);
                    }

                    // Update SOFA ini file with build dir paths
                    string outputIniPath = Path.Combine(binBuildPath, "sofa.ini");
                    using (StreamWriter outputIniFile = new StreamWriter(outputIniPath))
                    {
                        outputIniFile.WriteLine("SHARE_DIR=" + dataBuildPath);
                        outputIniFile.WriteLine("EXAMPLES_DIR=" + dataBuildPath);
                        outputIniFile.WriteLine("LICENSE_DIR=" + rootBuildPath + "/License/");
                        Debug.Log("Generate " + outputIniPath + " file.");
                    }
                    //}


                    // Copy SOFA scene folder
                    // Get current scene path
                    bool copied = false;
                    System.IO.FileInfo scenePathInfo = new FileInfo(scenePath);
                    DirectoryInfo sceneDirInfo = scenePathInfo.Directory;
                    string filename = System.IO.Path.GetFileName(scenePath);

                    string fullScenePath = scenePathInfo.DirectoryName;
                    string fullScenePathParent = fullScenePath.Replace(sceneDirInfo.Name, "");

                    string relativeScenePath = scenePath.Replace(filename, "");                    
                    relativeScenePath = relativeScenePath.Replace("Assets", "");
                    string relativeScenePathParent = relativeScenePath.Replace(sceneDirInfo.Name, "");

                    //Debug.Log("fullScenePath: " + fullScenePath);
                    //Debug.Log("fullScenePathParent: " + fullScenePathParent);
                    //Debug.Log("relativeScenePathParent: " + relativeScenePathParent);

                    // test first SofaScenes directory below Unity scenes
                    string buildSofaScenePath = rootBuildPath + relativeScenePath + "/SofaScenes";
                    string currentSofaScenePath = fullScenePath + "/SofaScenes";
                    if (Directory.Exists(currentSofaScenePath))
                    {
                        Debug.Log("Copying: " + currentSofaScenePath + "  ----->  " + buildSofaScenePath);
                        DirectoryCopy(currentSofaScenePath, buildSofaScenePath, true);
                        copied = true;
                    }

                    // test SofaScenes at same level than unity folder
                    string buildSofaScenePath2 = rootBuildPath + relativeScenePathParent + "/SofaScenes";
                    string currentSofaScenePath2 = fullScenePathParent + "/SofaScenes";
                    if (Directory.Exists(currentSofaScenePath2))
                    {
                        
                        Debug.Log("Copying: " + currentSofaScenePath2 + "  ----->  " + buildSofaScenePath2);
                        DirectoryCopy(currentSofaScenePath2, buildSofaScenePath2, true);
                        copied = true;
                    }

                    if (!copied)
                    {
                        Debug.LogError("No 'SofaScenes' folder found to copy at: " + currentSofaScenePath + " or " + currentSofaScenePath2);
                    }

                    break;
                }

        }
    }
}
