using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaFEMForceField : SofaBaseComponent
    {
        override public void Init()
        {
            SofaLog("Init SofaFEMForceField");
        }


        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaFEMForceField");
        }
    }

} // namespace SofaUnity
