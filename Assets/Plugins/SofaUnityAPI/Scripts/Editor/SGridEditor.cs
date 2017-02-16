using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBox), true)]
public class SGridEditor : Editor
{
    [MenuItem("SofaUnity/SBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/SBox")]
    public static GameObject CreateNew()
    {
        Debug.Log("CreateNew CreateSCube");
        GameObject go = new GameObject();
        go.AddComponent<SBox>();
        go.name = "SBox";
        return go;
    }

    public override void OnInspectorGUI()
    {
        SBox grid = (SBox)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();

        grid.mass = EditorGUILayout.FloatField("Mass", grid.mass);
        EditorGUILayout.Separator();

        grid.young = EditorGUILayout.FloatField("Young Modulus", grid.young);
        EditorGUILayout.Separator();

        grid.poisson = EditorGUILayout.FloatField("Poisson Ratio", grid.poisson);
        EditorGUILayout.Separator();
    }

    
}
