using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Create full VR player from prefab and add sphere to collide with Sofa 
/// </summary>
public class CreatePlayerInEditor : Editor
{
    /// <summary>
    /// Instance of player prefab
    /// </summary>
    private static GameObject m_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SofaUnity/Prefabs/Sofa Complete XR Origin Set Up Physic.prefab");

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

        // Add sphere due to capsule position to collide with sofa
        selectObj.AddComponent<SofaSphereCollisionHand>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionHand. Can't access SofaDAGNodeManager.");

        // Get reference to SofaSphereCollisionHand component to disable / enable colision on grab.
        selectObj.GetComponent<DesactivateCollision>().SetSofaSphereCollisionHand(selectObj.GetComponent<SofaSphereCollisionHand>());
        selectObj.GetComponent<DesactivateCollision>().enabled = true;


        // Right Hand (exacly like the left hand but for the right)
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

        // Add sphere due to capsule position to collide with sofa
        selectObj.AddComponent<SofaSphereCollisionHand>();


        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionHand. Can't access SofaDAGNodeManager.");

        // Get reference to SofaSphereCollisionHand component to disable / enable colision on grab.
        selectObj.GetComponent<DesactivateCollision>().SetSofaSphereCollisionHand(selectObj.GetComponent<SofaSphereCollisionHand>());
        selectObj.GetComponent<DesactivateCollision>().enabled = true;

        return ;
   }
}
