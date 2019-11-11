using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    protected int m_nbrScenes;    

    public class SceneMenuInfo
    {
        public SceneMenuInfo(string sceneName, string sceneInfo, Sprite sceneImage)
        {
            m_sceneName = sceneName;
            m_sceneInfo = sceneInfo;
            m_sceneImage = sceneImage;
        }

        public string m_sceneName;
        public string m_sceneInfo;
        public Sprite m_sceneImage;

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
    
    public SceneMenuInfo getSceneInfo(int id)
    {
        if (id < 0 || id >= m_nbrScenes)
            return null;

        return m_scenesInfo[id];
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
            if (name.Contains("organs"))
                sceneInfo = name + " multi threaded manipulation";
            else if (name.Contains("xray"))
                sceneInfo = name + " view manipulation";
            else if (name.Contains("cadus"))
                sceneInfo = name + " demo";
            else
                sceneInfo = "3D " + name + " interaction";
            
            m_scenesInfo.Add(new SceneMenuInfo(name, sceneInfo, sprite));            
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
