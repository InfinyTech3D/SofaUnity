using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaParticlesModel
/// Provide only a create method to create SofaParticlesModel from Unity Menu
/// </summary>
[CustomEditor(typeof(SofaParticlesModel), true)]
public class SofaParticlesModelEditor : Editor
{
    /// <summary>
    ///  Add SofaParticlesModel creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaParticlesModel GameObject</returns>
    [MenuItem("SofaUnity/SofaComponent/SofaParticlesModel")]
    [MenuItem("GameObject/Create Other/SofaComponent/SofaParticlesModel")]
    public static GameObject CreateNew()
    {
        if (Selection.activeTransform != null)
        {
            GameObject selectObj = Selection.activeGameObject;
            SofaDAGNode dagN = selectObj.GetComponent<SofaDAGNode>();

            if (dagN == null)
            {
                Debug.LogError("Error2 creating SofaParticlesModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            SofaMesh mesh = dagN.GetSofaMesh();
            if (mesh == null)
                mesh = dagN.FindSofaMesh();

            if (mesh == null)
            {
                Debug.LogError("Error3 creating SofaParticlesModel object. No SofaDAGNode with a valid SofaMesh selected.");
                return null;
            }

            GameObject go = new GameObject("SofaParticlesModel  -  " + dagN.UniqueNameId);
            SofaParticlesModel pModel = go.AddComponent<SofaParticlesModel>();
            go.transform.parent = selectObj.transform;
            pModel.m_sofaMesh = mesh;

            return go;
        }
        else
        {
            Debug.LogError("Error1 creating SofaBeamModel object. No SofaDAGNode with a valid SofaMesh selected.");
        }

        return null;
    }


}
