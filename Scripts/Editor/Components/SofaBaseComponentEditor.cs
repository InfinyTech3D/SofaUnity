using UnityEngine;
using UnityEditor;
using SofaUnity;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(SofaBaseComponent), true)]
public class SofaBaseComponentEditor : Editor
{
    AnimBool m_ShowUnsupportedFields;

    void OnEnable()
    {
        m_ShowUnsupportedFields = new AnimBool(false);
        m_ShowUnsupportedFields.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        SofaBaseComponent compo = (SofaBaseComponent)this.target;
        //if (compo.isAwake() == false)
        //return;
        
        EditorGUI.BeginDisabledGroup(true);
        compo.UniqueNameId = EditorGUILayout.TextField("Unique Name Id", compo.UniqueNameId);
        EditorGUILayout.ObjectField("Sofa Context", compo.m_sofaContext, typeof(Object), true);
        EditorGUILayout.ObjectField("Sofa DAG Node", compo.m_ownerNode, typeof(Object), true);
        EditorGUILayout.EnumPopup("BaseComponentType", compo.m_baseComponentType);
        compo.m_componentType = EditorGUILayout.TextField("Component Type", compo.m_componentType);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();

        compo.m_log = EditorGUILayout.Toggle("Dump logs", compo.m_log);

        EditorGUILayout.Separator();
        SofaDataArchiver dataArchiver = compo.m_dataArchiver;
        if (dataArchiver == null)
            return;

        for (int i = 0; i < dataArchiver.m_names.Count; i++)
        {
            string dataType = dataArchiver.m_types[i];
            string dataName = dataArchiver.m_names[i];

            if (dataType == "string")
            {
                SofaStringData data = dataArchiver.GetSofaStringData(dataName);
                data.Value = EditorGUILayout.TextField(data.DataName, data.Value);
            }
            else if (dataType == "bool")
            {
                SofaBoolData data = dataArchiver.GetSofaBoolData(dataName);
                data.Value = EditorGUILayout.Toggle(data.DataName, data.Value);
            }
            else if (dataType == "int" || dataType == "uint")
            {
                SofaIntData data = dataArchiver.GetSofaIntData(dataName);
                data.Value = EditorGUILayout.IntField(data.DataName, data.Value);                
            }
            else if (dataType == "float")
            {
                SofaFloatData data = dataArchiver.GetSofaFloatData(dataName);
                data.Value = EditorGUILayout.FloatField(data.DataName, data.Value);
            }
            else if (dataType == "double")
            {
                SofaDoubleData data = dataArchiver.GetDoubleIntData(dataName);
                data.Value = EditorGUILayout.FloatField(data.DataName, data.Value);
            }
            else if (dataType == "Vec2f" || dataType == "Vec2d")
            {
                SofaVec2Data data = dataArchiver.GetSofaVec2Data(dataName);
                data.Value = EditorGUILayout.Vector2Field(data.DataName, data.Value);
            }
            else if (dataType == "Vec3f" || dataType == "Vec3d")
            {
                SofaVec3Data data = dataArchiver.GetSofaVec3Data(dataName);
                data.Value = EditorGUILayout.Vector3Field(data.DataName, data.Value);
            }
            else if (dataType == "Vec4f" || dataType == "Vec4d")
            {
                SofaVec4Data data = dataArchiver.GetSofaVec4Data(dataName);
                data.Value = EditorGUILayout.Vector4Field(data.DataName, data.Value);
            }
            else
            {
                SofaData data = dataArchiver.GetGenericData(dataName);
                EditorGUILayout.TextField(data.DataName, "Unsupported type: " + data.DataType);
            }
    
        }

        EditorGUILayout.Separator();
        m_ShowUnsupportedFields.target = EditorGUILayout.ToggleLeft("Show unsupported Data", m_ShowUnsupportedFields.target);
        if (EditorGUILayout.BeginFadeGroup(m_ShowUnsupportedFields.faded))
        {
            for (int i = 0; i < dataArchiver.m_otherNames.Count; i++)
            {
                string dataName = dataArchiver.m_otherNames[i];
                SofaData data = dataArchiver.GetGenericData(dataName);
                EditorGUILayout.TextField(data.DataName, "Unsupported type: " + data.DataType);


            }
        }
        EditorGUILayout.Separator();
        EditorGUILayout.EndFadeGroup();
    }
}
