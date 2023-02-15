using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Class to init XRay SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitXRayPlugin
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("VirtualXRay");
    }
#endif
}
