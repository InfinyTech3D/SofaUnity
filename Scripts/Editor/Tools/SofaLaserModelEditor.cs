using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaLaserModel), true)]
public class SofaLaserModelEditor : SofaRayCasterEditor
{
    /// <summary>
    ///  
    /// </summary>
    /// <returns>Pointer to the SofaLaserModel GameObject</returns>
    [MenuItem("SofaUnity/SofaComponent/SofaLaserModel")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaComponent/SofaLaserModel")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaLaserModel");
        go.AddComponent<SofaLaserModel>();

        if (Selection.activeTransform != null)
            go.transform.parent = Selection.activeTransform;

        return go;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaLaserModel model = this.target as SofaLaserModel;
        if (model == null)
            return;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();


        model.DrawLaser = EditorGUILayout.Toggle("Draw Laser Particles", model.DrawLaser);
        if (model.DrawLaser)
        {
            model.LaserWidth = EditorGUILayout.FloatField("Laser Width", model.LaserWidth);
            model.LaserStartColor = EditorGUILayout.ColorField("Laser start Color", model.LaserStartColor);
            model.LaserEndColor = EditorGUILayout.ColorField("Laser end Color", model.LaserEndColor);

            model.m_particleMat = (Material)EditorGUILayout.ObjectField("Laser Material", model.m_particleMat, typeof(Material));
        }
    }
}
