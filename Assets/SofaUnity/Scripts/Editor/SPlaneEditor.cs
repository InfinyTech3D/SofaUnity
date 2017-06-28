using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SRigidPlane), true)]
public class SRigidPlaneEditor : SRigidGridEditor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidPlane")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidPlane")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidPlane");
        go.AddComponent<SRigidPlane>();
        return go;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SRigidPlane grid = (SRigidPlane)this.target;
    }
}