using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SofaUnity
{
    // TODO find a way to interactively add more type if plugin are loaded
    public enum SBaseComponentType
    {
        SofaSolver,
        SofaLoader,
        SofaMesh,
        SofaMass,
        SofaFEMForceField,
        SofaMechanicalMapping,
        SofaCollisionModel,
        SofaVisualModel,
        SofaUnknown
    };  

    public class SofaBaseComponent : SofaBase
    {
        // do generic stuff for baseComponent here
        public SofaDAGNode m_ownerNode = null;
        
        public SBaseComponentType m_baseComponentType;
        protected List<string> m_possibleComponentTypes;
        public string m_componentType;

        /// Pointer to the Sofa Context API.
        protected SofaBaseComponentAPI m_impl = null;

        public string BaseTypeToString(SBaseComponentType type)
        {
            return type.ToString();
        }

        public SBaseComponentType BaseTypeFromString(string typeS)
        {
            SBaseComponentType enumRes = SBaseComponentType.SofaUnknown;
            var enumValues = Enum.GetValues(typeof(SBaseComponentType));
            foreach (SBaseComponentType enumVal in enumValues)
                if (enumVal.ToString() == typeS)
                    return enumVal;

            return enumRes;
        }

        public void setDAGNode(SofaDAGNode _node)
        {
            m_ownerNode = _node;
            m_sofaContext = m_ownerNode.m_sofaContext;
        }


        ////////////////////////////////////////////
        /////          Component API           /////
        ////////////////////////////////////////////

        protected override void InitImpl()
        {
            Debug.Log("####### SofaBaseComponent::InitImpl: " + UniqueNameId);
            if (m_impl == null)
            {
                CreateSofaAPI();

                FillPossibleTypes();

                GetAllData();
            }
            else
                Debug.Log("SofaBaseComponent::InitImpl, already created: " + UniqueNameId);
        }


        protected virtual void CreateSofaAPI()
        {
            Debug.Log("####### SofaBaseComponent::CreateSofaAPI: " + UniqueNameId);
            if (m_impl != null)
            {
                Debug.LogError("SofaBaseComponent " + UniqueNameId + " already has a SofaBaseComponentAPI.");
                return;
            }

            m_impl = new SofaBaseComponentAPI(m_sofaContext.getSimuContext(), UniqueNameId);
        }


        protected virtual void FillPossibleTypes()
        {

        }




        ////////////////////////////////////////////
        /////        Internal Sata API         /////
        ////////////////////////////////////////////

        /// List of Data parsed
        protected List<SData> m_datas = null;
        public List<SData> datas
        {
            get { return m_datas; }
        }

        /// Map of the Data parsed. Key is the dataName of the Data, value is the type of this Data.
        protected Dictionary<string, string> m_dataMap = null;
        public Dictionary<string, string> dataMap
        {
            get { return m_dataMap; }
        }

        virtual protected void GetAllData()
        {
            if (m_impl != null)
            {
                string allData = m_impl.LoadAllData();
                SofaLog("AllData: " + allData);
                if (allData == "None")
                    return;

                List<String> datas = allData.Split(';').ToList();
                m_dataMap = new Dictionary<string, string>();
                m_datas = new List<SData>();
                foreach (String data in datas)
                {
                    String[] values = data.Split(',');
                    if (values.GetLength(0) == 2)
                    {
                        m_datas.Add(new SData(values[0], values[1], null));
                    }
                }
            }
        }
    }

} // namespace SofaUnity