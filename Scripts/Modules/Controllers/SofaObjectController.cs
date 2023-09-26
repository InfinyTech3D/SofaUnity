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

    /// Parameter bool to store information if vec3 or rigid are parsed.
    [SerializeField]
    public bool isRigidMesh = false;

    private bool m_ready = false;
    private Vector3 unityToSofa;
    private Vector3 sofaToUnity;

    private Vector3 objectOri = Vector3.zero;
    private Vector3[] newPosition;
    private Vector3[] stopVelocity;

    private float[] newPositionRigid;
    private float[] stopVelocityRigid;

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

        newPositionRigid = new float[7];
        stopVelocityRigid = new float[7];
        for (int i = 0; i < 6; i++)
        {
            newPositionRigid[i] = 0;
            stopVelocityRigid[i] = 0;
        }
        newPositionRigid[6] = 1;
        stopVelocityRigid[6] = 0;

        m_ready = true;
    }

    // Update is called once per frame
    void FixedUpdate()
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyAction();
        }

        UpdateFromSofa();
    }

    protected void UpdateFromSofa()
    {
        int nbrV = m_sofaMesh.NbVertices();
        float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;

        for (int i = 0; i < 3; i++)
        {
            objectOri[i] = sofaVertices[i] * sofaToUnity[i];
        }

        this.transform.position = objectOri;
    }

    public void MoveForward()
    {
        newPosition[0] = objectOri - this.transform.up * m_speed;
        UpdateToSofa(newPosition[0]);        
    }

    public void MoveBackward()
    {
        newPosition[0] = objectOri + this.transform.up * m_speed;
        UpdateToSofa(newPosition[0]);
    }




    public void MoveUp()
    {
        newPosition[0] = objectOri + this.transform.forward * m_speed;
        UpdateToSofa(newPosition[0]);
    }

    public void MoveDown()
    {
        newPosition[0] = objectOri - this.transform.forward * m_speed;
        UpdateToSofa(newPosition[0]);
    }

    public void MoveLeft()
    {
        newPosition[0] = objectOri + this.transform.right * m_speed;
        UpdateToSofa(newPosition[0]);
    }

    public void MoveRight()
    {
        newPosition[0] = objectOri - this.transform.right * m_speed;
        UpdateToSofa(newPosition[0]);
    }

    public void ApplyAction()
    {
        if (m_value != null)
        {
            valueTracker = !valueTracker;
            m_value.Value = valueTracker;
        }
    }

    protected void UpdateToSofa(Vector3 my_newPosition)
    {
        if (isRigidMesh)
        {
            newPositionRigid[0] = my_newPosition[0];
            newPositionRigid[1] = my_newPosition[1];
            newPositionRigid[2] = my_newPosition[2];
            m_sofaMesh.SetVelocities(stopVelocityRigid);
            m_sofaMesh.SetPositions(newPositionRigid);
        }
        else
        {
            m_sofaMesh.SetVelocities(stopVelocity);
            m_sofaMesh.SetVertices(newPosition);
        }

        this.transform.position = my_newPosition;
        objectOri = my_newPosition;
    }
}
