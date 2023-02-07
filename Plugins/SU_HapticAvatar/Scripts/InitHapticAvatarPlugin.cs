using UnityEngine;
using UnityEditor;

/// <summary>
/// Class to init HapticAvatar SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
/// </summary>
class InitHapticAvatarPlugin
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void RegisterPluginDll()
    {
        SofaUnity.PluginManager.Instance.AddPlugin("SofaGeneralRigid");
        SofaUnity.PluginManager.Instance.AddPlugin("SofaHaptics");
        SofaUnity.PluginManager.Instance.AddPlugin("SofaHapticAvatar");
    }
#endif
}
