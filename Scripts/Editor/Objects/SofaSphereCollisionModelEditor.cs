using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SofaSphereCollisionModel), true)]
public class SofaSphereCollisionModelEditor : Editor
{
    public override void OnInspectorGUI()
    {

        SofaSphereCollisionModel model = (SofaSphereCollisionModel)this.target;
        model.usePositionOnly = EditorGUILayout.Toggle("Use Object Position Only", model.usePositionOnly);
        model.factor = EditorGUILayout.Slider("Interpolation factor", model.factor, 1, 100);
        model.radius = EditorGUILayout.Slider("Sphere radius", model.radius, 0.001f, 10);
        model.activated = EditorGUILayout.Toggle("Activate collision", model.activated);
        model.stiffness = EditorGUILayout.Slider("Contact stiffness", model.stiffness, 1, 5000);
        model.m_startOnPlay = EditorGUILayout.Toggle("Start on Play", model.m_startOnPlay);

        EditorGUILayout.LabelField("Number of spheres", model.nbrSpheres.ToString());
    }
}
