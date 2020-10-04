using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init Imaging SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitImagingPlugin
{
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("VirtualXRay");
        SofaUnity.PluginManager.Instance.AddPlugin("ImagingUS");
    }
}
