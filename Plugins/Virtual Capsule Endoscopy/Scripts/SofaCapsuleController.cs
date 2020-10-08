using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaCapsuleController : MonoBehaviour
{
    public SofaMesh m_sofaCapsuleMesh = null;
    public float m_speed = 0.1f;

    protected bool m_ready = false;

    protected Vector3 unityToSofa;
    protected Vector3 sofaToUnity;
    protected Vector3 capsuleOri = Vector3.zero;
    //protected Vector3 capsuleTip = Vector3.zero;

    protected Vector3[] newPosition;

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

        Debug.Log("nbrV: " + nbrV);
        m_sofaCapsuleMesh.AddListener();

        sofaToUnity = m_sofaCapsuleMesh.m_sofaContext.GetScaleSofaToUnity();
        unityToSofa = m_sofaCapsuleMesh.m_sofaContext.GetScaleUnityToSofa();
        newPosition = new Vector3[1];
        m_ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ready)
            return;

        //m_sofaCapsuleMesh.SofaMeshTopology.

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.Keypad5))
                MoveBackward();
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad5))
                MoveForward();
        }

        if (Input.GetKey(KeyCode.Keypad4))
            TurnLeft();
        else if (Input.GetKey(KeyCode.Keypad6))
            TurnRight();
        else if (Input.GetKey(KeyCode.Keypad8))
            RotateUp();
        else if (Input.GetKey(KeyCode.Keypad2))
            RotateDown();
        else if (Input.GetKey(KeyCode.Keypad7))
            RotateLeft();
        else if (Input.GetKey(KeyCode.Keypad9))
            RotateRight();
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
        newPosition[0] = capsuleOri + this.transform.up * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void MoveBackward()
    {
        newPosition[0] = capsuleOri - this.transform.up * m_speed;
        m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void RotateUp()
    {
        //Vector3 center = (capsuleOri + capsuleTip) * 0.5f;
        transform.Rotate(this.transform.right, m_speed * 57.3f, Space.Self);

        //newPosition[0] = capsuleOri - this.transform.forward * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void RotateDown()
    {
        transform.Rotate(this.transform.right, -m_speed * 57.3f, Space.Self);
        //newPosition[0] = capsuleOri + this.transform.forward * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void TurnLeft()
    {
        transform.Rotate(this.transform.forward, -m_speed * 57.3f, Space.Self);
        //newPosition[0] = capsuleOri - this.transform.right * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void TurnRight()
    {
        transform.Rotate(this.transform.forward, m_speed * 57.3f, Space.Self);
        //newPosition[0] = capsuleOri + this.transform.right * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void RotateLeft()
    {
        transform.Rotate(this.transform.up, -m_speed * 57.3f, Space.Self);
        //newPosition[0] = capsuleOri - this.transform.right * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }

    protected void RotateRight()
    {
        transform.Rotate(this.transform.up, m_speed * 57.3f, Space.Self);
        //newPosition[0] = capsuleOri + this.transform.right * m_speed;
        //m_sofaCapsuleMesh.SetVertices(newPosition);
    }



    //protected void MoveUp()
    //{
    //    newPosition[0] = capsuleOri + this.transform.forward * m_speed;
    //    m_sofaCapsuleMesh.SetVertices(newPosition);
    //}

    //protected void MoveDown()
    //{
    //    newPosition[0] = capsuleOri - this.transform.forward * m_speed;
    //    m_sofaCapsuleMesh.SetVertices(newPosition);
    //}

    //protected void MoveLeft()
    //{
    //    newPosition[0] = capsuleOri + this.transform.right * m_speed;
    //    m_sofaCapsuleMesh.SetVertices(newPosition);
    //}

    //protected void MoveRight()
    //{
    //    newPosition[0] = capsuleOri - this.transform.right * m_speed;
    //    m_sofaCapsuleMesh.SetVertices(newPosition);
    //}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(capsuleOri, 0.1f);

        //Gizmos.color = Color.red;
       // Gizmos.DrawSphere(capsuleTip, 0.1f);
    }
}
