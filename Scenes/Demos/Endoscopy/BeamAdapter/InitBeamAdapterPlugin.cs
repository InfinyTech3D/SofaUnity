using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init BeamAdapter SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitBeamAdapterPlugin
{
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("BeamAdapter");
    }
}
