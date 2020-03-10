using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SofaCylinder GameObject
/// </summary>
[CustomEditor(typeof(SofaCylinder), true)]
public class SofaCylinderEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaCylinder GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaDeformableObject/SofaCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaCylinder");
        go.AddComponent<SofaCylinder>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SofaRigidCylinder GameObject
/// </summary>
[CustomEditor(typeof(SofaRigidCylinder), true)]
public class SofaRigidCylinderEditor : SofaRigidGridEditor
{
    /// <summary>
    ///  Add SofaRigidCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidCylinder GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaRigidCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaRigidObject/SofaRigidCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaRigidCylinder");
        go.AddComponent<SofaRigidCylinder>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
