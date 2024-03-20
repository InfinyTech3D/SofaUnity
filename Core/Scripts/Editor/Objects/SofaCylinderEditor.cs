using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor class corresponding to @sa SofaCylinder object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Cylinder deformable object.
/// Provide create method to create SofaCylinder from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaCylinder), true)]
public class SofaCylinderEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaCylinder GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaCylinder")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaCylinder")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaCylinder");
        go.AddComponent<SofaCylinder>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaCylinder object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}


/// <summary>
/// Editor class corresponding to @sa SofaRigidCylinder object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Cylinder regid object.
/// Provide create method to create SofaRigidCylinder from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaRigidCylinder), true)]
public class SofaRigidCylinderEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidCylinder Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidCylinder GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaRigidCylinder")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaRigidCylinder")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaRigidCylinder");
        go.AddComponent<SofaRigidCylinder>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaRigidCylinder object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidCylinder GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
