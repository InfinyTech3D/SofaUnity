using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SGrid), true)]
public class SGridEditor : SDeformableMeshEditor
{    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SGrid grid = (SGrid)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();

        
    }
}

[CustomEditor(typeof(SRigidGrid), true)]
public class SRigidGridEditor : SRigidMeshEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SRigidGrid grid = (SRigidGrid)this.target;

        grid.gridSize = EditorGUILayout.Vector3Field("Grid resolution", grid.gridSize);
        EditorGUILayout.Separator();        
    }
}


