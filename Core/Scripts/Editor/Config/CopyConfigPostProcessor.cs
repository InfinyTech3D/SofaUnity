using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace SofaUnity
{
    [InitializeOnLoad]
    public class CopyConfigPostProcessor
    {
        /// <summary>
        /// Generate .ini file on load.
        /// </summary>
        static CopyConfigPostProcessor()
        {
            string sofaIniFile = Application.dataPath + "/SofaUnity/Core/Plugins/Native/x64/sofa.ini";
            using (StreamWriter outputIniFile = new StreamWriter(sofaIniFile))
            {
                string SofaUnityDir = Application.dataPath + "/SofaUnity/scenes/SofaScenes";
                outputIniFile.WriteLine("SHARE_DIR=" + SofaUnityDir);
                outputIniFile.WriteLine("SHARE_DIR=C:/projects/sofa-src/share/");
                outputIniFile.WriteLine("EXAMPLES_DIR=" + SofaUnityDir);
                outputIniFile.WriteLine("LICENSE_DIR=" + Application.dataPath + "/SofaUnity/License/");
                outputIniFile.WriteLine("PYTHON_DIR=" + Application.dataPath + "/SofaUnity/Core/Plugins/Native/x64/");
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
                file.CopyTo(temppath, true);
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

        static string unityScenePath = "";
        static string sofaScenePath = "";

        [PostProcessSceneAttribute(2)]
        public static bool OnPostprocessScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            unityScenePath = scene.path;

            List<GameObject> rootGameObjects= new List<GameObject>();
            SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjects);

            // Retrieving SofaContext object
            SofaContext _sofaContext = null;
            foreach (GameObject obj in rootGameObjects)
            {
                _sofaContext = obj.GetComponent<SofaContext>();
                if (_sofaContext != null)
                    break;
            }

            if (_sofaContext == null)
            {
                return false;
            }

            sofaScenePath = _sofaContext.SceneFileMgr.SceneFilename;
            return true;
        }


        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (unityScenePath.Length == 0)
            {
                bool res = OnPostprocessScene();
                if (!res)
                    Debug.LogError("No 'SofaContext' found in the Root GameObject of the scene: " + unityScenePath);
            }

            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    {
                        Debug.Log("[SofaUnity - Build process] StandaloneWindows build to: " + pathToBuiltProject);
                        BuildForWindows(pathToBuiltProject);

                        break;
                    }
                    // else rootBuildPath = pathToBuiltProject + "/Contents/Resources/Data";
            }
        }

        // Specific method to build SofaUnity folders on windows build. Will create SofaUnity/Core with plugin and sofa.ini file as well as SofaUnity/Scenes/SofaScenes folder
        static void BuildForWindows(string pathToBuiltProject)
        {
            // Get build root Path
            string rootBuildPath = pathToBuiltProject.Replace(".exe", "") + "_Data";

            // Create bin build path
            string binBuildPath = rootBuildPath + "/SofaUnity/Core/Plugins/Native/x64/";
            string dataBuildPath = rootBuildPath + "/SofaUnity/scenes/SofaScenes";

            Directory.CreateDirectory(binBuildPath);
            Debug.Log("[SofaUnity - BuildForWindows] Create directory: " + binBuildPath);
            Directory.CreateDirectory(dataBuildPath);
            Debug.Log("[SofaUnity - BuildForWindows] Create directory: " + dataBuildPath);

            // Copy current License
            DirectoryCopy(Application.dataPath + "/SofaUnity/License/", rootBuildPath + "/License/", true);

            // Update SOFA ini file with build dir paths
            string outputIniPath = Path.Combine(binBuildPath, "sofa.ini");
            using (StreamWriter outputIniFile = new StreamWriter(outputIniPath))
            {
                outputIniFile.WriteLine("SHARE_DIR=" + dataBuildPath);
                outputIniFile.WriteLine("EXAMPLES_DIR=" + dataBuildPath);
                outputIniFile.WriteLine("LICENSE_DIR=" + rootBuildPath + "/License/");
                Debug.Log("[SofaUnity - BuildForWindows] Generate: " + outputIniPath + " file.");
            }

            // Copy SOFA scene folder
            bool copyDone = false;
            Debug.Log("[SofaUnity - BuildForWindows] Unity scene Path: " + unityScenePath);
            Debug.Log("[SofaUnity - BuildForWindows] SOFA scenePath: " + sofaScenePath);

            System.IO.FileInfo sofaScenePathInfo = new FileInfo("Assets/" + sofaScenePath);
            string filename = System.IO.Path.GetFileName(sofaScenePath);
            
            string currentScenePath = sofaScenePathInfo.DirectoryName;
            string relativeScenePath = sofaScenePath.Replace(filename, "");
            relativeScenePath = relativeScenePath.Replace("Assets", "");

            copyDone = CopySofaSceneData(currentScenePath, rootBuildPath + relativeScenePath);

            if (!copyDone)
            {
                Debug.LogError("No 'SofaScenes' folder found to copy Sofa scene related to: " + unityScenePath);
            }
        }


        static bool CopySofaSceneData(string currentSofaScenePath, string buildSofaScenePath)
        {
            if (Directory.Exists(currentSofaScenePath))
            {
                Debug.Log("[SofaUnity - Build process] Copying SofaScenes from directory: " + currentSofaScenePath + "  ----->  " + buildSofaScenePath);
                DirectoryCopy(currentSofaScenePath, buildSofaScenePath, true);
                return true;
            }

            return false;
        }


    }
}
