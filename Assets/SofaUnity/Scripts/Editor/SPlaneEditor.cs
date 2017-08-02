using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SRigidPlane GameObject
/// </summary>
[CustomEditor(typeof(SRigidPlane), true)]
public class SRigidPlaneEditor : SRigidGridEditor
{
    /// <summary>
    ///  Add SRigidPlane Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidPlane GameObject</returns>
    [MenuItem("SofaUnity/Sofa 3D Object/SRigidPlane")]
    [MenuItem("GameObject/Create Other/SofaUnity/Sofa 3D Object/SRigidPlane")]
    new public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SRigidPlane");
        go.AddComponent<SRigidPlane>();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SRigidGrid and SRigidMesh UI creation
        base.OnInspectorGUI();

        SRigidPlane grid = (SRigidPlane)this.target;
    }
}