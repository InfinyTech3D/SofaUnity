using System;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Script to Add to a GameOject in order to move it using the keybord.
/// I = move forward
/// K = move backword
/// U = move left
/// O = move right
/// J = move Up
/// L = move down
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// Pointer to the current Mesh of the GameObject
    SBaseMesh m_mesh;
    void Start()
    {
        // Get the Mesh
        m_mesh = GetComponent<SBaseMesh>();
    }

    /// Method calle at each update, will move the object regardings keys pushed.
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.J))
            m_mesh.rotation = new Vector3(m_mesh.rotation.x, m_mesh.rotation.y, m_mesh.rotation.z + 0.1f);
        else if (Input.GetKey(KeyCode.L))
            m_mesh.rotation = new Vector3(m_mesh.rotation.x, m_mesh.rotation.y, m_mesh.rotation.z - 0.1f);
        else if (Input.GetKey(KeyCode.U))
            m_mesh.rotation = new Vector3(m_mesh.rotation.x - 0.1f, m_mesh.rotation.y, m_mesh.rotation.z);
        else if (Input.GetKey(KeyCode.O))
            m_mesh.rotation = new Vector3(m_mesh.rotation.x + 0.1f, m_mesh.rotation.y, m_mesh.rotation.z);
        else if (Input.GetKey(KeyCode.K))
        {
            double angle = Math.PI * 0.77f;
            Vector3 trans = m_mesh.translation;
            trans.y += (float)Math.Sin(angle) * 0.05f;
            trans.z += (float)Math.Cos(angle) * 0.05f;
            m_mesh.translation = trans;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            double angle = Math.PI * 0.77f;
            Vector3 trans = m_mesh.translation;
            trans.y -= (float)Math.Sin(angle) * 0.05f;
            trans.z -= (float)Math.Cos(angle) * 0.05f;
            m_mesh.translation = trans;
        }

    }
    
}
