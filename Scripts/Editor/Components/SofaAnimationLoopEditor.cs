using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaAnimationLoop component
/// This class inherite from @sa SofaBaseComponentEditor and will add specific data after the Data display
/// </summary>
[CustomEditor(typeof(SofaAnimationLoop), true)]
public class SofaAnimationLoopEditor : SofaBaseComponentEditor
{
    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
