using UnityEditor;
using UnityEngine;

#if UNITY_ANDROID
#if UNITY_EDITOR
public class GenerateZIP
{
    [MenuItem("Assets/Build ZIP")]
    static void BuildZIP()
    {
        string dataDirectory = Application.dataPath + "/SofaUnity/";
        string fileToCreate = Application.streamingAssetsPath + "/Resources.tgz";

        Utility_SharpZipCommands.CreateTarGZ_FromDirectory(fileToCreate, dataDirectory);
    }
}
#endif
#endif