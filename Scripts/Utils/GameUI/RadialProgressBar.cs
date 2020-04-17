using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class RadialProgressBar : MonoBehaviour
{
    public float currentProgress;
    public float speed = 1;
    public float maxSeconds = 100;
    public bool infinitLoop = false;

    public UnityEvent onCompleted;
    public bool isCompleted = false;

    protected Image loadingImage = null;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("RadialProgressBar start");
        loadingImage = this.GetComponent<Image>();

        if (speed <= 0)
            speed = 1;

        if (maxSeconds <= 0)
            maxSeconds = 100;

        currentProgress = 0;
        isCompleted = false;
    }

    private void OnEnable()
    {
        currentProgress = 0;
        isCompleted = false;
        if (loadingImage != null)
            loadingImage.fillAmount = currentProgress / maxSeconds;
    }

    // Update is called once per frame
    void Update ()
    {
	    if (currentProgress < maxSeconds)
        {
            currentProgress += speed * Time.deltaTime;
        }
        else
        {
            // finished
            isCompleted = true;
            onCompleted.Invoke();

            if (infinitLoop)
            {
                currentProgress = 0;
                isCompleted = false;
            }
        }

        if (loadingImage != null)
            loadingImage.fillAmount = currentProgress / maxSeconds;
    }
}
