using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMechanicalMapping : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaMechanicalMapping");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaMechanicalMapping");
        }
    }

} // namespace SofaUnity
