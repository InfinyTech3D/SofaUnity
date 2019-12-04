using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMass : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaMass");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaMass");
        }
    }

} // namespace SofaUnity
