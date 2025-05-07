using UnityEngine;
using UnityEditor;
using SofaUnityAPI;
using System.Collections.Generic;

namespace SofaUnity
{
    /// <summary>
    /// Editor class corresponding to @sa SofaMouseInteractor
    /// This editor is a specialization of @sa SofaRayCasterEditor to only add the option to display selected primitive.
    /// </summary>
    [CustomEditor(typeof(SofaMouseInteractor), true)]
    public class SofaMouseInteractorEditor : Editor
    {
        /// Method to create parameters GUI
        public override void OnInspectorGUI()
        {
            // display SofaRayCasterEditor first
        //base.OnInspectorGUI();
            SofaMouseInteractor model = this.target as SofaMouseInteractor;
            if (model == null)
                return;

            model.Length = EditorGUILayout.FloatField("Ray Length", model.Length);

            /// SofaRayCaster inspector
            model.RayInteractionType = (SofaDefines.SRayInteraction)EditorGUILayout.EnumPopup("Tool interaction", model.RayInteractionType);

            if (model.RayInteractionType != SofaDefines.SRayInteraction.None)
            {
                model.startOnPlay = EditorGUILayout.Toggle("StartOnPlay mode", model.startOnPlay);
                model.ActivateTool = EditorGUILayout.Toggle("Activate Tool", model.ActivateTool);
            }

            if (model.RayInteractionType == SofaDefines.SRayInteraction.AttachTool)
            {
                EditorGUILayout.Separator();
                model.AttachStiffness = EditorGUILayout.FloatField("Tool Attach Stiffness", model.AttachStiffness);
            }

            EditorGUILayout.Separator();
            model.DrawSelection = EditorGUILayout.Toggle("Draw Selection", model.DrawSelection);
            model.DrawSpring = EditorGUILayout.Toggle("Draw Mouse Spring", model.DrawSpring);

            model.m_selectionMaterial = EditorGUILayout.ObjectField("Selection Material", model.m_selectionMaterial, typeof(Material)) as Material;
            model.m_springMaterial = EditorGUILayout.ObjectField("Spring Material", model.m_springMaterial, typeof(Material)) as Material;
        }
    }
}
