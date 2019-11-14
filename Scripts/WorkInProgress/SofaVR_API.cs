using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaVR_API : MonoBehaviour
{
    public CurvedInterface m_curvedUI = null;
    public ScenesManager m_scenes = null;
    public LoadSceneScript m_loader = null;

    protected SofaContext m_sofaContext = null;
    
    protected bool m_isReady = false;
    protected bool m_working = false;

    protected bool m_loading = false;
    protected int m_currentSceneId = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (m_scenes == null || m_curvedUI == null || m_loader == null)
            return;

        m_scenes.parseScenes();
        m_curvedUI.initUI(this, m_scenes);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_loading && m_loader != null) // wait for new loaded scene
        {
            if (!m_loader.isLoading())
            {
                // do stuff here to update everyone from new sofa scene
                OnSofaSceneLoaded();
                m_loading = false;
            }
        }
    }



    public void loadSofaScene(int sceneID)
    {
        m_currentSceneId = sceneID;
        string sceneName = m_scenes.getSceneName(m_currentSceneId);

        // check valid scene
        if (sceneName.Length == 0)
        {
            Debug.LogError("Scene error not found in sceneManager.");
            return;
        }

        if (m_loader == null)
        {
            Debug.LogError("No LoadSceneScript created.");
            return;
        }

        // stop current sofa application and remove pointer
        if (m_sofaContext)
        {
            m_sofaContext.IsSofaUpdating = false;
            m_sofaContext = null;
        }

        // Set loading info
        m_loading = true;    
        
        Debug.Log("Load scene: " + sceneName);
        m_loader.loadSofaScene(sceneName);
    }

    public void startSofaSimulation()
    {
        if (m_sofaContext != null)
        {
            Debug.Log("startSofaSimulation");
            m_sofaContext.IsSofaUpdating = true;
        }
    }

    public void stopSofaSimulation()
    {        
        if (m_sofaContext != null)
        {
            Debug.Log("stopSofaSimulation");
            m_sofaContext.IsSofaUpdating = false;
        }
    }

    public void restartSofaSimulation()
    {        
        if (m_sofaContext != null)
        {
            Debug.Log("restartSofaSimulation");
            m_sofaContext.resetSofa();
        }
    }


    protected void OnSofaSceneLoaded()
    {
        // normally should enable sofa player here in real gui.

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

        // update actions here.
    }
}
