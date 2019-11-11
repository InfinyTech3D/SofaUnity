using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private string m_currentScene;

    bool m_isReady = false;

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

        m_currentScene = info.m_sceneName;
    }

    public void loadScene()
    {
        if (!m_isReady)
            return;

        if (m_currentScene.Length == 0)
            return;

        Debug.Log("Load scene: " + m_currentScene);
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
}
