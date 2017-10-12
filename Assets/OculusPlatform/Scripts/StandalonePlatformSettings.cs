namespace Oculus.Platform
{
  using UnityEngine;

  // This only exists for the Unity Editor
  public sealed class StandalonePlatformSettings : ScriptableObject
  {
    public static string OculusPlatformTestUserEmail
    {
      get
      {
#if UNITY_EDITOR
        return UnityEditor.EditorPrefs.GetString("OculusStandaloneUserEmail");
#else
        return string.Empty;
#endif
      }
      set
      {
#if UNITY_EDITOR
        UnityEditor.EditorPrefs.SetString("OculusStandaloneUserEmail", value);
#endif
      }
    }

    public static string OculusPlatformTestUserPassword
    {
      get
      {
#if UNITY_EDITOR
        return UnityEditor.EditorPrefs.GetString("OculusStandaloneUserPassword");
#else
        return string.Empty;
#endif
      }
      set
      {
#if UNITY_EDITOR
        UnityEditor.EditorPrefs.SetString("OculusStandaloneUserPassword", value);
#endif
      }
    }
  }
}
