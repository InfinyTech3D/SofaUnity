using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using SofaUnity;

public class Benchmark01Editor : Editor
{
    [MenuItem("SofaUnity/Benchmarks/Benchmark01")]
    public static GameObject CreateNew()
    {
        GameObject _contextObject = GameObject.Find("SofaContext");
        if (_contextObject != null)
        {
            Debug.LogError("Already a context.");
            return null;
        }            

        // Add sofa context first
        GameObject context = new GameObject();
        context.AddComponent<SofaContext>();
        context.name = "SofaContext";

        // Add a floor
        GameObject floor = new GameObject();
        floor.AddComponent<SRigidPlane>();
        floor.name = "SRigidPlane";
        SRigidPlane plane = floor.GetComponent<SRigidPlane>();
        plane.m_gridSize[0] = 2;
        plane.m_gridSize[1] = 2;
        plane.m_gridSize[2] = 2;

        plane.m_scale[0] = 50;
        plane.m_scale[1] = 1;
        plane.m_scale[2] = 50;


        // Add the cubes
        for (int i=0; i<5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                GameObject cube = new GameObject();
                cube.AddComponent<SBox>();
                cube.name = "SBox_" + i + "_" + j;
                SBox box = cube.GetComponent<SBox>();
                Debug.Log(box.translation);
                box.m_translation[0] = -15 + j * 2;
                box.m_translation[1] = 6+i*2;
                //box.translation = new Vector3(-15 + i * 2, 6, 0);
                //go.transform.parent = this.gameObject.transform;
            }
        }

        return context;
    }
}
