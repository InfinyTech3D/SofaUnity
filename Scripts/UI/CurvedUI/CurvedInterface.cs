using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SofaUnity;

public class CurvedInterface : MonoBehaviour
{                
    public Image m_CurrentImage;
    public Text[] SelectedEnvironmentText;
    public ToggleGroup playerButtons;

    //public ImageSwitch m_imageSwitcher;
    private int m_environmentCount = 0;
    private int m_targetSceneId = -1;
    private int m_currentSceneId = -1;

    protected bool m_isReady = false;
    protected SofaVR_API m_sofaVR_API = null;
    protected ScenesManager m_scenes = null;
    
    public void initUI(SofaVR_API api, ScenesManager _scenes)
    {
        m_sofaVR_API = api;
        m_scenes = _scenes;
        if (m_scenes == null || m_sofaVR_API == null)
            m_isReady = false;
        else
            m_isReady = true;

        if (m_scenes.getNbrScenes() > 0)
            ChangeScene(0);
    }
    

    public void ChangePlayerStatus(Toggle _toggle)
    {
        if (!m_isReady)
            return;

        if (_toggle.isOn)
        {
            if (m_sofaVR_API == null)
            {
                Debug.LogWarning("No m_sofaVR_API found");
                return;
            }

            if (_toggle.gameObject.name.Contains("play"))
            {
                m_sofaVR_API.startSofaSimulation();
            }
            else if (_toggle.gameObject.name.Contains("stop"))
            {
                m_sofaVR_API.restartSofaSimulation();
            }
            else if (_toggle.gameObject.name.Contains("pause"))
            {
                m_sofaVR_API.stopSofaSimulation();
            }
        }
    }

    public void ChangeScene(int _direction)
    {
        if (!m_isReady)
            return;

        //Debug.Log("ChangeEnvironment, direction: " + _direction);

        m_environmentCount += _direction;
        //Debug.Log("ChangeEnvironment, m_environmentCount: " + m_environmentCount);

        int nbrScene = m_scenes.getNbrScenes();
        m_targetSceneId = Mathf.Abs(m_environmentCount % nbrScene);
        //Debug.Log("ChangeEnvironment, id: " + m_targetSceneId + " | nbr scene: " + nbrScene);

        // get info from scene manager
        ScenesManager.SceneMenuInfo info = m_scenes.getSceneInfo(m_targetSceneId);

        if (m_CurrentImage)
            m_CurrentImage.sprite = info.m_sceneImage;

        StopAllCoroutines();
        for (int i = 0; i < SelectedEnvironmentText.Length; i++)
            StartCoroutine(ChangeText(SelectedEnvironmentText[i], info.m_sceneInfo));
    }

    public void loadScene()
    {
        if (!m_isReady)
            return;

        if (m_targetSceneId == -1 || m_targetSceneId == m_currentSceneId)
            return;
        
        m_sofaVR_API.loadSofaScene(m_targetSceneId);

        playerButtons.SetAllTogglesOff();
        m_currentSceneId = m_targetSceneId;
    }


    public void resetSofaView()
    {
        if (!m_isReady)
            return;

        m_sofaVR_API.resetSofaView();
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
