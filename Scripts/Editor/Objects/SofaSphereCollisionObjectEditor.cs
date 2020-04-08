using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor class corresponding to @sa SofaSphereCollisionModel object
/// This class inherite from @sa SofaMeshObjectEditor and will add specific parameter for Number of sphere collision to create.
/// Provide create method to create SofaSphereCollisionModel from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaSphereCollisionObject), true)]
public class SofaSphereCollisionObjectEditor : SofaMeshObjectEditor
{
    /// <summary>
    ///  Add SofaSphereCollisionModel creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaSphereCollisionModel GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaSphereCollisionObject")]
    [MenuItem("GameObject/Create Other/SofaObject/SofaSphereCollisionObject")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("CollsionObject");
        go.AddComponent<SofaSphereCollisionObject>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionObject. Can't access SofaDAGNodeManager.");

        return go;
    }

    public override void OnInspectorGUI()
    {

        SofaSphereCollisionObject model = (SofaSphereCollisionObject)this.target;
        model.usePositionOnly = EditorGUILayout.Toggle("Use Object Position Only", model.usePositionOnly);
        model.factor = EditorGUILayout.Slider("Interpolation factor", model.factor, 1, 100);
        model.radius = EditorGUILayout.Slider("Sphere radius", model.radius, 0.001f, 10);
        model.activated = EditorGUILayout.Toggle("Activate collision", model.activated);
        model.stiffness = EditorGUILayout.Slider("Contact stiffness", model.stiffness, 1, 5000);
        model.m_startOnPlay = EditorGUILayout.Toggle("Start on Play", model.m_startOnPlay);

        EditorGUILayout.LabelField("Number of spheres", model.nbrSpheres.ToString());
    }
}
