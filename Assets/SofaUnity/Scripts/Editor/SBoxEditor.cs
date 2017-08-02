using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SBox GameObject
/// </summary>
[CustomEditor(typeof(SBox), true)]
public class SBoxEditor : SGridEditor
{
    /// <summary>
    ///  Add SBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SBox GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SBox");
        go.AddComponent<SBox>();        
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SGrid and SDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor Class to define the creation and UI of SRigidBox GameObject
/// </summary>
[CustomEditor(typeof(SRigidBox), true)]
public class SRigidBoxEditor : SRigidGridEditor
{
    /// <summary>
    ///  Add SRigidBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidBox GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidBox")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidBox")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidBox");
        go.AddComponent<SRigidBox>();        
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SRigidGrid and SRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
