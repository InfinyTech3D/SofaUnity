namespace Oculus.Platform
{
  using UnityEngine;
  using System;
  using System.Collections;
  using System.Runtime.InteropServices;

  public sealed class StandalonePlatform
  {
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UnityLogDelegate(IntPtr tag, IntPtr msg);

    public Request<Models.PlatformInitialize> InitializeInEditor()
    {
#if UNITY_ANDROID
      if (String.IsNullOrEmpty(PlatformSettings.MobileAppID))
      {
        throw new UnityException("Update your App ID by selecting 'Oculus Platform' -> 'Edit Settings'");
      }
      var appID = PlatformSettings.MobileAppID;
#else
      if (String.IsNullOrEmpty(PlatformSettings.AppID))
      {
        throw new UnityException("Update your App ID by selecting 'Oculus Platform' -> 'Edit Settings'");
      }
      var appID = PlatformSettings.AppID;
#endif
      if (String.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserEmail))
      {
        throw new UnityException("Update your standalone email address by selecting 'Oculus Platform' -> 'Edit Settings'");
      }
      if (String.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserPassword))
      {
        throw new UnityException("Update your standalone user password by selecting 'Oculus Platform' -> 'Edit Settings'");
      }
      CAPI.ovr_UnityResetTestPlatform();
      CAPI.ovr_UnityInitGlobals(IntPtr.Zero);

      CAPI.OculusInitParams init = new CAPI.OculusInitParams();
      init.sType = 1; // ovrPlatformStructureType_OculusInitParams
      init.appId = UInt64.Parse(appID);
      init.email = StandalonePlatformSettings.OculusPlatformTestUserEmail;
      init.password = StandalonePlatformSettings.OculusPlatformTestUserPassword;

      return new Request<Models.PlatformInitialize>(CAPI.ovr_Platform_InitializeStandaloneOculus(ref init));
    }
  }
}
