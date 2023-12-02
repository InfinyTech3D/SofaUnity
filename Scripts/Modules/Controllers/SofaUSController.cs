using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SofaUSController : MonoBehaviour
{
    public GameObject m_sofaTransducer = null;
    protected SofaMesh m_sofaUSController = null;


    protected bool m_isReady = false;
    protected Vector3 unityToSofa;
    protected Vector3 sofaToUnity;
    protected Vector3 capsuleOri = Vector3.zero;

    protected float[] sofaVertices;

    // Start is called before the first frame update
    void Start()
    {
        if (m_sofaTransducer == null)
        {
            Debug.LogError("Sofa Trasnducer GameObject not set.");
            return;
        }
        else
        {
            m_sofaUSController = m_sofaTransducer.GetComponent<SofaMesh>();
            if (m_sofaUSController == null)
            {
                Debug.LogError("No USTransducer component found in GameObject.");
                return;
            }
        }

        int nbrV = m_sofaUSController.NbVertices();
        if (nbrV != 1)
        {
            Debug.LogError("Support only single rigid object for the moment");
            return;
        }
        sofaToUnity = m_sofaUSController.m_sofaContext.GetScaleSofaToUnity();
        unityToSofa = m_sofaUSController.m_sofaContext.GetScaleUnityToSofa();

        Debug.Log("sofaToUnity: " + sofaToUnity);
        sofaVertices = m_sofaUSController.SofaMeshTopology.m_vertexBuffer;
        this.transform.position = new Vector3(sofaVertices[0] * sofaToUnity[0], sofaVertices[1] * sofaToUnity[1], sofaVertices[2] * sofaToUnity[2]);
        this.transform.rotation = new Quaternion(sofaVertices[3], sofaVertices[4], sofaVertices[5], sofaVertices[6]);
        m_isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isReady)
            return;

        //sofaVertices = m_sofaUSController.SofaMeshTopology.m_vertexBuffer;
        //Debug.Log("sofaVertices: " + sofaVertices[0] + " " + sofaVertices[1] + " " + sofaVertices[2] + " " + sofaVertices[3] + " " + sofaVertices[4] + " " + sofaVertices[5] + " " + sofaVertices[6]);

        Vector3 position = this.transform.position;
        Quaternion orientation = this.transform.rotation;

        sofaVertices[0] = position[0] * unityToSofa[0];
        sofaVertices[1] = position[1] * unityToSofa[1];
        sofaVertices[2] = position[2] * unityToSofa[2];

        sofaVertices[3] = orientation[0];
        sofaVertices[4] = orientation[1];
        sofaVertices[5] = orientation[2];
        sofaVertices[6] = orientation[3];
        
        //Debug.Log("pos: " + position + " oriu: " + orientation);
        m_sofaUSController.SetPositions(sofaVertices);
    }
}
