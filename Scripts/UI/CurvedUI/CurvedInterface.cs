using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;

public class CurvedInterface : MonoBehaviour
{
    protected SofaContext m_sofaContext;

    public Image LevelImage;
    public enum Environment { LIVER, ORGANS, CADUCEUS }
    [HideInInspector]
    public Environment SelectedEnvironment;
    public Animator EnvironmentImageAnimator;
    public Text[] SelectedEnvironmentText;
    public ImageSwitch m_imageSwitcher;
    private int m_environmentCount;

    // Start is called before the first frame update
    void Start()
    {
        SelectedEnvironment = Environment.LIVER;
        m_environmentCount = System.Enum.GetValues(typeof(Environment)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangePlayerStatus(Toggle _toggle)
    {
        if (_toggle.isOn)
        {
            //if (m_sofaContext == null)
            //    return;

            if (_toggle.gameObject.name.Contains("play"))
                Debug.Log("PLAY");
            else if (_toggle.gameObject.name.Contains("stop"))
                Debug.Log("STOP");
            else if (_toggle.gameObject.name.Contains("pause"))
                Debug.Log("PAUSE");
        }
    }

    public void ChangeScene(int _direction)
    {
        Debug.Log("ChangeEnvironment");
        SelectedEnvironment += _direction;
        int _environmentIndex = (int)SelectedEnvironment;
        if (_environmentIndex < 0) SelectedEnvironment = (Environment)m_environmentCount - 1;
        else if (_environmentIndex == m_environmentCount) SelectedEnvironment = 0;
        EnvironmentImageAnimator.SetTrigger("" + SelectedEnvironment);        

        for (int i = 0; i < SelectedEnvironmentText.Length; i++)
        {
            switch (SelectedEnvironment)
            {
                case (Environment.LIVER):
                    StartCoroutine(ChangeText(SelectedEnvironmentText[i], "Liver Interaction"));
                    if (m_imageSwitcher)
                        m_imageSwitcher.changeImage("liver");
                    break;
                case (Environment.ORGANS):
                    StartCoroutine(ChangeText(SelectedEnvironmentText[i], "Organs"));
                    if (m_imageSwitcher)
                        m_imageSwitcher.changeImage("organs");
                    break;
                case (Environment.CADUCEUS):
                    StartCoroutine(ChangeText(SelectedEnvironmentText[i], "Caduceus"));
                    if (m_imageSwitcher)
                        m_imageSwitcher.changeImage("caduceus");
                    break;
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator ChangeText(Text _text, string _string, float _timeBetweenLetters = 0.05f)
    {
        _text.text = "";
        for (int i = 0; i < _string.Length; i++)
        {
            _text.text += "" + _string[i];
            yield return new WaitForSeconds(_timeBetweenLetters);
        }

        yield return null;
    }
}
