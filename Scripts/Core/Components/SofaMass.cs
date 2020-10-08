using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Mass component 
    /// </summary>
    public class SofaMass : SofaBaseComponent
    {
        // For editor tests
        public int[] myTab;
        public Vector3 m_MyVector = new Vector3(20, 1, 0);
        void Start()
        {
            //Debug.Log("##!!!## SofaMass: Start: ");
            //foreach (string key in m_dataMap.Keys)
            //{
            //    string val = m_dataMap[key];
            //    Debug.Log(key + " | type:  " + val);
            //}

            //if (m_impl != null)
            //{
            //    string type = m_impl.GetComponentType();
            //    Debug.Log("##!!!## SofaMass: " + type);
            //}
            //else
            //    Debug.LogError("SofaMass No Impl at start: ");

        }


        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            //SofaLog("UpdateImpl SofaMass");
        }

        /// Method called by @sa SofaBaseComponent::Create_impl() method. To specify specific types of components
        protected override void FillPossibleTypes()
        {
            string typesS = m_impl.GetPossiblesTypes();
            //Debug.Log("##!!!## SofaMass: " + UniqueNameId + " -> " + typesS);

            m_possibleComponentTypes = ConvertStringToList(typesS);
            m_componentType = m_impl.GetComponentType();

            //myTab = new int[10];
            //myTab[0] = 1;
            //myTab[1] = 1;
            //if (m_impl != null)
            //{
            //    string type = m_impl.GetComponentType();
            //    //Debug.Log("##!!!## SofaMass: " + type);
            //}
            //else
            //    Debug.LogError("SofaMass No Impl at Init: ");
        }

        protected override void FillDataStructure()
        {

        }
    }

} // namespace SofaUnity
