using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;

namespace SofaUnity
{
    /// <summary>
    /// Class to gather information from the simulation scene and display those information in a Text Gameobject.
    /// Information displayed will be mesh size and number of triangles and  the simulation FPS.
    /// </summary>
    public class SceneInfo : MonoBehaviour
    {
        ////////////////////////////////////////////
        /////     SceneInfo public members     /////
        ////////////////////////////////////////////

        /// Pointer to the text GameObject where to display information
        public GameObject textUI = null;

        /// Dedicated text object to display the number of simulated meshes.
        public Text t_nbrModels = null;
        /// Dedicated text object to display the total number of vertices of all meshes.
        public Text t_nbrVertices = null;
        /// Dedicated text object to display the total number of triangles of all meshes.
        public Text t_nbrTriangles = null;
        /// Dedicated text object to display the total number of tetrahedra of all meshes.
        public Text t_nbrTetrahedra = null;
        /// Dedicated text object to display the unity simulation FPS.
        public Text t_unityFPS = null;
        /// Dedicated text object to display the sofa simulation FPS.
        public Text t_sofaFPS = null;

        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;

        /// Pointer to the SofaContext to inspect.
        protected SofaContext m_sofaContext = null;
        


        ////////////////////////////////////////////
        /////    SceneInfo internal members    /////
        ////////////////////////////////////////////

        /// Internal parameters for FPS computation
        /// {
        protected int frameCount = 0;
        protected float nextUpdate = 0.0f;
        protected float fps = 0.0f;
        protected float updateRate = 4.0f;  // 4 updates per sec.
        /// }

        /// Parameter to know if data have been computed
        protected bool computed = false;

        /// Number of visual meshes in the scene.
        protected int m_nbrVModel = 0;
        /// Total number of vertices over all visual meshes.
        protected int m_nbrVVertices = 0;
        /// Total number of triangles over all visual meshes.
        protected int m_nbrVTriangles = 0;

        /// Number of mechanical meshes in the scene.
        protected int m_nbrMesh = 0;
        /// Total number of vertices over all mechanical meshes.
        protected int m_nbrMVertices = 0;
        /// Total number of triangles over all mechanical meshes.
        protected int m_nbrMTriangles = 0;
        /// Total number of tetrahedra over all mechanical meshes.
        protected int m_nbrMTetra = 0;

        /// Internal text variable to store scene info, not the fps.
        protected string sceneInfoText = "";

        ////////////////////////////////////////////
        /////    SceneInfo member accessors    /////
        ////////////////////////////////////////////

        /// Accessor to the number of visual meshes in the scene. @sa m_nbrVModel
        int GetNbrVisualModels() { return m_nbrVModel; }
        /// Accessor to the number of vertices over all visual meshes. @sa m_nbrVVertices
        int GetNbrVisualVertices() { return m_nbrVVertices; }
        /// Accessor to the number of triangles over all visual meshes. @sa m_nbrVTriangles
        int GetNbrVisualTriangles() { return m_nbrVTriangles; }


        /// Accessor to the number of visual meshes in the scene. @sa m_nbrMesh
        int GetNbrMeshModels() { return m_nbrMesh; }
        /// Accessor to the number of visual meshes in the scene. @sa m_nbrMVertices
        int GetNbrMeshVertices() { return m_nbrMVertices; }
        /// Accessor to the number of visual meshes in the scene. @sa m_nbrMTriangles
        int GetNbrMeshTriangles() { return m_nbrMTriangles; }
        /// Accessor to the number of visual meshes in the scene. @sa m_nbrMTetra
        int GetNbrMeshTetrahedra() { return m_nbrMTetra; }



        ////////////////////////////////////////////
        /////       SceneInfo public API       /////
        ////////////////////////////////////////////

        /// Method call by Unity loop when simulation start. Will get the target SofaContext to inspect.
        void Start ()
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
                    //Debug.LogError("SceneInfo::Start - No SofaContext found.");
                    return;
                }
            }

            nextUpdate = Time.time;   
        }


        /// Public method to manually set the SofaContext to inspect. Will remove previous Sofacontext if one and update info.
        public void SetSofaContext(SofaUnity.SofaContext _context)
        {
            m_sofaContext = _context;
            if (m_sofaContext)
                ComputeSceneInfo();

            UpdateSceneInfo();
        }


        /// Public method to manually unload the current SofaContext and update all information
        public void UnloadSofaContext()
        {
            m_sofaContext = null;
            m_nbrVModel = 0;
            m_nbrVVertices = 0;
            m_nbrVTriangles = 0;

            m_nbrMesh = 0;
            m_nbrMVertices = 0;
            m_nbrMTriangles = 0;
            m_nbrMTetra = 0;

            UpdateSceneInfo();
        }


        // LateUpdate is called once per frame after Update. Will call @sa ComputeSceneInfo and @sa UpdateSceneInfo
        void LateUpdate()
        {
            if (m_sofaContext == null)
                return;

            // if not computed, compute and update info
            if (!computed)
            {
                ComputeSceneInfo();
                UpdateSceneInfo();
            }

            // if true will compute the FPS of the scene
            if (displayFPS)
            {
                // compute current FPS
                frameCount++;
                if (Time.time > nextUpdate)
                {
                    nextUpdate += 1.0f / updateRate;
                    fps = frameCount * updateRate * 1.5f;
                    frameCount = 0;
                }

                // update all FPS info
                if (t_unityFPS)
                {
                    t_unityFPS.text = "<b>FPS: </b>" + fps;
                }

                if (t_sofaFPS)
                {
                    t_sofaFPS.text = "<b>SOFA FPS: </b>" + m_sofaContext.SimulationFPS;
                }

                if (textUI)
                {
                    Text txt = textUI.GetComponent<Text>();
                    txt.text = sceneInfoText;
                    txt.text = txt.text + "\n<b>Unity FPS: </b>" + fps;
                    txt.text = txt.text + "\n<b>SOFA FPS: </b>" + m_sofaContext.SimulationFPS;
                }
            }
        }

        /// Main method to compute the scene information
        protected void ComputeSceneInfo()
        {
            if (m_sofaContext == null)
                return;

            foreach (Transform child in m_sofaContext.transform)
            {
                ParseChildrenInfo(child);
                SofaVisualModel objV = child.GetComponent<SofaVisualModel>();
                if (objV != null)
                {
                    m_nbrVModel++;
                    m_nbrVVertices += objV.NbVertices();
                    m_nbrVTriangles += objV.NbTriangles();
                }

                SofaMesh objM = child.GetComponent<SofaMesh>();
                if (objM != null)
                {
                    m_nbrMesh++;
                    m_nbrMVertices += objM.NbVertices();
                    m_nbrMTriangles += objM.NbTriangles();
                    m_nbrMTetra += objM.NbTetrahedra();
                }
            }
           
            computed = true;
        }

        /// Internal method to get information of all leafs
        protected void ParseChildrenInfo(Transform parent)
        {
            foreach (Transform child in parent)
            {
                ParseChildrenInfo(child);

                SofaVisualModel objV = child.GetComponent<SofaVisualModel>();
                if (objV != null)
                {
                    m_nbrVModel++;
                    m_nbrVVertices += objV.NbVertices();
                    m_nbrVTriangles += objV.NbTriangles();
                }

                SofaMesh objM = child.GetComponent<SofaMesh>();
                if (objM != null)
                {
                    m_nbrMesh++;
                    m_nbrMVertices += objM.NbVertices();
                    m_nbrMTriangles += objM.NbTriangles();
                    m_nbrMTetra += objM.NbTetrahedra();
                }
            }
        }

        /// Internal method to update all text labels from computed info.
        protected void UpdateSceneInfo()
        {
            if (t_nbrModels)
            {
                t_nbrModels.text = "<b>Visual Models: </b>" + m_nbrVModel;
            }

            if (t_nbrVertices)
            {
                t_nbrVertices.text = "<b>Vertices: </b>" + m_nbrVVertices;
            }

            if (t_nbrTriangles)
            {
                t_nbrTriangles.text = "<b>Triangles: </b>" + m_nbrVTriangles;
            }

            if (textUI)
            {
                sceneInfoText = "<b>Visual Models: </b>" + m_nbrVModel + "\n" +
                    "<b> Vertices: </b>" + m_nbrVVertices + "\n" +
                    "<b> Triangles: </b>" + m_nbrVTriangles + "\n";

                sceneInfoText = sceneInfoText + "\n" + "<b>Mesh Models: </b>" + m_nbrMesh + "\n" +
                    "<b> Vertices: </b>" + m_nbrMVertices + "\n" +
                    "<b> Triangles: </b>" + m_nbrMTriangles + "\n" +
                    "<b> Tetrahedra: </b>" + m_nbrMTetra + "\n";
            }

        }

    }
}
