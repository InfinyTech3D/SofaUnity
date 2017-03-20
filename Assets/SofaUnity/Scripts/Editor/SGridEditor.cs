using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBaseGrid), true)]
public class SGridEditor : Editor
{    
    public override void OnInspectorGUI()
    {
        SBaseGrid grid = (SBaseGrid)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();

        grid.translation = EditorGUILayout.Vector3Field("Translation", grid.translation);
        EditorGUILayout.Separator();

        grid.rotation = EditorGUILayout.Vector3Field("Rotation", grid.rotation);
        EditorGUILayout.Separator();

        grid.scale = EditorGUILayout.Vector3Field("Scale", grid.scale);
        EditorGUILayout.Separator();

        grid.mass = EditorGUILayout.FloatField("Mass", grid.mass);
        EditorGUILayout.Separator();

        grid.young = EditorGUILayout.FloatField("Young Modulus", grid.young);
        EditorGUILayout.Separator();

        grid.poisson = EditorGUILayout.FloatField("Poisson Ratio", grid.poisson);
        EditorGUILayout.Separator();
    }
}

[CustomEditor(typeof(SBaseGrid), true)]
public class SRigidGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SBaseGrid grid = (SBaseGrid)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();

        grid.translation = EditorGUILayout.Vector3Field("Translation", grid.translation);
        EditorGUILayout.Separator();

        grid.rotation = EditorGUILayout.Vector3Field("Rotation", grid.rotation);
        EditorGUILayout.Separator();

        grid.scale = EditorGUILayout.Vector3Field("Scale", grid.scale);
        EditorGUILayout.Separator();        
    }
}


