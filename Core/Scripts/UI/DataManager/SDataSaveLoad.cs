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
        private DynamicSDataManager m_SDManager;
        private DynamicDataSaveList m_DataSaveList;

        void Start()
        {
            m_SavePath = Application.dataPath + "/SofaUnity/Core/Scripts/UI/DataManager/DynamicDataSaves/";
            m_SceneName = SceneManager.GetActiveScene().name+".JSON";
            m_DataSaveList = new DynamicDataSaveList();
            m_SDManager = this.GetComponent<DynamicSDataManager>();
            if (m_SDManager == null)
            {
                Debug.LogError("SDataSaveLoad:Can't find any Data manager");
                return;
            }

            saveButton.onClick.AddListener(SaveDynamicData);
            loadButton.onClick.AddListener(LoadDynamicData);


        }

        public void SaveDynamicData()
        {
            m_DataSaveList.dataSaveList.Clear();
            int i = 0;
            foreach(SofaDataReference sdr in m_SDManager.DSDataList)
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

        public void LoadDynamicData()
        {
            if (!File.Exists(m_SavePath + m_SceneName))
            {
                Debug.LogError("JSON file not found: " + m_SavePath + m_SceneName);
                return;
            }

            //m_DataSaveList.dataSaveList.Clear();
            DynamicDataSaveList dataList;
            string json = File.ReadAllText(m_SavePath + m_SceneName);
            if (!string.IsNullOrEmpty(json))
            {
                dataList = JsonUtility.FromJson<DynamicDataSaveList>(json);
                if (dataList.dataSaveList.Count != m_SDManager.DSDataList.Count)
                {
                    Debug.LogError("LoadDynamicData : The Datas that your are trying to load doesn't match this scene datas");
                    return;
                }
                //We make the guess that you didn't change the data order in the editor between the save and the load
                //Heavy to check for a very special case so be carefull

                int i = 0;
                foreach (DynamicDataSave dds in dataList.dataSaveList)
                {
                    UpdateValueFromType(m_SDManager.DSDataList[i], dds.value);
                    //update UI sliders
                    UpdateDynamicDataUI(m_SDManager.DSDataList[i], dds.value);
                    i++;
                }
            }
            else
            {
                Debug.LogError("LoadDynamicData : Data file empty or not found");
            }

            



        }



        public void UpdateValueFromType(SofaDataReference sdr, string newValue)
        {
            if (sdr == null || string.IsNullOrEmpty(newValue))
                return;

            SofaBaseComponent sBaseComp = sdr.sofaComponent;
            string dataName = sdr.dataName;

            if (sBaseComp == null)
                return;

            switch (sdr.dataType)
            {
                case SofaDataType.Vectord:
                    {
                        if (!float.TryParse(newValue, out float parsedFloat))
                        {
                            Debug.LogWarning("Failed to parse Vectord value: " + newValue);
                            return;
                        }

                        float[] valFloatList = new float[1];
                        valFloatList[0] = parsedFloat;

                        int res = sBaseComp.m_impl.SetVectordValue(dataName, 1, valFloatList);
                        if (res != 0)
                            Debug.LogError("Failed to set VectordSizeOne");
                        break;
                    }

                case SofaDataType.Int:
                    {
                        if (!int.TryParse(newValue, out int parsedInt))
                        {
                            Debug.LogWarning("Failed to parse Int value: " + newValue);
                            return;
                        }

                        sBaseComp.m_impl.SetIntValue(dataName, parsedInt);
                        break;
                    }

                case SofaDataType.Float:
                    {
                        if (!float.TryParse(newValue, out float parsedFloat))
                        {
                            Debug.LogWarning("Failed to parse Float value: " + newValue);
                            return;
                        }

                        sBaseComp.m_impl.SetFloatValue(dataName, parsedFloat);
                        break;
                    }

                case SofaDataType.Double:
                    {
                        if (!float.TryParse(newValue, out float parsedDouble))
                        {
                            Debug.LogWarning("Failed to parse Double value: " + newValue);
                            return;
                        }

                        sBaseComp.m_impl.SetDoubleValue(dataName, parsedDouble);
                        break;
                    }

                case SofaDataType.Bool:
                    {
                        if (!bool.TryParse(newValue, out bool parsedBool))
                        {
                            Debug.LogWarning("Failed to parse Bool value: " + newValue);
                            return;
                        }

                        sBaseComp.m_impl.SetBoolValue(dataName, parsedBool);
                        break;
                    }
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdr"></param>
        /// <param name="newValue"></param>
        public void UpdateDynamicDataUI(SofaDataReference sdr, string newValue)
        {
            if (sdr == null || string.IsNullOrEmpty(newValue))
                return;

            if (!float.TryParse(newValue, out float thisValue))
            {
                Debug.LogWarning("Failed to parse Float value: " + newValue);
                return;
            }

            DynamicSdata[] allDynamicData = FindObjectsByType<DynamicSdata>(FindObjectsSortMode.None);
            foreach (DynamicSdata element in allDynamicData)
            {
                if (!string.IsNullOrEmpty(element.GetUIName()))
                {
                    if (element.GetUIName()== sdr.optionalCustomName)
                    {
                        element.GetSlider().value = Mathf.Clamp01(thisValue);
                        return;
                    }
                }
                if (element.GetDataName() == sdr.dataName)
                {
                    element.GetSlider().value = Mathf.Clamp01(thisValue);
                    return;
                }
            }
        }


    }//end class

}//end namespace

