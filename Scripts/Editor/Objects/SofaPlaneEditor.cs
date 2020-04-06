using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SofaRigidPlane GameObject
/// </summary>
[CustomEditor(typeof(SofaRigidPlane), true)]
public class SofaRigidPlaneEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidPlane Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidPlane GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaRigidPlane")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaRigidObject/SofaRigidPlane")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaRigidPlane");
        go.AddComponent<SofaRigidPlane>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();

        SofaRigidPlane grid = (SofaRigidPlane)this.target;
    }
}