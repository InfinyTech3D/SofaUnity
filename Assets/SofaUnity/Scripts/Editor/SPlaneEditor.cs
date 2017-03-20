using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SRigidPlane), true)]
public class SRigidPlaneEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidPlane")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidPlane")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidPlane>();
        go.name = "SRigidPlane";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SRigidPlane grid = (SRigidPlane)this.target;

        //grid.gridSize = EditorGUILayout.Vector2Field("Grid Plane Resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}