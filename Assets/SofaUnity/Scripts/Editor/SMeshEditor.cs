using UnityEngine;
using UnityEditor;
using SofaUnity;


[CustomEditor(typeof(SDeformableMesh), true)]
public class SDeformableMeshEditor : Editor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SDeformableMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SDeformableMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SDeformableMesh>();
        go.name = "SDeformableMesh";
        return go;
    }

    public override void OnInspectorGUI()
    {
        SDeformableMesh mesh = (SDeformableMesh)this.target;

        mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.translation);
        EditorGUILayout.Separator();

        mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.rotation);
        EditorGUILayout.Separator();

        mesh.scale = EditorGUILayout.Vector3Field("Scale", mesh.scale);
        EditorGUILayout.Separator();

        mesh.mass = EditorGUILayout.FloatField("Mass", mesh.mass);
        EditorGUILayout.Separator();

        mesh.young = EditorGUILayout.FloatField("Young Modulus", mesh.young);
        EditorGUILayout.Separator();

        mesh.poisson = EditorGUILayout.FloatField("Poisson Ratio", mesh.poisson);
        EditorGUILayout.Separator();

        mesh.drawFF = EditorGUILayout.Toggle("Show ForceField", mesh.drawFF);
        EditorGUILayout.Separator();
    }
}


[CustomEditor(typeof(SRigidMesh), true)]
public class SRigidMeshEditor : Editor
{
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject();
        go.AddComponent<SRigidMesh>();
        go.name = "SRigidMesh";
        return go;
    }

    public override void OnInspectorGUI()
    {
        SDeformableMesh mesh = (SDeformableMesh)this.target;

        mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.translation);
        EditorGUILayout.Separator();

        mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.rotation);
        EditorGUILayout.Separator();

        mesh.scale = EditorGUILayout.Vector3Field("Scale", mesh.scale);
        EditorGUILayout.Separator();
    }
}
