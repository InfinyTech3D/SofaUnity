using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
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

            /// Keys for BeamAdapter:
            // moving tool up, down, left, right: 19, 21, 18, 20
            if (Input.GetKeyDown(KeyCode.LeftArrow)) m_sofaContext.SofaKeyPressEvent(18);
            if (Input.GetKeyDown(KeyCode.RightArrow)) m_sofaContext.SofaKeyPressEvent(20);
            if (Input.GetKeyDown(KeyCode.UpArrow)) m_sofaContext.SofaKeyPressEvent(19);
            if (Input.GetKeyDown(KeyCode.DownArrow)) m_sofaContext.SofaKeyPressEvent(21);

            if (Input.GetKeyUp(KeyCode.LeftArrow)) m_sofaContext.SofaKeyReleaseEvent(18);
            if (Input.GetKeyUp(KeyCode.RightArrow)) m_sofaContext.SofaKeyReleaseEvent(20);
            if (Input.GetKeyUp(KeyCode.UpArrow)) m_sofaContext.SofaKeyReleaseEvent(19);
            if (Input.GetKeyUp(KeyCode.DownArrow)) m_sofaContext.SofaKeyReleaseEvent(21);

            // key to switch tool: 0, 1, 2
            if (Input.GetKeyDown(KeyCode.Alpha0)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.Alpha0);
            if (Input.GetKeyDown(KeyCode.Alpha1)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.Alpha1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.Alpha2);

            if (Input.GetKeyUp(KeyCode.Alpha0)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.Alpha0);
            if (Input.GetKeyUp(KeyCode.Alpha1)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.Alpha1);
            if (Input.GetKeyUp(KeyCode.Alpha2)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.Alpha2);

            // other keys, E export, D drop tool
            if (Input.GetKeyDown(KeyCode.E)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.E);
            if (Input.GetKeyDown(KeyCode.D)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.D);

            if (Input.GetKeyUp(KeyCode.E)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.E);
            if (Input.GetKeyUp(KeyCode.D)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.D);


            /// Other keys for empty slots
            if (Input.GetKeyDown(KeyCode.F)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.F);
            if (Input.GetKeyDown(KeyCode.C)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.C);
            if (Input.GetKeyDown(KeyCode.Space)) m_sofaContext.SofaKeyPressEvent((int)KeyCode.Space);
            
            if (Input.GetKeyUp(KeyCode.F)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.F);
            if (Input.GetKeyUp(KeyCode.C)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.C);
            if (Input.GetKeyUp(KeyCode.Space)) m_sofaContext.SofaKeyReleaseEvent((int)KeyCode.Space);
        }
    }
}
