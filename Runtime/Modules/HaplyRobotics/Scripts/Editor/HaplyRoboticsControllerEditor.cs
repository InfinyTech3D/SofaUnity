using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    [CustomEditor(typeof(SofaHaplyRoboticsController), true)]
    public class HaplyRoboticsControllerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            SofaHaplyRoboticsController model = (SofaHaplyRoboticsController)this.target;

            // Section for the models
            model.m_sofaInverse3_controller = (SofaComponent)EditorGUILayout.ObjectField("Haply_Inverse3Controller",
                model.m_sofaInverse3_controller, typeof(SofaComponent), true);

            model.m_LCPForce = (SofaComponent)EditorGUILayout.ObjectField("LCPForceFeedback",
                model.m_LCPForce, typeof(SofaComponent), true);

            model.m_toolCollisionModel = (SofaCollisionModel)EditorGUILayout.ObjectField("Tool Collision Model",
                model.m_toolCollisionModel, typeof(SofaCollisionModel), true);

            model.m_sofaMesh = (SofaMesh)EditorGUILayout.ObjectField("Tool Rigid position mesh",
                model.m_sofaMesh, typeof(SofaMesh), true);


            model.forceFeedBackCoef = EditorGUILayout.Slider("Device Force Coefficient", model.forceFeedBackCoef, 0.00001f, 0.001f);            

            model.dumpForce = EditorGUILayout.Toggle("Debug: Log device forces", model.dumpForce);

            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Button Pressed", model.ButtonStatus());
            EditorGUILayout.Separator();
            EditorGUILayout.Toggle("Collision active", model.collisionActive);
            EditorGUILayout.Vector3Field("Force values sent", model.rawForce);
            EditorGUI.EndDisabledGroup();
        }
    }
}
