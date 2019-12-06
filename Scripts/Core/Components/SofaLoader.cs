using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaLoader : SofaBaseComponent
    {
        protected override void FillPossibleTypes()
        {
            SofaLog("FillPossibleTypes SofaLoader");
        }


        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaLoader");
        }
    }

} // namespace SofaUnity
