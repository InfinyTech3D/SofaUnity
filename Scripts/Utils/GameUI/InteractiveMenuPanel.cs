using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SofaUnity;

public class InteractiveMenuPanel : MonoBehaviour
{
    public GameObject radialProgressBar = null;
    public RadialProgressBar progressB = null;
    public Dictionary<string, UnityEvent> buttonActions;

    public bool singleAction = true;
    protected bool actionDone = false;
    protected InteractiveButton m_currentIButton = null;


    void Awake()
    {
        foreach (Transform child in this.transform)
        {            
            InteractiveButton Ibtn = child.gameObject.AddComponent<InteractiveButton>() as InteractiveButton;
            if (Ibtn)
                Ibtn.m_menuOwner = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void buttonTriggerEnter(InteractiveButton button)
    {        
        actionDone = false;
        if (m_currentIButton != null)
        {
            m_currentIButton.buttonExit();
            buttonTriggerExit(m_currentIButton);
        }

        m_currentIButton = button;

        if (radialProgressBar)
            radialProgressBar.SetActive(true);
    }

    public void buttonTriggerStay(InteractiveButton button)
    {
        if (m_currentIButton != button)
            return;

        if (progressB && progressB.isCompleted)
            buttonAction();
        else if (progressB == null)
            buttonAction();
    }

    public void buttonTriggerExit(InteractiveButton button)
    {        
        if (m_currentIButton != button)
            return;

        m_currentIButton = null;

        if (radialProgressBar)
            radialProgressBar.SetActive(false);
    }

    public void buttonAction()
    {
        if (singleAction && actionDone)
            return;

        if (m_currentIButton)
            m_currentIButton.buttonAction();

        actionDone = true;
    }
}
