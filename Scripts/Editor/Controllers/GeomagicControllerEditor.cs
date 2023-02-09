using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;


/// <summary>
/// Editor class corresponding to the @sa GeomagicController
/// Provide interface to be able to set the GeomagicDriver component.
/// </summary>
[CustomEditor(typeof(GeomagicController), true)]
public class GeomagicControllerEditor : Editor
{
    /// <summary>
    /// Method to create GeomagicController UI
    /// </summary>
    public override void OnInspectorGUI()
    {
        GeomagicController model = this.target as GeomagicController;
        if (model == null)
            return;


        model.GeomagicDriver = (SofaComponent)EditorGUILayout.ObjectField("GeomagicDriver", model.GeomagicDriver, typeof(SofaComponent), true);

        EditorGUILayout.Separator();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("Button1 Pressed", model.IsButton1Pressed());
        EditorGUILayout.Toggle("Button2 Pressed", model.IsButton2Pressed());
        EditorGUILayout.Separator();
        EditorGUILayout.Toggle("Button1 Mode", model.Button1Status());
        EditorGUILayout.Toggle("Button2 Mode", model.Button2Status());
        EditorGUILayout.Toggle("Tool in Contact", model.IsToolInContact());
        EditorGUI.EndDisabledGroup();

        model.m_audioSource = (AudioSource)EditorGUILayout.ObjectField("GeomagicDriver", model.m_audioSource, typeof(AudioSource), true);

        model.smoke = (GameObject)EditorGUILayout.ObjectField("smoke", model.smoke, typeof(GameObject), true);
    }
}
