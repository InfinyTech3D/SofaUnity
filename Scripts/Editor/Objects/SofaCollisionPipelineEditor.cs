using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

[CustomEditor(typeof(SofaCollisionPipeline), true)]
public class SofaCollisionPipelineEditor : Editor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaCollisionPipeline collCompo = (SofaCollisionPipeline)this.target;

        if (collCompo.BroadPhase != null)
            EditorGUILayout.TextField("CollisionPipeline", collCompo.BroadPhase.m_componentType);

        if (collCompo.NarrowPhase != null)
            EditorGUILayout.TextField("BroadPhase", collCompo.NarrowPhase.m_componentType);

        if (collCompo.CollisionPipeline != null)
            EditorGUILayout.TextField("NarrowPhase", collCompo.CollisionPipeline.m_componentType);

        if (collCompo.Collisionresponse != null)
            EditorGUILayout.TextField("Collisionresponse", collCompo.Collisionresponse.m_componentType);
    }
}
