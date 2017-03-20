using UnityEngine;
using UnityEditor;
using SofaUnity;

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

        //SCylinder grid = (SCylinder)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}


[CustomEditor(typeof(SRigidCylinder), true)]
public class SRigidCylinderEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidCylinder")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidCylinder>();
        go.name = "SRigidCylinder";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //SRigidCylinder grid = (SRigidCylinder)this.target;

        //grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        //EditorGUILayout.Separator();
    }
}
