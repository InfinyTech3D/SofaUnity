using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

[CustomEditor(typeof(CuttingManager), true)]
public class CuttingManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CuttingManager model = this.target as CuttingManager;
        if (model == null)
            return;

        model.m_sofaCuttingMgr = (SofaComponent)EditorGUILayout.ObjectField("SofaCuttingController", model.m_sofaCuttingMgr, typeof(SofaComponent), true);

        EditorGUILayout.ObjectField("Point A", model.m_pointA, typeof(GameObject), true);
        EditorGUILayout.ObjectField("Point B", model.m_pointB, typeof(GameObject), true);
        EditorGUILayout.ObjectField("Point C", model.m_pointC, typeof(GameObject), true);

        model.CutPointA = EditorGUILayout.Vector3Field("CutPointA", model.CutPointA);
        model.CutPointB = EditorGUILayout.Vector3Field("CutPointB", model.CutPointB);
        model.CutDirection = EditorGUILayout.Vector3Field("CutDirection", model.CutDirection);
        model.CutDepth = EditorGUILayout.FloatField("CutDepth", model.CutDepth);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Process Cut"))
        {
            model.performCut();
        }
        EditorGUILayout.Separator();
    }
}
