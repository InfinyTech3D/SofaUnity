using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaViewController : MonoBehaviour
{
    private SofaContext sofaContext = null;
    private GameObject SofaObject = null;

    public GameObject m_controllerA = null;
    public GameObject m_controllerB = null;

    public bool startOnPlay = true;
    public float ratioScale = 1.0f;

    private Vector3 restControllerA;
    private Vector3 restControllerB;
    private GameObject m_sofaIninit = null;

    public enum MoveMode { FIX, TRANSLATION, ROTATION, SCALE, ALL }
    private MoveMode currentMode = MoveMode.FIX;

    // Start is called before the first frame update
    void Start()
    {
        if(startOnPlay)
            searchForSofaContext();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
            activeInteraction(MoveMode.TRANSLATION);

        if (Input.GetKey(KeyCode.R))
            activeInteraction(MoveMode.ROTATION);

        if (Input.GetKey(KeyCode.S))
            activeInteraction(MoveMode.SCALE);

        if (Input.GetKey(KeyCode.A))
            activeInteraction(MoveMode.ALL);

        if (Input.GetKey(KeyCode.E))
            resetSofaView();

        if (currentMode == MoveMode.FIX)
            return;

        // apply the transformation given the mode
        if (currentMode == MoveMode.TRANSLATION)
        {
            Debug.Log("## " + Time.fixedTime + "TRANSLATION");
            TranslateSofaContext();
        }
        else if (currentMode == MoveMode.ROTATION)
        {
            Debug.Log("## " + Time.fixedTime + "ROTATION");
            RotateSofaContext();
        }
        else if (currentMode == MoveMode.SCALE)
        {
            Debug.Log("## " + Time.fixedTime + "SCALE");
            ScaleSofaContext();
        }
        else if (currentMode == MoveMode.ALL)
        {
            TranslateSofaContext();
            RotateSofaContext();
            ScaleSofaContext();
        }

        // update position.
        restControllerA = m_controllerA.transform.position;
        restControllerB = m_controllerB.transform.position;
    }

    public void resetSofaView()
    {
        if (m_sofaIninit == null || SofaObject == null)
            return;

        //// restore to main immersive transform.
        SofaObject.transform.position = new Vector3(m_sofaIninit.transform.position.x, m_sofaIninit.transform.position.y, m_sofaIninit.transform.position.z);
        SofaObject.transform.rotation = new Quaternion(m_sofaIninit.transform.rotation.x, m_sofaIninit.transform.rotation.y, m_sofaIninit.transform.rotation.z, m_sofaIninit.transform.rotation.w);
        SofaObject.transform.localScale = new Vector3(m_sofaIninit.transform.localScale.x, m_sofaIninit.transform.localScale.y, m_sofaIninit.transform.localScale.z);
    }

    public void unloadSofaScene()
    {
        sofaContext = null;
        SofaObject = null;
        currentMode = MoveMode.FIX;
    }

    public void searchForSofaContext()
    {
        // find SofaContext and register this object
        GameObject _sofaObject = GameObject.Find("SofaContext");
        setSofaContext(_sofaObject);
    }

    public void setSofaContext(GameObject _sofaObject)
    {
        unloadSofaScene();

        SofaObject = _sofaObject;
        if (SofaObject != null)
        {
            // Get Sofa context
            sofaContext = SofaObject.GetComponent<SofaUnity.SofaContext>();
            if (sofaContext == null)
            {
                Debug.LogError("SofaViewController::Start - sofaContext not found");
                this.enabled = false;
            }
        }
        else
        {
            Debug.LogError("SofaViewController::Start - SofaObject not found");
            this.enabled = false;
        }

        // backup a transform the 2 transform version of SofaContext to the default one.
        m_sofaIninit = new GameObject();
        m_sofaIninit.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaIninit.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaIninit.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);        
    }

    public void activeInteraction(MoveMode mode, bool continuous = false)
    {
        if (!continuous)
        {
            restControllerA = m_controllerA.transform.position;
            restControllerB = m_controllerB.transform.position;
        }

        if (mode == currentMode) // exit mode
            currentMode = MoveMode.FIX;
        else
            currentMode = mode;    
    }


    private void updateTransform()
    {
        Vector3 diffPA = m_controllerA.transform.position - restControllerA;
        Vector3 diffPB = m_controllerB.transform.position - restControllerB;

        float normA = diffPA.magnitude;
        float normB = diffPB.magnitude;

        if (normA < 0.01 && normB < 0.01)
            return;

        Debug.Log("normA: " + normA);
        Debug.Log("normB: " + normB);

        Vector3 oldAB = restControllerB - restControllerA;
        Vector3 newAB = m_controllerB.transform.position - m_controllerA.transform.position;
        float oldNormAB = oldAB.magnitude;
        float newNormAB = newAB.magnitude;

        // translate center
        if (normA > 0.01)
            SofaObject.transform.position += diffPA;

        // scale
        float ratio = 0.0f;
        if (oldNormAB > 0.1f)
            ratio = newNormAB / oldNormAB;
        SofaObject.transform.localScale = SofaObject.transform.localScale * ratio;

        // rotation
        Quaternion rot = Quaternion.FromToRotation(oldAB, newAB);

        Debug.Log("SofaObject.transform.localEulerAngles: " + SofaObject.transform.localEulerAngles);
        Debug.Log("rot.eulerAngles: " + rot.eulerAngles);
        SofaObject.transform.localEulerAngles = SofaObject.transform.localEulerAngles + rot.eulerAngles;
        Debug.Log("SofaObject.transform.localEulerAngles apres: " + SofaObject.transform.localEulerAngles);

        // update rest positions
        if (normA > 0.01)
            restControllerA = m_controllerA.transform.position;
        if (normB > 0.01)
            restControllerB = m_controllerB.transform.position;
    }

    private void TranslateSofaContext()
    {
        Vector3 centerInit = (restControllerA + restControllerB)*0.5f;
        Vector3 centerCurrent = (m_controllerA.transform.position + m_controllerB.transform.position) * 0.5f;

        Vector3 transModel = centerCurrent - centerInit;

        SofaObject.transform.position += transModel;
    }

    private void RotateSofaContext()
    {
        Vector3 oldAB = restControllerB - restControllerA;
        Vector3 newAB = m_controllerB.transform.position - m_controllerA.transform.position;
        
        // rotation
        Quaternion rot = Quaternion.FromToRotation(oldAB, newAB);
        SofaObject.transform.eulerAngles = SofaObject.transform.eulerAngles + rot.eulerAngles;
    }

    private void ScaleSofaContext()
    {
        // suppose same scale on all direction
        Vector3 diffInit = restControllerA - restControllerB;
        Vector3 diffCurrent = m_controllerA.transform.position - m_controllerB.transform.position;
        Vector3 scaleInit = SofaObject.transform.localScale;

        float normInit = diffInit.magnitude;
        float normCurrent = diffCurrent.magnitude;

        Vector3 newScale = (normCurrent / normInit) * scaleInit;
        SofaObject.transform.localScale = newScale; 
    }
}
