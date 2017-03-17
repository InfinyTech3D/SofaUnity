using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SSphere), true)]
public class SSphereEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SSphere")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SSphere>();
        go.name = "SSphere";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SSphere grid = (SSphere)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}