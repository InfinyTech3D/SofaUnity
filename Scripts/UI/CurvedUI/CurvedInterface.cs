using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SofaUnity;

public class CurvedInterface : MonoBehaviour
{
    protected SofaContext m_sofaContext;
    public ScenesManager m_scenes;

    public Image m_CurrentImage;
    public Text[] SelectedEnvironmentText;

    //public enum Environment { LIVER, ORGANS, CADUCEUS }
    //[HideInInspector]
    //public Environment SelectedEnvironment;
    //public Animator EnvironmentImageAnimator;
    
    //public ImageSwitch m_imageSwitcher;
    private int m_environmentCount = 0;
    private string m_currentScene = "";
    private string m_targetScene = "";

    protected bool m_isReady = false;
    protected bool m_working = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_scenes == null)
            return;

        m_scenes.parseScenes();
        m_isReady = true;


        //SelectedEnvironment = Environment.LIVER;
        //m_environmentCount = System.Enum.GetValues(typeof(Environment)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangePlayerStatus(Toggle _toggle)
    {
        if (!m_isReady)
            return;

        if (_toggle.isOn)
        {
            if (m_sofaContext == null)
            {
                Debug.LogWarning("No m_sofaContext found");
                return;
            }

            if (_toggle.gameObject.name.Contains("play"))
            {
                Debug.Log("PLAY");
                m_sofaContext.IsSofaUpdating = true;
            }
            else if (_toggle.gameObject.name.Contains("stop"))
            {
                Debug.Log("STOP");
                m_sofaContext.resetSofa(); 
            }
            else if (_toggle.gameObject.name.Contains("pause"))
            {
                Debug.Log("PAUSE");
                m_sofaContext.IsSofaUpdating = false;
            }
        }
    }

    public void ChangeScene(int _direction)
    {
        if (!m_isReady)
            return;

        //Debug.Log("ChangeEnvironment, direction: " + _direction);

        m_environmentCount += _direction;
        Debug.Log("ChangeEnvironment, m_environmentCount: " + m_environmentCount);

        int nbrScene = m_scenes.getNbrScenes();
        int id = Mathf.Abs(m_environmentCount % nbrScene);
        Debug.Log("ChangeEnvironment, id: " + id + " | nbr scene: " + nbrScene);

        // get info from scene manager
        ScenesManager.SceneMenuInfo info = m_scenes.getSceneInfo(id);

        if (m_CurrentImage)
            m_CurrentImage.sprite = info.m_sceneImage;

        StopAllCoroutines();
        for (int i = 0; i < SelectedEnvironmentText.Length; i++)
            StartCoroutine(ChangeText(SelectedEnvironmentText[i], info.m_sceneInfo));

        m_targetScene = info.m_sceneName;
    }

    public void loadScene()
    {
        if (!m_isReady || m_working)
            return;

        if (m_targetScene.Length == 0 || m_targetScene == m_currentScene)
            return;

        if (m_sofaContext)
            m_sofaContext.IsSofaUpdating = false;

        m_working = true;
        Debug.Log("Load scene: " + m_targetScene);
        StartCoroutine(loadSceneAsync_impl(m_targetScene));
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator ChangeText(Text _text, string _string, float _timeBetweenLetters = 0.05f)
    {
        _text.text = "";
        for (int i = 0; i < _string.Length; i++)
        {
            _text.text += "" + _string[i];
            yield return new WaitForSeconds(_timeBetweenLetters);
        }

        yield return null;
    }


    IEnumerator loadSceneAsync_impl(string levelName)
    {
        yield return null;
        int cptSecu = 0;

        // first unload previous scene
        if (m_currentScene.Length != 0)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(m_currentScene);
            cptSecu = 0;
            while (!asyncUnload.isDone && cptSecu < 10000)
            {
                cptSecu++;
                yield return null;
            }

            m_sofaContext = null;
        }

        // load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        //asyncLoad.allowSceneActivation = false;
        cptSecu = 0;
        while (!asyncLoad.isDone && cptSecu < 10000)
        {
            //if (m_Text) //Output the current progress
            //{
            //    m_Text.text = "Loading progress: " + (asyncLoad.progress * 100) + "%";
            //}
            cptSecu++;
            yield return null;
        }

        m_currentScene = levelName;
        m_working = false;

        // look for new sofaContext
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_sofaContext = _contextObject.GetComponent<SofaContext>();
            if (m_sofaContext == null)
            {
                Debug.LogError("GetComponent<SofaContext> failed.");
            }
        }
        //if (m_Text)
        //{
        //    if (asyncLoad.isDone)
        //        m_Text.text = "Loading success.";
        //    else
        //        m_Text.text = "Loading failed.";
        //}
    }

    IEnumerator unloadSceneAsync_impl()
    {
        yield return null;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(m_currentScene);
        int cptSecu = 0;
        while (!asyncUnload.isDone && cptSecu < 10000)
        {
            cptSecu++;
            yield return null;
        }

        m_currentScene = "";
        m_working = false;
        m_sofaContext = null;
    }
}
