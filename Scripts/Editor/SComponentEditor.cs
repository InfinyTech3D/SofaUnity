using UnityEngine;
using UnityEditor;
using SofaUnity;
using System.Collections.Generic;


/// <summary>
/// Editor Class to define the creation and UI of SofaDeformableMesh GameObject
/// </summary>
[CustomEditor(typeof(SComponentObject), true)]
public class SComponentEditor : Editor
{
    /// <summary>
    /// Method to set the UI of the SofaDeformableMesh GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        SComponentObject _object = (SComponentObject)this.target;

        List<old_SofaData> datas = _object.datas;

        foreach (old_SofaData entry in datas)
        {
            if (entry.getType() == "string")
            {
                EditorGUILayout.TextField(entry.nameID, _object.impl.getStringValue_deprecated(entry.nameID));
            }
            else if (entry.getType() == "bool")
            {
                EditorGUILayout.Toggle(entry.nameID, _object.impl.getBoolValue_deprecated(entry.nameID));
            }
            else if (entry.getType() == "Vec3d" || entry.getType() == "Vec3f")
            {
                EditorGUILayout.Vector3Field(entry.nameID, _object.impl.getVector3fValue_deprecated(entry.nameID));
            }
            else if (entry.getType() == "vector < float >" || entry.getType() == "vector<float>")
            {
                entry.dataSize = _object.impl.getVecfSize_deprecated(entry.nameID);
                EditorGUILayout.TextField(entry.nameID, "vector<float> size " + entry.dataSize);
            }
            else if (entry.getType() == "vector < int >" || entry.getType() == "vector<int>")
            {
                entry.dataSize = _object.impl.getVecfSize_deprecated(entry.nameID);
                EditorGUILayout.TextField(entry.nameID, "vector<int> size " + entry.dataSize);
            }
            else if (entry.getType() == "float")
            {
                EditorGUILayout.FloatField(entry.nameID, _object.impl.getFloatValue_deprecated(entry.nameID));
            }
            else if (entry.getType() == "int")
            {
                EditorGUILayout.FloatField(entry.nameID, _object.impl.getIntValue_deprecated(entry.nameID));
            }
            else if (entry.getType() == "Rigid3dTypes::Coord")
            {
                double[] values = new double[7];
                _object.impl.getRigiddValue_deprecated(entry.nameID, entry.getType(), values);
                EditorGUILayout.Vector3Field(entry.nameID, new Vector3((float)values[0], (float)values[1], (float)values[2]));
                EditorGUILayout.Vector4Field(entry.nameID, new Vector4((float)values[3], (float)values[4], (float)values[5], (float)values[6]));
            }
            else
                EditorGUILayout.TextField(entry.nameID, "Unsopported type: "+ entry.getType());
        }

        //Dictionary<string, string> datas = _object.dataMap;
        //if (datas == null)
        //    return;

        //foreach (KeyValuePair<string, string> entry in datas)
        //{
        //    Debug.Log("DicT: " + entry.Key + " " + entry.Value);
        //    if (entry.Value == "string")
        //    {
        //        EditorGUILayout.TextField(entry.Key); 
        //    }
        //    else if (entry.Value == "bool")
        //    {
        //        EditorGUILayout.Toggle(entry.Key, true);
        //    }
        //}
    }
}

