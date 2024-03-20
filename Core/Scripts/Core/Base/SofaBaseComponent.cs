using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SofaUnity
{
    /// Enum listing the different Base type of SOFA component
    /// 
    /// TODO find a way to interactively add more type if plugin are loaded
    public enum SBaseComponentType
    {
        SofaAnimationLoop,
        SofaBehaviorModel,
        SofaCollisionAlgorithm,
        SofaCollisionDetection,
        SofaCollisionIntersection,
        SofaCollisionModel,
        SofaCollisionPipeline,
        SofaConfigurationSetting,
        SofaConstraint,
        SofaConstraintSolver,
        SofaContextObject,
        SofaController,
        SofaEngine,
        SofaFEMForceField,
        SofaForceFieldAndMass,
        SofaInteractionForceField,
        SofaLoader,
        SofaMass,
        SofaMechanicalMapping,
        SofaMesh,
        SofaRequiredPlugin,
        SofaOdeSolver,
        SofaLinearSolver,
        SofaVisualModel,
        SofaUnknown
    };

    
    /// <summary>
    /// Base class representing a SOFA component in the simulation as a GameObject.
    /// Will hold the list of Sofa Data and links inside the \sa m_dataArchiver and \sa m_linkArchiver
    /// </summary>
    public class SofaBaseComponent : SofaBase
    {
        ////////////////////////////////////////////
        //////    SofaBaseComponent members    /////
        ////////////////////////////////////////////

        /// Pointer to the DAGNode parent of this object.
        public SofaDAGNode m_ownerNode = null;
        
        /// base type of this component
        public SBaseComponentType m_baseComponentType;
        
        /// specialisation type of this component        
        public string m_componentType;
        /// List of possible specialisation of this component
        protected List<string> m_possibleComponentTypes;

        /// Pointer to the Sofa Context API.
        public SofaBaseComponentAPI m_impl = null;

        /// Pointer to this component Data archiver \sa SofaDataArchiver
        [SerializeField]
        public SofaDataArchiver m_dataArchiver = null;

        /// Pointer to this component Link archiver \sa SofaLinkArchiver
        [SerializeField]
        public SofaLinkArchiver m_linkArchiver = null;

        /// Bool to check if name the gameobject owner need to be updated by this object.
        protected bool m_propagateName = true;

        ////////////////////////////////////////////
        //////   SofaBaseComponent accessors   /////
        ////////////////////////////////////////////

        /// convert a BaseComponentType into string 
        public string BaseTypeToString(SBaseComponentType type)
        {
            return type.ToString();
        }

        /// convert a string into a BaseComponentType 
        public SBaseComponentType BaseTypeFromString(string typeS)
        {
            SBaseComponentType enumRes = SBaseComponentType.SofaUnknown;
            var enumValues = Enum.GetValues(typeof(SBaseComponentType));
            foreach (SBaseComponentType enumVal in enumValues)
                if (enumVal.ToString() == typeS)
                    return enumVal;

            return enumRes;
        }

        /// Set this DAGnode holder
        public void SetDAGNode(SofaDAGNode _node)
        {
            m_ownerNode = _node;
            m_sofaContext = m_ownerNode.m_sofaContext;
        }

        /// Setter for @sa m_propagateName
        public void SetPropagateName(bool value)
        {
            m_propagateName = value;
        }

        /// Method to destroy this component
        public void DestroyComponent(bool killGameObject = false)
        {
            if (killGameObject)
            {
                Component[] components;
                components = gameObject.GetComponents(typeof(SofaBaseComponent));

                if (components.Length == 1)
                    DestroyImmediate(gameObject);
            }

            DestroyImmediate(this);
        }

        ////////////////////////////////////////////
        /////  SofaBaseComponent internal API  /////
        ////////////////////////////////////////////

        /// Method called by @sa SofaBase::Create() when creating objects. Will create this component and its Data and link.
        protected override void Create_impl()
        {
            if (m_impl == null)
            {
                // Creation method of Sofa component API
                CreateSofaAPI();
                
                // overide method to fill specific possible types
                FillPossibleTypes();

                // overide name with current type
                SetComponentType();

                // Generic Data section
                GetAllData();

                // Generic Link section
                GetAllLinks();

                // overide method to fill specific data section
                FillDataStructure();
            }
            else
                SofaLog("SofaBaseComponent::InitImpl, already created: " + UniqueNameId, 1);
        }


        /// Method called by @sa SofaBase::Reconnect() when reloading objects. Will reconnect the component to its Sofa real component
        protected override void Reconnect_impl()
        {
            // 1- reconnect with SofaBaseComponentAPI
            CreateSofaAPI();

            // 2- reconnect and update edited data
            if (m_dataArchiver == null)
            {
                SofaLog("SofaBaseComponent::ReconnectImpl has a null DataArchiver.", 2);
                return;
            }

            // Add backward compatibility if project was not using m_dataArray
            //if (m_dataArchiver.m_dataArray == null)
            {
                m_dataArchiver = null;
                GetAllData();
            }

            bool modified = m_dataArchiver.UpdateEditedData();
            if (modified)
            {
                SofaLog("SofaBaseComponent::ReconnectImpl some Data modified will reinit component.");
                // call reinit here?
            }
        }


        /// Internal method to check SofaContext status and create/link to the real Sofa component. Will call CreateSofaAPI_Impl()
        protected void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaBaseComponent " + UniqueNameId + " already has a SofaBaseComponentAPI.");
                return;
            }

            if (m_sofaContext == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext is null", 1);
                return;
            }

            if (m_sofaContext.GetSimuContext() == IntPtr.Zero)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext.GetSimuContext() is null", 1);
                return;
            }

            CreateSofaAPI_Impl();

            m_isReady = true;
        }


        ////////////////////////////////////////////
        /////   SofaBaseComponent virtual API  /////
        ////////////////////////////////////////////

        /// Method called by @sa CreateSofaAPI() method. To be implemented by child class if specific ComponentAPI has to be created.
        protected virtual void CreateSofaAPI_Impl()
        {
            SofaLog("SofaBaseComponent::CreateSofaAPI_Impl: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaBaseComponentAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_isCustom);
        }


        /// Method called by @sa Create_impl() method. To be implemented by child class if specific type need to be set.
        protected virtual void SetComponentType()
        {
            // overide name with current type
            m_componentType = m_impl.GetComponentType();
            m_displayName = m_impl.GetComponentDisplayName();
            if (this.gameObject && m_propagateName)
                this.gameObject.name = m_componentType + "  -  " + m_displayName;
        }


        /// Method called by @sa Create_impl() method. To be implemented by child class.
        protected virtual void FillPossibleTypes()
        {
            
        }


        /// Method called by @sa Create_impl() method. To be implemented by child class.
        protected virtual void FillDataStructure()
        {

        }


        ////////////////////////////////////////////
        /////        Internal DATA API         /////
        ////////////////////////////////////////////

        /// Get this component list of Data and fill the DataArchiver
        virtual protected void GetAllData()
        {
            if (m_impl != null)
            {
                string allData = m_impl.LoadAllData();
                if (allData == "None")
                    return;

                if (m_dataArchiver == null)
                    m_dataArchiver = new SofaDataArchiver();

                List<String> datas = allData.Split(';').ToList();
                foreach (String data in datas)
                {
                    String[] values = data.Split(',');
                    if (values.GetLength(0) == 2)
                    {
                        m_dataArchiver.AddData(this, values[0], values[1]);
                    }
                }
            }
            else
            {
                SofaLog("GetAllData: m_impl is null.", 1);
            }
        }


        /// Get this component list of Links and fill the LinkArchiver
        virtual protected void GetAllLinks()
        {
            if (m_impl != null)
            {
                string allLinks = m_impl.LoadAllLinks();
                if (allLinks == "None" || allLinks.Length == 0)
                    return;

                List<String> links = allLinks.Split(';').ToList();
                if (m_linkArchiver == null)
                    m_linkArchiver = new SofaLinkArchiver();

                foreach (String link in links)
                {
                    String[] values = link.Split(',');
                    
                    if (values.GetLength(0) == 3)
                    {
                        m_linkArchiver.AddLink(this, values[0], values[2]);
                    }
                }
            }
            else
            {
                SofaLog("GetAllLinks: m_impl is null.", 1);
            }
        }
    }

} // namespace SofaUnity
