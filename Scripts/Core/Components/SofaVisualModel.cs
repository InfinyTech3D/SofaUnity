using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaVisualModel : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaVisualModel");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaVisualModel");
        }

    }

} // namespace SofaUnity
