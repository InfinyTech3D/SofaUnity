using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaMouseInteractor), true)]
public class SofaMouseInteractorEditor : SofaRayCasterEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaMouseInteractor model = this.target as SofaMouseInteractor;
        if (model == null)
            return;

        EditorGUILayout.Separator();
        //model.mat = (Material)EditorGUILayout.ObjectField("Laser Material", model.mat, typeof(Material));
        model.m_sofaMesh = (SofaMesh)EditorGUILayout.ObjectField("Laser Material", model.m_sofaMesh, typeof(SofaMesh));
    }
}