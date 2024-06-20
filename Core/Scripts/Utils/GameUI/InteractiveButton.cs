using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractiveButton : MonoBehaviour
{
    public InteractiveMenuPanel m_menuOwner = null;       
    protected Button btn = null;

    // Use this for initialization
    void Start ()
    {
        btn = this.GetComponent<Button>();
        if (btn == null)
            Debug.LogError("InteractiveButton " + this.name + " couldn't find its corresponding Button component");
    }


    public void buttonEnter()
    {
        if (btn)
            btn.image.color = Color.gray;
    }

    public void buttonAction()
    {
        if (btn)
        {
            btn.onClick.Invoke();
        }
    }

    public void buttonExit()
    {
        if (btn)
            btn.image.color = Color.white;
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonEnter();

        if (m_menuOwner)
            m_menuOwner.buttonTriggerEnter(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_menuOwner)
            m_menuOwner.buttonTriggerStay(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_menuOwner)
            m_menuOwner.buttonTriggerExit(this);

        buttonExit();
    }
}
