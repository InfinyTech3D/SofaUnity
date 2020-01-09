using UnityEditor;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Editor Class to define the creation and UI of SofaContext GameObject
/// </summary>
[CustomEditor(typeof(SofaContext))]
public class SofaContextEditor : Editor
{
    /// <summary>
    ///  Add SofaContext Object creation to the SofaUnity Menu
    /// </summary>
    /// <returns>Pointer to the SofaContext GameObject</returns>
    [MenuItem("SofaUnity/SofaContext")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaContext")]  //right click menu
    public static GameObject CreateNew()
    {
        if (GameObject.FindObjectOfType<SofaContext>() != null)
        {
            Debug.LogError("The Scene already includes a SofaContext. Only one context is possible.");
            return null;
        }
        GameObject go = new GameObject("SofaContext");
        go.AddComponent<SofaContext>();

        return go;
    }

    /// <summary>
    ///  Create Sofa logo for the Editor Menu
    /// </summary>
    private static Texture2D m_SofaLogo;
    public static Texture2D SofaLogo
    {
        get
        {
            if (m_SofaLogo == null)
            {
                Object logo = Resources.Load("sofa_logo");
                m_SofaLogo = (Texture2D)logo;
            }
            return m_SofaLogo;
        }
    }

    /// <summary>
    /// Method to set the UI of the SofaContext GameObject
    /// </summary>
    public override void OnInspectorGUI()
    {
        SofaContext context = (SofaContext)this.target;

        // Add Sofa Logo
        GUIStyle logoGUIStyle = new GUIStyle();
        logoGUIStyle.border = new RectOffset(0, 0, 0, 0);
        EditorGUILayout.LabelField(new GUIContent(SofaLogo), GUILayout.MinHeight(100.0f), GUILayout.ExpandWidth(true));

        // Add field for gravity
        context.gravity = EditorGUILayout.Vector3Field("Gravity", context.gravity);
        EditorGUILayout.Separator();

        // Add field for timestep
        context.timeStep = EditorGUILayout.FloatField("TimeStep", context.timeStep);
        EditorGUILayout.Separator();

        // Add field for simulation
        context.IsSofaUpdating = EditorGUILayout.Toggle("Activate Simulation", context.IsSofaUpdating);
        context.CatchSofaMessages = EditorGUILayout.Toggle("Activate Sofa Logs", context.CatchSofaMessages);
        context.StartOnPlay = EditorGUILayout.Toggle("Start Sofa on Play", context.StartOnPlay);
        context.StepbyStep = EditorGUILayout.Toggle("StepByStep", context.StepbyStep);

        // Add pluigin section
        PluginSection(context);


        if (GUILayout.Button("Step"))
        {
            context.StepbyStep = true;
            context.IsSofaUpdating = true;
        }

        // Add Button to load a filename
        if (GUILayout.Button("Load Scene"))
        {
            string absolutePath = EditorUtility.OpenFilePanel("Load file scene (*.scn)", "", "scn");
            context.filename = absolutePath.Substring(Application.dataPath.Length);
            EditorGUILayout.Separator();
        }
        // Label of the filename loaded
        EditorGUILayout.LabelField("filename", context.filename);

        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(context);
        }
    }

    void PluginSection(SofaContext context)
    {
        EditorGUILayout.Separator();
        EditorGUI.BeginDisabledGroup(true);

        if (context.PluginManager == null)
        {
            EditorGUILayout.IntField("Plugins Count", 0);
            return;
        }

        int nbrPlugin = EditorGUILayout.IntField("Plugins Count", context.PluginManager.NbrPlugin);
        context.PluginManager.NbrPlugin = nbrPlugin;
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < nbrPlugin; i++)
        {
            string pluginName = EditorGUILayout.TextField("Plugin Name: ", context.PluginManager.GetPluginName(i));
            context.PluginManager.SetPluginName(i, pluginName);
        }
        EditorGUI.indentLevel -= 1;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Separator();
    }

}
