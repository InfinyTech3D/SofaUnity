using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor Class to define the UI of SofaGrid GameObject. As Intermediate class. No Object is created here.
/// </summary>
[CustomEditor(typeof(SofaGrid), true)]
public class SofaGridEditor : SofaDeformableMeshEditor
{
    /// <summary>
    /// Method to set the UI of the SofaGrid GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaDeformableMesh UI creation
        base.OnInspectorGUI();

        SofaGrid grid = (SofaGrid)this.target;

        // Add Grid resolution field
        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();
    }
}

/// <summary>
/// Editor Class to define the UI of SofaRigidGrid GameObject. As Intermediate class. No Object is created here.
/// </summary>
[CustomEditor(typeof(SofaRigidGrid), true)]
public class SofaRigidGridEditor : SofaRigidMeshEditor
{
    /// <summary>
    /// Method to set the UI of the SofaRigidGrid GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidMesh UI creation
        base.OnInspectorGUI();

        SofaRigidGrid grid = (SofaRigidGrid)this.target;

        // Add Grid resolution field
        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();        
    }
}
