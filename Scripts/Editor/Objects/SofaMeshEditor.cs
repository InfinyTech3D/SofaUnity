using UnityEngine;
using UnityEditor;
using SofaUnity;


/// <summary>
/// Editor Class to define the creation and UI of SofaDeformableMesh GameObject
/// </summary>
[CustomEditor(typeof(SofaDeformableMesh), true)]
public class SofaDeformableMeshEditor : Editor
{
    bool normalBtn = false;

    /// <summary>
    ///  Add SofaDeformableMesh Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaDeformableMesh GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaDeformableMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaDeformableObject/SofaDeformableMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaDeformableMesh");
        go.AddComponent<SofaDeformableMesh>();
        return go;
    }


    /// <summary>
    /// Method to set the UI of the SofaDeformableMesh GameObject
    /// </summary>
    
    public override void OnInspectorGUI()
    {     
        SofaDeformableMesh mesh = (SofaDeformableMesh)this.target;
        if (mesh.isAwake() == false)
            return;

        // Check box to change normals direction
        normalBtn = EditorGUILayout.Toggle("Inverse Normals", mesh.m_invertNormals);
        mesh.invertNormals = normalBtn;

        // Add Triansformation fields
        mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.m_translation);
        EditorGUILayout.Separator();

        mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.m_rotation);
        EditorGUILayout.Separator();

        mesh.scale = EditorGUILayout.Vector3Field("Scale", mesh.m_scale);
        EditorGUILayout.Separator();


        // Add FEM fields
        if (mesh.m_mass != float.MinValue)
        {
            mesh.mass = EditorGUILayout.Slider("Mass", mesh.m_mass, 0.0001f, 1000);
            EditorGUILayout.Separator();
        }

        if (mesh.m_young != float.MinValue)
        {
            mesh.young = EditorGUILayout.Slider("Young Modulus", mesh.m_young, 0.0001f, 10000);
            EditorGUILayout.Separator();
        }

        if (mesh.m_poisson != float.MinValue)
        {
            mesh.poisson = EditorGUILayout.Slider("Poisson Ratio", mesh.m_poisson, 0.0001f, 0.49f);
            EditorGUILayout.Separator();
        }

        if (mesh.m_stiffness != float.MinValue)
        {
            mesh.stiffness = EditorGUILayout.Slider("Stiffness", mesh.m_stiffness, 0.0001f, 10000);
            EditorGUILayout.Separator();
        }

        if (mesh.m_damping != float.MinValue)
        {
            mesh.damping = EditorGUILayout.Slider("Damping", mesh.m_damping, 0.0001f, 100);
            EditorGUILayout.Separator();
        }

        if (mesh.hasCollisionSphere())
        {
            mesh.radius = EditorGUILayout.Slider("Sphere radius", mesh.radius, 0.001f, 10);
            mesh.contactStiffness = EditorGUILayout.Slider("Spheree Contact stiffness", mesh.contactStiffness, 1, 5000);
            mesh.showCollisionSphere = EditorGUILayout.Toggle("Display Collision Spheres", mesh.showCollisionSphere);
            EditorGUILayout.Separator();
        }
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SofaRigidMesh GameObject
/// </summary>
[CustomEditor(typeof(SofaRigidMesh), true)]
public class SofaRigidMeshEditor : Editor
{
    bool normalBtn = false;

    /// <summary>
    ///  Add SofaRigidMesh Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidMesh GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaRigidMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaRigidObject/SofaRigidMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaRigidMesh");
        go.AddComponent<SofaRigidMesh>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidMesh GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        SofaRigidMesh mesh = (SofaRigidMesh)this.target;
        if (mesh.isAwake() == false)
            return;

        // Check box to change normals direction
        normalBtn = EditorGUILayout.Toggle("Inverse Normals", normalBtn);
        mesh.invertNormals = normalBtn;

        // Add Triansformation fields
        mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.translation);
        EditorGUILayout.Separator();

        mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.rotation);
        EditorGUILayout.Separator();

        mesh.scale = EditorGUILayout.Vector3Field("Scale", mesh.scale);
        EditorGUILayout.Separator();


    }
}
