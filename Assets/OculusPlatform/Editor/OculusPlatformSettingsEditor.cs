namespace Oculus.Platform
{
  using System;
  using UnityEditor;
  using UnityEngine;

  // This classes implements a UI to edit the PlatformSettings class.
  // The UI is accessible from a the menu bar via: Oculus Platform -> Edit Settings
  [CustomEditor(typeof(PlatformSettings))]
  public class OculusPlatformSettingsEditor : Editor
  {
    private bool isUnityEditorSettingsExpanded;
    private bool isBuildSettingsExpanded;

    private void OnEnable()
    {
      isUnityEditorSettingsExpanded = PlatformSettings.UseStandalonePlatform;
#if UNITY_ANDROID
      isBuildSettingsExpanded = true;
#else
      isBuildSettingsExpanded = false;
#endif
    }

    [UnityEditor.MenuItem("Oculus Platform/Edit Settings")]
    public static void Edit()
    {
      UnityEditor.Selection.activeObject = PlatformSettings.Instance;
    }

    public override void OnInspectorGUI()
    {
      //
      // Application IDs section
      //
      GUIContent riftAppIDLabel = new GUIContent("Oculus Rift App Id [?]", "This AppID will be used when building to the Windows target.");
      GUIContent gearAppIDLabel = new GUIContent("Gear VR App Id [?]", "This AppID will be used when building to the Android target");
      PlatformSettings.AppID = MakeTextBox(riftAppIDLabel, PlatformSettings.AppID);
      PlatformSettings.MobileAppID = MakeTextBox(gearAppIDLabel, PlatformSettings.MobileAppID);

      if (GUILayout.Button("Create / Find your app on https://dashboard.oculus.com"))
      {
        UnityEngine.Application.OpenURL("https://dashboard.oculus.com/");
      }

#if UNITY_ANDROID
      if (String.IsNullOrEmpty(PlatformSettings.MobileAppID))
      {
        EditorGUILayout.HelpBox("Please enter a valid Gear VR App ID.", MessageType.Error);
      }
      else
      {
        var msg = "Configured to connect with App ID " + PlatformSettings.MobileAppID;
        EditorGUILayout.HelpBox(msg, MessageType.Info);
      }
#else
      if (String.IsNullOrEmpty(PlatformSettings.AppID))
      {
        EditorGUILayout.HelpBox("Please enter a valid Oculus Rift App ID.", MessageType.Error);
      }
      else
      {
        var msg = "Configured to connect with App ID " + PlatformSettings.AppID;
        EditorGUILayout.HelpBox(msg, MessageType.Info);
      }
#endif
      EditorGUILayout.Separator();

      //
      // Unity Editor Settings section
      //
      isUnityEditorSettingsExpanded = EditorGUILayout.Foldout(isUnityEditorSettingsExpanded, "Unity Editor Settings");
      if (isUnityEditorSettingsExpanded)
      {
        GUIHelper.HInset(6, () =>
        {
          if (PlatformSettings.UseStandalonePlatform &&
            (String.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserEmail) ||
            String.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserPassword)))
          {
            EditorGUILayout.HelpBox("Please enter a valid user credentials.", MessageType.Error);
          }
          else
          {
            var msg = "The Unity editor will use the supplied test user credentials and operate in standalone mode.  Some user data will be mocked.";
            EditorGUILayout.HelpBox(msg,  MessageType.Info);
          }

          var useStandaloneLabel = "Use Standalone Platform [?]";
          var useStandaloneHint = "If this is checked your app will use a debug platform with the User info below.  "
            + "Otherwise your app will connect to the Oculus Platform.  This setting only applies to the Unity Editor";
          PlatformSettings.UseStandalonePlatform =
            MakeToggle(new GUIContent(useStandaloneLabel, useStandaloneHint), PlatformSettings.UseStandalonePlatform);

          GUI.enabled = PlatformSettings.UseStandalonePlatform;

          var emailLabel = "Test User Email: ";
          var emailHint = "Test users can be configured at " +
            "https://dashboard.oculus.com/organizations/<your org ID>/testusers " +
            "however any valid Oculus account email may be used.";
          StandalonePlatformSettings.OculusPlatformTestUserEmail =
            MakeTextBox(new GUIContent(emailLabel, emailHint), StandalonePlatformSettings.OculusPlatformTestUserEmail);

          var passwdLabel = "Test User Password: ";
          var passwdHint = "Password associated with the email address.";
          StandalonePlatformSettings.OculusPlatformTestUserPassword =
            MakePasswordBox(new GUIContent(passwdLabel, passwdHint), StandalonePlatformSettings.OculusPlatformTestUserPassword);

          GUI.enabled = true;
        });
      }
      EditorGUILayout.Separator();

      //
      // Build Settings section
      //
      isBuildSettingsExpanded = EditorGUILayout.Foldout(isBuildSettingsExpanded, "Build Settings");
      if (isBuildSettingsExpanded)
      {
        GUIHelper.HInset(6, () => {
          if (!PlayerSettings.virtualRealitySupported)
          {
            EditorGUILayout.HelpBox("VR Support isn't enabled in the Player Settings", MessageType.Warning);
          }
          else
          {
            EditorGUILayout.HelpBox("VR Support is enabled", MessageType.Info);
          }

          PlayerSettings.virtualRealitySupported = MakeToggle(new GUIContent("Virtual Reality Support"), PlayerSettings.virtualRealitySupported);
          PlayerSettings.bundleVersion = MakeTextBox(new GUIContent("Bundle Version"), PlayerSettings.bundleVersion);
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
          PlayerSettings.bundleIdentifier = MakeTextBox(new GUIContent("Bundle Identifier"), PlayerSettings.bundleIdentifier);
#else
          BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
          PlayerSettings.SetApplicationIdentifier(
            buildTargetGroup,
            MakeTextBox(
              new GUIContent("Bundle Identifier"),
              PlayerSettings.GetApplicationIdentifier(buildTargetGroup)));
#endif
        });
      }
      EditorGUILayout.Separator();
    }

    private string MakeTextBox(GUIContent label, string variable)
    {
      return GUIHelper.MakeControlWithLabel(label, () => {
        GUI.changed = false;
        var result = EditorGUILayout.TextField(variable);
        SetDirtyOnGUIChange();
        return result;
      });
    }
    private string MakePasswordBox(GUIContent label, string variable)
    {
      return GUIHelper.MakeControlWithLabel(label, () => {
        GUI.changed = false;
        var result = EditorGUILayout.PasswordField(variable);
        SetDirtyOnGUIChange();
        return result;
      });
    }


    private bool MakeToggle(GUIContent label, bool variable)
    {
      return GUIHelper.MakeControlWithLabel(label, () => {
        GUI.changed = false;
        var result = EditorGUILayout.Toggle(variable);
        SetDirtyOnGUIChange();
        return result;
      });
    }

    private void SetDirtyOnGUIChange()
    {
      if (GUI.changed)
      {
        EditorUtility.SetDirty(PlatformSettings.Instance);
        GUI.changed = false;
      }
    }
  }
}
