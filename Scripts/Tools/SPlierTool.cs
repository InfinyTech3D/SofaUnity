using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

/// <summary>
/// Base class inherite from MonoBehavior that design a Plier tool.
/// This class is a work in progress. And need to have GameObject for each plier jaw as well as a target object.
/// </summary>
//[ExecuteInEditMode]
public class SPlierTool : MonoBehaviour
{
    ////////////////////////////////////////////
    /////          Object members          /////
    ////////////////////////////////////////////

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;
    /// Pointer to the corresponding SOFA API object
    protected SofaPliers m_sofaPlier = null;
    

    /// Pointer to the unity GameObject corresponding to the upper jaw
    public GameObject Mord_UP = null;
    /// Pointer to the unity GameObject corresponding to the lower jaw
    public GameObject Mord_Down = null;
    /// Pointer to the unity GameObject to interact with
    public GameObject Model = null;
    /// Pointer to the unity GameObject that correspond to the visual object to interact with (could be similar to Model)
    public GameObject ModelVisu = null;

    /// stiffness value of the plier forcefields
    public float m_stiffness = 300;

    /// length of cut
    public float m_cutLength = 0.15f;

    /// Mesh renderer of the ModelVisu GameObject
    protected Mesh modelMesh = null;


    /// speed factor of the plier animation
    private float animSpeed = 0.1f;
    
    /// Number of vertex grabed by the plier
    protected int m_nbrGrabed = 0;
    /// Buffer of vertex id grabed by the plier
    protected int[] m_idGrabed = null;



    ////////////////////////////////////////////
    /////       Object creation API        /////
    ////////////////////////////////////////////

    /// Method called at GameObject creation. Will search for SofaContext @sa loadContext() which call @sa createObject() . Then call @see awakePostProcess()
    void Awake()
    {
        bool contextOk = true;
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            // Get Sofa context
            m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            if (m_sofaContext == null)
            {
                Debug.LogError("RayCaster::loadContext - GetComponent<SofaUnity.SofaContext> failed.");
                contextOk = false;
            }
        }
        else
        {
            Debug.LogError("RayCaster::loadContext - No SofaContext found.");
            contextOk = false;
        }

        // Call internal method that will create a ray caster in Sofa.
        if (contextOk)
            StartCoroutine(createSofaPlier());
    }


    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    public virtual IEnumerator createSofaPlier()
    {
        // Get access to the sofaContext // TODO remove this HACK: All components need to be created before the SofaPlier
        yield return new WaitForSeconds(1);

        IntPtr _simu = m_sofaContext.getSimuContext();
        if (_simu != IntPtr.Zero && Mord_UP != null && Mord_Down != null && Model != null && ModelVisu != null)
        {
            SBaseMesh mesh = Model.GetComponent<SBaseMesh>();

            m_sofaPlier = new SofaPliers(_simu, name, Mord_UP.name, Mord_Down.name, mesh.nameId, m_stiffness);
        }
    }




    ////////////////////////////////////////////
    /////       Object behavior API        /////
    ////////////////////////////////////////////

    private void Start()
    {
        animator.speed = animSpeed;
    }

    private Animator animator { get { return GetComponent<Animator>(); } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Clamp());
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Unclamp());
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Cut();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            testCutPath();
        }
    }


    /// Real method to activate the clamping in Sofa object
    public bool clampSofaPlier()
    {
        if (m_sofaPlier == null)
            return false;

        m_nbrGrabed = m_sofaPlier.closePliers();

        if (m_nbrGrabed > 0)
        {
            m_idGrabed = new int[m_nbrGrabed];
            int resIds = m_sofaPlier.getIdsGrabed(m_idGrabed);
            if (resIds < 0)
                m_idGrabed = null;
            else
                Debug.Log(m_idGrabed[0] + " " + m_idGrabed[1]);

            return true;
        }
        else
        {
            m_nbrGrabed = 0;
            return false;
        }
    }

    /// Real method to rlease the clamping in Sofa object
    public bool releaseSofaPlier()
    {
        if (m_sofaPlier == null)
            return false;

        m_sofaPlier.releasePliers();
        m_idGrabed = null;
        m_nbrGrabed = 0;
        return true;
    }

    
    /// Method to activate the clamping animation and call the plier clamping computation
    public IEnumerator Clamp()
    {
        animator.SetBool("isClamped", true);
        yield return new WaitForSeconds(2.5f);
        clampSofaPlier();
    }

    /// Method to activate the unclamping animation and call the plier release computation
    public IEnumerator Unclamp()
    {
        animator.SetBool("isClamped", false);
        yield return new WaitForSeconds(1);
        releaseSofaPlier();
    }

    public void Cut()
    {
        if(m_sofaPlier == null)
            return;

        Debug.Log("Start cut");
        //Debug.Log("transform.position: " + transform.position);
        //Debug.Log("transform.forward: " + transform.forward); // z
        //Debug.Log("transform.up: " + transform.up);// y
        //Debug.Log("transform.up: " + transform.right); // -x

        animator.SetBool("isClamped", false);
        releaseSofaPlier();
        int res = m_sofaPlier.cutPliers(transform.position * m_sofaContext.getFactorUnityToSofa(), -transform.right, transform.up, transform.forward, m_cutLength * m_sofaContext.getFactorUnityToSofa());       
    }

    public void testCutPath()
    {
        if (m_sofaPlier == null)
            return;

        animator.SetBool("isClamped", false);
        releaseSofaPlier();
        int res = m_sofaPlier.cutPliersPath(transform.position * m_sofaContext.getFactorUnityToSofa(), -transform.right, transform.up, transform.forward, m_cutLength * m_sofaContext.getFactorUnityToSofa());
       
        if (res > 0 && res < 1000)
        {
            m_idGrabed = null;
            m_nbrGrabed = res;

            m_idGrabed = new int[m_nbrGrabed];
            int resIds = m_sofaPlier.getIdsGrabed(m_idGrabed);
            //Debug.Log("Cut : " + res + " - " + m_idGrabed[0] + " " + m_idGrabed[1]);
        }       
    }


    /// Method to draw debug information like the vertex being grabed
    void OnDrawGizmosSelected()
    {
        if (m_idGrabed == null || ModelVisu == null || m_sofaContext == null)
            return;

        modelMesh = ModelVisu.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = modelMesh.vertices;
        float factor = m_sofaContext.getFactorSofaToUnity();

        for (int i = 0; i < m_nbrGrabed; i++)
        {
            Vector3 vert = new Vector3(vertices[m_idGrabed[i]].x, vertices[m_idGrabed[i]].y, vertices[m_idGrabed[i]].z);
            Gizmos.DrawWireSphere(ModelVisu.transform.TransformPoint(vert), 0.1f * factor);
        }
        
    }
}
