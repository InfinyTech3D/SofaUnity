using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaCollisionModel), true)]
public class SofaCollisionModelEditor : SofaBaseComponentEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaCollisionModel compo = this.target as SofaCollisionModel;
        if (compo == null)
            return;

        compo.DrawCollision = EditorGUILayout.Toggle("Draw collision elements", compo.DrawCollision);
        
    }
}
