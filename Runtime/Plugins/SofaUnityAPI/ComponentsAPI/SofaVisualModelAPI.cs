using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace SofaUnityAPI
{
    public class SofaVisualModelAPI : SofaBaseComponentAPI
    {
        public SofaVisualModelAPI(IntPtr simu, string nameID, bool isCustom = false)
            : base(simu, nameID, isCustom)
        {

        }
    }
}
