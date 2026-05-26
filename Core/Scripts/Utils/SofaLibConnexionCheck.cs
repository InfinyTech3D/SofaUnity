using UnityEngine;
using SofaUnityAPI;

public class SofaLibConnexionCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SofaVerseAPI_ConnexionTest test = new SofaVerseAPI_ConnexionTest();
        int res = test.testSofaVerseAPI_connexion();

        if (res == 666)
        {
            Debug.Log("SofaVerse API connexion test: SUCCESS");
        }
        else
        {
            Debug.LogError("SofaVerse API connexion test: FAILED");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
