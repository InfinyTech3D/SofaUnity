using UnityEngine;
using UnityEditor;

namespace SofaUnity
{
    /// <summary>
    /// Class to init HapticAvatar SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
    /// </summary>
    class InitHapticAvatarPlugin
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void RegisterPluginDll()
        {
            SofaUnity.PluginManager.Instance.AddPlugin("Sofa.Component.Haptics");
            SofaUnity.PluginManager.Instance.AddPlugin("SofaHapticAvatar");
        }
#endif
    }
}

