using System;
using UnityEngine;
using SofaUnity;

namespace SofaScripts
{
    /// <summary>
    /// This class is a toolkit of methods to create benchmarks
    /// </summary>
    public class Benchmarks
    {
        /// <summary>
        /// Method to check if the current scene already contains a SofaContext before creating objects.
        /// </summary>
        /// <returns>Bool True if found, otherwise False.</returns>
        static public bool hasSofaContext()
        {
            GameObject _contextObject = GameObject.Find("SofaContext");
            if (_contextObject != null)
            {
                Debug.LogError("A SofaContext already exist.");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Method to create a SofaContext and return it.
        /// </summary>
        /// <returns>Pointer to the SofaContext</returns>
        static public GameObject createSofaContext()
        {
            GameObject context = new GameObject();
            context.AddComponent<SofaContext>();
            context.name = "SofaContext";

            return context;
        }


        /// <summary>
        /// Method to add the ThirdPartyCamera script to the current Camera
        /// </summary>
        static public void addThirdPartyCamera()
        {
            GameObject _camera = GameObject.Find("Main Camera");
            if (_camera != null)
            {
                _camera.AddComponent<ThirdPersonCamera>();
            }
            else
                Debug.LogError("Camera not found");
        }


        /// <summary>
        /// Method to Add a floor to the scene, I.e a SofaRigidPlane
        /// </summary>
        /// <returns>Pointer to this GameObject</returns>
        static public GameObject createFloor()
        {
            GameObject floor = new GameObject();
            floor.AddComponent<SofaRigidPlane>();
            floor.name = "Rigid Floor";
            SofaRigidPlane plane = floor.GetComponent<SofaRigidPlane>();
            plane.m_gridSize[0] = 2;
            plane.m_gridSize[1] = 2;
            plane.m_gridSize[2] = 2;

            //plane.m_scale[0] = 25;
            //plane.m_scale[1] = 1;
            //plane.m_scale[2] = 25;

            return floor;
        }

        /// <summary>
        /// Method to create a Cube with given Name.
        /// </summary>
        /// <param name="name">Name of this GameObject</param>
        /// <returns>Pointer to this GameObject</returns>
        static public GameObject createCube(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SofaBox>();
            obj.name = name;
            return obj;
        }

        /// <summary>
        /// Method to create a Sphere with given Name.
        /// </summary>
        /// <param name="name">Name of this GameObject</param>
        /// <returns>Pointer to this GameObject</returns>
        static public GameObject createSphere(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SofaSphere>();
            obj.name = name;
            return obj;
        }

        /// <summary>
        /// Method to create a Cylinder with given Name.
        /// </summary>
        /// <param name="name">Name of this GameObject</param>
        /// <returns>Pointer to this GameObject</returns>
        static public GameObject createCylinder(string name)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SofaCylinder>();
            obj.name = name;
            return obj;
        }

    }
}
