using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SofaUnityAPI;

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
            string sofaIniFile =
                SofaUtils.GetPluginFullPrefixPath() +
                "sofa.ini";

            using (
                StreamWriter outputIniFile =
                    new StreamWriter(
                        sofaIniFile
                    )
            )
            {
                string SofaUnityDir =
                    Application.dataPath +
                    "/SofaUnity/scenes/SofaScenes";

                outputIniFile.WriteLine(
                    "SHARE_DIR=" +
                    SofaUnityDir
                );

                outputIniFile.WriteLine(
                    "EXAMPLES_DIR=" +
                    SofaUnityDir
                );

                outputIniFile.WriteLine(
                    "LICENSE_DIR=" +
                    Application.dataPath +
                    "/SofaUnity/License/"
                );

                outputIniFile.WriteLine(
                    "BUILD_DIR=" +
                    SofaUtils
                        .GetPluginFullPrefixPath()
                );
            }
        }

        // ==================================================
        // DIRECTORY COPY
        // ==================================================

        private static void DirectoryCopy(
            string sourceDirName,
            string destDirName,
            bool copySubDirs)
        {
            if (string.IsNullOrWhiteSpace(
                    sourceDirName))
            {
                Debug.LogError(
                    "[SofaUnity - Build] Source directory path is empty."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(
                    destDirName))
            {
                Debug.LogError(
                    "[SofaUnity - Build] Destination directory path is empty."
                );

                return;
            }

            DirectoryInfo dir =
                new DirectoryInfo(
                    sourceDirName
                );

            // Check BEFORE calling GetDirectories().
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: " +
                    sourceDirName
                );
            }

            DirectoryInfo[] dirs =
                dir.GetDirectories();

            if (!Directory.Exists(
                    destDirName))
            {
                Directory.CreateDirectory(
                    destDirName
                );
            }

            FileInfo[] files =
                dir.GetFiles();

            foreach (
                FileInfo file
                in files)
            {
                // Do not copy meta files.
                if (file.Extension ==
                        ".meta" ||
                    file.Extension ==
                        "meta")
                {
                    continue;
                }

                // Do not copy Unity scene files.
                if (file.Extension ==
                        ".unity" ||
                    file.Extension ==
                        "unity")
                {
                    continue;
                }

                string tempPath =
                    Path.Combine(
                        destDirName,
                        file.Name
                    );

                file.CopyTo(
                    tempPath,
                    true
                );
            }

            if (!copySubDirs)
                return;

            foreach (
                DirectoryInfo subdir
                in dirs)
            {
                string tempPath =
                    Path.Combine(
                        destDirName,
                        subdir.Name
                    );

                DirectoryCopy(
                    subdir.FullName,
                    tempPath,
                    true
                );
            }
        }

        // ==================================================
        // CURRENT SCENE DATA
        // ==================================================

        private static string unityScenePath =
            "";

        private static string sofaScenePath =
            "";

        // ==================================================
        // POST PROCESS SCENE
        // ==================================================

        [PostProcessSceneAttribute(2)]
        public static bool OnPostprocessScene()
        {
            Scene scene =
                SceneManager.GetActiveScene();

            unityScenePath =
                scene.path;

            /*
             * IMPORTANT:
             *
             * Previously this script checked ONLY:
             *
             * rootGameObject.GetComponent<SofaContext>()
             *
             * That fails when SofaContext is inside
             * a Face prefab or another nested hierarchy.
             *
             * We now search the complete hierarchy.
             */

            List<GameObject> rootGameObjects =
                new List<GameObject>();

            scene.GetRootGameObjects(
                rootGameObjects
            );

            SofaContext sofaContext =
                null;

            foreach (
                GameObject rootObject
                in rootGameObjects)
            {
                if (rootObject == null)
                    continue;

                sofaContext =
                    rootObject
                        .GetComponentInChildren
                        <
                            SofaContext
                        >(
                            true
                        );

                if (sofaContext != null)
                    break;
            }

            if (sofaContext ==
                null)
            {
                sofaScenePath =
                    "";

                Debug.LogWarning(
                    "[SofaUnity - Build] No SofaContext found in scene hierarchy: " +
                    unityScenePath
                );

                return false;
            }

            if (sofaContext.SceneFileMgr ==
                null)
            {
                sofaScenePath =
                    "";

                Debug.LogWarning(
                    "[SofaUnity - Build] SofaContext found but SceneFileMgr is NULL."
                );

                return false;
            }

            sofaScenePath =
                sofaContext
                    .SceneFileMgr
                    .SceneFilename;

            if (string.IsNullOrWhiteSpace(
                    sofaScenePath))
            {
                Debug.LogWarning(
                    "[SofaUnity - Build] SofaContext found but SceneFilename is empty."
                );

                return false;
            }

            Debug.Log(
                "[SofaUnity - Build] SofaContext found."
            );

            Debug.Log(
                "[SofaUnity - Build] Unity scene path: " +
                unityScenePath
            );

            Debug.Log(
                "[SofaUnity - Build] SOFA scene path: " +
                sofaScenePath
            );

            return true;
        }

        // ==================================================
        // POST PROCESS BUILD
        // ==================================================

        [PostProcessBuild]
        public static void OnPostprocessBuild(
            BuildTarget target,
            string pathToBuiltProject)
        {
            /*
             * Re-scan if we have no stored SOFA path.
             */
            if (string.IsNullOrWhiteSpace(
                    sofaScenePath))
            {
                bool found =
                    OnPostprocessScene();

                if (!found)
                {
                    Debug.LogWarning(
                        "[SofaUnity - Build] No valid SOFA scene path was found. " +
                        "SOFA scene data copy will be skipped."
                    );
                }
            }

            switch (target)
            {
                case BuildTarget
                    .StandaloneWindows:

                case BuildTarget
                    .StandaloneWindows64:
                    {
                        Debug.Log(
                            "[SofaUnity - Build process] StandaloneWindows build to: " +
                            pathToBuiltProject
                        );

                        BuildForWindows(
                            pathToBuiltProject
                        );

                        break;
                    }
            }
        }

        // ==================================================
        // WINDOWS BUILD
        // ==================================================

        static void BuildForWindows(
            string pathToBuiltProject)
        {
            // ----------------------------------------------
            // Root build path
            // ----------------------------------------------

            string executableDirectory =
                Path.GetDirectoryName(
                    pathToBuiltProject
                );

            string executableName =
                Path.GetFileNameWithoutExtension(
                    pathToBuiltProject
                );

            if (string.IsNullOrWhiteSpace(
                    executableDirectory))
            {
                executableDirectory =
                    Directory
                        .GetCurrentDirectory();
            }

            string rootBuildPath =
                Path.Combine(
                    executableDirectory,
                    executableName +
                    "_Data"
                );

            // ----------------------------------------------
            // Build directories
            // ----------------------------------------------

            string nativeBuildRelativePath =
                SofaUtils.GetNativeBuildPath();

            /*
             * GetNativeBuildPath() can contain slash-prefixed
             * paths, so normalize it before Path.Combine.
             */
            nativeBuildRelativePath =
                nativeBuildRelativePath
                    .TrimStart(
                        '/',
                        '\\'
                    );

            string binBuildPath =
                Path.Combine(
                    rootBuildPath,
                    nativeBuildRelativePath
                );

            string dataBuildPath =
                Path.Combine(
                    rootBuildPath,
                    "SofaUnity",
                    "scenes",
                    "SofaScenes"
                );

            string licenseBuildPath =
                Path.Combine(
                    rootBuildPath,
                    "License"
                );

            Directory.CreateDirectory(
                binBuildPath
            );

            Debug.Log(
                "[SofaUnity - BuildForWindows] Create 'binBuildPath' directory: " +
                binBuildPath
            );

            Directory.CreateDirectory(
                dataBuildPath
            );

            Debug.Log(
                "[SofaUnity - BuildForWindows] Create 'dataBuildPath' directory: " +
                dataBuildPath
            );

            Directory.CreateDirectory(
                licenseBuildPath
            );

            // ----------------------------------------------
            // License
            // ----------------------------------------------

            string licenseSourcePath =
                Application.dataPath +
                "/SofaUnity/License/";

            if (Directory.Exists(
                    licenseSourcePath))
            {
                DirectoryCopy(
                    licenseSourcePath,
                    licenseBuildPath,
                    true
                );
            }
            else
            {
                Debug.LogWarning(
                    "[SofaUnity - BuildForWindows] License folder not found: " +
                    licenseSourcePath
                );
            }

            // ----------------------------------------------
            // sofa.ini
            // ----------------------------------------------

            string outputIniPath =
                Path.Combine(
                    binBuildPath,
                    "sofa.ini"
                );

            using (
                StreamWriter outputIniFile =
                    new StreamWriter(
                        outputIniPath
                    )
            )
            {
                outputIniFile.WriteLine(
                    "SHARE_DIR=" +
                    dataBuildPath
                );

                outputIniFile.WriteLine(
                    "EXAMPLES_DIR=" +
                    dataBuildPath
                );

                outputIniFile.WriteLine(
                    "LICENSE_DIR=" +
                    licenseBuildPath
                );

                outputIniFile.WriteLine(
                    "BUILD_DIR=" +
                    binBuildPath
                );

                Debug.Log(
                    "[SofaUnity - BuildForWindows] Generate: " +
                    outputIniPath +
                    " file."
                );
            }

            // ----------------------------------------------
            // SOFA SCENE DATA
            // ----------------------------------------------

            Debug.Log(
                "[SofaUnity - BuildForWindows] Unity scene Path: " +
                unityScenePath
            );

            Debug.Log(
                "[SofaUnity - BuildForWindows] SOFA scene Path: " +
                sofaScenePath
            );

            /*
             * IMPORTANT:
             *
             * Never continue here with an empty scene path.
             *
             * The old implementation did:
             *
             * filename = Path.GetFileName("");
             *
             * then:
             *
             * "".Replace("", "")
             *
             * which throws:
             *
             * ArgumentException:
             * String cannot be of zero length.
             * Parameter name: oldValue
             */

            if (!string.IsNullOrWhiteSpace(
                    sofaScenePath))
            {
                CopyCurrentSofaScene(
                    rootBuildPath
                );
            }
            else
            {
                Debug.LogWarning(
                    "[SofaUnity - BuildForWindows] SOFA scene path is empty. " +
                    "Skipping SOFA scene folder copy."
                );
            }

            // ----------------------------------------------
            // Python
            // ----------------------------------------------

            string sourcePython3Path =
                SofaUtils
                    .GetPluginFullPrefixPath() +
                "python3/";

            string destPython3Path =
                Path.Combine(
                    binBuildPath,
                    "python3"
                );

            if (Directory.Exists(
                    sourcePython3Path))
            {
                DirectoryCopy(
                    sourcePython3Path,
                    destPython3Path,
                    true
                );

                Debug.Log(
                    "[SofaUnity - BuildForWindows] Copied python3 from directory: " +
                    sourcePython3Path +
                    " -----> " +
                    destPython3Path
                );
            }
            else
            {
                Debug.LogWarning(
                    "[SofaUnity - BuildForWindows] python3 folder not found at: " +
                    sourcePython3Path +
                    " - skipping python3 copy."
                );
            }
        }

        // ==================================================
        // COPY CURRENT SOFA SCENE
        // ==================================================

        private static void CopyCurrentSofaScene(
            string rootBuildPath)
        {
            string normalizedSofaScenePath =
                sofaScenePath
                    .Replace(
                        '\\',
                        '/'
                    )
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                    normalizedSofaScenePath))
            {
                Debug.LogWarning(
                    "[SofaUnity - Build] Cannot copy SOFA scene because its path is empty."
                );

                return;
            }

            /*
             * SceneFilename may be:
             *
             * SofaUnity/scenes/...
             *
             * or:
             *
             * /SofaUnity/scenes/...
             *
             * or:
             *
             * Assets/SofaUnity/scenes/...
             */

            string projectRelativePath =
                normalizedSofaScenePath
                    .TrimStart('/');

            if (!projectRelativePath
                    .StartsWith(
                        "Assets/"
                    ))
            {
                projectRelativePath =
                    "Assets/" +
                    projectRelativePath;
            }

            /*
             * Convert Unity asset path to a real
             * absolute filesystem path.
             */

            string projectRoot =
                Directory.GetParent(
                    Application.dataPath
                ).FullName;

            string absoluteScenePath =
                Path.Combine(
                    projectRoot,
                    projectRelativePath
                        .Replace(
                            '/',
                            Path.DirectorySeparatorChar
                        )
                );

            string currentScenePath =
                Path.GetDirectoryName(
                    absoluteScenePath
                );

            if (string.IsNullOrWhiteSpace(
                    currentScenePath))
            {
                Debug.LogError(
                    "[SofaUnity - Build] Could not determine SOFA scene directory for: " +
                    sofaScenePath
                );

                return;
            }

            // ----------------------------------------------
            // Destination relative path
            // ----------------------------------------------

            string relativeSceneDirectory =
                Path.GetDirectoryName(
                    projectRelativePath
                );

            if (string.IsNullOrWhiteSpace(
                    relativeSceneDirectory))
            {
                Debug.LogError(
                    "[SofaUnity - Build] Could not determine relative SOFA directory."
                );

                return;
            }

            relativeSceneDirectory =
                relativeSceneDirectory
                    .Replace(
                        '\\',
                        '/'
                    );

            if (relativeSceneDirectory
                    .StartsWith(
                        "Assets/"
                    ))
            {
                relativeSceneDirectory =
                    relativeSceneDirectory
                        .Substring(
                            "Assets/"
                                .Length
                        );
            }

            string buildSofaScenePath =
                Path.Combine(
                    rootBuildPath,
                    relativeSceneDirectory
                        .Replace(
                            '/',
                            Path.DirectorySeparatorChar
                        )
                );

            Debug.Log(
                "[SofaUnity - Build] Source SOFA folder: " +
                currentScenePath
            );

            Debug.Log(
                "[SofaUnity - Build] Destination SOFA folder: " +
                buildSofaScenePath
            );

            bool copyDone =
                CopySofaSceneData(
                    currentScenePath,
                    buildSofaScenePath
                );

            if (!copyDone)
            {
                Debug.LogError(
                    "No SOFA scene folder found to copy SOFA scene related to: " +
                    unityScenePath
                );
            }
        }

        // ==================================================
        // COPY SOFA SCENE DATA
        // ==================================================

        static bool CopySofaSceneData(
            string currentSofaScenePath,
            string buildSofaScenePath)
        {
            if (string.IsNullOrWhiteSpace(
                    currentSofaScenePath))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    buildSofaScenePath))
            {
                return false;
            }

            if (!Directory.Exists(
                    currentSofaScenePath))
            {
                Debug.LogWarning(
                    "[SofaUnity - Build process] SOFA source directory does not exist: " +
                    currentSofaScenePath
                );

                return false;
            }

            Debug.Log(
                "[SofaUnity - Build process] Copying SofaScenes from directory: " +
                currentSofaScenePath +
                " -----> " +
                buildSofaScenePath
            );

            DirectoryCopy(
                currentSofaScenePath,
                buildSofaScenePath,
                true
            );

            return true;
        }
    }
}