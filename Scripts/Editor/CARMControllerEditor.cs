using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CArmController), true)]
public class CArmControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CArmController model = (CArmController)this.target;
        model.visu_target = (GameObject)EditorGUILayout.ObjectField("Flouro", model.visu_target, typeof(Object), true);
        model.m_target = (GameObject)EditorGUILayout.ObjectField("target",model.m_target, typeof(Object), true);
        model.sourceDistance = EditorGUILayout.Slider("Source distance", model.sourceDistance, -1f, -10f);
        model.refreshRate = EditorGUILayout.Slider("Refresh rate (sec)", model.refreshRate, 0.0001f, 5.0f);
    }
}
