using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(XRayController), true)]
public class XRayControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        XRayController model = (XRayController)this.target;
        model.m_XRayRendererObject = (GameObject)EditorGUILayout.ObjectField("XRayrenderer", model.m_XRayRendererObject, typeof(Object), true);
        model.SourcePosition = EditorGUILayout.Vector3Field("Source position", model.SourcePosition);
        model.TargetPosition = EditorGUILayout.Vector3Field("Target position", model.TargetPosition);

        model.TargetDistance = EditorGUILayout.Slider("Target distance", model.TargetDistance, 0.0f, 1000.0f);
        model.BeamPower = EditorGUILayout.Slider("Beam power", model.BeamPower, 0.0f, 100.0f);
    }
}
