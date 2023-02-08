using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;



[CustomEditor(typeof(SofaBeamAdapterModel), true)]
public class SofaBeamAdapterModelEditor : Editor
{
    /// <summary>
    ///  Add SofaBeamAdapterModel creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaBeamAdapterModel GameObject</returns>
    [MenuItem("SofaUnity/SofaComponent/SofaBeamAdapterModel")]
    [MenuItem("GameObject/Create Other/SofaComponent/SofaBeamAdapterModel")]
    public static GameObject CreateNew()
    {
        if (Selection.activeTransform != null)
        {
            GameObject selectObj = Selection.activeGameObject;
            SofaDAGNode dagN = selectObj.GetComponent<SofaDAGNode>();

            if (dagN == null)
            {
                Debug.LogError("Error2 creating SofaBeamAdapterModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            SofaMesh mesh = dagN.GetSofaMesh();
            if (mesh == null)
                mesh = dagN.FindSofaMesh();

            if (mesh == null)
            {
                Debug.LogError("Error3 creating SofaBeamAdapterModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            GameObject go = new GameObject("SofaBeamAdapterModel  -  " + dagN.UniqueNameId);
            SofaBeamAdapterModel bModel = go.AddComponent<SofaBeamAdapterModel>();
            go.transform.parent = selectObj.transform;
            bModel.m_sofaMesh = mesh;

            return go;
        }
        else
        {
            Debug.LogError("Error1 creating SofaBeamAdapterModel object. No SofaDAGNode with a valid SofaMesh selected.");
        }

        return null;
    }


    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaBeamAdapterModel model = this.target as SofaBeamAdapterModel;
        if (model == null)
            return;

        model.m_sofaMesh = (SofaMesh)EditorGUILayout.ObjectField("Beam SofaMesh", model.m_sofaMesh, typeof(SofaMesh), true);
        model.BeamDiscretisation = EditorGUILayout.IntField("Beam Discretisation", model.BeamDiscretisation);
        model.BeamRadius = EditorGUILayout.Slider("Beam Radius", model.BeamRadius, 0.001f, 30);
        model.m_childCameraScript = (GameObject)EditorGUILayout.ObjectField("Child Tip Camera", model.m_childCameraScript, typeof(GameObject), true);
    }
}