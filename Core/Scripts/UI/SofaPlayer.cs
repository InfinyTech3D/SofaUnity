using System.Collections;
using System.Collections.Generic;
using SofaUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SofaUnity
{
    public class SofaPlayer : MonoBehaviour
    {
        public Toggle m_playButton;
        public Toggle m_stopButton;
        public Toggle m_restartButton;


        /// Dedicated text object to display the SOFA filename.
        public Text t_sofaFilename = null;
        /// Dedicated text object to display the unity simulation FPS.
        public Text t_unityFPS = null;
        /// Dedicated text object to display the sofa simulation FPS.
        public Text t_sofaFPS = null;


        public Toggle m_displayFPS = null;
        public GameObject m_viewsPanel = null;
        public GameObject m_debugConsole = null;

        protected bool m_isReady = false;
        protected SofaContext m_sofaContext = null;


        /// Internal parameters for FPS computation
        /// {
        protected int frameCount = 0;
        protected float nextUpdate = 0.0f;
        protected float fps = 0.0f;
        protected float updateRate = 4.0f;  // 4 updates per sec.
        /// }

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

            m_isReady = true;

            if (m_sofaContext.IsSofaUpdating)
            {
                m_playButton.isOn = true;
                m_stopButton.isOn = false;
            }
            else
            {
                m_playButton.isOn = false;
                m_stopButton.isOn = true;
            }
        }


        // Update is called once per frame
        void LateUpdate()
        {
            if (m_sofaContext == null)
                return;

            if (m_displayFPS && m_displayFPS.isOn)
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
                    t_unityFPS.text = "<b>Unity FPS: </b>" + fps;
                }

                if (t_sofaFPS)
                {
                    t_sofaFPS.text = "<b>SOFA FPS: </b>" + m_sofaContext.SimulationFPS;
                }
            }
        }

        public void ChangePlayerStatus(Toggle _toggle)
        {
            if (!m_isReady)
                return;

            if (_toggle.isOn)
            {
                //if (m_sofaVR_API == null)
                //{
                //    Debug.LogWarning("No m_sofaVR_API found");
                //    return;
                //}

                if (_toggle.gameObject.name.Contains("play"))
                {
                    this.startSofaSimulation();
                }
                else if (_toggle.gameObject.name.Contains("reload"))
                {
                    this.restartSofaSimulation();
                }
                else if (_toggle.gameObject.name.Contains("pause"))
                {
                    this.stopSofaSimulation();
                }
            }
        }

        public void startSofaSimulation()
        {
            if (m_sofaContext != null)
                m_sofaContext.IsSofaUpdating = true;
        }

        public void stopSofaSimulation()
        {
            if (m_sofaContext != null)
                m_sofaContext.IsSofaUpdating = false;
        }

        public void restartSofaSimulation()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void DisplayFPS(Toggle _toggle)
        {
            t_unityFPS.gameObject.SetActive(_toggle.isOn);
            t_sofaFPS.gameObject.SetActive(_toggle.isOn);
        }

        public void DisplayViews(Toggle _toggle)
        {
            m_viewsPanel.SetActive(_toggle.isOn);
        }


        public void DisplayConsole(Toggle _toggle)
        {
            m_debugConsole.SetActive(_toggle.isOn);
        }
    }
}
