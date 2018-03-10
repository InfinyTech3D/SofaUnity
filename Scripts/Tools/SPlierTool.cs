using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;

/// <summary>
/// Base class inherite from MonoBehavior that design a Ray casting object.
/// </summary>
//[ExecuteInEditMode]
public class SPlierTool : MonoBehaviour
{
    private Animator animator { get { return GetComponent<Animator>(); } }

    private float animSpeed = 0.1f;

    /// Pointer to the Sofa context this GameObject belongs to.
    protected SofaUnity.SofaContext m_sofaContext = null;

    /// Pointer to the corresponding SOFA API object
    protected SofaPliers m_sofaPlier = null;
    public GameObject Mord_UP = null;
    public GameObject Mord_Down = null;
    public GameObject Model = null;
    public GameObject ModelVisu = null;
    protected Mesh modelMesh = null;

    protected int[] m_idGrabed = null;

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

    private void Start()
    {
        animator.speed = animSpeed;
    }

    /// Method called by @sa loadContext() method. To create the object when Sofa context has been found. To be implemented by child class.
    protected virtual IEnumerator createSofaPlier()
    {
        // Get access to the sofaContext
        yield return new WaitForSeconds(1);

        IntPtr _simu = m_sofaContext.getSimuContext();
        if (_simu != IntPtr.Zero && Mord_UP != null && Mord_Down != null && Model != null && ModelVisu != null)
        {
            SBaseMesh mesh = Model.GetComponent<SBaseMesh>();

            m_sofaPlier = new SofaPliers(_simu, name, Mord_UP.name, Mord_Down.name, mesh.nameId);
        }
    }

    protected int m_nbrGrabed = 0;
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

    public bool releaseSofaPlier()
    {
        if (m_sofaPlier == null)
            return false;

        m_sofaPlier.releasePliers();
        m_idGrabed = null;
        m_nbrGrabed = 0;
        return true;
    }

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

    }

    public IEnumerator Clamp()
    {
        animator.SetBool("isClamped", true);
        yield return new WaitForSeconds(2.5f);
        clampSofaPlier();
    }

    public IEnumerator Unclamp()
    {
        animator.SetBool("isClamped", false);
        yield return new WaitForSeconds(1);
        releaseSofaPlier();
    }

    void OnDrawGizmosSelected()
    {
        if (m_idGrabed == null || ModelVisu == null)
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
