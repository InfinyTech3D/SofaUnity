using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using SofaUnity;
using SofaScripts;


public class Benchmark01Editor : Editor
{
    [MenuItem("SofaUnity/Benchmarks/Benchmark01")]
    public static GameObject CreateNew()
    {
        if (!SofaScripts.Benchmarks.hasSofaContext())
            return null;

        // Add sofa context first
        GameObject context = SofaScripts.Benchmarks.createSofaContext();

        // Add a floor        
        SofaScripts.Benchmarks.createFloor();

        // Add the cubes
        for (int i=0; i<5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                GameObject cube = SofaScripts.Benchmarks.createCube("SBox_" + i + "_" + j);
                SBox box = cube.GetComponent<SBox>();
                box.m_translation[0] = -15 + j * 2;
                box.m_translation[1] = 6+i*2;
            }
        }

        return context;
    }
}


public class Benchmark02Editor : Editor
{
    [MenuItem("SofaUnity/Benchmarks/Benchmark02")]
    public static GameObject CreateNew()
    {        
        if (!SofaScripts.Benchmarks.hasSofaContext())
            return null;

        // Add sofa context first
        GameObject context = SofaScripts.Benchmarks.createSofaContext();

        // Add a floor        
        SofaScripts.Benchmarks.createFloor();

        // Add Spheres
        for (int i = 0; i < 1; ++i)
        {
            GameObject obj = SofaScripts.Benchmarks.createSphere("SSphere_" + i);
            SSphere objImpl = obj.GetComponent<SSphere>();
            objImpl.m_translation[0] = -3;
            objImpl.m_translation[1] = 6 + i * 2;
        }

        // Add Cubes
        for (int i = 0; i < 1; ++i)
        {
            GameObject obj = SofaScripts.Benchmarks.createCube("SBox_" + i);
            SBox objImpl = obj.GetComponent<SBox>();
            objImpl.m_translation[0] = 0;
            objImpl.m_translation[1] = 6 + i * 2;
        }

        // Add Cylinders
        for (int i = 0; i < 1; ++i)
        {
            GameObject obj = SofaScripts.Benchmarks.createCylinder("SCylinder_" + i);
            SCylinder objImpl = obj.GetComponent<SCylinder>();
            objImpl.m_translation[0] = 3;
            objImpl.m_translation[1] = 5.5f + i * 2;
        }

        return context;
    }
}
