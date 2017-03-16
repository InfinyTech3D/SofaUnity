using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBox), true)]
public class SBoxEditor : SGridEditor
{    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SBaseGrid grid = (SBaseGrid)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();

        //grid.mass = EditorGUILayout.FloatField("Mass", grid.mass);
        //EditorGUILayout.Separator();

        //grid.young = EditorGUILayout.FloatField("Young Modulus", grid.young);
        //EditorGUILayout.Separator();

        //grid.poisson = EditorGUILayout.FloatField("Poisson Ratio", grid.poisson);
        //EditorGUILayout.Separator();

        //grid.translation = EditorGUILayout.Vector3Field("Translation", grid.translation);
        //EditorGUILayout.Separator();

    }

}

