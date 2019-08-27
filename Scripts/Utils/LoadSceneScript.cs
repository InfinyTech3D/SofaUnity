using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneScript : MonoBehaviour
{
    public GameObject loadingImage = null;

    public void LoadScene(int level)
    {
        if (loadingImage)
            loadingImage.SetActive(true);

        Application.LoadLevel(level);
    }

    public void testMethod(int level)
    {
        if (loadingImage)
            loadingImage.SetActive(true);
        Debug.Log("LoadSceneScript::testMethod level: " + level);
    }
}
