using UnityEngine;
using UnityEngine.UI;
using SofaUnity;
using SofaUnityAPI;
using TMPro;
using System;
using System.Collections.Generic;


namespace SofaUnityXR
{
    public class DynamicSdata : MonoBehaviour
    {


        [Header("UI")]
        [SerializeField] private Slider mainPropretySlider;
        [SerializeField] private TextMeshProUGUI PropretyName;
        [SerializeField] private TextMeshProUGUI PropretyValue;


        private SofaBaseComponent sBaseComp;
        private string dataName;
        private string UIName;
        private SofaDataType dataType;

        //Data type 0 Vector d size 1:
        private float[] valFloatList;

        //Data type 1 int :
        private int valInt;

        //Data type 2 float :
        private float valFloat;

        //Data type 3 Double :
        private float valDouble;

        //Data type Bool :
        private bool valBool;

        //Data type Vec3 :
        private Vector3 valVector3;


        // Bornes de conversion
        public float MIN = 0;
        public float MAX = 0;


        public void DynamicSdataSetup(SofaBaseComponent SofaBaseComp)
        {
            sBaseComp = SofaBaseComp;
            

            if (sBaseComp == null 
                || sBaseComp.m_impl == null 
                || mainPropretySlider == null
                || dataName == null)
            {
                Debug.LogError("Missing reference DynamicSData : " + dataName);
                return;
            }


            PropretyName.text = UIName;

      

            switch (dataType)
            {
                case SofaDataType.Vectord: //vectord size 1
                    
                    valFloatList = new float[1];

                    // Init
                    sBaseComp.m_impl.GetVectordValue(dataName, 1, valFloatList);
                    if (MAX == 0) AutoMIN_MAX((float)valFloatList[0]);

                    mainPropretySlider.value = Mathf.InverseLerp(MIN, MAX, valFloatList[0]);
                    mainPropretySlider.onValueChanged.AddListener(SliderVectordSizeOne);
                    break;

                case SofaDataType.Int: // int
                    
                    valInt=sBaseComp.m_impl.GetIntValue(dataName);

                    // Init
                    if(MAX==0)AutoMIN_MAX((float)valInt);
                    mainPropretySlider.value = Mathf.InverseLerp(MIN, MAX, (float)valInt);
                    mainPropretySlider.onValueChanged.AddListener(SliderInt);

                    break;

                case SofaDataType.Float: // float

                    valFloat = sBaseComp.m_impl.GetIntValue(dataName);

                    // Init
                    if(MAX==0)if(MAX==0)AutoMIN_MAX(valFloat);
                    mainPropretySlider.value = Mathf.InverseLerp(MIN, MAX, valFloat);
                    mainPropretySlider.onValueChanged.AddListener(SliderFloat);
                    break;

                case SofaDataType.Double:

                   
                   
                    valDouble = sBaseComp.m_impl.GetDoubleValue(dataName);
                    
                    if (MAX == 0)
                        AutoMIN_MAX(valDouble);

                    mainPropretySlider.value = (float)((valDouble - MIN) / (MAX - MIN));
                    mainPropretySlider.onValueChanged.AddListener(SliderDouble);
                    break;



                case SofaDataType.Bool: // Bool

                    mainPropretySlider.minValue = 0;
                    mainPropretySlider.maxValue = 1;
                    mainPropretySlider.wholeNumbers = true;

                    valBool = sBaseComp.m_impl.GetBoolValue(dataName);


                    mainPropretySlider.value = valBool ? 1 : 0;

                    mainPropretySlider.onValueChanged.AddListener(SliderBool);
                    break;

                case SofaDataType.Vec3: //Vec3 special case
                    if (mainPropretySlider == null )
                    {
                        Debug.LogError("One slider reference is not set for Vec3 control");
                        break;
                    }
                    valVector3 = sBaseComp.m_impl.GetVector3Value(dataName,true);
                    XorYorZ(); //setup depending if it's X, Y or Z of the vector3
                    break;

                default:
                    break;
            }
        }

        void OnDestroy()
        {
            // Clean
            if (mainPropretySlider != null)
                mainPropretySlider.onValueChanged.RemoveListener(SliderVectordSizeOne);
                mainPropretySlider.onValueChanged.RemoveListener(SliderInt);
                mainPropretySlider.onValueChanged.RemoveListener(SliderFloat);
                mainPropretySlider.onValueChanged.RemoveListener(SliderDouble);
                mainPropretySlider.onValueChanged.RemoveListener(SliderBool);
                mainPropretySlider.onValueChanged.RemoveListener(XSliderVect3);
                mainPropretySlider.onValueChanged.RemoveListener(YSliderVect3);
                mainPropretySlider.onValueChanged.RemoveListener(ZSliderVect3);
        }


        void SliderVectordSizeOne(float sliderValue)
        {
            
            valFloatList[0] = Mathf.Lerp(MIN, MAX, sliderValue);
            int res = sBaseComp.m_impl.SetVectordValue(dataName, 1, valFloatList);
            if (res != 0)
                Debug.LogError("Failed to set VectordSizeOne");

            
        }


        void SliderInt(float sliderValue)
        {
            
            int myInt =(int) Mathf.Lerp(MIN, MAX, sliderValue);
            sBaseComp.m_impl.SetIntValue(dataName,myInt);
        }

        void SliderFloat(float sliderValue)
        {

            float myFloat = Mathf.Lerp(MIN, MAX, sliderValue);
            sBaseComp.m_impl.SetFloatValue(dataName, myFloat);
        }

        void SliderDouble(float sliderValue)
        {
            float myDouble = MIN + (MAX - MIN) * sliderValue;
            sBaseComp.m_impl.SetDoubleValue(dataName, myDouble);
        }

        void SliderBool(float sliderValue)
        {
            bool newValue = (sliderValue == 1);
            sBaseComp.m_impl.SetBoolValue(dataName, newValue);
        }

        void XSliderVect3(float sliderValue)
        {
            Vector3 xVec = sBaseComp.m_impl.GetVector3Value(dataName,true);
            xVec.x = MIN + (MAX - MIN) * sliderValue; 
            sBaseComp.m_impl.SetVector3Value(dataName,xVec,true );
        }

        void YSliderVect3(float sliderValue)
        {
            Vector3 yVec = sBaseComp.m_impl.GetVector3Value(dataName, true);
            yVec.y = MIN + (MAX - MIN) * sliderValue;
            sBaseComp.m_impl.SetVector3Value(dataName, yVec, true);
        }

        void ZSliderVect3(float sliderValue)
        {
            Vector3 zVec = sBaseComp.m_impl.GetVector3Value(dataName, true);
            zVec.z = MIN + (MAX - MIN) * sliderValue;
            sBaseComp.m_impl.SetVector3Value(dataName, zVec, true);
        }

        /// <summary>
        /// Setup the slider for Vec3 depending if we are using X Y or Z
        /// </summary>
        void XorYorZ()
        {
            string sliderName = mainPropretySlider.gameObject.name;

            if (sliderName == "XSlider")
            {
                if(MAX==0)AutoMIN_MAX(valVector3.x);
                mainPropretySlider.value = (float)((valDouble - MIN) / (MAX - MIN));
                mainPropretySlider.onValueChanged.AddListener(XSliderVect3);
            }
            else if (sliderName == "YSlider")
            {
                if(MAX==0)AutoMIN_MAX(valVector3.y);
                mainPropretySlider.value = (float)((valDouble - MIN) / (MAX - MIN));
                mainPropretySlider.onValueChanged.AddListener(YSliderVect3);
            }
            else if (sliderName == "ZSlider")
            {
                if(MAX==0)AutoMIN_MAX(valVector3.z);
                mainPropretySlider.value = (float)((valDouble - MIN) / (MAX - MIN));
                mainPropretySlider.onValueChanged.AddListener(ZSliderVect3);
            }
            else Debug.LogError("Unknown slider name: " + sliderName);
            
        }





        /// <summary>
        /// Set bondary values for the slider 
        /// Important to prevent simulation crash
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        public void SetMIN_MAX(float Min, float Max)
        {
            MIN = Min;
            MAX = Max;
        }

        /// <summary>
        /// Set bondary values for the slider 
        /// Important to prevent simulation crash
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        public void AutoMIN_MAX(float DataValue)
        {
            float delta = DataValue * 0.2f;
            MIN = DataValue - delta;
            MAX = DataValue + delta;
        }

        //API

        public void SetDataName( string DataName)
        {
            dataName = DataName;
        }

        public string GetDataName()
        {
            return dataName;
        }

        public string GetUIName()
        {
            return UIName;
        }

        public Slider GetSlider()
        {
            return (mainPropretySlider);
        }

        public void SetUIName(string thisUIName)
        {
            UIName = thisUIName;
        }

        public void SetDataType(SofaDataType DataType)
        {
            dataType = DataType;
        }

    } // end class
} //end namespace


