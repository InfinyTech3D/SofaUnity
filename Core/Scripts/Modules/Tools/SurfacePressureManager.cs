using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class SurfacePressureManager : MonoBehaviour
{
    // Input parameter for the manager
    public SofaFEMForceField m_pressureFF = null;
    public float pressureValue = 20.0f;
    public List<int> trianglesIndices = new List<int>();

    // Internal parameters
    protected bool m_isReady = false;
    protected int m_stateCounter = 0;

    // Sofa Data pointers
    protected SofaDoubleData m_pressureData = null;
    protected SofaDataVectorInt m_trianglesIndices = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_pressureFF == null)
        {
            Debug.LogError("No Pressure ForceField set.");
            m_isReady = false;
            return;
        }

        // Store pointer to Unity SOFA Data objects
        // Double value, the conversion from float to double is automatically done.
        m_pressureData = m_pressureFF.m_dataArchiver.GetSofaDoubleData("pressure");
        // Data of a vector of int. Same conversion from unisgned int to int is automatically done. Unsinged int is not handle in Unity.
        m_trianglesIndices = (SofaDataVectorInt)m_pressureFF.m_dataArchiver.GetVectorData("triangleIndices");

        // better to store the value, because each call to GetSize, GetValue, etc... launch a request to SOFA.
        int nbrIndices = m_trianglesIndices.GetSize();

        Debug.Log("pressure value: " + m_pressureData.Value);
        Debug.Log("Nbr triangles indices: " + nbrIndices);

        // Get the potential values. 
        if (nbrIndices > 0)
        {
            // Need to allocated buffer and send it to SOFA
            int[] values = new int[nbrIndices];
            m_trianglesIndices.GetValues(values);
            trianglesIndices.Clear();
            for (int i=0; i< nbrIndices; ++i)
            {
                trianglesIndices.Add(values[i]);
            }
        }

        m_isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isReady)
            return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space bar touched");
            if (m_stateCounter == 0) // first step, apply some pressure
            {
                Debug.Log("Apply first pressure");
                // only set the pressure here. Will use indices already set in the scene file
                m_pressureData.Value = pressureValue;                

                m_stateCounter++;
            }
            else if (m_stateCounter == 1) // second step, another pressure set
            {
                Debug.Log("Apply second pressure");
                m_pressureData.Value = pressureValue;
                int[] values = new int[8] { 836, 840, 841, 830, 822, 977, 813, 807 };
                m_trianglesIndices.SetValue(values, 8);

                m_stateCounter++;
            }
            else if (m_stateCounter == 2) // third step, reapply pressure set
            {
                Debug.Log("Apply third pressure");
                m_pressureData.Value = pressureValue;

                // Get values from unity UI, they were set to the on in scene file at Start, but can be changed interactively
                int nbrIndices = trianglesIndices.Count;
                int[] values = new int[nbrIndices];
                for (int i = 0; i < nbrIndices; i++)
                    values[i] = trianglesIndices[i];
                
                m_trianglesIndices.SetValue(values, nbrIndices);
                m_stateCounter++;
            }
            else // release pressure
            {
                Debug.Log("Release pressure");
                m_stateCounter = 0;
                m_pressureData.Value = 0.0f;
            }
        }
    }
}
