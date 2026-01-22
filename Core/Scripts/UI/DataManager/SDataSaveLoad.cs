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
        public SofaDataType dataType; // help to indentify special cases (vec3)


    }

    [System.Serializable]
    public class DynamicDataSaveList
    {
        public List<DynamicDataSave> dataSaveList = new List<DynamicDataSave>();
    }

    /// <summary>
    /// Class used to save and load datas from DynamicSDataManager script
    /// </summary>
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
            m_SceneName = SceneManager.GetActiveScene().name + ".JSON";
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

        /// <summary>
        /// Fonction call by main save button
        /// </summary>
        public void SaveDynamicData()
        {
            if (!File.Exists(m_SavePath))
            {
#if !UNITY_EDITOR
                //build mode
                Debug.LogWarning($"File not find in {m_SavePath}. Creation in Application.dataPath.");
                m_SavePath = Application.dataPath;
#endif
            }
            m_DataSaveList.dataSaveList.Clear();
            int i = 0;
            foreach (SofaDataReference sdr in m_SDManager.DSDataList)
            {
                string valueCall = GetValueFromType(sdr);
                if (valueCall != null)
                {
                    DynamicDataSave my_dynamicDataSave = new DynamicDataSave
                    {
                        dataName = sdr.dataName,
                        optionalCustomName = sdr.optionalCustomName,
                        value = valueCall,
                        dataType = sdr.dataType
                    };
                    m_DataSaveList.dataSaveList.Add(my_dynamicDataSave);
                }
                else
                {
                    Debug.LogError("SDataSaveLoad: Probleme finding the right type of the data to save for:"+ sdr.dataName);
                    i++;
                    return;
                }
                //Debug.Log(sdr.dataName + " has been Saved");
            }
            string json = JsonUtility.ToJson(m_DataSaveList, true);
            File.WriteAllText(m_SavePath + m_SceneName, json);
        }

        /// <summary>
        /// Fonction call by main Load button
        /// </summary>
        public void LoadDynamicData()
        {
            if (!File.Exists(m_SavePath))
            {
#if !UNITY_EDITOR
                Debug.LogWarning($"File not find in {m_SavePath}. Creation in Application.dataPath.");
                m_SavePath = Application.dataPath;
#endif
            }
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
                    UpdateDynamicDataUI(m_SDManager.DSDataList[i], dds.value);
                    i++;
                }
            }
            else
            {
                Debug.LogError("LoadDynamicData : Data file empty or not found");
            }


        }


        /// <summary>
        /// use to update the real sofa data after loading datas from file
        /// </summary>
        /// <param name="sdr"></param>
        /// <param name="newValue"></param>
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
                case SofaDataType.Vec3:
                    {
                        // Le format est "x,y,z"
                        string[] values = newValue.Split('.');
                        foreach(string toto in values)
                        {
                            Debug.Log("values is : " + toto);
                        }

                        if (values.Length != 3)
                        {
                            Debug.LogWarning("Failed to parse Vec3 value: " + newValue + ". Expected format: x,y,z");
                            return;
                        }

                        if (!float.TryParse(values[0], out float x) ||
                            !float.TryParse(values[1], out float y) ||
                            !float.TryParse(values[2], out float z))
                        {
                            Debug.LogWarning("Failed to parse Vec3 components: " + newValue);
                            return;
                        }

                        Vector3 vec3Values = new Vector3 ( x, y, z );
                        sBaseComp.m_impl.SetVector3Value(dataName, vec3Values,true);
                        break;
                    }

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


        /// <summary>
        /// Get the value of a data using the sofaunity API and return it as a string
        /// </summary>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public string GetValueFromType(SofaDataReference sdr)
        {
            if (sdr == null || sdr.sofaComponent == null)
                return null;

            SofaBaseComponent sBaseComp = sdr.sofaComponent;
            string dataName = sdr.dataName;

            switch (sdr.dataType)
            {
                case SofaDataType.Vec3:
                    var vec3Values = new Vector3(0,0,0);
                    vec3Values = sBaseComp.m_impl.GetVector3Value(dataName, true);
                    if (vec3Values[0] != float.MinValue)
                    {
                        // Format: "x.y.z" use . instead of , is important 
                        return $"{vec3Values[0]}.{vec3Values[1]}.{vec3Values[2]}";
                    }
                    break;

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
                    if (valFloat != float.MinValue)
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
        /// Update sliders from UI after loading data from file 
        /// </summary>
        /// <param name="sdr"></param>
        /// <param name="newValue"></param>
        public void UpdateDynamicDataUI(SofaDataReference sdr, string newValue)
        {
            if (sdr == null || string.IsNullOrEmpty(newValue))
                return;

            // special case vec3
            if (sdr.dataType == SofaDataType.Vec3)
            {
                string[] values = newValue.Split('.');
                if (values.Length != 3)
                {
                    Debug.LogWarning("Failed to parse Vec3 value for UI update: " + newValue);
                    return;
                }

                float[] tempValues = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    if (!float.TryParse(values[i], out tempValues[i]))
                    {
                        Debug.LogWarning($"Failed to parse Vec3 component {i}: {values[i]}");
                        return;
                    }
                }
                //easier to do with float first to parse
                Vector3 vec3FloatValues = new Vector3(tempValues[0], tempValues[1], tempValues[2]);

                
                DynamicSdata[] allDynamicData = FindObjectsByType<DynamicSdata>(FindObjectsSortMode.None);
                List<DynamicSdata> vec3Components = new List<DynamicSdata>();

                foreach (DynamicSdata element in allDynamicData)
                {
                    // Check if dynamic data is linked to our vec3
                    bool matchesCustomName = !string.IsNullOrEmpty(sdr.optionalCustomName) &&
                                            !string.IsNullOrEmpty(element.GetUIName()) &&
                                            element.GetUIName() == sdr.optionalCustomName;

                    bool matchesDataName = element.GetDataName() == sdr.dataName;

                    if ((matchesCustomName || matchesDataName) && element.GetDataType() == SofaDataType.Vec3)
                    {
                        vec3Components.Add(element);
                    }
                }

                
                if (vec3Components.Count == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        
                        float normalizedValue = Mathf.InverseLerp(vec3Components[i].MIN, vec3Components[i].MAX, vec3FloatValues[i]);
                        vec3Components[i].GetSlider().value = normalizedValue;
                    }
                }
                else
                {
                    Debug.LogWarning($"Found {vec3Components.Count} Vec3 components instead of 3 for {sdr.dataName}");
                }
                return;
            }

            // Normal cases 
            if (!float.TryParse(newValue, out float thisValue))
            {
                Debug.LogWarning("Failed to parse Float value: " + newValue);
                return;
            }

            DynamicSdata[] allDynamicDataSimple = FindObjectsByType<DynamicSdata>(FindObjectsSortMode.None);
            foreach (DynamicSdata element in allDynamicDataSimple)
            {
                if (!string.IsNullOrEmpty(element.GetUIName()))
                {
                    if (element.GetUIName() == sdr.optionalCustomName)
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