using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SofaUnity;

public class ImmersiveController : MonoBehaviour {

    private SofaContext sofaContext = null;
    private GameObject SofaObject = null;
    public GameObject m_mainScene = null;
    public GameObject m_immScene = null;

    public GameObject m_controllerA = null;
    public GameObject m_controllerB = null;

    public Material wire = null;
    public Material organs = null;
    public Material heart = null;
    public Material bone = null;
    public Material bone_trans = null;
    public Material col = null;

    private bool isActivated = false;
    private Vector3 restControllerA;
    private Vector3 restControllerB;
    private bool inImmersiveWorld = false;

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

        if (m_controllerA == null || m_controllerB == null)
        {
            Debug.LogError("ImmersiveController::Start - Controllers pointer not set.");
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

        if(inImmersiveWorld)
        {
            if (Input.GetKey(KeyCode.B))
                activeInteraction();

            

            if (Input.GetKey(KeyCode.N))
                showMechanicalModels();
            if (Input.GetKey(KeyCode.X))
                restoreNormalView();

            if (!first)
                return;

            SofaObject.transform.localScale = SofaObject.transform.localScale * 4.0f;
            SofaObject.transform.localEulerAngles = new Vector3(180, 90, 180);
            first = false;
        }
        
    }

    void OnPreRender()
    {
        GL.wireframe = true;
    }
    void OnPostRender()
    {
        GL.wireframe = false;
    }

    public void activeInteraction()
    {
        isActivated = !isActivated;
        if (isActivated) // first time backup positions
        {
            Debug.Log("isActivated");
            restControllerA = m_controllerA.transform.position;
            restControllerB = m_controllerB.transform.position;
        }
    }
    bool first = true;

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
        float ratio = 1.0f;
        if (oldNormAB > 0.1f)
            ratio = newNormAB / oldNormAB;
        SofaObject.transform.localScale = SofaObject.transform.localScale * ratio * 0.1f;

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

    public void showMechanicalModels()
    {
        foreach (Transform child in SofaObject.transform)
        {
            if (child.name.Contains("skeleton"))
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material = bone_trans;
                }
                continue;
            }
            
            // Hack: assume only 2 level of hierarchy for the moment
            foreach (Transform littleChild in child)
            {
                if (littleChild.name.Contains("collision"))
                {
                    littleChild.GetComponent<MeshRenderer>();
                    //MeshRenderer mr = littleChild.GetComponent<MeshRenderer>();
                    //if (mr != null)
                    //    mr.enabled = true;
                }
                else if (littleChild.name.Contains("visu"))
                {
                    MeshRenderer mr = littleChild.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.material = wire;
                    }
                }
            }
        }
    }

    public void restoreNormalView()
    {
        foreach (Transform child in SofaObject.transform)
        {
            if (child.name.Contains("skeleton"))
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null)
                    mr.material = bone;
            }

            // Hack: assume only 2 level of hierarchy for the moment
            foreach (Transform littleChild in child)
            {
                if (littleChild.name.Contains("heart"))
                {
                    MeshRenderer mr = littleChild.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.sharedMaterial = heart;
                }
                else if (littleChild.name.Contains("collision"))
                {
                    MeshRenderer mr = littleChild.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.enabled = false;
                }
                else if (littleChild.name.Contains("visu"))
                {
                    MeshRenderer mr = littleChild.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.enabled = organs;
                }
            }
        }
    }


    public void setNormalScene()
    {
        if (!inImmersiveWorld)
            return;

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

        inImmersiveWorld = false;
        isActivated = false;
    }

    public void setImmersiveScene()
    {
        if (inImmersiveWorld)
            return;

        m_mainScene.SetActive(false);
        m_immScene.SetActive(true);

        // backup current main trans
        m_sofaInMain.transform.position = new Vector3(SofaObject.transform.position.x, SofaObject.transform.position.y, SofaObject.transform.position.z);
        m_sofaInMain.transform.rotation = new Quaternion(SofaObject.transform.rotation.x, SofaObject.transform.rotation.y, SofaObject.transform.rotation.z, SofaObject.transform.rotation.w);
        m_sofaInMain.transform.localScale = new Vector3(SofaObject.transform.localScale.x, SofaObject.transform.localScale.y, SofaObject.transform.localScale.z);

        //// set to current immersive transform.
        SofaObject.transform.position = new Vector3(m_sofaInImm.transform.position.x, m_sofaInImm.transform.position.y, m_sofaInImm.transform.position.z);
        SofaObject.transform.rotation = new Quaternion(m_sofaInImm.transform.rotation.x, m_sofaInImm.transform.rotation.y, m_sofaInImm.transform.rotation.z, m_sofaInImm.transform.rotation.w);
        SofaObject.transform.localScale = new Vector3(m_sofaInImm.transform.localScale.x, m_sofaInImm.transform.localScale.y, m_sofaInImm.transform.localScale.z);

        inImmersiveWorld = true;
    }


}
