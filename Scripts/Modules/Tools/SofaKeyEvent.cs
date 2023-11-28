using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaKeyEvent : MonoBehaviour
{
    /// Pointer to the Sofa context this GameObject belongs to.
    public SofaUnity.SofaContext m_sofaContext = null;

    public bool m_isListening = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_sofaContext == null)
        {
            GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
            if (_contextObject != null)
            {
                // Get Sofa context
                m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
            }
            else
            {
                Debug.LogError("RayCaster::loadContext - No SofaContext found.");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isListening == false || m_sofaContext == null)
            return;

        if (Input.GetKey(KeyCode.LeftArrow))
            m_sofaContext.SofaKeyPressEvent(18);

        if (Input.GetKey(KeyCode.RightArrow))
            m_sofaContext.SofaKeyPressEvent(20);

        if (Input.GetKey(KeyCode.UpArrow))
            m_sofaContext.SofaKeyPressEvent(19);

        if (Input.GetKey(KeyCode.DownArrow))
            m_sofaContext.SofaKeyPressEvent(21);


        if (Input.GetKeyUp(KeyCode.LeftArrow))
            m_sofaContext.SofaKeyReleaseEvent(18);

        if (Input.GetKeyUp(KeyCode.RightArrow))
            m_sofaContext.SofaKeyReleaseEvent(20);

        if (Input.GetKeyUp(KeyCode.UpArrow))
            m_sofaContext.SofaKeyReleaseEvent(19);

        if (Input.GetKeyUp(KeyCode.DownArrow))
            m_sofaContext.SofaKeyReleaseEvent(21);
    }
}
