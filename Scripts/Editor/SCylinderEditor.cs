using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SCylinder GameObject
/// </summary>
[CustomEditor(typeof(SCylinder), true)]
public class SCylinderEditor : SGridEditor
{
    /// <summary>
    ///  Add SCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SCylinder GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SCylinder");
        go.AddComponent<SCylinder>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SGrid and SDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SRigidCylinder GameObject
/// </summary>
[CustomEditor(typeof(SRigidCylinder), true)]
public class SRigidCylinderEditor : SRigidGridEditor
{
    /// <summary>
    ///  Add SRigidCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidCylinder GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidCylinder")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidCylinder")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidCylinder");
        go.AddComponent<SRigidCylinder>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SRigidGrid and SRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
