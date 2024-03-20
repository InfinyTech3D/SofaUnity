using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

public class SofaSceneFixer : Editor
{
    [MenuItem("SofaUnity/Utils/FixSceneDAGNodeNames")]
    static void DoFixSofaScene()
    {
        Debug.Log("Performing: FixSceneDAGNodeNames");
        GameObject SofaObj = GameObject.Find("SofaContext");
        SofaContext sofaCon = SofaObj.GetComponent<SofaContext>();

        if (sofaCon)
        {
            sofaCon.NodeGraphMgr.FixGraphNames();
        }
        else
            Debug.LogWarning("No Sofacontext found");
    }

}
