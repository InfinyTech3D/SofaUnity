using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Unity script to be set on an empty GameObject to controll the SOFA Haply Inverse3 component behavior
    /// inside the SOFA simulation.
    /// By default tool collision is deactivated at start. Press space bar to activate/unactivate the collision
    /// This component can also change the Force Feedback value and display current force sent to the device.
    /// </summary>
    public class SofaHaplyRoboticsController : MonoBehaviour
    {

        /// Pointer to the Sofa context this GameObject belongs to.
        protected SofaUnity.SofaContext m_sofaContext = null;

        /// Pointer to the SOFA Haply component
        [SerializeField]
        public SofaComponent m_sofaInverse3_controller = null;
        /// Pointer to the SOFA ForceFeedback component
        public SofaComponent m_LCPForce = null;
        /// Pointer to the sphere collision of the tool tip
        public SofaCollisionModel m_toolCollisionModel = null;
        /// Pointer to the rigid mesh of the tip
        public SofaMesh m_sofaMesh = null;


        /// Unity Parameter to activate/unactivate the collision
        public bool collisionActive = false;
        /// Pointer to the equivalent SOFA collision status. Create 2 objects to avoid sending too many (set/get) request to SOFA
        protected SofaBoolData m_collisionState = null;


        /// Unity parameter to change the force feedback coefficient
        public float forceFeedBackCoef = 0.0f;
        /// Pointer to the equivalent SOFA FFBack coefficient. Create 2 objects to avoid sending too many (set/get) request to SOFA
        protected SofaDoubleData m_ffBackCoef = null;



        /// Bool to store a toogle status value when pressing 1st button.
        protected bool m_statusButton = false;

        protected SofaBoolData m_statusButtonData = null;

        /// Accessor to know 1st button toogle status. @sa m_statusButton1
        public bool ButtonStatus() { return m_statusButton; }


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

        /// Bool to know if tool is currently in contact, sending some forcefeedback.
        //protected bool m_toolInContact = false;



        /// Raw position for RigidCoord position of the device
        private float[] sofaPositionRigid;

        


        /// Getter/setter to the GeomagicDriver SofaComponent object.
        public SofaComponent Inverse3Driver
        {
            get { return m_sofaInverse3_controller; }
            set
            {
                if (value != m_sofaInverse3_controller)
                {
                    m_sofaInverse3_controller = value;
                    CreateDeviceController();
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Retrive the SOFA components
            if (!m_sofaInverse3_controller || !m_LCPForce)
            {
                int found = 0;
                SofaComponent[] components = GameObject.FindObjectsByType<SofaComponent>(FindObjectsSortMode.None);
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
                Debug.LogWarning("SofaHaplyRobotics Error: pointer to SOFA Inverse3 controller is missing.");
                m_isReady = false;
                return;
            }

            if (!m_LCPForce)
            {
                Debug.LogWarning("SofaHaplyRobotics Error: pointer to SOFA LCPForceFeedBack is missing.");
                m_isReady = false;
                return;
            }

            if (m_toolCollisionModel)
            {                
                // Retrieve Collision status access and value
                m_collisionState = m_toolCollisionModel.m_dataArchiver.GetSofaBoolData("active");
                
                // deactivate collision at start
                m_collisionState.Value = false;
                collisionActive = m_collisionState.Value;
            }



            // Retrieve FFBack access and value
            m_ffBackCoef = m_LCPForce.m_dataArchiver.GetSofaDoubleData("forceCoef");
            forceFeedBackCoef = m_ffBackCoef.Value;
            oldCoef = forceFeedBackCoef;

            m_rawForceValue = m_sofaInverse3_controller.m_dataArchiver.GetSofaVec3Data("rawForceDevice");
            m_statusButtonData = m_sofaInverse3_controller.m_dataArchiver.GetSofaBoolData("handleButton");

            m_sofaInverse3_controller.m_dataArchiver.PrintAllDataAndTypes();



            CreateDeviceController();

            sofaPositionRigid = new float[7];
            for (int i = 0; i < 6; i++)
            {
                sofaPositionRigid[i] = 0;
            }
            sofaPositionRigid[6] = 1;

            m_isReady = true;
        }

        /// Main method to create the controller on the SOFA side.
        protected void CreateDeviceController()
        {
            if (m_sofaInverse3_controller == null)
                return;

            m_sofaContext = m_sofaInverse3_controller.m_sofaContext;
            //bool contextOk = true;
            //if (m_sofaContext == null)
            //{
            //    Debug.LogError("m_sofaInverse3_controller::loadContext - GetComponent<SofaUnity.SofaContext> failed.");
            //    contextOk = false;
            //}

            //if (contextOk)
            //{
            //    m_sofaInverse3 = new SofaHaplyRobotics(m_sofaContext.GetSimuContext(), m_sofaInverse3_controller.DisplayName);
            //}
        }

        void FixedUpdate()
        {
            if (!m_isReady)
                return;

            UpdateFromSofa();
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


        protected void UpdateFromSofa()
        {
            // 1. Update device position from SOFA
            if (m_sofaMesh != null)
            {
                m_sofaMesh.GetRawPositions(sofaPositionRigid);
            }
            else
            {
                m_sofaInverse3_controller.m_impl.GetRigidValue("positionDevice", sofaPositionRigid, true);
            }

            // Get raw values from SOFA, need to inverse left-right hand coordinate system
            Vector3 worldPos = new Vector3(-sofaPositionRigid[0], sofaPositionRigid[1], sofaPositionRigid[2]);
            // Project world position into SofaContext frame
            Vector3 localPos = this.m_sofaContext.transform.TransformPoint(worldPos);
            this.transform.localPosition = localPos;

            // Get SOFA quaternion and inverse rotation
            var rotation = new Quaternion(sofaPositionRigid[3], sofaPositionRigid[4], sofaPositionRigid[5], sofaPositionRigid[6]);
            Vector3 angles = rotation.eulerAngles;
            this.transform.localEulerAngles = new Vector3(angles[0], -angles[1], -angles[2]);

            // Combine current rotation with SofaContext one
            this.transform.rotation = this.m_sofaContext.transform.rotation * this.transform.rotation;


            // 2. Update tool button status            
            m_statusButton = m_statusButtonData.Value;
            
        }

    }
}
