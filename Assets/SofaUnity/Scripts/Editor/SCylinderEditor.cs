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
        GameObject go = new GameObject("SCylinder");
        go.AddComponent<SCylinder>();
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
        GameObject go = new GameObject("SRigidCylinder");
        go.AddComponent<SRigidCylinder>();
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
