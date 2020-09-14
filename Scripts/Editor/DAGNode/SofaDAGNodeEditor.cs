using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaDAGNode 
/// </summary>
[CustomEditor(typeof(SofaDAGNode), true)]
public class SofaDAGNodeEditor : Editor
{

    [MenuItem("SofaUnity/SofaDAGNode")]
    [MenuItem("GameObject/Create Other/SofaDAGNode")]
    public static void CreateNew()
    {
        if (Selection.activeTransform != null)
        {
            GameObject selectObj = Selection.activeGameObject;
            SofaDAGNode parentDagN = selectObj.GetComponent<SofaDAGNode>();

            if (parentDagN == null)
            {
                // not a DAGNode selected. Check if SofaComponent
                SofaBaseComponent sofaComponent = selectObj.GetComponent<SofaBaseComponent>();

                // If neither a sofa component, nothing can be done
                if (sofaComponent == null)
                {
                    Debug.LogError("Error2 creating SofaDAGNode GameObject. No valid SofaDAGNode or SofaComponent selected.");
                    return;
                }

                // otherwise  will search for DAGNode owner of this component and add New DAGNode as child of this owner
                parentDagN = sofaComponent.m_ownerNode;
                if (parentDagN == null)
                {
                    Debug.LogError("Error3 creating SofaDAGNode GameObject. SofaComponent selected has no valid SofaDAGNode owner.");
                    return;
                }
            }

            SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
            nodeMgr.RegisterCustomNode("Node", parentDagN.UniqueNameId);
        }
        else
        {
            Debug.LogError("Error1 creating SofaDAGNode GameObject. No valid SofaContext nor SofaDAGNode selected.");
        }

        return;
    }


    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaDAGNode node = (SofaDAGNode)this.target;
        if (node == null)
            return;

        EditorGUI.BeginDisabledGroup(true);        
        EditorGUILayout.ObjectField("Sofa Context", node.m_sofaContext, typeof(Object), true);
        EditorGUILayout.TextField("Parent DAGNode", node.ParentNodeName);
        EditorGUILayout.TextField("DAGNode UniqID", node.UniqueNameId);
        EditorGUI.EndDisabledGroup();

        node.DisplayName = EditorGUILayout.TextField("DAGNode Name", node.DisplayName);

        if (node.HasTransform()) // no tranformation for root node
        {
            EditorGUILayout.Separator();
            // Add Triansformation fields
            node.Translation = EditorGUILayout.Vector3Field("SOFA Translation", node.Translation);
            EditorGUILayout.Separator();

            node.Rotation = EditorGUILayout.Vector3Field("SOFA Rotation", node.Rotation);
            EditorGUILayout.Separator();

            node.Scale = EditorGUILayout.Vector3Field("SOFA Scale", node.Scale);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }        

        EditorGUI.BeginDisabledGroup(true);
        if (node.m_sofaComponents != null)
            EditorGUILayout.IntField("Number of Components", node.m_sofaComponents.Count);
        EditorGUI.EndDisabledGroup();
    }
}
