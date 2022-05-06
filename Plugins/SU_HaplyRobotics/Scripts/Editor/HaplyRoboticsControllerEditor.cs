using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SofaUnity;

[CustomEditor(typeof(SofaHaplyRobotics), true)]
public class HaplyRoboticsControllerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        SofaHaplyRobotics model = (SofaHaplyRobotics)this.target;

        // Section for the models
        model.m_sofaInverse3_controller = (SofaComponent)EditorGUILayout.ObjectField("Haply_Inverse3Controller", 
            model.m_sofaInverse3_controller, typeof(SofaComponent), true);

        model.m_LCPForce = (SofaComponent)EditorGUILayout.ObjectField("LCPForceFeedback",
            model.m_LCPForce, typeof(SofaComponent), true);

        model.m_toolCollisionModel = (SofaCollisionModel)EditorGUILayout.ObjectField("Tool Collision Model",
            model.m_toolCollisionModel, typeof(SofaCollisionModel), true);


        model.forceFeedBackCoef = EditorGUILayout.Slider("ForceFeedBack Coef", model.forceFeedBackCoef, 0.0001f, 0.01f);
        model.dumpForce = EditorGUILayout.Toggle("Dump forces sent to device", model.dumpForce);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("Collision active", model.collisionActive);
        EditorGUILayout.Vector3Field("Force values sent", model.rawForce);
        EditorGUI.EndDisabledGroup();
    }
}
