using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;
using SofaUnityAPI;
using TMPro;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SofaUnityXR
{
    [System.Serializable]
    public class DynamicDataSave
    {
        
        public string dataName;
        public string optionalCustomName;
        public string value;


    }

    [System.Serializable]
    public class DynamicDataSaveList
    {
        public List<DynamicDataSave> dataSaveList = new List<DynamicDataSave>();
    }

    public class SDataSaveLoad : MonoBehaviour
    {
        [Header("Files will be saved in : ")]
        public static string m_SavePath;
        private static string m_SceneName;
        public Button saveButton;
        public Button loadButton;
        private DynamicSDataManager m_SDManger;
        private DynamicDataSaveList m_DataSaveList;

        void Start()
        {
            m_SavePath = Application.dataPath + "/SofaUnity/Core/Scripts/UI/DataManager/DynamicDataSaves/";
            m_SceneName = SceneManager.GetActiveScene().name+".JSON";
            m_DataSaveList = new DynamicDataSaveList();
            m_SDManger = this.GetComponent<DynamicSDataManager>();
            if (m_SDManger == null)
            {
                Debug.LogError("SDataSaveLoad:Can't find any Data manager");
                return;
            }

            saveButton.onClick.AddListener(SaveDynamicData);


        }

        public void SaveDynamicData()
        {
            m_DataSaveList.dataSaveList.Clear();
            int i = 0;
            foreach(SofaDataReference sdr in m_SDManger.DSDataList)
            {
                string valueCall = GetValueFromType(sdr);
                if (valueCall != null)
                {
                    DynamicDataSave my_dynamicDataSave = new DynamicDataSave
                    {
                        dataName = sdr.dataName,
                        optionalCustomName = sdr.optionalCustomName,
                        value = valueCall
                    };
                    m_DataSaveList.dataSaveList.Add(my_dynamicDataSave);
                }
                else
                {
                    Debug.LogError("SDataSaveLoad: Probleme finding the right type of the data to save");
                    i++;
                    return;
                }
                //Debug.Log(sdr.dataName + " has been Saved");
            }
            string json = JsonUtility.ToJson(m_DataSaveList, true);
            File.WriteAllText(m_SavePath+m_SceneName, json);
        }

        public string GetValueFromType(SofaDataReference sdr)
        {
            if (sdr == null || sdr.sofaComponent == null)
                return null;

            SofaBaseComponent sBaseComp = sdr.sofaComponent;
            string dataName = sdr.dataName;

            switch (sdr.dataType)
            {
                case SofaDataType.Vectord:

                    var valFloatList = new float[1];
                    int resVec = sBaseComp.m_impl.GetVectordValue(dataName, 1, valFloatList);
                    if (resVec == 0)
                        return valFloatList[0].ToString();
                    break;

                case SofaDataType.Int:

                    var valInt = sBaseComp.m_impl.GetIntValue(dataName);
                    if (valInt != int.MinValue)
                        return valInt.ToString();
                    break;

                case SofaDataType.Float:

                    var valFloat = sBaseComp.m_impl.GetFloatValue(dataName);
                    if (valFloat!=float.MinValue)
                        return valFloat.ToString();
                    break;

                case SofaDataType.Double:

                    var valDouble = sBaseComp.m_impl.GetDoubleValue(dataName);
                    if (valDouble != float.MinValue)
                        return valDouble.ToString();
                    break;

                case SofaDataType.Bool:

                    var valBool = sBaseComp.m_impl.GetBoolValue(dataName);
                    //no real way to test
                    return valBool.ToString();
            }

            
            return null;
        }

    }//end class

}//end namespace

