using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

[CustomEditor(typeof(ComponentDataTest), true)]
public class DataEditor : Editor
{
    /// <summary>
    ///  Add ComponentDataTest Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SRigidPlane GameObject</returns>
    [MenuItem("SofaUnity/ComponentDataTest")]
    [MenuItem("GameObject/Create Other/SofaUnity/ComponentDataTest")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("ComponentDataTest");
        ComponentDataTest compo = go.AddComponent<ComponentDataTest>();
        compo.initData();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SRigidPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        ComponentDataTest _object = (ComponentDataTest)this.target;
        _object.data1.Value = EditorGUILayout.FloatField(_object.data1.DataName, _object.data1.Value);
        _object.data2.Value = EditorGUILayout.Vector3Field(_object.data2.DataName, _object.data2.Value);

        //    // call SRigidGrid and SRigidMesh UI creation
        //    base.OnInspectorGUI();

    }
}

[CustomEditor(typeof(SofaDataFloat), true)]
public class SofaDataFloatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Debug.Log("SofaDataFloatEditor OnInspectorGUI");
        // call SRigidGrid and SRigidMesh UI creation
        //base.OnInspectorGUI();

    }
}


