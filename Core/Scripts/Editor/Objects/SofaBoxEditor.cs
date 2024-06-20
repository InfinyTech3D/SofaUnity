using UnityEngine;
using UnityEditor;
using SofaUnity;

/// <summary>
/// Editor class corresponding to @sa SofaBox object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Box deformable object.
/// Provide create method to create SofaBox from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaBox), true)]
public class SofaBoxEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaBox GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaBox")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaBox")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaBox");
        go.AddComponent<SofaBox>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaBox object. Can't access SofaDAGNodeManager.");

        return go;
    }



    /// <summary>
    /// Method to set the UI of the SofaBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}





/// <summary>
/// Editor class corresponding to @sa SofaRigidBox object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Box regid object.
/// Provide create method to create SofaRigidBox from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaRigidBox), true)]
public class SofaRigidBoxEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidBox Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidBox GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaRigidBox")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaRigidBox")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaRigidBox");
        go.AddComponent<SofaRigidBox>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaRigidBox object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidBox GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}
