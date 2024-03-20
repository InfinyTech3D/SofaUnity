using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaVisualModel
/// This class inherite from @sa SofaBaseComponentEditor and will add specific data after the Data display
/// WIP tests
/// </summary>
[CustomEditor(typeof(SofaVisualModel), true)]
public class SofaVisualModelEditor : SofaBaseComponentEditor
{
    public override void OnInspectorGUI()
    {
        m_showData = false;
        base.OnInspectorGUI();

        SofaVisualModel _object = (SofaVisualModel)this.target;
        _object.UVType = (e_UVType)EditorGUILayout.EnumPopup("UV unwrap method", _object.UVType);
    }
}
