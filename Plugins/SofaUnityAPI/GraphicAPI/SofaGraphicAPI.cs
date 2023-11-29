using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SofaUnityAPI
{

    public class SofaGraphicAPI : IDisposable
    {
        public void Dispose()
        {
        }

        [DllImport("SAPAPI")]
        public static extern int AddRenderEvent_SetTexture(System.IntPtr simuContext, System.IntPtr texture, int w, int h);

        [DllImport("SAPAPI")]
        public static extern int AddRenderEvent_SetPluginTexture(System.IntPtr simuContext, System.IntPtr texture, int w, int h, string objectName);

        [DllImport("VirtualXRay")]
        public static extern int AddRenderEvent_SetVirtualXRayTexture(System.IntPtr simuContext, System.IntPtr texture, int w, int h, string objectName);

        [DllImport("ImagingUS")]
        public static extern int AddRenderEvent_SetImagingUSTexture(System.IntPtr simuContext, System.IntPtr texture, int w, int h, string objectName);

        [DllImport("SAPAPI")]
        public static extern void clearUp(System.IntPtr simuContext);

        [DllImport("SAPAPI")]
        public static extern IntPtr getRenderEventFunc();
    }
    
}
