using UnityEngine;
using UnityEditor;

namespace SofaUnity
{
    /// <summary>
    /// Class to init HaplyRobotics SOFA-Unity plugin. Can be used to load a specific dll plugin inside SOFA or load prefabs
    /// </summary>
    class InitHaplyRoboticsPlugin
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void RegisterPluginDll()
        {
            SofaUnity.PluginManager.Instance.AddPlugin("Sofa.Component.Haptics");
            SofaUnity.PluginManager.Instance.AddPlugin("SofaHaplyRobotics");
        }
#endif
    }
}
