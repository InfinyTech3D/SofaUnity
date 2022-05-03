using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init HaplyRobotics SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitHaplyRoboticsPlugin
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("SofaHaptics");
        SofaUnity.PluginManager.Instance.AddPlugin("SofaHaplyRobotics");
    }
#endif
}
