using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    protected int m_nbrScenes;    
    public string prefixFiles = "VR_";

    public class SceneMenuInfo
    {
        public SceneMenuInfo(string sceneName, string sceneInfo, Sprite sceneImage)
        {
            m_sceneName = sceneName;
            m_sceneInfo = sceneInfo;
            m_sceneImage = sceneImage;
            m_rightToolType = SofaDefines.SRayInteraction.None;
            m_leftToolType = SofaDefines.SRayInteraction.None;
        }

        public string m_sceneName;
        public string m_sceneInfo;
        public Sprite m_sceneImage;
        public SofaDefines.SRayInteraction m_rightToolType;
        public SofaDefines.SRayInteraction m_leftToolType;

        public void printInfo()
        {
            Debug.Log("Scene name: " + m_sceneName + " | info: " + m_sceneInfo + " |  with image: " + m_sceneImage.name);
        }
    };
    
    public List<Sprite> scenes;
    protected List<SceneMenuInfo> m_scenesInfo;

    public int getNbrScenes()
    {
        return m_nbrScenes;
    }
    
    public SceneMenuInfo getSceneInfo(int sceneID)
    {
        if (sceneID < 0 || sceneID >= m_nbrScenes)
            return null;

        return m_scenesInfo[sceneID];
    }

    public string getSceneName(int sceneID)
    {
        if (sceneID < 0 || sceneID >= m_nbrScenes)
            return "";

        return m_scenesInfo[sceneID].m_sceneName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void parseScenes()
    {        
        m_scenesInfo = new List<SceneMenuInfo>();
        for (int i=0; i< scenes.Count; i++)
        {
            Sprite sprite = scenes[i];
            if (sprite == null)
                continue;

            int separator = sprite.name.LastIndexOf("_");
            string name = sprite.name.Substring(separator + 1);
            string sceneInfo = "";
            SofaDefines.SRayInteraction _rightToolType = SofaDefines.SRayInteraction.None;
            SofaDefines.SRayInteraction _leftToolType = SofaDefines.SRayInteraction.None;

            // TODO: to that better
            if (name.Contains("organs"))
            {
                sceneInfo = name + " multi threaded manipulation";
                //_rightToolType = SofaDefines.SRayInteraction.AttachTool;
                //_leftToolType = SofaDefines.SRayInteraction.AttachTool;
            }
            else if (name.Contains("xray"))
            {
                sceneInfo = name + " view manipulation";
            }
            else if (name.Contains("caduceus"))
            {
                sceneInfo = name + " demo";
                //_rightToolType = SofaDefines.SRayInteraction.AttachTool;
                //_leftToolType = SofaDefines.SRayInteraction.None;
            }
            else if (name.Contains("liver"))
            {
                sceneInfo = "3D " + name + " interaction";
                _rightToolType = SofaDefines.SRayInteraction.AttachTool;
                _leftToolType = SofaDefines.SRayInteraction.CuttingTool;
            }
            else if (name.Contains("cloth"))
            {
                sceneInfo = "3D " + name + " interaction";
                _rightToolType = SofaDefines.SRayInteraction.AttachTool;
                _leftToolType = SofaDefines.SRayInteraction.CuttingTool;
            }

            SceneMenuInfo info = new SceneMenuInfo(prefixFiles + name, sceneInfo, sprite);
            info.m_rightToolType = _rightToolType;
            info.m_leftToolType = _leftToolType;
            m_scenesInfo.Add(info);
        }

        m_nbrScenes = m_scenesInfo.Count;
        //Debug.Log("ScenesManager::parseScenes: " + m_nbrScenes);
        //for (int i=0; i<m_scenesInfo.Count; ++i)
        //{
        //    m_scenesInfo[i].printInfo();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
