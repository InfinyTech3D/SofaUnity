using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;
using SofaUnityAPI;
using TMPro;

namespace SofaUnityXR
{
    /// <summary>
    /// Data types
    /// </summary>
    public enum SofaDataType
    {
        Int,
        Float,
        Double,
        Bool,
        Vectord,
        Vec3
    }

    [System.Serializable]
    public class SofaDataReference
    {
        [Header("Give EditorLink OR UniqueId :")]
        public SofaBaseComponent sofaComponent;
        public string UniqueId;
        [Header("Data Name and type :")]
        public string dataName;
        public SofaDataType dataType;
        public string optionalCustomName;
        [Header("Is +/- 20% if not set :")]
        public float MIN;
        public float MAX;

        public SofaDataReference(SofaBaseComponent sofaComponent, string dataName, SofaDataType dataType,string uid)
        {
            this.sofaComponent = sofaComponent;
            this.dataName = dataName;
            this.dataType = dataType;
            this.UniqueId = uid;
        }
    }


    public class DynamicSDataManager : MonoBehaviour
    {
        [SerializeField] public List<SofaDataReference> DSDataList = new List<SofaDataReference>();
        public GameObject UIContainer;
        public GameObject DSDataprefab;
        public GameObject Vec3_DSDataprefab;
        //public SofaBaseComponent BaseCompTest;




        void Start()
        {
            //To get the type and exact name use this line :
            //Debug.Log(BaseCompTest.m_impl.LoadAllData()); 

            for (int i = 0; i < DSDataList.Count; i++)
            {
                CreateUIElement(DSDataList[i]);
            }

            //manual test :
            //SofaDataReference dynamicData = new SofaDataReference(BaseCompTest, "youngModulus", 0);
            //CreateUIElement(dynamicData);
        }

        /// <summary>
        /// Creat the ui element with a slider link to the given proprety name and type
        /// </summary>
        /// </param>
        public void CreateUIElement (SofaDataReference data)
        {
            if (data.sofaComponent == null && (string.IsNullOrEmpty(data.UniqueId)))
            {
                //No Link No Id
                Debug.LogError("DynamicSDataManager: missing name or id");
                
            }
            else if (!string.IsNullOrEmpty(data.UniqueId) && data.sofaComponent != null)
            {
                //Link and Id
                FindSofaComponentInObjectById(data);
                
            }else if (!string.IsNullOrEmpty(data.UniqueId) && data.sofaComponent == null)
            {
                //No Link Just Id
                FindSofaComponentInScene(data);

            }

            if (data.sofaComponent == null) {
                return;
            }
            
            SofaBaseComponent SBcomp = data.sofaComponent; 
            string dataName = data.dataName;
            SofaDataType dataType = data.dataType;

            if (UIContainer == null || DSDataprefab == null)
            {
                Debug.LogError("DynamicSDataManager: missing reference");
                return;
            }

            GameObject instance;
            if (dataType == SofaDataType.Vec3)
            {
                instance = Instantiate(Vec3_DSDataprefab);
                instance.transform.SetParent(UIContainer.transform, false);
                DynamicSdata[] components = instance.GetComponents<DynamicSdata>();

                //To help you undersant what happend :
                //DynamicSdata X_DS= components[0];
                //DynamicSdata Y_DS = components[1];
                //DynamicSdata Z_DS = components[2];

                for(int i  = 0; i < components.Length; i++)
                {
                    components[i].MIN= data.MIN;
                    components[i].MAX= data.MAX;
                    components[i].SetDataName(dataName);
                    components[i].SetDataType(dataType);
                    components[i].DynamicSdataSetup(SBcomp);
                }


            }
            else
            {
                instance = Instantiate(DSDataprefab);
                instance.transform.SetParent(UIContainer.transform, false);
                DynamicSdata dynamicSdata = instance.GetComponent<DynamicSdata>();
                if (dynamicSdata == null)
                    return;


                dynamicSdata.SetDataName(dataName);
                dynamicSdata.SetDataType(dataType);
                dynamicSdata.MIN = data.MIN;
                dynamicSdata.MAX = data.MAX;

                if (string.IsNullOrEmpty(data.optionalCustomName))
                {
                    dynamicSdata.SetUIName(dataName);//No custom name so default
                }
                else
                {
                    dynamicSdata.SetUIName(data.optionalCustomName);
                }

                    dynamicSdata.DynamicSdataSetup(SBcomp);
            }
        }

        /// <summary>
        /// Finding element the hard way by looking at every sofaComponent in the scene
        /// cost in time
        /// </summary>
        /// <param name="data"></param>
        void FindSofaComponentInScene(SofaDataReference data)
        {
            SofaBaseComponent[] allBaseComponents = FindObjectsOfType<SofaBaseComponent>();

            bool found = false;

            foreach (SofaBaseComponent thisComponent in allBaseComponents)
            {
                if (thisComponent.UniqueNameId == data.UniqueId)
                {
                    data.sofaComponent = thisComponent;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogError("CreateUIElement: can't find unique id " + data.UniqueId);
                
            }
        }

        /// <summary>
        /// Look for all Base component in a given object, 
        /// sould be use for sofaobject with multiple SofaComponents
        /// </summary>
        /// <param name="data"></param>
        void FindSofaComponentInObjectById(SofaDataReference data)
        {
            GameObject go = data.sofaComponent.gameObject;
            List<SofaBaseComponent> baseComponents = new List<SofaBaseComponent>();
            go.GetComponents(baseComponents);
            bool found = false;

            foreach (SofaBaseComponent thisComponent in baseComponents)
            {
                if (thisComponent.UniqueNameId == data.UniqueId)
                {
                    data.sofaComponent = thisComponent;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.LogError("CreateUIElement: can't find unique id " + data.UniqueId);

            }
        }

    }//end class
}// end namespace
