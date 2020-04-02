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
        int cpt = 0;
        if (GameObject.FindObjectOfType<SofaContext>() != null)
        {
            Debug.LogWarning("The Scene already includes a SofaContext. Only one context is possible for the moment.");
            return null;
            cpt++;
        }
        GameObject go = new GameObject("SofaContext_" + cpt.ToString());
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
        context.Gravity = EditorGUILayout.Vector3Field("Gravity", context.Gravity);
        EditorGUILayout.Separator();

        // Add field for timestep
        context.TimeStep = EditorGUILayout.FloatField("TimeStep", context.TimeStep);
        EditorGUILayout.Separator();

        {
            context.CatchSofaMessages = EditorGUILayout.Toggle("Activate Sofa Logs", context.CatchSofaMessages);
            context.IsSofaUpdating = EditorGUILayout.Toggle("Animate SOFA simulation", context.IsSofaUpdating);
            EditorGUILayout.Separator();

            if (GUILayout.Button("Step"))
            {
                context.StepbyStep = true;
                context.IsSofaUpdating = true;
            }
        }

        
        // Add field for simulation
        EditorGUI.BeginDisabledGroup(true);
        
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Separator();        
        EditorGUILayout.Separator();


        // Add plugin section
        PluginSection(context);
        EditorGUILayout.Separator();

        // Add scene file section
        SceneFileSection(context);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(context);
        }
    }


    void PluginSection(SofaContext context)
    {
        EditorGUILayout.Separator();
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
        EditorGUILayout.Separator();
    }


    void SceneFileSection(SofaContext context)
    {
        EditorGUILayout.Separator();
        if (context.SceneFileMgr == null)
        {
            EditorGUILayout.LabelField("No scene file manager available");
            return;
        }

        // Add Button to load a filename
        if (GUILayout.Button("Load SOFA Scene (.scn) file"))
        {
            string absolutePath = EditorUtility.OpenFilePanel("Load file scene (*.scn)", "", "scn");
            context.SceneFileMgr.SceneFilename = absolutePath.Substring(Application.dataPath.Length);
            EditorGUILayout.Separator();
        }
        // Label of the filename loaded
        EditorGUILayout.LabelField("Scene Filename: ", context.SceneFileMgr.SceneFilename);

        context.UnLoadScene = GUILayout.Button("Unload Scene file");
        EditorGUILayout.Separator();
    }
    
}
