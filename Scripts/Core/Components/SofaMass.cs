using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMass : SofaBaseComponent
    {
        void Start()
        {
            Debug.Log("##!!!## SofaMass: Start: ");
            foreach (string key in m_dataMap.Keys)
            {
                string val = m_dataMap[key];
                // Debug.Log(key + " | type:  " + val);
            }

            if (m_impl != null)
            {
                string type = m_impl.GetComponentType();
                Debug.Log("##!!!## SofaMass: " + type);
            }
            else
                Debug.LogError("SofaMass No Impl at start: ");

        }


        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            //SofaLog("UpdateImpl SofaMass");
        }

        protected override void FillPossibleTypes()
        {
            string typesS = m_impl.GetPossiblesTypes();
            //Debug.Log("##!!!## SofaMass: " + UniqueNameId + " -> " + typesS);

            m_possibleComponentTypes = ConvertStringToList(typesS);
            m_componentType = m_impl.GetComponentType();

            if (m_impl != null)
            {
                string type = m_impl.GetComponentType();
                Debug.Log("##!!!## SofaMass: " + type);
            }
            else
                Debug.LogError("SofaMass No Impl at Init: ");
        }       
    }

} // namespace SofaUnity
