using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SofaUnityAPI
{
    static public class SofaUtils
    {
        /// Application.dataPath: Unity Editor: <path to project folder>/Assets
        static public string GetPluginFullPrefixPath()
        {
            string pluginPath;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (Application.isEditor)
                pluginPath = "/SofaUnity/Core/Plugins/Native/Windows/x64/";
            else
                pluginPath = "/Plugins/x86_64/";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        if (Application.isEditor)
            pluginPath = "/SofaUnity/Core/Plugins/Native/macOS/";
        else
            pluginPath = "/Plugins/x86_64/";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
        if (Application.isEditor)
            pluginPath = "/SofaUnity/Core/Plugins/Native/Linux/x86_64/";
        else
            pluginPath = "/Plugins/x86_64/";
#elif UNITY_ANDROID
        return Application.persistentDataPath;
#else
throw new PlatformNotSupportedException();
#endif
            return Application.dataPath + pluginPath;
        }


        static public string GetNativePluginPath()
        {
            string path;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (Application.isEditor)
                path = "/SofaUnity/Core/Plugins/Native/Windows/x64/";
            else
                path = "/Plugins/x86_64/";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            path = "/SofaUnity/Core/Plugins/Native/macOS/";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            path = "/SofaUnity/Core/Plugins/Native/Linux/x86_64/";
#endif
            return path;
        }

        static public string GetNativeBuildPath()
        {
            string path;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            path = "/Plugins/x86_64/";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            path = "/SofaUnity/Core/Plugins/Native/macOS/";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            path = "/SofaUnity/Core/Plugins/Native/Linux/x86_64/";
#endif
            return path;
        }


        static public string GetPluginFullName(string pluginName)
        {
            string pluginFullPath = "";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            pluginFullPath = pluginName + ".dll";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
            pluginFullPath = "lib" + pluginName + ".so";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            pluginFullPath = "lib" + pluginName + ".dylib";
#endif
            return pluginFullPath;
        }

    }
}

