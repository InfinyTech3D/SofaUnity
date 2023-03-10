using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaBeamModel
/// Provide create method to create SofaBeamModel from Unity Menu
/// Provide interface to change beam radius and discretisation
/// </summary>
[CustomEditor(typeof(SofaBeamModel), true)]
public class SofaBeamModelEditor : Editor
{
    /// <summary>
    ///  Add SofaBeamModel creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaBeamModel GameObject</returns>
    [MenuItem("SofaUnity/SofaComponent/SofaBeamModel")]
    [MenuItem("GameObject/Create Other/SofaComponent/SofaBeamModel")]
    public static GameObject CreateNew()
    {
        if (Selection.activeTransform != null)
        {
            GameObject selectObj = Selection.activeGameObject;
            SofaDAGNode dagN = selectObj.GetComponent<SofaDAGNode>();

            if (dagN == null)
            {
                Debug.LogError("Error2 creating SofaBeamModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            SofaMesh mesh = dagN.GetSofaMesh();
            if (mesh == null)
                mesh = dagN.FindSofaMesh();

            if (mesh == null)
            {
                Debug.LogError("Error3 creating SofaBeamModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            GameObject go = new GameObject("SofaBeamModel  -  " + dagN.UniqueNameId);
            SofaBeamModel bModel = go.AddComponent<SofaBeamModel>();
            go.transform.parent = selectObj.transform;
            bModel.m_sofaMesh = mesh;

            return go;
        }
        else
        {
            Debug.LogError("Error1 creating SofaBeamModel object. No SofaDAGNode with a valid SofaMesh selected.");
        }
        
        return null;
    }

    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaBeamModel model = this.target as SofaBeamModel;
        if (model == null)
            return;

        model.m_sofaMesh = (SofaMesh)EditorGUILayout.ObjectField("Beam SofaMesh", model.m_sofaMesh, typeof(SofaMesh), true);
        model.BeamDiscretisation = EditorGUILayout.IntField("Beam Discretisation", model.BeamDiscretisation);
        model.BeamRadius = EditorGUILayout.Slider("Beam Radius", model.BeamRadius, 0.001f, 30);
        model.isRigidMesh = EditorGUILayout.Toggle("use Rigid Dof", model.isRigidMesh);
    }
}
