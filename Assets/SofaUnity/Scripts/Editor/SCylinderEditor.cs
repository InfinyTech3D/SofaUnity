using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SCylinder), true)]
public class SCylinderEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SCylinder>();
        go.name = "SCylinder";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}


[CustomEditor(typeof(SRigidCylinder), true)]
public class SRigidCylinderEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidCylinder>();
        go.name = "SRigidCylinder";
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
