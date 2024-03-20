using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public GameObject m_textCanvas = null;
    public int m_maxstack = 20;

    string myLog;
    Queue myLogQueue = new Queue();

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (myLogQueue.Count > m_maxstack)
            //myLogQueue.Clear();
            return;

        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    void OnGUI()
    {
        if (m_textCanvas == null)
            GUILayout.Label(myLog);
        else
        {
            Text txt = m_textCanvas.GetComponent<Text>();
            if (txt)
                txt.text = myLog;
        }
    }
}
