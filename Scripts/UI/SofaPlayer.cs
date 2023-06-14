using System.Collections;
using System.Collections.Generic;
using SofaUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SofaPlayer : MonoBehaviour
{
    public SofaContext m_sofaContext;
    public Button m_playButton;
    public Button m_stopButton;
    public Button m_restartButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSofaSimulation()
    {
        Debug.Log("startSofaSimulation");
        if (m_sofaContext != null)
            m_sofaContext.IsSofaUpdating = true;
    }

    public void stopSofaSimulation()
    {
        Debug.Log("stopSofaSimulation");
        if (m_sofaContext != null)
            m_sofaContext.IsSofaUpdating = false;
    }

    public void restartSofaSimulation()
    {
        Debug.Log("restartSofaSimulation");
        SceneManager.LoadScene(0);
    }
}
