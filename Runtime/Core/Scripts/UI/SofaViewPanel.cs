using System.Collections;
using System.Collections.Generic;
using SofaUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SofaUnity
{
    public class SofaViewPanel : MonoBehaviour
    {
        public Toggle m_visuModel;
        public Toggle m_visuFEM;
        public Toggle m_visuSprings;
        public Toggle m_visuCollision;
        public Toggle m_visuCollisionOutputs;

        protected bool m_isReady = false;
        protected SofaContext m_sofaContext = null;


        protected List<SofaVisualModel> m_visualModels = null;
        protected List<SofaFEMForceField> m_forceFields = null;
        protected List<SofaCollisionModel> m_collisionModels = null;
        protected SofaCollisionDisplay m_collisionDisplay = null;


        /// Number of visual meshes in the scene.
        protected int m_nbrVModel = 0;
        /// Total number of vertices over all visual meshes.
        protected int m_nbrVVertices = 0;
        /// Total number of triangles over all visual meshes.
        protected int m_nbrVTriangles = 0;

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
                    Debug.LogError("SofaPlayer::Start - No SofaContext found.");
                    m_isReady = false;
                    return;
                }
            }

            m_collisionDisplay = GameObject.FindObjectOfType<SofaCollisionDisplay>();

            m_isReady = true;

            ParseSceneModels();
        }

        protected void ParseSceneModels()
        {
            if (!m_isReady)
                return;

            m_visualModels = new List<SofaVisualModel>();
            m_forceFields = new List<SofaFEMForceField>();
            m_collisionModels = new List<SofaCollisionModel>();

            foreach (Transform child in m_sofaContext.transform)
            {
                ParseChildrenModels(child);
                SofaVisualModel objV = child.GetComponent<SofaVisualModel>();
                if (objV != null)
                {
                    m_visualModels.Add(objV);
                    m_nbrVModel++;
                    m_nbrVVertices += objV.NbVertices();
                    m_nbrVTriangles += objV.NbTriangles();
                }

                SofaFEMForceField objFEM = child.GetComponent<SofaFEMForceField>();
                if (objFEM != null)
                {
                    m_forceFields.Add(objFEM);
                }

                SofaCollisionModel objCol = child.GetComponent<SofaCollisionModel>();
                if (objCol != null)
                {
                    m_collisionModels.Add(objCol);
                }
            }
        }


        /// Internal method to get information of all leafs
        protected void ParseChildrenModels(Transform parent)
        {
            foreach (Transform child in parent)
            {
                ParseChildrenModels(child);

                SofaVisualModel objV = child.GetComponent<SofaVisualModel>();
                if (objV != null)
                {
                    m_visualModels.Add(objV);
                    m_nbrVModel++;
                    m_nbrVVertices += objV.NbVertices();
                    m_nbrVTriangles += objV.NbTriangles();
                }

                SofaFEMForceField objFEM = child.GetComponent<SofaFEMForceField>();
                if (objFEM != null)
                {
                    m_forceFields.Add(objFEM);
                }

                SofaCollisionModel objCol = child.GetComponent<SofaCollisionModel>();
                if (objCol != null)
                {
                    m_collisionModels.Add(objCol);
                }
            }
        }



        // Update is called once per frame
        void LateUpdate()
        {
            if (m_sofaContext == null)
                return;

        }

        public void DisplayVisualModel(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            foreach (SofaVisualModel visuModel in m_visualModels)
            {
                visuModel.gameObject.SetActive(_toggle.isOn);
            }
        }

        public void DisplayForceField(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            foreach (SofaFEMForceField fem in m_forceFields)
            {
                fem.DisplayForcefield(_toggle.isOn);
            }
        }

        public void DisplaySpringsForceField(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            Debug.Log("DisplaySpringsForceField Not implemented yet: " + _toggle.isOn);
        }

        public void DisplayCollisionModels(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            foreach (SofaCollisionModel col in m_collisionModels)
            {
                col.DrawCollision = _toggle.isOn;
            }
        }

        public void DisplayCollisionOutputs(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            if (m_collisionDisplay)
                m_collisionDisplay.DrawCollisions = _toggle.isOn;
        }



        public void UnloadSofaContext()
        {
            m_sofaContext = null;
            m_nbrVModel = 0;
            m_nbrVVertices = 0;
            m_nbrVTriangles = 0;
        }
    }
}
