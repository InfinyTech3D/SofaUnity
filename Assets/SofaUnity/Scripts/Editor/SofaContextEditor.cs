using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using SofaUnity;

[CustomEditor(typeof(SofaContext))]
public class SofaContextEditor : Editor
{
    [MenuItem("SofaUnity/SofaContext")]
    [MenuItem("GameObject/Create Other/SofaUnity/SofaContext")]  //right click menu
    public static GameObject CreateNew()
    {
        if (GameObject.FindObjectOfType<SofaContext>() != null)
        {
            Debug.LogError("The Scene already includes a SofaContext. Only one context is possible.");
            return null;
        }
        GameObject go = new GameObject();
        go.AddComponent<SofaContext>();
        go.name = "SofaContext";
        return go;
    }

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

    public override void OnInspectorGUI()
    {
        SofaContext context = (SofaContext)this.target;

        // Add Sofa Logo
        GUIStyle logoGUIStyle = new GUIStyle();
        logoGUIStyle.border = new RectOffset(0, 0, 0, 0);
        EditorGUILayout.LabelField(new GUIContent(SofaLogo), GUILayout.MinHeight(100.0f), GUILayout.ExpandWidth(true));

        //Color GUIBlue = new Color32(192, 219, 255, 255);

        context.gravity = EditorGUILayout.Vector3Field("Gravity", context.gravity);
        EditorGUILayout.Separator();

        context.timeStep = EditorGUILayout.FloatField("TimeStep", context.timeStep);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Load Scene"))
        {
            context.filename = EditorUtility.OpenFilePanel("Load file scene (*.scn)", "", "scn");
            //context.filename = "C:/projects/sofa-dev/examples/Demos/TriangleSurfaceCutting.scn";
            EditorGUILayout.Separator();
        }

        EditorGUILayout.LabelField("filename", context.filename);



        if (GUI.changed)
        {
            EditorUtility.SetDirty(context);
            //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            //Undo.RecordObject(pw, "Undo Physics World");
        }
    }
}
