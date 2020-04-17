using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;

/// <summary>
/// Editor class corresponding to @sa SofaMass
/// This class inherite from @sa SofaBaseComponentEditor and will add specific data after the Data display
/// WIP tests
/// </summary>
[CustomEditor(typeof(SofaMass), true)]
public class SofaMassEditor : SofaBaseComponentEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SofaMass _object = (SofaMass)this.target;
        EditorGUILayout.ObjectField("Sofa Context", _object.m_sofaContext, typeof(Object), true);
        //int[] m_VectorProp = new int[10];
        
        //SerializedProperty m_VectorProp = serializedObject.FindProperty("myTab");
        //EditorGUILayout.PropertyField(m_VectorProp, true);
    }
    //    Debug.Log("SofaMassEditor OnInspectorGUI");
    //    SofaMass _object = (SofaMass)this.target;

    //    List<SofaData> datas = _object.datas;

    //    foreach (SofaData entry in datas)
    //    {
    //        if (entry.getType() == "string")
    //        {
    //            EditorGUILayout.TextField(entry.nameID, _object.m_impl.getStringValue(entry.nameID));
    //        }
    //        else if (entry.getType() == "bool")
    //        {
    //            EditorGUILayout.Toggle(entry.nameID, _object.m_impl.GetBoolValue(entry.nameID));
    //        }
    //        else if (entry.getType() == "Vec3d" || entry.getType() == "Vec3f")
    //        {
    //            EditorGUILayout.Vector3Field(entry.nameID, _object.m_impl.GetVector3fValue(entry.nameID));
    //        }
    //        else if (entry.getType() == "vector < float >" || entry.getType() == "vector<float>")
    //        {
    //            entry.dataSize = _object.m_impl.GetVecfSize(entry.nameID);
    //            EditorGUILayout.TextField(entry.nameID, "vector<float> size " + entry.dataSize);
    //        }
    //        else if (entry.getType() == "vector < int >" || entry.getType() == "vector<int>")
    //        {
    //            entry.dataSize = _object.m_impl.GetVecfSize(entry.nameID);
    //            EditorGUILayout.TextField(entry.nameID, "vector<int> size " + entry.dataSize);
    //        }
    //        else if (entry.getType() == "float")
    //        {
    //            EditorGUILayout.FloatField(entry.nameID, _object.m_impl.GetFloatValue(entry.nameID));
    //        }
    //        else if (entry.getType() == "double")
    //        {
    //            EditorGUILayout.FloatField(entry.nameID, _object.m_impl.GetDoubleValue(entry.nameID));
    //        }
    //        else if (entry.getType() == "int")
    //        {
    //            EditorGUILayout.FloatField(entry.nameID, _object.m_impl.GetIntValue(entry.nameID));
    //        }
    //        //else if (entry.getType() == "Rigid3dTypes::Coord")
    //        //{
    //        //    double[] values = new double[7];
    //        //    _object.m_impl.GetRigidfValue(entry.nameID, entry.getType(), values);
    //        //    EditorGUILayout.Vector3Field(entry.nameID, new Vector3((float)values[0], (float)values[1], (float)values[2]));
    //        //    EditorGUILayout.Vector4Field(entry.nameID, new Vector4((float)values[3], (float)values[4], (float)values[5], (float)values[6]));
    //        //}
    //        else
    //            EditorGUILayout.TextField(entry.nameID, "Unsopported type: " + entry.getType());
    //    }
    //}
}
