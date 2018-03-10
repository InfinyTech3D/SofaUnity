using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SSphere GameObject
/// </summary>
[CustomEditor(typeof(SSphere), true)]
public class SSphereEditor : SGridEditor
{
    /// <summary>
    ///  Add SSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SSphere GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SSphere");
        go.AddComponent<SSphere>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SGrid and SDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SRigidSphere GameObject
/// </summary>
[CustomEditor(typeof(SRigidSphere), true)]
public class SRigidSphereEditor : SRigidGridEditor
{
    /// <summary>
    ///  Add SRigidSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidSphere GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidSphere");
        go.AddComponent<SRigidSphere>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SRigidGrid and SRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
