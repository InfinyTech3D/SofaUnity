using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Loader component 
    /// </summary>
    public class SofaAnimationLoop : SofaBaseComponent
    {
        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            string typesS = m_impl.GetPossiblesTypes();
            m_possibleComponentTypes = ConvertStringToList(typesS);
            //Debug.Log("typesS: " + m_possibleComponentTypes);
        }


        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {

        }
    }

} // namespace SofaUnity
