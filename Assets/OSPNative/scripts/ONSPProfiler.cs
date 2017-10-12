using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;


public class ONSPProfiler : MonoBehaviour
{
    // Import functions
    public const string strONSPS = "AudioPluginOculusSpatializer";

    public bool profilerEnabled = false;
    const int DEFAULT_PORT = 2121;
    public int port = DEFAULT_PORT;

    void Start()
    {
        Application.runInBackground = true;
        if (profilerEnabled)
        {
            Debug.Log("Oculus Audio Profiler enabled, IP address = " + Network.player.ipAddress);
        }
    }

    void Update()
    {
        if (port < 0 || port > 65535)
        {
            port = DEFAULT_PORT;
        }
        ONSP_SetProfilerPort(port);
        ONSP_SetProfilerEnabled(profilerEnabled);
    }

    [DllImport(strONSPS)]
    private static extern int ONSP_SetProfilerEnabled(bool enabled);

    [DllImport(strONSPS)]
    private static extern int ONSP_SetProfilerPort(int port);
}
