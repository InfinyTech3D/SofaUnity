using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaVR_API : MonoBehaviour
{
    public CurvedInterface m_curvedUI = null;
    public ScenesManager m_scenes = null;
    public LoadSceneScript m_loader = null;
    public SofaViewController m_viewCtrl = null;

    public GameObject m_rightHand = null;
    public GameObject m_leftHand = null;

    protected VRHandController m_rightHandCtrl = null;
    protected VRHandController m_leftHandCtrl = null;
    protected SLaserRay m_rightRayCaster = null;
    protected SLaserRay m_leftRayCaster = null;

    protected SofaContext m_sofaContext = null;
    
    protected bool m_isReady = false;
    protected bool m_working = false;

    protected bool m_loading = false;
    protected int m_currentSceneId = -1;

    protected bool m_VRControlMode = false;

    protected bool m_viewMode = false;
    protected bool m_rightCtrlActivated = false;
    protected bool m_leftCtrlActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_scenes == null || m_curvedUI == null || m_loader == null)
            return;

        if (m_rightHand != null && m_leftHand != null)
        {
            m_rightHandCtrl = m_rightHand.GetComponent<VRHandController>();
            m_rightRayCaster = m_rightHand.GetComponent<SLaserRay>();

            m_leftHandCtrl = m_leftHand.GetComponent<VRHandController>();
            m_leftRayCaster = m_leftHand.GetComponent<SLaserRay>();

            if (m_rightHandCtrl == null || m_rightRayCaster == null)
            {
                Debug.LogError("Problem with right hand");
                m_VRControlMode = false;
            }
            else if (m_leftHandCtrl == null || m_leftRayCaster == null)
            {
                Debug.LogError("Problem with left hand");
                m_VRControlMode = false;
            }
            else
                m_VRControlMode = true;
        }

        m_scenes.parseScenes();
        m_curvedUI.initUI(this, m_scenes);
    }

    // Update is called once per frame
    void Update()
    {
        // check if loading
        if (m_loading && m_loader != null) // wait for new loaded scene
        {
            if (!m_loader.isLoading())
            {
                // do stuff here to update everyone from new sofa scene
                m_loading = false;
                OnSofaSceneLoaded();                
            }
        }

        if (!m_VRControlMode || m_sofaContext == null)
            return;

        // check if controller view
        bool gripR = m_rightHandCtrl.isGripPressed();
        bool gripL = m_leftHandCtrl.isGripPressed();

        bool trigR = m_rightHandCtrl.isTriggerPressed();
        bool trigL = m_leftHandCtrl.isTriggerPressed();

        // check view mode first
        if (m_viewCtrl)
        {
            if (gripR && gripL)
            {
                if (!m_viewMode) // first time
                {
                    m_viewCtrl.activeInteraction(SofaViewController.MoveMode.ALL);
                    m_viewMode = true;
                }
            }
            else if (m_viewMode) // unactive
            {
                m_viewCtrl.activeInteraction(SofaViewController.MoveMode.FIX);
                m_viewMode = false;
            }
        }
        
        // in view mode no ray casting
        if (m_viewMode)
        {
            if (m_rightCtrlActivated)
            {
                m_rightRayCaster.activeTool(false);
                m_rightCtrlActivated = false;
            }

            if (m_leftCtrlActivated)
            {
                m_leftRayCaster.activeTool(false);
                m_leftCtrlActivated = false;
            }

            return;
        }

        // check ray casting interaction
        if (trigR)
        {
            m_rightRayCaster.activeTool(true);
            m_rightCtrlActivated = true;
        }
        else if (m_rightCtrlActivated)
        {
            m_rightRayCaster.activeTool(false);
            m_rightCtrlActivated = false;
        }


        //bool gripR = m_rightHandCtrl.isGripPressed();
        //bool gripR = m_rightHandCtrl.isGripPressed();
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
        
        if (m_viewCtrl != null)
        {
            m_viewCtrl.unloadSofaScene();
        }

        //if (m_leftRayCaster != null)
        //{
        //    m_leftRayCaster.unloadSofaRayCaster();
        //}

        if (m_rightRayCaster != null)
        {
            m_rightRayCaster.unloadSofaRayCaster();
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
        Debug.Log("OnSofaSceneLoaded: " + this.name);
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

        if (m_viewCtrl != null)
        {
            m_viewCtrl.setSofaContext(_contextObject);
        }

        //if (m_leftRayCaster != null)
        //{
        //    m_leftRayCaster.startSofaRayCaster(m_sofaContext);
        //}

        if (m_rightRayCaster != null)
        {
            m_rightRayCaster.startSofaRayCaster(m_sofaContext);
        }
        // update actions here.
    }
}
