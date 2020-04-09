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


        model.m_SofaMesh = (Mesh)EditorGUILayout.ObjectField("Unity Mesh", model.m_SofaMesh, typeof(Mesh));

        model.UsePositionOnly = EditorGUILayout.Toggle("Use Object Position Only", model.UsePositionOnly);
        model.Factor = EditorGUILayout.Slider("Interpolation factor", model.Factor, 1, 100);
        model.Radius = EditorGUILayout.Slider("Sphere radius", model.Radius, 0.001f, 10);
        model.Activated = EditorGUILayout.Toggle("Activate collision", model.Activated);
        model.Stiffness = EditorGUILayout.Slider("Contact stiffness", model.Stiffness, 1, 5000);
        model.m_startOnPlay = EditorGUILayout.Toggle("Start on Play", model.m_startOnPlay);

        EditorGUILayout.LabelField("Number of spheres", model.NbrSpheres.ToString());
    }
}
