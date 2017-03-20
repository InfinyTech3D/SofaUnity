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

        //SSphere grid = (SSphere)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}


[CustomEditor(typeof(SRigidSphere), true)]
public class SRigidSphereEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidSphere")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidSphere>();
        go.name = "SRigidSphere";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //SRigidSphere grid = (SRigidSphere)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}