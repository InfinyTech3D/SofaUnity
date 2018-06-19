using UnityEngine;
using UnityEditor;
using SofaUnity;

[CustomEditor(typeof(SSphereCollisionModel), true)]
public class SSphereCollisionModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SSphereCollisionModel model = (SSphereCollisionModel)this.target;
        model.factor = EditorGUILayout.Slider("Interpolation factor", model.factor, 0.001f, 100);
        model.radius = EditorGUILayout.Slider("Sphere radius", model.radius, 0.001f, 10);
        model.activated = EditorGUILayout.Toggle("Activate collision", model.activated);
        model.stiffness = EditorGUILayout.Slider("Contact stiffness", model.stiffness, 1, 5000);

        EditorGUILayout.LabelField("Number of spheres", model.nbrSpheres.ToString());
    }
}
