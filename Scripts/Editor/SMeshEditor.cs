using UnityEngine;
using UnityEditor;
using SofaUnity;


/// <summary>
/// Editor Class to define the creation and UI of SDeformableMesh GameObject
/// </summary>
[CustomEditor(typeof(SDeformableMesh), true)]
public class SDeformableMeshEditor : Editor
{
    bool normalBtn = false;

    /// <summary>
    ///  Add SDeformableMesh Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SDeformableMesh GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SDeformableMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SDeformableMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SDeformableMesh");
        go.AddComponent<SDeformableMesh>();
        return go;
    }


    /// <summary>
    /// Method to set the UI of the SDeformableMesh GameObject
    /// </summary>
    
    public override void OnInspectorGUI()
    {     
        SDeformableMesh mesh = (SDeformableMesh)this.target;
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
/// Editor Class to define the creation and UI of SRigidMesh GameObject
/// </summary>
[CustomEditor(typeof(SRigidMesh), true)]
public class SRigidMeshEditor : Editor
{
    bool normalBtn = false;

    /// <summary>
    ///  Add SRigidMesh Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidMesh GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidMesh")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidMesh")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidMesh");
        go.AddComponent<SRigidMesh>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidMesh GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        SRigidMesh mesh = (SRigidMesh)this.target;
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
