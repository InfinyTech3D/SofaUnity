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

        //SBox grid = (SBox)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}

[CustomEditor(typeof(SRigidBox), true)]
public class SRigidBoxEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidBox")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidBox>();
        go.name = "SRigidBox";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //SRigidBox grid = (SRigidBox)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}

