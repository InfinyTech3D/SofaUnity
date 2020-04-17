using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaRayCaster
/// Provide interface for RayCaster parameters like origin, direction and length and option to draw it.
/// Provide interface for SofaRayCaster like the type of interaction and option to activate and start on play.
/// </summary>
[CustomEditor(typeof(SofaRayCaster), true)]
public class SofaRayCasterEditor : Editor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaRayCaster model = this.target as SofaRayCaster;
        if (model == null)
            return;

        /// RayCaster inspector
        model.Origin = EditorGUILayout.Vector3Field("Ray Origin", model.Origin);
        model.Direction = EditorGUILayout.Vector3Field("Ray Direction", model.Direction);
        model.Length = EditorGUILayout.FloatField("Ray Length", model.Length);

        model.ActivateRay = EditorGUILayout.Toggle("Activate Ray", model.ActivateRay);
        model.m_drawRay = EditorGUILayout.Toggle("Draw Ray", model.m_drawRay);

        if (model.m_drawRay)
        {
            EditorGUILayout.Separator();
            model.RayWidth = EditorGUILayout.FloatField("Ray Width", model.RayWidth);
            model.RayColor = EditorGUILayout.ColorField("Ray Color", model.RayColor);
        }

        EditorGUILayout.Separator();

        /// SofaRayCaster inspector
        model.RayInteractionType = (SofaDefines.SRayInteraction)EditorGUILayout.EnumPopup("Tool interaction", model.RayInteractionType);

        if (model.RayInteractionType != SofaDefines.SRayInteraction.None)
        {
            model.startOnPlay = EditorGUILayout.Toggle("StartOnPlay mode", model.startOnPlay);
            model.ActivateTool = EditorGUILayout.Toggle("Activate Tool", model.ActivateTool);
        }

        if (model.RayInteractionType == SofaDefines.SRayInteraction.AttachTool)
        {
            EditorGUILayout.Separator();
            model.AttachStiffness = EditorGUILayout.FloatField("Tool Attach Stiffness", model.AttachStiffness);
        }     
    }
}
