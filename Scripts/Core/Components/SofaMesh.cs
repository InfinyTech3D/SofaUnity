using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMesh : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaMesh");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaMesh");
        }
    }

} // namespace SofaUnity
