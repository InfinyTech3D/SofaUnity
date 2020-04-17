using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaComponent
/// This class inherite from @sa SofaBaseComponentEditor and will show or Base Data
/// </summary>
[CustomEditor(typeof(SofaComponent), true)]
public class SofaComponentEditor : SofaBaseComponentEditor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        SofaComponent compo = (SofaComponent)this.target;
        // will see later if this option should be accessible from gui
        if (compo.ShowData) // if showData is true, display normal BaseComponentEditor gui
        {
            base.OnInspectorGUI();
        }
        else // show only a few info
        {
            EditorGUI.BeginDisabledGroup(true);
            compo.UniqueNameId = EditorGUILayout.TextField("Unique Name Id", compo.UniqueNameId);
            EditorGUILayout.EnumPopup("BaseComponentType", compo.m_baseComponentType);
            compo.m_componentType = EditorGUILayout.TextField("Component Type", compo.m_componentType);
            EditorGUI.EndDisabledGroup();
        }
        
    }
}
