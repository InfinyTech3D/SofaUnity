using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaFEMForceField : SofaBaseComponent
    {
        protected override void FillPossibleTypes()
        {
            SofaLog("FillPossibleTypes SofaFEMForceField");
        }

        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaFEMForceField");
        }
    }

} // namespace SofaUnity
