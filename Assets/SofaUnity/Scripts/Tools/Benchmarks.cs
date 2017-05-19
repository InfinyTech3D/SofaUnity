using System;
using UnityEngine;
using SofaUnity;

namespace SofaScripts
{
    public class Benchmarks
    {
        static public bool hasSofaContext()
        {
            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                Debug.LogError("Already a context.");
                return false;
            }
            else
                return true;
        }

        static public GameObject createSofaContext()
        {
            GameObject context = new GameObject();
            context.AddComponent<SofaContext>();
            context.name = "SofaContext";

            return context;
        }

        static public void addThirdPartyCamera()
        {
            GameObject _camera = GameObject.Find("Main Camera");
            if (_camera != null)
            {
                _camera.AddComponent<ThirdPersonCamera>();
            }
            else
                Debug.Log("Camera not found");
        }

        static public GameObject createFloor()
        {
            GameObject floor = new GameObject();
            floor.AddComponent<SRigidPlane>();
            floor.name = "Rigid Floor";
            SRigidPlane plane = floor.GetComponent<SRigidPlane>();
            plane.m_gridSize[0] = 2;
            plane.m_gridSize[1] = 2;
            plane.m_gridSize[2] = 2;

            plane.m_scale[0] = 25;
            plane.m_scale[1] = 1;
            plane.m_scale[2] = 25;

            return floor;
        }


        static public GameObject createCube(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SBox>();
            obj.name = name;
            return obj;
        }

        static public GameObject createSphere(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SSphere>();
            obj.name = name;
            return obj;
        }

        static public GameObject createCylinder(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SCylinder>();
            obj.name = name;
            return obj;
        }

    }
}
