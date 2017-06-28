using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SSphere), true)]
public class SSphereEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SSphere");
        go.AddComponent<SSphere>();
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}


[CustomEditor(typeof(SRigidSphere), true)]
public class SRigidSphereEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidSphere");
        go.AddComponent<SRigidSphere>();
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}