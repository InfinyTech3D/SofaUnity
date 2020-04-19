using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;


public class CArmController : MonoBehaviour
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////
    

    [SerializeField]
    public GameObject visu_target = null;
    public GameObject m_target = null;

    //[SerializeField]
    protected GameObject sofaContextObjet = null;
    protected SofaComponent m_sofaObject = null;
    protected float sofaScale = 1.0f;

    
    protected float nextUpdate = 0.0f;
    
    // position in unity world of the target position
    protected Vector3 m_targetPosition;
    // position in sofa world of the target position
    protected Vector3 sofaTargetPosition;
    // position in sofa world of the Carm position, // in unity world it the gameobject position
    protected Vector3 CARMPosition;
    // Bool to store the information that position have changed and need to be propagated to sofa.
    protected bool changed = false;

    /// <summary>
    /// position of the CArm source, will recompute XRay casting position
    /// </summary>
    [SerializeField]
    protected float m_sourceDistance = 0.59f;
    public float sourceDistance
    {
        get { return m_sourceDistance; }
        set
        {
            if (value != m_sourceDistance)
            {
                m_sourceDistance = value;
                if (m_target != null)
                    computePositions();
            }
        }
    }

    /// <summary>
    /// refresh rate of the CArm/XRay, this value will be set by default by Sofa value. Changing it here at reuntime will
    /// propagate to Sofa.
    /// </summary>
    [SerializeField]
    protected float m_refreshRatesec = 0.01f; //sec
    protected int m_refreshRateMSec = 100; // msec
    public float refreshRate // ms
    {
        get { return m_refreshRatesec; }
        set
        {
            if (value != m_refreshRatesec)
            {
                m_refreshRatesec = value;
                m_refreshRateMSec = (int)m_refreshRatesec * 1000;

                //if (this.m_sofaObject != null)
                //    this.m_sofaObject.impl.setIntValue("xrayFrameRate", m_refreshRateMSec);
            }
        }
    }


    // Callback Method that can be linked in Unity GUI to move the C-ARM
    public void zoomCARM() { m_sourceDistance -= 0.2f; computePositions(); }

    public void unZoomCARM() { m_sourceDistance += 0.2f; computePositions(); }


    /// Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            zoomCARM();
        else if (Input.GetKey(KeyCode.Alpha2))
            unZoomCARM();

        if (this.transform.hasChanged)
        {
            computePositions();
            this.transform.hasChanged = false;
            if (visu_target != null)
                visu_target.SetActive(true);
        }
        else
        {
            if (visu_target != null)
                visu_target.SetActive(false);
        }

        //if (Time.time >= nextUpdate) // uncomment that line if you don't want to send only at each refresh. The image is only recomputed at each framerate anyway.
        {
          //  nextUpdate += refreshRate;            
            if(changed)
            {
            //    Debug.Log("Time.time: " + Time.time + " nextUpdate " + nextUpdate);
                //m_sofaObject.impl.setVector3fValue("sourcePosition", CARMPosition);
                //m_sofaObject.impl.setVector3fValue("detectorPosition", sofaTargetPosition);
                changed = false;
            }
        }
    }



    void Awake()
    {
        // get Sofa current scale
        sofaContextObjet = GameObject.Find("SofaContext");
        if (sofaContextObjet != null)
        {
            SofaContext context = sofaContextObjet.GetComponent<SofaContext>();
            sofaScale = context.GetFactorUnityToSofa();
        }
        else
            Debug.LogError("No sofa context found. Sofa scale won't be init.");
    }


    void Start ()
    {
        // search the target GameObject
        if (m_target == null)
            m_target = GameObject.Find("SComponent - XRayViewer");

        if (visu_target == null)
            visu_target = GameObject.Find("FlouroLight");

        // get the sofa object behind
        if (m_target != null)
        {
            //m_sofaObject = m_target.GetComponent<SComponentObject>();
            this.computePositions();
            if (this.m_sofaObject != null)
            {
              //  int res = this.m_sofaObject.impl.getIntValue("xrayFrameRate");
               // this.refreshRate = (float)res * 0.001f;
            }
        }

    }
    
    void computePositions()
    {
        Vector3 sofaContextPosition = new Vector3(0, 0, 0);
        sofaContextPosition = m_target.transform.position;

        Vector3 detecDir = this.transform.up;
        
        // compute relative position of the CARM
        CARMPosition = this.transform.position;

        // compute the target position on the axis
        m_targetPosition = CARMPosition + detecDir * m_sourceDistance;

        if (this.m_sofaObject != null)
        {
            //CARMPosition = sofaContextObjet.transform.localRotation * CARMPosition;
            CARMPosition = new Vector3(-CARMPosition.x, CARMPosition.z, CARMPosition.y);
            
            CARMPosition = (CARMPosition - sofaContextPosition) * sofaScale;
            //CARMPosition.x *= -1;

            //m_targetPosition = sofaContextObjet.transform.localRotation * m_targetPosition;
            m_targetPosition = new Vector3(-m_targetPosition.x, m_targetPosition.z, m_targetPosition.y);
            sofaTargetPosition = (m_targetPosition - sofaContextPosition) * sofaScale;
           // sofaTargetPosition.x *= -1;

            changed = true;
        }        
    }


    void updateXray()
    {
        Debug.Log("updateXray");

    }    


    void OnDrawGizmosSelected()
    {
        if (m_target == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_target.transform.TransformPoint(m_target.transform.position), 0.1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_targetPosition, 0.1f);
        Gizmos.DrawLine(this.transform.position, m_targetPosition);
    }
}
