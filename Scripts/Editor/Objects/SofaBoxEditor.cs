using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SofaBox GameObject
/// </summary>
[CustomEditor(typeof(SofaBox), true)]
public class SofaBoxEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaBox GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SofaBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SofaBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaBox");
        go.AddComponent<SofaBox>();        
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SofaRigidBox GameObject
/// </summary>
[CustomEditor(typeof(SofaRigidBox), true)]
public class SofaRigidBoxEditor : SofaRigidGridEditor
{
    /// <summary>
    ///  Add SofaRigidBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidBox GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SofaRigidBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SofaRigidBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaRigidBox");
        go.AddComponent<SofaRigidBox>();        
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
