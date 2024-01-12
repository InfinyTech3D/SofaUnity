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
    /// <returns>Pointer to the SofaRigidPlane GameObject</returns>
    [MenuItem("SofaUnity/Test/ComponentDataTest")]
    [MenuItem("GameObject/Create Other/Test/ComponentDataTest")]
    public static GameObject CreateNew()
    {
        GameObject go = new GameObject("ComponentDataTest");
        ComponentDataTest compo = go.AddComponent<ComponentDataTest>();
        compo.initData();
        return go;
    }

    /// <summary>
    /// Method to set the UI of the SofaRigidPlane GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        ComponentDataTest _object = (ComponentDataTest)this.target;
        //_object.data1.Value = EditorGUILayout.FloatField(_object.data1.DataName, _object.data1.Value);
        //_object.data2.Value = EditorGUILayout.Vector3Field(_object.data2.DataName, _object.data2.Value);

        //    // call SofaRigidGrid and SofaRigidMesh UI creation
        //    base.OnInspectorGUI();

    }
}



