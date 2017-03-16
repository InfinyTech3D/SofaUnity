using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBaseGrid), true)]
public class SGridEditor : Editor
{    
    public override void OnInspectorGUI()
    {
        SBaseGrid grid = (SBaseGrid)this.target;

        grid.translation = EditorGUILayout.Vector3Field("Translation", grid.translation);
        EditorGUILayout.Separator();
        
        grid.mass = EditorGUILayout.FloatField("Mass", grid.mass);
        EditorGUILayout.Separator();

        grid.young = EditorGUILayout.FloatField("Young Modulus", grid.young);
        EditorGUILayout.Separator();

        grid.poisson = EditorGUILayout.FloatField("Poisson Ratio", grid.poisson);
        EditorGUILayout.Separator();
    }

    
}
