using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaCollisionModel : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaCollisionModel");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaCollisionModel");
        }
    }

} // namespace SofaUnity
