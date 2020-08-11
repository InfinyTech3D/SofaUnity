using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor class corresponding to @sa SofaSphere object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Sphere deformable object.
/// Provide create method to create SofaSphere from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaSphere), true)]
public class SofaSphereEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaSphere GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaSphere")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaSphere")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaSphere");
        go.AddComponent<SofaSphere>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaSphere object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}




/// <summary>
/// Editor class corresponding to @sa SofaRigidSphere object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Sphere regid object.
/// Provide create method to create SofaRigidSphere from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaRigidSphere), true)]
public class SofaRigidSphereEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidSphere Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidSphere GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaRigidSphere")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaRigidSphere")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaRigidSphere");
        go.AddComponent<SofaRigidSphere>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaRigidSphere object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidSphere GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
