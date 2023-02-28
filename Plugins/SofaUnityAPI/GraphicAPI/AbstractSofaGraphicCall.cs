using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbtractSofaGraphicCall : MonoBehaviour
{
    private bool m_bNewStep = false;
    private bool m_GLRenderEnd = true;
    private List<int> m_registeredRenderIDList = new List<int>();
    public SofaUnity.SofaContext m_sofaContext = null;

    protected abstract int InitCall();
    protected abstract void BeforeDestroy();

    public void RegisterID(int renderID)
    {
        m_registeredRenderIDList.Add(renderID);
    }

    public bool GLRenderEnded()
    {
        return m_GLRenderEnd;
    }

    //// MonoBehavior API
    IEnumerator Start()
    {
        string graphicVersion = SystemInfo.graphicsDeviceVersion;
        if (!graphicVersion.Contains("OpenGL")) // only openGL version is supported
        {
            Debug.LogError("SofaGraphicCall - XRay and Ultrasound rendering are only available with Unity in OpenGL mode.");
            yield break;
        }

        bool successInit = false;
        //Get SofaContext
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
                Debug.LogError("SofaGraphicCall - Could not find GameObject with tag GameController.");
            }

            if (m_sofaContext)
            {
                int res = InitCall();
                if (res >= 0)
                    RegisterID(res);

                successInit = true;
            }
            else
            {
                Debug.LogError("SofaGraphicCall - No SofaContext found.");
            }
        }

        if (successInit)
        {
            yield return StartCoroutine("CallWaitForEndOfFrame");
        }
        else
            yield break;
    }

    void Update()
    {
        m_bNewStep = true;
    }

    private IEnumerator CallWaitForEndOfFrame()
    {
        while (true)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            m_GLRenderEnd = false;

            // Issue a plugin event with arbitrary integer identifier.
            // The plugin can distinguish between different
            // things it needs to do based on this ID.
            // For our simple plugin, it does not matter which ID we pass here.
            if (m_bNewStep && m_sofaContext != null)
            {
                foreach (int renderID in m_registeredRenderIDList)
                {
                    //Debug.Log("Calling renderID " + renderID);
                    GL.IssuePluginEvent(SofaUnityAPI.SofaGraphicAPI.getRenderEventFunc(), renderID);
                }
            }

            m_GLRenderEnd = true;
            m_bNewStep = false;
        }
    }

    void OnDestroy()
    {
        BeforeDestroy();

        if (m_sofaContext)
        {
            SofaUnityAPI.SofaGraphicAPI.clearUp(m_sofaContext.GetSimuContext());
        }
    }
}
