using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa ODE solver and linear solver components in a signle one. 
    /// </summary>
    public class SofaSolver : SofaBaseComponent
    {
        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaSolver");
        }

        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            //SofaLog("UpdateImpl SofaSolver");
        }
    }

} // namespace SofaUnity
