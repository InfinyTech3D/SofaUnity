using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Constraint component 
    /// </summary>
    public class SofaConstraint : SofaBaseComponent
    {
        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaConstraint");
        }

        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            //SofaLog("UpdateImpl SofaConstraint");
        }
    }

} // namespace SofaUnity