using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    public GameObject loadingImage = null;

    public void LoadScene(int level)
    {
        if (loadingImage)
            loadingImage.SetActive(true);

        //Application.LoadLevel(level);
        SceneManager.LoadScene(level);
    }

    public void testMethod(int level)
    {
        if (loadingImage)
            loadingImage.SetActive(true);
        Debug.Log("LoadSceneScript::testMethod level: " + level);
    }
}
