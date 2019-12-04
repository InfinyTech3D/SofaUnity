using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaLoader : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaLoader");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaLoader");
        }
    }

} // namespace SofaUnity
