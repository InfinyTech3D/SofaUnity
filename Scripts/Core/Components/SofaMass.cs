using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaMass : SofaBaseComponent
    {
        /// Method called by @sa Update() method.
        override public void UpdateImpl()
        {
            SofaLog("UpdateImpl SofaMass");
        }

        override protected void FillPossibleTypes()
        {
            string typesS = m_impl.GetPossiblesTypes();
            Debug.Log("##!!!## SofaMass: " + UniqueNameId + " -> " + typesS);

            m_possibleComponentTypes = ConvertStringToList(typesS);
            m_componentType = m_impl.GetComponentType();
        }

        void Start()
        {
            Debug.Log("##!!!## SofaMass: Data: ");
            foreach (string key in m_dataMap.Keys)
            {
                string val = m_dataMap[key];
                Debug.Log(key + " | type:  " + val);
            }
        }
    }

} // namespace SofaUnity
