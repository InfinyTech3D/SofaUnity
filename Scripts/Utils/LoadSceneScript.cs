using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneScript : MonoBehaviour
{
    public GameObject loadingImage = null;
    public Text m_Text;    

    protected string m_currentSceneName = "";
    protected int m_levelId = -1;

    protected bool m_working = false;

    public bool isLoading() { return m_working; }

    public void loadSofaScene(int level)
    {
        if (m_working) // already in process, exit
            return;

        if (loadingImage)
            loadingImage.SetActive(true);

        m_working = true;
        StartCoroutine(loadSceneAsync_impl(level));
    }


    public void loadSofaScene(string name)
    {
        if (m_working) // already in process, exit
            return;

        if (loadingImage)
            loadingImage.SetActive(true);

        m_working = true;        
        StartCoroutine(loadSceneAsync_impl(name));
    }    


    public void unLoadScene()
    {
        if (m_working) // already in process, exit
            return;

        if (m_levelId != -1)
        {
            m_working = true;
            StartCoroutine(unloadSceneAsync_impl());
        }
    }


    IEnumerator loadSceneAsync_impl(int level)
    {
        yield return null;
        int cptSecu = 0;

        // first unload previous scene
        if (m_levelId != -1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(m_levelId);
            cptSecu = 0;
            while (!asyncUnload.isDone && cptSecu < 10000)
            {
                cptSecu++;
                yield return null;
            }
        }

        // load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        //asyncLoad.allowSceneActivation = false;
        cptSecu = 0;
        while (!asyncLoad.isDone && cptSecu < 10000)
        {
            if (m_Text) //Output the current progress
            {
                m_Text.text = "Loading progress: " + (asyncLoad.progress * 100) + "%";
            }
            cptSecu++;
            yield return null;
        }
        
        m_levelId = level;
        m_working = false;
        if (m_Text)
        {
            if (asyncLoad.isDone)
                m_Text.text = "Loading success.";
            else
                m_Text.text = "Loading failed.";
        }
    }


    IEnumerator loadSceneAsync_impl(string sceneName)
    {
        yield return null;
        int cptSecu = 0;

        // first unload previous scene
        if (m_currentSceneName.Length != 0)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(m_currentSceneName);
            cptSecu = 0;
            while (!asyncUnload.isDone && cptSecu < 10000)
            {
                cptSecu++;
                yield return null;
            }
        }

        // load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //asyncLoad.allowSceneActivation = false;
        cptSecu = 0;
        while (!asyncLoad.isDone && cptSecu < 10000)
        {
            if (m_Text) //Output the current progress
            {
                m_Text.text = "Loading progress: " + (asyncLoad.progress * 100) + "%";
            }
            cptSecu++;
            yield return null;
        }

        m_currentSceneName = sceneName;
        m_working = false;
        if (m_Text)
        {
            if (asyncLoad.isDone)
                m_Text.text = "Loading success.";
            else
                m_Text.text = "Loading failed.";
        }
    }        

    IEnumerator unloadSceneAsync_impl()
    {
        yield return null;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(m_levelId);
        int cptSecu = 0;
        while (!asyncUnload.isDone && cptSecu < 10000)
        {
            cptSecu++;
            yield return null;
        }

        m_levelId = -1;
        m_working = false;
    }


    //////////////////////////////////// WIP //////////////////////////////////////////
    public void testMethod(int level)
    {
        if (loadingImage)
            loadingImage.SetActive(true);
        Debug.Log("LoadSceneScript::testMethod level: " + level);
    }
}
