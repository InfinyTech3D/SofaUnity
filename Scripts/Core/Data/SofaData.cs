using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    //public class SData<T> : SBaseData
    public class SofaData : SofaBaseData
    {
        public string m_type = "None";

        public SofaData(string nameID, string type, SBaseObject owner)
            : base(nameID, owner)
        {
            m_type = type;
        }

        //readonly int m_size;
        //T[] values;

        public int dataSize = 0;

        public void init()
        {

        }

        public override string getType()
        {
            return m_type;
            //Type toto = typeof(T);
            //Debug.Log("Type: " + toto);
        }

        
    }
}
