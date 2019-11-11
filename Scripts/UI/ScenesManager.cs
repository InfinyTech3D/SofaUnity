using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    protected int m_nbrScenes;    

    public class SceneInfo
    {
        string sceneName;
        string scenePath;
        Sprite sceneImage;
    };

    public List<Sprite> scenes;

    public int getNbrScenes()
    {
        return m_nbrScenes;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
