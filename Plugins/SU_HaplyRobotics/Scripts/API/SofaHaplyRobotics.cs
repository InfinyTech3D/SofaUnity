using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Unity script to be set on an empty GameObject to controll the SOFA Haply Inverse3 component behavior
/// inside the SOFA simulation.
/// By default tool collision is deactivated at start. Press space bar to activate/unactivate the collision
/// This component can also change the Force Feedback value and display current force sent to the device.
/// </summary>
public class SofaHaplyRobotics : MonoBehaviour
{
    /// Pointer to the SOFA Haply component
    public SofaComponent m_sofaInverse3_controller = null;
    /// Pointer to the SOFA ForceFeedback component
    public SofaComponent m_LCPForce = null;
    /// Pointer to the sphere collision of the tool tip
    public SofaCollisionModel m_toolCollisionModel = null;

    /// Unity Parameter to activate/unactivate the collision
    public bool collisionActive = false;
    /// Pointer to the equivalent SOFA collision status. Create 2 objects to avoid sending too many (set/get) request to SOFA
    protected SofaBoolData m_collisionState = null;


    /// Unity parameter to change the force feedback coefficient
    public float forceFeedBackCoef = 0.0f;
    /// Pointer to the equivalent SOFA FFBack coefficient. Create 2 objects to avoid sending too many (set/get) request to SOFA
    protected SofaDoubleData m_ffBackCoef = null;


    /// Parameter to activate/unactivate the force display (sent to the device)
    public bool dumpForce = false;
    /// Unity Vec3 to to display the raw force sent to the device
    public Vector3 rawForce;
    /// Pointer to the equivalent SOFA Force value. Create 2 objects to avoid sending too many (set/get) request to SOFA
    protected SofaVec3Data m_rawForceValue = null;


    /// Internal member for GameObject status
    protected bool m_isReady = false;
    /// Internal value to detect change of coef
    protected float oldCoef = 0.0f;
    
    /// For debug: min and max bounding box value the device can access (computed at start)
    public Vector3 minBB, maxBB;


    // Start is called before the first frame update
    void Start()
    {
        // Retrive the SOFA components
        if (!m_sofaInverse3_controller || !m_LCPForce)
        {
            int found = 0;
            SofaComponent[] components = GameObject.FindObjectsOfType<SofaComponent>();
            foreach (SofaComponent comp in components)
            {
                if (comp.m_componentType == "Haply_Inverse3Controller")
                {
                    m_sofaInverse3_controller = comp;
                    found++;
                }

                if (comp.m_componentType == "LCPForceFeedback")
                {
                    m_LCPForce = comp;
                    found++;
                }

                if (found == 2)
                    break;
            }
        }

        if (!m_sofaInverse3_controller)
        {
            Debug.LogError("SofaHaplyRobotics Error: pointer to SOFA Inverse3 controller is missing.");
            m_isReady = false;
            return;
        }

        if (!m_LCPForce)
        {
            Debug.LogError("SofaHaplyRobotics Error: pointer to SOFA LCPForceFeedBack is missing.");
            m_isReady = false;
            return;
        }

        if (!m_toolCollisionModel)
        {
            SofaCollisionModel[] components = GameObject.FindObjectsOfType<SofaCollisionModel>();
            foreach (SofaCollisionModel comp in components)
            {
                if (comp.DisplayName == "ToolCollisionModel")
                {
                    m_toolCollisionModel = comp;
                    break;
                }
            }

            if (!m_toolCollisionModel) // still not found
            {
                Debug.LogError("SofaHaplyRobotics Error: pointer to tool SOFA collision model is missing.");
                m_isReady = false;
                return;
            }
        }

        // Retrieve Collision status access and value
        m_collisionState = m_toolCollisionModel.m_dataArchiver.GetSofaBoolData("active");
        collisionActive = m_collisionState.Value;

        // Retrieve FFBack access and value
        m_ffBackCoef = m_LCPForce.m_dataArchiver.GetSofaDoubleData("forceCoef");
        forceFeedBackCoef = m_ffBackCoef.Value;
        oldCoef = forceFeedBackCoef;

        // Retrieve BBbox of the device for debug display
        minBB = m_sofaInverse3_controller.m_dataArchiver.GetSofaVec3Data("fullBBmins").Value;
        maxBB = m_sofaInverse3_controller.m_dataArchiver.GetSofaVec3Data("fullBBmaxs").Value;

        m_rawForceValue = m_sofaInverse3_controller.m_dataArchiver.GetSofaVec3Data("rawForceDevice");

        m_isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isReady)
            return;

        if (Input.GetKeyUp(KeyCode.Space)) // activate/unactivate device collision.
        {
            collisionActive = !collisionActive;
            m_collisionState.Value = collisionActive;
        }

        if (oldCoef != forceFeedBackCoef)
        {
            m_ffBackCoef.Value = forceFeedBackCoef;
            oldCoef = forceFeedBackCoef;
        }

        if (dumpForce)
        {
            if (m_rawForceValue.CheckIfDirty())
                rawForce = m_rawForceValue.Value;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!m_sofaInverse3_controller)
            return;

        // Draw BB
        Gizmos.color = Color.red;
        Vector3 scale = m_sofaInverse3_controller.m_sofaContext.GetScaleSofaToUnity();
        Vector3 pA = new Vector3(minBB.x * scale.x, minBB.y * scale.y, minBB.z * scale.z);
        Vector3 pB = new Vector3(maxBB.x * scale.x, minBB.y * scale.y, minBB.z * scale.z);
        Vector3 pC = new Vector3(maxBB.x * scale.x, maxBB.y * scale.y, minBB.z * scale.z);
        Vector3 pD = new Vector3(minBB.x * scale.x, maxBB.y * scale.y, minBB.z * scale.z);

        Vector3 pE = new Vector3(minBB.x * scale.x, minBB.y * scale.y, maxBB.z * scale.z);
        Vector3 pF = new Vector3(maxBB.x * scale.x, minBB.y * scale.y, maxBB.z * scale.z);
        Vector3 pG = new Vector3(maxBB.x * scale.x, maxBB.y * scale.y, maxBB.z * scale.z);
        Vector3 pH = new Vector3(minBB.x * scale.x, maxBB.y * scale.y, maxBB.z * scale.z);

        // Draw BB
        Gizmos.DrawLine(pA, pB);
        Gizmos.DrawLine(pB, pC);
        Gizmos.DrawLine(pA, pD);
        Gizmos.DrawLine(pC, pD);

        Gizmos.DrawLine(pE, pF);
        Gizmos.DrawLine(pF, pG);
        Gizmos.DrawLine(pG, pH);
        Gizmos.DrawLine(pH, pE);

        Gizmos.DrawLine(pA, pE);
        Gizmos.DrawLine(pB, pF);
        Gizmos.DrawLine(pD, pH);
        Gizmos.DrawLine(pC, pG);
    }
}
