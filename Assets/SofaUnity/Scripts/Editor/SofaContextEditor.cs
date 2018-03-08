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
    private static Texture2D m_sofaLogo;
    public static Texture2D SofaLogo
    {
        get
        {
            if (m_sofaLogo == null)
            {
                Object logo = Resources.Load("sofa_logo");
                m_sofaLogo = (Texture2D)logo;
            }
            return m_sofaLogo;
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
}
