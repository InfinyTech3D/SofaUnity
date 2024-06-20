using UnityEngine;
using UnityEditor;
using SofaUnity;


/// <summary>
/// Editor Class to define the creation and UI of SofaMeshObject GameObject
/// </summary>
[CustomEditor(typeof(SofaMeshObject), true)]
public class SofaMeshObjectEditor : Editor
{

    public static SofaDAGNode GetDAGNodeSelected()
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogError("Error1 creating SofaDAGNode GameObject. No valid SofaContext nor SofaDAGNode selected.");
            return null;
        }
        
        GameObject selectObj = Selection.activeGameObject;
        SofaDAGNode parentDagN = selectObj.GetComponent<SofaDAGNode>();
        if (parentDagN == null)
        {
            // not a DAGNode selected. Check if SofaComponent
            SofaBaseComponent sofaComponent = selectObj.GetComponent<SofaBaseComponent>();

            // If neither a sofa component, nothing can be done
            if (sofaComponent == null)
            {
                Debug.LogError("Error2 creating SofaDAGNode GameObject. No valid SofaDAGNode or SofaComponent selected.");
                return null;
            }

            // otherwise  will search for DAGNode owner of this component and add New DAGNode as child of this owner
            parentDagN = sofaComponent.m_ownerNode;
            if (parentDagN == null)
            {
                Debug.LogError("Error3 creating SofaDAGNode GameObject. SofaComponent selected has no valid SofaDAGNode owner.");
                return null;
            }
        }

        return parentDagN;       
    }


    bool normalBtn = false;

    /// <summary>
    ///  Add SofaMeshObject Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaMeshObject GameObject</returns>
    [MenuItem("SofaUnity/SofaObject/SofaMeshObject")]
    [MenuItem("GameObject/Create Other/SofaObject/SofaMeshObject")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("SofaMeshObject");
        go.AddComponent<SofaMeshObject>();
        return go;
    }


    /// <summary>
    /// Method to set the UI of the SofaMeshObject GameObject
    /// </summary>
    
    public override void OnInspectorGUI()
    {     
        SofaMeshObject mesh = (SofaMeshObject)this.target;
        if (mesh.IsAwake() == false)
            return;

        // Check box to change normals direction
        normalBtn = EditorGUILayout.Toggle("Inverse Normals", mesh.m_invertNormals);
        mesh.invertNormals = normalBtn;

        //// Add Triansformation fields
        //mesh.translation = EditorGUILayout.Vector3Field("Translation", mesh.m_translation);
        //EditorGUILayout.Separator();

        //mesh.rotation = EditorGUILayout.Vector3Field("Rotation", mesh.m_rotation);
        //EditorGUILayout.Separator();

        //mesh.scale = EditorGUILayout.Vector3Field("Scale", mesh.m_scale);
        //EditorGUILayout.Separator();


        //// Add FEM fields
        //if (mesh.m_mass != float.MinValue)
        //{
        //    mesh.mass = EditorGUILayout.Slider("Mass", mesh.m_mass, 0.0001f, 1000);
        //    EditorGUILayout.Separator();
        //}

        //if (mesh.m_young != float.MinValue)
        //{
        //    mesh.young = EditorGUILayout.Slider("Young Modulus", mesh.m_young, 0.0001f, 10000);
        //    EditorGUILayout.Separator();
        //}

        //if (mesh.m_poisson != float.MinValue)
        //{
        //    mesh.poisson = EditorGUILayout.Slider("Poisson Ratio", mesh.m_poisson, 0.0001f, 0.49f);
        //    EditorGUILayout.Separator();
        //}

        //if (mesh.m_stiffness != float.MinValue)
        //{
        //    mesh.stiffness = EditorGUILayout.Slider("Stiffness", mesh.m_stiffness, 0.0001f, 10000);
        //    EditorGUILayout.Separator();
        //}

        //if (mesh.m_damping != float.MinValue)
        //{
        //    mesh.damping = EditorGUILayout.Slider("Damping", mesh.m_damping, 0.0001f, 100);
        //    EditorGUILayout.Separator();
        //}

        //if (mesh.hasCollisionSphere())
        //{
        //    mesh.radius = EditorGUILayout.Slider("Sphere radius", mesh.radius, 0.001f, 10);
        //    mesh.contactStiffness = EditorGUILayout.Slider("Spheree Contact stiffness", mesh.contactStiffness, 1, 5000);
        //    mesh.showCollisionSphere = EditorGUILayout.Toggle("Display Collision Spheres", mesh.showCollisionSphere);
        //    EditorGUILayout.Separator();
        //}
    }
}
