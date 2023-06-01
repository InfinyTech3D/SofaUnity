using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;


public class CreatePlayerInEditor : Editor
{
    private static GameObject m_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Sofa Complete XR Origin Set Up Physic.prefab");

    [MenuItem("SofaUnity/VR/VRPlayer")]
    [MenuItem("GameObject/Create Other/VR/VRPlayer")]
   public static void CreateNew()
   {
        Instantiate(m_playerPrefab);

        //Left hand
        GameObject selectObj = GameObject.FindGameObjectWithTag("LeftHandModel");
        if (selectObj.GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("Error2 creating SofaSphereCollisionHand GameObject. Object should have a valid MeshFilter.");
            return;
        }

        SofaDAGNode parentDagN = selectObj.GetComponentInParent<SofaDAGNode>();
        if (parentDagN == null)
        {
            // not under DAGNode, will look for SofaContext
            GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
            if (_contextObject != null)
            {
                // Get Sofa context
                parentDagN = _contextObject.GetComponent<SofaDAGNode>();
            }

            // still null, no SofaContext found
            if (parentDagN == null)
            {
                Debug.LogError("Error3 creating SofaSphereCollisionHand GameObject. No valid SofaDAGNode found as parent of this gameObject or in SofaContext.");
                return ;
            }
            else
            {
                Debug.Log("parentDagN found: " + parentDagN.name);
            }
        }


        selectObj.AddComponent<SofaSphereCollisionHand>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionHand. Can't access SofaDAGNodeManager.");

        selectObj.GetComponent<DesactivateCollision>().SetSofaSphereCollisionHand(selectObj.GetComponent<SofaSphereCollisionHand>());
        selectObj.GetComponent<DesactivateCollision>().enabled = true;


        // Right Hand
        selectObj = GameObject.FindGameObjectWithTag("RightHandModel");

        if (selectObj.GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("Error2 creating SofaSphereCollisionHand GameObject. Object should have a valid MeshFilter.");
            return;
        }

        parentDagN = selectObj.GetComponentInParent<SofaDAGNode>();
        if (parentDagN == null)
        {
            // not under DAGNode, will look for SofaContext
            GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
            if (_contextObject != null)
            {
                // Get Sofa context
                parentDagN = _contextObject.GetComponent<SofaDAGNode>();
            }

            // still null, no SofaContext found
            if (parentDagN == null)
            {
                Debug.LogError("Error3 creating SofaSphereCollisionHand GameObject. No valid SofaDAGNode found as parent of this gameObject or in SofaContext.");
                return;
            }
            else
            {
                Debug.Log("parentDagN found: " + parentDagN.name);
            }
        }


        selectObj.AddComponent<SofaSphereCollisionHand>();


        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionHand. Can't access SofaDAGNodeManager.");

        selectObj.GetComponent<DesactivateCollision>().SetSofaSphereCollisionHand(selectObj.GetComponent<SofaSphereCollisionHand>());
        selectObj.GetComponent<DesactivateCollision>().enabled = true;

        return ;
    }
}
