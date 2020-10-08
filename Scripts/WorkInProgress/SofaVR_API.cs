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
    public SceneInfo m_sceneInfo = null;

    public GameObject m_rightHand = null;
    public GameObject m_leftHand = null;

    public GameObject m_rightTooltip = null;
    public GameObject m_leftTooltip = null;

    public SofaSphereCollisionObject m_rightCollisionModel = null;
    public SofaSphereCollisionObject m_leftCollisionModel = null;

    protected HandlerController m_rightHandCtrl = null;
    protected HandlerController m_leftHandCtrl = null;
    protected SofaLaserModel m_rightRayCaster = null;
    protected SofaLaserModel m_leftRayCaster = null;

    protected SofaContext m_sofaContext = null;
    
    protected bool m_isReady = false;
    protected bool m_working = false;

    protected bool m_loading = false;
    protected int m_currentSceneId = -1;

    protected bool m_VRControlMode = false;

    protected bool m_viewMode = false;
    protected bool m_rightCtrlActivated = false;
    protected bool m_leftCtrlActivated = false;

    protected bool m_showToolTip = true;
    protected bool m_showFirstToolTip = true;

    // Start is called before the first frame update
    void Start()
    {
        if (m_scenes == null || m_curvedUI == null || m_loader == null)
            return;

        if (m_rightHand != null && m_leftHand != null)
        {
            m_rightHandCtrl = m_rightHand.GetComponent<HandlerController>();
            m_rightRayCaster = m_rightHand.GetComponent<SofaLaserModel>();

            m_leftHandCtrl = m_leftHand.GetComponent<HandlerController>();
            m_leftRayCaster = m_leftHand.GetComponent<SofaLaserModel>();

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

        showToolTip(true, true);
        m_showFirstToolTip = true;
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

        if (!m_VRControlMode)
            return;

        // show tooltip
        bool buttonTT = m_rightHandCtrl.isButtonTwoPressed();
        showToolTip(buttonTT);

        if (m_sofaContext == null)
            return;

        showWireframe(m_leftHandCtrl.isButtonTwoPressed());

        // check if controller view
        bool gripR = m_rightHandCtrl.isGripPressed();
        bool trigR = m_rightHandCtrl.isTriggerPressed();

        bool gripL = m_leftHandCtrl.isGripPressed();
        bool trigL = m_leftHandCtrl.isTriggerPressed();

        // check view mode first
        if (m_viewCtrl)
        {
            handleViewController(gripR, gripL);
        }

        // handle right tool
        if (m_viewCtrl)
        {
            handleRightController(gripR, trigR);
        }

        // handle left tool
        if (m_viewCtrl)
        {
            handleLeftController(gripL, trigL);
        }
    }


    protected void handleViewController(bool gripR, bool gripL)
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

    protected void handleRightController(bool gripR, bool trigR)
    {
        if (m_viewMode)
        {
            Debug.Log("handleRightController Off");
            if (m_rightCtrlActivated) // was activated and now view mode
            {
                m_rightCollisionModel.Activated = false;
                m_rightRayCaster.ActivateTool = false;
                m_rightCtrlActivated = false;
            }
            return;
        }


        if (m_rightCollisionModel)
        {
            if (gripR)
                m_rightCollisionModel.Activated = true;
            else
                m_rightCollisionModel.Activated = false;
        }

        if (m_rightRayCaster.RayInteractionType == SofaDefines.SRayInteraction.AttachTool) // need trigger
        {
            if (trigR)
            {
                m_rightRayCaster.ActivateTool = true;
                m_rightCtrlActivated = true;
            }
            else if (m_rightCtrlActivated)
            {
                m_rightRayCaster.ActivateTool = false;
                m_rightCtrlActivated = false;
            }
        }
        else if (m_rightRayCaster.RayInteractionType == SofaDefines.SRayInteraction.CuttingTool) // need grip
        {
            if (trigR)
            {
                m_rightRayCaster.ActivateTool = true;
                m_rightCtrlActivated = true;
            }
            else if (m_rightCtrlActivated)
            {
                m_rightRayCaster.ActivateTool = false;
                m_rightCtrlActivated = false;
            }
        }
        else if (m_rightRayCaster.RayInteractionType == SofaDefines.SRayInteraction.FixTool) // not yet
        {

        }
    }

    protected void handleLeftController(bool gripL, bool trigL)
    {
        if (m_viewMode)
        {
            if (m_leftCtrlActivated) // was activated and now view mode
            {
                m_leftCollisionModel.Activated = false;
                m_leftRayCaster.ActivateTool = false;
                m_leftCtrlActivated = false;
            }
            return;
        }

        if (m_leftCollisionModel)
        {
            if (gripL)
                m_leftCollisionModel.Activated = true;
            else
                m_leftCollisionModel.Activated = false;
        }


        if (m_leftRayCaster.RayInteractionType == SofaDefines.SRayInteraction.AttachTool) // need trigger
        {
            if (trigL)
            {
                m_leftRayCaster.ActivateTool = true;
                m_leftCtrlActivated = true;
            }
            else if (m_leftCtrlActivated)
            {
                m_leftRayCaster.ActivateTool = false;
                m_leftCtrlActivated = false;
            }
        }
        else if (m_leftRayCaster.RayInteractionType == SofaDefines.SRayInteraction.CuttingTool) // need grip
        {
            if (trigL)
            {
                m_leftRayCaster.ActivateTool = true;
                m_leftCtrlActivated = true;
            }
            else if (m_leftCtrlActivated)
            {
                m_leftRayCaster.ActivateTool = false;
                m_leftCtrlActivated = false;
            }
        }
        else if (m_leftRayCaster.RayInteractionType == SofaDefines.SRayInteraction.FixTool) // not yet
        {

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
        
        if (m_viewCtrl != null)
        {
            m_viewCtrl.unloadSofaScene();
        }

        if (m_leftRayCaster != null)
        {
            m_leftRayCaster.UnloadSofaRayCaster();
        }

        if (m_rightRayCaster != null)
        {
            m_rightRayCaster.UnloadSofaRayCaster();
        }

        if (m_sceneInfo != null)
        {
            m_sceneInfo.UnloadSofaContext();
        }

        // stop current sofa application and remove pointer
        if (m_sofaContext)
        {
            m_sofaContext.IsSofaUpdating = false;
            m_sofaContext = null;
        }

        if (sceneName.Contains("xray") || sceneName.Contains("cadu"))
            return;

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
            m_sofaContext.ResetSofa();
        }
    }

    public void resetSofaView()
    {
        Debug.Log("resetSofaView");
        if (m_viewCtrl)
            m_viewCtrl.resetSofaView();
    }


    protected void OnSofaSceneLoaded()
    {
        // normally should enable sofa player here in real gui.
        //Debug.Log("OnSofaSceneLoaded: " + this.name);
        
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

        // set sofaContext to viewCtrl
        if (m_viewCtrl != null)
        {
            m_viewCtrl.setSofaContext(_contextObject);
        }

        // set sofaContext to collision model
        //if (m_rightCollisionModel != null)
        //    m_rightCollisionModel.setSofaContext(m_sofaContext);

        //if (m_leftCollisionModel != null)
        //    m_leftCollisionModel.setSofaContext(m_sofaContext);

        // update actions here.
        ScenesManager.SceneMenuInfo sceneInfo = m_scenes.getSceneInfo(m_currentSceneId);        
        if (m_leftRayCaster != null)
        {
            m_leftRayCaster.RayInteractionType = sceneInfo.m_leftToolType;
            m_leftRayCaster.LoadSofaRayCaster(m_sofaContext);
        }

        if (m_rightRayCaster != null)
        {
            m_rightRayCaster.RayInteractionType = sceneInfo.m_rightToolType;
            m_rightRayCaster.LoadSofaRayCaster(m_sofaContext);
        }

        // update scene info here
        if (m_sceneInfo != null)
        {
            m_sceneInfo.SetSofaContext(m_sofaContext);
        }

        // update tooltip
        showToolTip(true, true);
        m_showFirstToolTip = true;

    }

    protected void updateToolTip()
    {

    }

    protected void showToolTip(bool value, bool force = false)
    {
        //Debug.Log("showToolTip: " + value + " | m_showToolTip: " + m_showToolTip);
        if (force)
        {
            if (m_rightTooltip != null)
                m_rightTooltip.SetActive(value);

            if (m_leftTooltip != null)
                m_leftTooltip.SetActive(value);

            m_showToolTip = value;
            return;
        }

        if (m_showFirstToolTip)// first time
        {
            if (value)
            {
                Debug.Log("First time");
                m_showFirstToolTip = false;
            }
            else
                return;
        }

        if (value == m_showToolTip)
            return;

        if (m_rightTooltip != null)
            m_rightTooltip.SetActive(value);

        if (m_leftTooltip != null)
            m_leftTooltip.SetActive(value);

        m_showToolTip = value;
    }

    protected bool firstWireClick = false;
    protected void showWireframe(bool value)
    {
        if (m_sofaContext == null)
            return;

        if (value == false) // not click
        {
            firstWireClick = true;
            return;
        }

        if (!firstWireClick)
            return;

        // only one object would be better...
        foreach (Transform child in m_sofaContext.transform)
        {
            showWireFrameChilds(child, value);
            // TODO: restore that
            //SofaVisualMesh obj = child.GetComponent<SofaVisualMesh>();

            
            //if (obj != null)
            //{
            //    if (obj.m_isSelected)
            //        obj.ShowWireframe(!obj.m_isWireframe);
            //}
        }

        firstWireClick = false;
    }

    protected void showWireFrameChilds(Transform parent, bool value)
    {
        foreach (Transform child in parent)
        {
            showWireFrameChilds(child, value);

            // TODO: restore that
            //SofaVisualMesh obj = child.GetComponent<SofaVisualMesh>();
            //if (obj != null)
            //{
            //    if (obj.m_isSelected)
            //        obj.ShowWireframe(!obj.m_isWireframe);
            //}
        }
    }
}
