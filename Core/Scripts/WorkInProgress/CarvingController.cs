using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class CarvingController : MonoBehaviour {

    public GameObject m_carvingElem = null;
    public bool isActive = false;

    protected SofaMeshObject m_object = null;
    protected GameObject sofaContextObjet = null;
    protected SofaContext m_sofaObject = null;

    void Awake()
    {
        // get Sofa current scale
        sofaContextObjet = GameObject.Find("SofaContext");
        if (sofaContextObjet != null)
        {
            m_sofaObject = sofaContextObjet.GetComponent<SofaContext>();
        }
        else
            Debug.LogError("No sofa context found. Sofa scale won't be init.");
    }

    // Use this for initialization
    void Start () {
        if (m_carvingElem == null)
        {
            Debug.LogError("CarvingController::Start - Controllers pointer not set.");
            this.enabled = false;
            return;
        }

        m_object = m_carvingElem.GetComponent<SofaMeshObject>();
        if (m_object == null)
        {
            Debug.LogError("CarvingController::Start - SofaMeshObject pointer not set.");
            this.enabled = false;
            return;
        }
        //oldPosition = Vector3.zero;
    }

   // Vector3 oldPosition;
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Vector3 sofaContextPosition = new Vector3(0, 0, 0);
            sofaContextPosition = sofaContextObjet.transform.position;
            Debug.Log(this.transform.position);

            Vector3 position = (this.transform.position - sofaContextPosition);

            position = new Vector3(position.x, position.z, position.y);


            position = position * m_sofaObject.GetFactorUnityToSofa();
            //TODO restore that
            //m_object.m_impl.setNewPosition(position);
            Debug.Log("position : " + position + " sofaContextPosition " + sofaContextPosition);
            //oldPosition = position;
        }
    }

    
}
