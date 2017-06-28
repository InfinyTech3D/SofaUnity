using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SBox), true)]
public class SBoxEditor : SGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SBox");
        go.AddComponent<SBox>();        
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(SRigidBox), true)]
public class SRigidBoxEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidBox");
        go.AddComponent<SRigidBox>();        
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

