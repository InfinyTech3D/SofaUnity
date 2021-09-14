using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaCapsuleController : MonoBehaviour
{
    public SofaMesh m_sofaCapsuleMesh = null;
    public float m_speed = 0.1f;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    protected bool m_ready = false;

    protected Vector3 unityToSofa;
    protected Vector3 sofaToUnity;
    protected Vector3 capsuleOri = Vector3.zero;
    //protected Vector3 capsuleTip = Vector3.zero;

    protected Vector3[] newPosition;
    protected Vector3[] stopVelocity;

    // Start is called before the first frame update
    void Start()
    {
        if (m_sofaCapsuleMesh == null)
        {
            Debug.LogError("m_sofaCapsuleMesh is not set.");
            m_ready = false;
            return;
        }

        int nbrV = m_sofaCapsuleMesh.NbVertices();

        if (nbrV != 1)
        {
            Debug.LogError("Not the good number of vertices found in m_sofaCapsuleMesh.");
            m_ready = false;
            return;
        }

        m_sofaCapsuleMesh.AddListener();

        sofaToUnity = m_sofaCapsuleMesh.m_sofaContext.GetScaleSofaToUnity();
        unityToSofa = m_sofaCapsuleMesh.m_sofaContext.GetScaleUnityToSofa();
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

        //m_sofaCapsuleMesh.SofaMeshTopology.

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            yaw -= speedH * Input.GetAxis("Mouse X");
            pitch += speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if (Input.GetKey(KeyCode.Keypad5))
                MoveForward();
        }


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
    }

    void FixedUpdate()
    {
        if (!m_ready)
            return;

        UpdateCapsuleFromSofa();
    }


    protected void UpdateCapsuleFromSofa()
    {
        int nbrV = m_sofaCapsuleMesh.NbVertices();
        float[] sofaVertices = m_sofaCapsuleMesh.SofaMeshTopology.m_vertexBuffer;

        for (int i=0; i<3; i++)
        {
            capsuleOri[i] = sofaVertices[i] * sofaToUnity[i];
        }

        this.transform.position = capsuleOri;
    }

    protected void MoveForward()
    {
        newPosition[0] = capsuleOri - this.transform.up * m_speed;
        m_sofaCapsuleMesh.SetVelocities(stopVelocity);
        m_sofaCapsuleMesh.SetVertices(newPosition);        
    }

    protected void MoveBackward()
    {
        newPosition[0] = capsuleOri + this.transform.up * m_speed;
        m_sofaCapsuleMesh.SetVelocities(stopVelocity);
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    


    protected void MoveUp()
    {
        newPosition[0] = capsuleOri + this.transform.forward * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void MoveDown()
    {
        newPosition[0] = capsuleOri - this.transform.forward * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void MoveLeft()
    {
        newPosition[0] = capsuleOri + this.transform.right * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void MoveRight()
    {
        newPosition[0] = capsuleOri - this.transform.right * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }


    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(capsuleOri, 0.1f);

    //    //Gizmos.color = Color.red;
    //   // Gizmos.DrawSphere(capsuleTip, 0.1f);
    //}
}
