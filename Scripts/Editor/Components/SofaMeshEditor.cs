using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaMesh), true)]
public class SofaMeshEditor : SofaBaseComponentEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaMesh compo = (SofaMesh)this.target;

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
        EditorGUI.EndDisabledGroup();
    }   
}
