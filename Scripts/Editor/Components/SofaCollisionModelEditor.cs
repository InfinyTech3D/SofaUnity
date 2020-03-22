using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaCollisionModel
/// This class inherite from @sa SofaBaseComponentEditor and will add specific data after the Data display
/// Provide option to show or not the collision model.
/// </summary>
[CustomEditor(typeof(SofaCollisionModel), true)]
public class SofaCollisionModelEditor : SofaBaseComponentEditor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaCollisionModel compo = this.target as SofaCollisionModel;
        if (compo == null)
            return;

        compo.DrawCollision = EditorGUILayout.Toggle("Draw collision elements", compo.DrawCollision);
        
    }
}
