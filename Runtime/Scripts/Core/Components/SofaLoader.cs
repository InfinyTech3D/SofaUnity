using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Loader component 
    /// </summary>
    public class SofaLoader : SofaBaseComponent
    {
        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaLoader");
        }


        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            //SofaLog("UpdateImpl SofaLoader");
        }
    }

} // namespace SofaUnity
