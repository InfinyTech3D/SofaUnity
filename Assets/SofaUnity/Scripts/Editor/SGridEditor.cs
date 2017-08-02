using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the UI of SGrid GameObject. As Intermediate class. No Object is created here.
/// </summary>
[CustomEditor(typeof(SGrid), true)]
public class SGridEditor : SDeformableMeshEditor
{
    /// <summary>
    /// Method to set the UI of the SGrid GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SDeformableMesh UI creation
        base.OnInspectorGUI();

        SGrid grid = (SGrid)this.target;

        // Add Grid resolution field
        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}

/// <summary>
/// Editor Class to define the UI of SRigidGrid GameObject. As Intermediate class. No Object is created here.
/// </summary>
[CustomEditor(typeof(SRigidGrid), true)]
public class SRigidGridEditor : SRigidMeshEditor
{
    /// <summary>
    /// Method to set the UI of the SRigidGrid GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SRigidMesh UI creation
        base.OnInspectorGUI();

        SRigidGrid grid = (SRigidGrid)this.target;

        // Add Grid resolution field
        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();        
    }
}
