using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SofaUnity;

namespace SofaUnity
{

    public class SceneInfo : MonoBehaviour
    {
        /// Pointer to the text GameObject.
        public GameObject textUI = null;

        public Text t_nbrModels = null;
        public Text t_nbrVertices = null;
        public Text t_nbrTriangles = null;
        public Text t_FPS = null;

        /// Booleen to set if FPS need to be computed and display.
        public bool displayFPS = false;
        public bool m_startOnPlay = true;

        protected SofaContext m_sofaContext = null;

        /// Internal parameters for FPS
        /// {
        int frameCount = 0;
        float nextUpdate = 0.0f;
        float fps = 0.0f;
        float updateRate = 4.0f;  // 4 updates per sec.
                                  /// }

        bool computed = false;
        int m_nbrVModel = 0;
        int m_nbrVVertices = 0;
        int m_nbrVTriangles = 0;

        int m_nbrMesh = 0;
        int m_nbrMVertices = 0;
        int m_nbrMTriangles = 0;
        int m_nbrMTetra = 0;

        // Use this for initialization
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
                    Debug.LogError("RayCaster::loadContext - No SofaContext found.");
                    return;
                }
            }


            if (!m_startOnPlay)
                return;

            nextUpdate = Time.time;   
        }


        public void setSofaContext(SofaUnity.SofaContext _context)
        {
            m_sofaContext = _context;
            if (m_sofaContext)
                computeInfo();

            updateInfo();
        }

        public void unloadSofaContext()
        {
            m_sofaContext = null;
            m_nbrVModel = 0;
            m_nbrVVertices = 0;
            m_nbrVTriangles = 0;

            m_nbrMesh = 0;
            m_nbrMVertices = 0;
            m_nbrMTriangles = 0;
            m_nbrMTetra = 0;

            updateInfo();
        }

        protected void computeInfo()
        {
            if (m_sofaContext == null)
                return;

            foreach (Transform child in m_sofaContext.transform)
            {
                parseInfoChilds(child);
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

        protected void parseInfoChilds(Transform parent)
        {
            foreach (Transform child in parent)
            {
                parseInfoChilds(child);

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

        protected void updateInfo()
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
        }


        // Update is called once per frame
        void LateUpdate ()
        {
            if (m_sofaContext == null)
                return;

            if (!computed)
            {
                computeInfo();
                updateInfo();
            }

            // if true will compute the FPS of the scene
            if (displayFPS)
            {
                frameCount++;
                if (Time.time > nextUpdate)
                {
                    nextUpdate += 1.0f / updateRate;
                    fps = frameCount * updateRate * 1.5f;
                    frameCount = 0;
                }

                if (t_FPS)
                {
                    t_FPS.text = "<b>FPS: </b>" + fps;
                }
            }

            if (textUI)
            {
                Text txt = textUI.GetComponent<Text>();
                txt.text = "<b>Visual Models: </b>" + m_nbrVModel + "\n" +
                    "<b> Vertices: </b>" + m_nbrVVertices + "\n" +
                    "<b> Triangles: </b>" + m_nbrVTriangles + "\n";

                txt.text = txt.text + "\n" + "<b>Mesh Models: </b>" + m_nbrMesh + "\n" +
                    "<b> Vertices: </b>" + m_nbrMVertices + "\n" +
                    "<b> Triangles: </b>" + m_nbrMTriangles + "\n" +
                    "<b> Tetrahedra: </b>" + m_nbrMTetra + "\n";
                

                if (displayFPS)
                {
                    txt.text = txt.text + "\n<b>Unity FPS: </b>" + fps;
                    txt.text = txt.text + "\n<b>SOFA FPS: </b>" + m_sofaContext.SimulationFPS;
                }
            }
        }
    }
}
