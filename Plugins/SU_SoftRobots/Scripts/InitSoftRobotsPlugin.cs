using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init SoftRobots SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitSoftRobotsPlugin
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("SoftRobots");
    }
#endif
}
