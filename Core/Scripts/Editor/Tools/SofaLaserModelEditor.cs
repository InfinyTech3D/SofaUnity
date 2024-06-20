using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaLaserModelEditor
/// This editor is a specialization of @sa SofaRayCasterEditor to add Laser display parameters
/// Provide method to create SofaLaserModel from Unity Menu
/// </summary>
[CustomEditor(typeof(SofaLaserModel), true)]
public class SofaLaserModelEditor : SofaRayCasterEditor
{
    /// <summary>
    ///  Add SofaLaserModel creation to SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaLaserModel GameObject</returns>
    [MenuItem("SofaUnity/SofaComponent/SofaLaserModel")]
    [MenuItem("GameObject/Create Other/SofaComponent/SofaLaserModel")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaLaserModel");
        go.AddComponent<SofaLaserModel>();

        if (Selection.activeTransform != null)
            go.transform.parent = Selection.activeTransform;

        return go;
    }


    /// Method to create parameters GUI
    public override void OnInspectorGUI()
    {
        // display SofaRayCasterEditor first
        base.OnInspectorGUI();

        SofaLaserModel model = this.target as SofaLaserModel;
        if (model == null)
            return;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        model.DrawLight = EditorGUILayout.Toggle("Draw Laser Light", model.DrawLight);
        if (model.DrawLight)
        {
            model.LaserStartColor = EditorGUILayout.ColorField("Laser Color", model.LaserStartColor);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        model.DrawLaser = EditorGUILayout.Toggle("Draw Laser Particles", model.DrawLaser);
        if (model.DrawLaser)
        {
            model.LaserWidth = EditorGUILayout.FloatField("Laser Width", model.LaserWidth);
            if (!model.DrawLight)
                model.LaserStartColor = EditorGUILayout.ColorField("Laser start Color", model.LaserStartColor);
            model.LaserEndColor = EditorGUILayout.ColorField("Laser end Color", model.LaserEndColor);
            
            model.m_particleMat = EditorGUILayout.ObjectField("Laser Material", model.m_particleMat, typeof(Material)) as Material;
        }
    }
}
