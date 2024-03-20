using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Generic class describing a Sofa unspecialized component 
    /// 
    /// </summary>
    public class SofaComponent : SofaBaseComponent
    {
        ////////////////////////////////////////////
        //////      SofaComponent members      /////
        ////////////////////////////////////////////

        /// Parameter to show/hide the data in the Editor
        [SerializeField]
        protected bool m_showData = true;

        ////////////////////////////////////////////
        //////   SofaCollisionModel accessors  /////
        ////////////////////////////////////////////

        /// Getter/ Setter to show Data or not @sa m_showData
        public bool ShowData
        {
            get { return m_showData; }
            set { m_showData = value; }
        }

        ////////////////////////////////////////////
        //////        SofaComponent API        /////
        ////////////////////////////////////////////

        protected override void CreateSofaAPI_Impl()
        {
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_isCustom);
        }

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