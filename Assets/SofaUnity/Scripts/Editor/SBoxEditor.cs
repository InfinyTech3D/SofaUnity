using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBox), true)]
public class SBoxEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SBox")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SBox>();
        go.name = "SBox";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SBox grid = (SBox)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}


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


[CustomEditor(typeof(SCylinder), true)]
public class SCylinderEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SCylinder")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SCylinder>();
        go.name = "SCylinder";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SCylinder grid = (SCylinder)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}


[CustomEditor(typeof(SPlane), true)]
public class SPlaneEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SPlane")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SPlane")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SPlane>();
        go.name = "SPlane";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SPlane grid = (SPlane)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}