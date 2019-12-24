using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class ComponentDataTest : MonoBehaviour//, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    [SerializeField]
    public SofaDataFloat data1;

    [SerializeField]
    public SofaDataVec3Float data2;

    public string nameId = "mytoto";

    void Awake()
    {
        Debug.Log("ComponentDataTest::Awake");        
    }

    public void initData()
    {
        if (!Application.isPlaying)
        {
            data1 = new SofaDataFloat(this, "toto", 666.0f);
            data1.Log();

            data2 = new SofaDataVec3Float(this, "tata", 10, 50, 80);
            data2.Log();
        }
    }

    void Start()
    {
        Debug.Log("ComponentDataTest::Start");
        data1.Log();
        data2.Log();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void OnBeforeSerialize()
    //{
    //    Debug.Log("ComponentDataTest OnBeforeSerialize");


    //}

    //public void OnAfterDeserialize()
    //{
    //    Debug.Log("ComponentDataTest OnAfterDeserialize");

    //}
}
