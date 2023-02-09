using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init Geomagic SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitGeomagicPlugin
{
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("SofaHaptics");
        SofaUnity.PluginManager.Instance.AddPlugin("Geomagic");
    }
}
