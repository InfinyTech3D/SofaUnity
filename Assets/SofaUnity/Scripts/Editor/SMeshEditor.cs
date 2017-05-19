using UnityEngine;
using UnityEditor;
using SofaUnity;


[CustomEditor(typeof(SMesh), true)]
public class SMeshEditor : Editor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SMesh>();
        go.name = "SMesh";
        return go;
    }

    public override void OnInspectorGUI()
    {
        SMesh mesh = (SMesh)this.target;

        mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.translation);
        EditorGUILayout.Separator();

        mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.rotation);
        EditorGUILayout.Separator();

        mesh.drawFF = EditorGUILayout.Toggle("Show ForceField", mesh.drawFF);

        //SSphere mesh = (SSphere)this.target;

        //mesh.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}
