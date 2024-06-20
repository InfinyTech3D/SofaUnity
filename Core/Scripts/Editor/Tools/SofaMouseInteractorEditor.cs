using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaMouseInteractor
/// This editor is a specialization of @sa SofaRayCasterEditor to only add the option to display selected primitive.
/// </summary>
[CustomEditor(typeof(SofaMouseInteractor), true)]
public class SofaMouseInteractorEditor : SofaRayCasterEditor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        // display SofaRayCasterEditor first
        base.OnInspectorGUI();

        SofaMouseInteractor model = this.target as SofaMouseInteractor;
        if (model == null)
            return;

        EditorGUILayout.Separator();
        model.DrawSelection = EditorGUILayout.Toggle("Draw Selection", model.DrawSelection);
    }
}