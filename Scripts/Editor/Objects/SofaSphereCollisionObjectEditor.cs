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
        if (Selection.activeTransform == null)
        {
            Debug.LogError("Error1 creating SofaSphereCollisionObject GameObject. No valid gameObject selected under SofaContext.");
            return null;
        }

        GameObject selectObj = Selection.activeGameObject;

        if (selectObj.GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("Error2 creating SofaSphereCollisionObject GameObject. Object should have a valid MeshFilter.");
            return null;
        }

        SofaDAGNode parentDagN = selectObj.GetComponentInParent<SofaDAGNode>();
        if (parentDagN == null)
        {
            Debug.LogError("Error3 creating SofaSphereCollisionObject GameObject. No valid gameObject selected child of SofaContext or SofaDAGNode.");
            return null;
        }



        selectObj.AddComponent<SofaSphereCollisionObject>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionObject. Can't access SofaDAGNodeManager.");

        return selectObj;
    }

    public override void OnInspectorGUI()
    {
        

        SofaSphereCollisionObject model = (SofaSphereCollisionObject)this.target;

        model.UsePositionOnly = EditorGUILayout.Toggle("Use Object Position Only (1 dof)", model.UsePositionOnly);
        model.Factor = EditorGUILayout.Slider("Interpolation factor", model.Factor, 1, 100);
        model.Radius = EditorGUILayout.Slider("Sphere radius", model.Radius, 0.001f, 10);
        model.Activated = EditorGUILayout.Toggle("Activate collision", model.Activated);
        model.Stiffness = EditorGUILayout.Slider("Contact stiffness", model.Stiffness, 1, 5000);
        model.m_startOnPlay = EditorGUILayout.Toggle("Start on Play", model.m_startOnPlay);

        EditorGUILayout.LabelField("Number of spheres", model.NbrSpheres.ToString());
    }
}
