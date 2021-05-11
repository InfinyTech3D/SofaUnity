using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaMesh
/// This class inherite from @sa SofaBaseComponentEditor and will add specific data after the Data display
/// Provides some information regarding the sofa topology handle by this Mesh
/// </summary>
[CustomEditor(typeof(SofaMesh), true)]
public class SofaMeshEditor : SofaBaseComponentEditor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaMesh compo = (SofaMesh)this.target;
        compo.DrawForces = EditorGUILayout.Toggle("DrawGizmoForces", compo.DrawForces);
        
        if (!compo.HasTopology())
            return;

        EditorGUILayout.Separator();

        EditorGUI.BeginDisabledGroup(true);
        TopologyObjectType type = compo.TopologyType();
        EditorGUILayout.EnumPopup("MeshTopology Type", type);

        if (type == TopologyObjectType.HEXAHEDRON)
        {
            EditorGUILayout.IntField("Nb Hexahedra", compo.NbHexahedra());
        }
        else if (type == TopologyObjectType.TETRAHEDRON)
        {
            EditorGUILayout.IntField("Nb Tetrahedra", compo.NbTetrahedra());
        }
        else if (type == TopologyObjectType.QUAD || type == TopologyObjectType.TRIANGLE)
        {
            EditorGUILayout.IntField("Nb Triangles", compo.NbTriangles());
            EditorGUILayout.IntField("Nb Quads", compo.NbQuads());
        }
        else if (type == TopologyObjectType.EDGE)
        {
            EditorGUILayout.IntField("Nb Edges", compo.NbEdges());
        }
        else
        {
            EditorGUILayout.IntField("Nb Points", compo.NbVertices());
        }

        
        EditorGUI.EndDisabledGroup();
    }   
}
