using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class libTest : MonoBehaviour {

    public int size = 512;
    private IntPtr m_simu;

    [DllImport("libraryTest1")]
    private static extern float FooPluginFunction();
    [DllImport("libraryTest1")]
    public static extern double Add(double a, double b);



    [DllImport("SofaAdvancePhysicsAPI")]
    private static extern float FooPluginFunction1();

    [DllImport("SofaAdvancePhysicsAPI")]
    private static extern float FooPluginFunction2();

    [DllImport("SofaAdvancePhysicsAPI")]   
    public static extern int getAPI();


    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern IntPtr sofaPhysicsAPI_create();
    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern void sofaPhysicsAPI_createScene(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern void sofaPhysicsAPI_start(IntPtr obj);
    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern void sofaPhysicsAPI_step(IntPtr obj);
    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern void sofaPhysicsAPI_stop(IntPtr obj);

    [DllImport("SofaAdvancePhysicsAPI")]
    public static extern int sofaPhysicsAPI_getNumberObjects(IntPtr obj);


    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi)]
    public static extern void sofaPhysicsAPI_addCube(IntPtr obj, [MarshalAs(UnmanagedType.LPStr)] string lpString);

    void Start()
    {
        Debug.Log("TOTO");
        Debug.Log("test1: " + libTest.FooPluginFunction());
        Debug.Log("test2: " + Add(2.0, 5.5));

        Debug.Log("test5: " + FooPluginFunction1());
        Debug.Log("test7: " + getAPI());

        m_simu = sofaPhysicsAPI_create();
        sofaPhysicsAPI_createScene(m_simu);

        Debug.Log("Ttest1: " + sofaPhysicsAPI_getNumberObjects(m_simu));
        sofaPhysicsAPI_start(m_simu);
        //sofaPhysicsAPI_addCube(m_simu, "toto");
        sofaPhysicsAPI_step(m_simu);
        sofaPhysicsAPI_stop(m_simu);
        //classTest_addOffest(classA, 10);
        //Debug.Log("test4: " + classTest_getCpt(classA));
    }
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
