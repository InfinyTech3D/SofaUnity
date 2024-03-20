using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Editor class corresponding to @sa SofaCollisionPipeline 
/// Provide create method to create SofaCollisionPipeline from Unity Menu
/// Provide interface to see the different Sofa objects used to solve the SOFA collision pipeline
/// </summary>
[CustomEditor(typeof(SofaCollisionPipeline), true)]
public class SofaCollisionPipelineEditor : Editor
{
    [MenuItem("SofaUnity/SofaCollisionPipeline")]
    [MenuItem("GameObject/Create Other/SofaCollisionPipeline")]
    public static void CreateNew()
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogError("Error1 creating SofaCollisionPipeline object. No SofaContext GameObject selected.");
            return;
        }

        GameObject selectObj = Selection.activeGameObject;
        SofaContext sofaContext = selectObj.GetComponent<SofaContext>();

        if (sofaContext == null)
        {
            Debug.LogError("Error2 creating SofaCollisionPipeline object. No GameObject with a valid sofaContext selected.");
            return;
        }

        GameObject go = new GameObject("SofaCollisionPipeline");
        SofaCollisionPipeline pipe = go.AddComponent<SofaCollisionPipeline>();
        go.transform.parent = sofaContext.gameObject.transform;
        pipe.CreateObject(sofaContext, "SofaCollisionPipeline", "root");
        sofaContext.RegisterSofaObject(pipe);
    }

    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaCollisionPipeline collCompo = (SofaCollisionPipeline)this.target;

        if (collCompo.CollisionPipeline != null)
            EditorGUILayout.TextField("Collision Pipeline", collCompo.CollisionPipeline.m_componentType);

        if (collCompo.BroadPhase != null)
            EditorGUILayout.TextField("Broad Phase", collCompo.BroadPhase.m_componentType);

        if (collCompo.NarrowPhase != null)
            EditorGUILayout.TextField("Narrow Phase", collCompo.NarrowPhase.m_componentType);

        if (collCompo.Collisionresponse != null)
            EditorGUILayout.TextField("Collision Response", collCompo.Collisionresponse.m_componentType);
    }
}
