using UnityEngine;
using UnityEditor;
using SofaUnity;


/// <summary>
/// Editor class corresponding to @sa SofaPlane object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Plane deformable object.
/// Provide create method to create SofaPlane from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaPlane), true)]
public class SofaPlaneEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaPlane Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaPlane GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaPlane")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaPlane")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaPlane");
        go.AddComponent<SofaPlane>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaPlane object. Can't access SofaDAGNodeManager.");

        return go;
    }



    /// <summary>
    /// Method to set the UI of the SofaPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaGrid and SofaDeformableMesh UI creation
        base.OnInspectorGUI();
    }
}



/// <summary>
/// Editor class corresponding to @sa SofaRigidPlane object
/// This class inherite from @sa SofaGridEditor and will add specific parameter for Plane regid object.
/// Provide create method to create SofaRigidPlane from Unity Menu and register it inside DAGNodeManager
/// Provide interface for user interaction
/// </summary>
[CustomEditor(typeof(SofaRigidPlane), true)]
public class SofaRigidPlaneEditor : SofaGridEditor
{
    /// <summary>
    ///  Add SofaRigidPlane Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaRigidPlane GameObject</returns>
    [MenuItem("SofaUnity/PrimitiveObject/SofaRigidPlane")]
    [MenuItem("GameObject/Create Other/SofaPrimitiveObject/SofaRigidPlane")]
    new public static GameObject CreateNew()
    {
        SofaDAGNode parentDagN = GetDAGNodeSelected();
        if (parentDagN == null)
            return null;

        GameObject go = new GameObject("SofaRigidPlane");
        go.AddComponent<SofaRigidPlane>();

        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(go, parentDagN, true);
        else
            Debug.LogError("Error creating SofaRigidPlane object. Can't access SofaDAGNodeManager.");

        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        // call SofaRigidGrid and SofaRigidMesh UI creation
        base.OnInspectorGUI();
    }
}