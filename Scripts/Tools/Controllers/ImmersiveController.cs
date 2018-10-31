using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class ImmersiveController : MonoBehaviour {

    private SofaContext sofaContext = null;
    private GameObject SofaObject = null;
    public GameObject m_mainScene = null;
    public GameObject m_immScene = null;

    private GameObject m_sofaInMain = null;
    private GameObject m_sofaInImm = null;


    // Use this for initialization
    void Start ()
    {
        // find SofaContext and register this object
        SofaObject = GameObject.Find("SofaContext");
        if (SofaObject != null)
        {
            // Get Sofa context
            sofaContext = SofaObject.GetComponent<SofaUnity.SofaContext>();
            if (sofaContext == null)
            {
                Debug.LogError("ImmersiveController::Start - sofaContext not found");
                this.enabled = false;
            }
        }
        else
        {
            Debug.LogError("ImmersiveController::Start - SofaObject not found");
            this.enabled = false;
        }

        if (m_mainScene == null || m_immScene == null)
        {
            Debug.LogError("ImmersiveController::Start - Pointer to the scenes not set.");
            this.enabled = false;
            return;
        }

        // init the 2 transform version of SofaContext to the default one.
        m_sofaInMain = new GameObject();
        m_sofaInImm = new GameObject();

        m_sofaInMain.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaInMain.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaInMain.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);

        m_sofaInImm.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaInImm.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaInImm.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.V))
            setNormalScene();
        if (Input.GetKey(KeyCode.C))
            setImmersiveScene();
    }

    void setNormalScene()
    {
        m_mainScene.SetActive(true);
        m_immScene.SetActive(false);

        //// backup current sofa trans in imm
        m_sofaInImm.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaInImm.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaInImm.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);
        
        //// restore to main immersive transform.
        SofaObject.transform.position = new Vector3(m_sofaInMain.transform.position.x, m_sofaInMain.transform.position.y, m_sofaInMain.transform.position.z);
        SofaObject.transform.rotation = new Quaternion(m_sofaInMain.transform.rotation.x, m_sofaInMain.transform.rotation.y, m_sofaInMain.transform.rotation.z, m_sofaInMain.transform.rotation.w);
        SofaObject.transform.localScale = new Vector3(m_sofaInMain.transform.localScale.x, m_sofaInMain.transform.localScale.y, m_sofaInMain.transform.localScale.z);
    }

    void setImmersiveScene()
    {
        m_mainScene.SetActive(false);
        m_immScene.SetActive(true);

        // backup current main trans
        //m_sofaTransInMain = SofaObject.transform;
        //Debug.Log("setImmersiveScene: m_sofaTransInMain.localScale1: " + m_sofaTransInMain.localScale);
        
        Debug.Log("setImmersiveScene: SofaObject.transform.position: " + SofaObject.transform.position);
        Debug.Log("pos.x: " + SofaObject.transform.position.x);
        Debug.Log("setImmersiveScene: SofaObject.transform.rotation: " + SofaObject.transform.rotation);
        Debug.Log("setImmersiveScene: SofaObject.transform.lossyScale: " + SofaObject.transform.localScale);

        m_sofaInMain.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaInMain.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaInMain.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);
        Debug.Log("m_sofaTransInMain.scale.x: " + m_sofaInMain.transform.localScale.x);

        //// set to current immersive transform.
        SofaObject.transform.position = new Vector3(m_sofaInImm.transform.position.x, m_sofaInImm.transform.position.y, m_sofaInImm.transform.position.z);
        SofaObject.transform.rotation = new Quaternion(m_sofaInImm.transform.rotation.x, m_sofaInImm.transform.rotation.y, m_sofaInImm.transform.rotation.z, m_sofaInImm.transform.rotation.w);
        SofaObject.transform.localScale = new Vector3(m_sofaInImm.transform.localScale.x, m_sofaInImm.transform.localScale.y, m_sofaInImm.transform.localScale.z);
    }
}
