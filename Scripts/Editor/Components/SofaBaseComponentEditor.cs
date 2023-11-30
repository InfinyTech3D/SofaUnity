using UnityEngine;
using UnityEditor;
using SofaUnity;
using UnityEditor.AnimatedValues;

/// <summary>
/// Editor class corresponding to @sa SofaBaseComponent
/// Provide interface for all component inheriting from @sa SofaBaseComponent which will display all Data.
/// Not all Data types are shown, for example vectors. Option to show unsupported Data is availble.
/// </summary>
[CustomEditor(typeof(SofaBaseComponent), true)]
public class SofaBaseComponentEditor : Editor
{
    AnimBool m_ShowUnsupportedFields;
    protected bool m_showData = true;

    /// Callback method to show or not unsupported Data
    void OnEnable()
    {
        m_ShowUnsupportedFields = new AnimBool(false);
        m_ShowUnsupportedFields.valueChanged.AddListener(Repaint);
    }

    /// Method to create parameters GUI
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
        if (dataArchiver == null || !m_showData)
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
            else if (dataType == "i" || dataType == "uint")
            {
                SofaIntData data = dataArchiver.GetSofaIntData(dataName);
                data.Value = EditorGUILayout.IntField(data.DataName, data.Value);                
            }
            else if (dataType == "f")
            {
                SofaFloatData data = dataArchiver.GetSofaFloatData(dataName);
                data.Value = EditorGUILayout.FloatField(data.DataName, data.Value);
            }
            else if (dataType == "d")
            {
                SofaDoubleData data = dataArchiver.GetSofaDoubleData(dataName);
                data.Value = EditorGUILayout.FloatField(data.DataName, data.Value);
            }
            else if (dataType == "Vec2i")
            {
                SofaVec2IntData data = dataArchiver.GetSofaVec2IntData(dataName);
                data.Value = EditorGUILayout.Vector2IntField(data.DataName, data.Value);
            }
            else if (dataType == "Vec2f" || dataType == "Vec2d")
            {
                SofaVec2Data data = dataArchiver.GetSofaVec2Data(dataName);
                data.Value = EditorGUILayout.Vector2Field(data.DataName, data.Value);
            }
            else if (dataType == "Vec3i")
            {
                SofaVec3IntData data = dataArchiver.GetSofaVec3IntData(dataName);
                data.Value = EditorGUILayout.Vector3IntField(data.DataName, data.Value);
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
                if (data != null)
                    EditorGUILayout.TextField(data.DataName, "Unsupported type: " + data.DataType);
            }
    
        }

        // Add the links
        SofaLinkArchiver linkArchiver = compo.m_linkArchiver;
        if (linkArchiver != null && linkArchiver.m_links != null)
        {
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(true);
            foreach (SofaLink link in linkArchiver.m_links)
            {
                EditorGUILayout.TextField("@" + link.LinkName, link.LinkPath);
            }
            EditorGUI.EndDisabledGroup();
        }


        if (dataArchiver.m_otherNames.Count > 0)
        {
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
}
