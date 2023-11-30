using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Class to init Imaging SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitImagingPlugin
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("ImagingUS");
    }
#endif
}
