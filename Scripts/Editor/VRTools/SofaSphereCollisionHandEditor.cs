using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(SofaSphereCollisionHand), true)]
public class SofaSphereCollisionHandEditor : Editor
{
    [MenuItem("SofaUnity/SofaObject/VR/SofaSphereCollisionHand")]
    [MenuItem("GameObject/Create Other/SofaObject/VR/SofaSphereCollisionHand")]
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
            // not under DAGNode, will look for SofaContext
            GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
            if (_contextObject != null)
            {
                // Get Sofa context
                parentDagN = _contextObject.GetComponent<SofaDAGNode>();
            }

            // still null, no SofaContext found
            if (parentDagN == null)
            {
                Debug.LogError("Error3 creating SofaSphereCollisionObject GameObject. No valid SofaDAGNode found as parent of this gameObject or in SofaContext.");
                return null;
            }
            else
            {
                Debug.Log("parentDagN found: " + parentDagN.name);
            }
        }


        selectObj.AddComponent<SofaSphereCollisionHand>();
        SofaDAGNodeManager nodeMgr = parentDagN.m_sofaContext.NodeGraphMgr;
        if (nodeMgr != null)
            nodeMgr.RegisterCustomObject(selectObj, parentDagN);
        else
            Debug.LogError("Error creating SofaSphereCollisionObject. Can't access SofaDAGNodeManager.");

        return selectObj;
    }

    public override void OnInspectorGUI()
    {
        Debug.Log("SofaSphereCollisionObjectEditor::OnInspectorGUI");

        SofaSphereCollisionHand model = (SofaSphereCollisionHand)this.target;
        model.SofaSphereCollision.ParentT = (GameObject)EditorGUILayout.ObjectField("Parent Gameobject to mirror position", model.SofaSphereCollision.ParentT, typeof(GameObject), true);
        model.Radius = EditorGUILayout.Slider("Sphere radius", model.Radius, 0.001f, 10);
        model.SofaSphereCollision.Activated = EditorGUILayout.Toggle("Activate collision", model.SofaSphereCollision.Activated);
        model.SofaSphereCollision.Stiffness = EditorGUILayout.Slider("Contact stiffness", model.SofaSphereCollision.Stiffness, 1, 5000);
        model.SofaSphereCollision.StartOnPlay = EditorGUILayout.Toggle("Start on Play", model.SofaSphereCollision.StartOnPlay);

        int size = Mathf.Max(0, EditorGUILayout.IntField("Size", model.CapsuleColliderList.Count));
        while (size > model.CapsuleColliderList.Count)
        {
            model.CapsuleColliderList.Add(null);
        }
        while (size < model.CapsuleColliderList.Count)
        {
            model.CapsuleColliderList.RemoveAt(model.CapsuleColliderList.Count - 1);
        }
        for (int i = 0; i < model.CapsuleColliderList.Count; i++)
        {
            model.CapsuleColliderList[i] = EditorGUILayout.ObjectField("Capsule " + i.ToString(), model.CapsuleColliderList[i], typeof(GameObject), true) as GameObject;
        }


        EditorGUILayout.LabelField("Number of spheres", model.SofaSphereCollision.NbrSpheres.ToString());

    }
}
