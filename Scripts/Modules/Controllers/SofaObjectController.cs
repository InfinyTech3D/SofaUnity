using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaObjectController : MonoBehaviour
{
    public SofaMesh m_sofaMesh = null;
    public SofaBaseComponent m_sofaComponent = null;
    public string m_dataName = "";
    public float m_speed = 0.1f;


    private bool m_ready = false;
    private Vector3 unityToSofa;
    private Vector3 sofaToUnity;

    private Vector3 objectOri = Vector3.zero;
    private Vector3[] newPosition;
    private Vector3[] stopVelocity;
    private bool valueTracker;
    private SofaBoolData m_value = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_sofaMesh == null)
        {
            Debug.LogError("m_sofaMesh is not set.");
            m_ready = false;
            return;
        }
        

        if (m_dataName.Length > 0)
        {
            if (m_sofaComponent == null)
            {
                m_sofaComponent = m_sofaMesh;
            }

            m_value = m_sofaComponent.m_dataArchiver.GetSofaBoolData(m_dataName);
            if (m_value != null)
            {
                valueTracker = m_value.Value;
            }
        }

        int nbrV = m_sofaMesh.NbVertices();

        if (nbrV != 1)
        {
            Debug.LogError("This controller can only act on rigid object with a unique coordinate position.");
            m_ready = false;
            return;
        }

        m_sofaMesh.AddListener();

        sofaToUnity = m_sofaMesh.m_sofaContext.GetScaleSofaToUnity();
        unityToSofa = m_sofaMesh.m_sofaContext.GetScaleUnityToSofa();
        newPosition = new Vector3[1];
        stopVelocity = new Vector3[1];
        stopVelocity[0] = Vector3.zero;
        m_ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ready)
            return;

        if (Input.GetKey(KeyCode.Keypad5))
        {
            MoveBackward();
        }
        else if (Input.GetKey(KeyCode.Keypad0))
            MoveForward();
        else if (Input.GetKey(KeyCode.Keypad6))
            MoveRight();
        else if (Input.GetKey(KeyCode.Keypad4))
            MoveLeft();
        else if (Input.GetKey(KeyCode.Keypad8))
            MoveUp();
        else if (Input.GetKey(KeyCode.Keypad2))
            MoveDown();

        if (Input.GetKeyDown(KeyCode.Space) && m_value != null)
        {
            valueTracker = !valueTracker;
            m_value.Value = valueTracker;
        }
    }

    void FixedUpdate()
    {
        if (!m_ready)
            return;

        UpdateCapsuleFromSofa();
    }


    protected void UpdateCapsuleFromSofa()
    {
        int nbrV = m_sofaMesh.NbVertices();
        float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;

        for (int i = 0; i < 3; i++)
        {
            objectOri[i] = sofaVertices[i] * sofaToUnity[i];
        }

        this.transform.position = objectOri;
    }

    protected void MoveForward()
    {
        newPosition[0] = objectOri - this.transform.up * m_speed;
        m_sofaMesh.SetVelocities(stopVelocity);
        m_sofaMesh.SetVertices(newPosition);
    }

    protected void MoveBackward()
    {
        newPosition[0] = objectOri + this.transform.up * m_speed;
        m_sofaMesh.SetVelocities(stopVelocity);
        m_sofaMesh.SetVertices(newPosition);
    }




    protected void MoveUp()
    {
        newPosition[0] = objectOri + this.transform.forward * m_speed;
        m_sofaMesh.SetVertices(newPosition);
    }

    protected void MoveDown()
    {
        newPosition[0] = objectOri - this.transform.forward * m_speed;
        m_sofaMesh.SetVertices(newPosition);
    }

    protected void MoveLeft()
    {
        newPosition[0] = objectOri + this.transform.right * m_speed;
        m_sofaMesh.SetVertices(newPosition);
    }

    protected void MoveRight()
    {
        newPosition[0] = objectOri - this.transform.right * m_speed;
        m_sofaMesh.SetVertices(newPosition);
    }
}
