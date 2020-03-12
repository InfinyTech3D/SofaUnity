using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaRayCaster), true)]
public class SofaRayCasterEditor : Editor
{

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


        EditorGUILayout.Separator();

        /// SofaRayCaster inspector
        System.Enum toto = EditorGUILayout.EnumPopup("Tool interaction", model.m_laserType);
        
       // model.m_laserType = (SofaDefines.SRayInteraction);
        model.startOnPlay = EditorGUILayout.Toggle("StartOnPlay mode", model.startOnPlay);
        model.ActivateTool = EditorGUILayout.Toggle("Activate Tool", model.ActivateTool);


    }
}
