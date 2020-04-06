using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SofaSphere GameObject
/// </summary>
[CustomEditor(typeof(SofaSphere), true)]
public class SofaSphereEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaSphere GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaDeformableObject/SofaSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaSphere");
        go.AddComponent<SofaSphere>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SofaRigidSphere GameObject
/// </summary>
[CustomEditor(typeof(SofaRigidSphere), true)]
public class SofaRigidSphereEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidSphere GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaRigidSphere")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaRigidObject/SofaRigidSphere")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaRigidSphere");
        go.AddComponent<SofaRigidSphere>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
