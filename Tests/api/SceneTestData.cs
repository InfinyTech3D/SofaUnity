using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class MeshTestData
{
    public string m_meshName = "None";
    public int m_nbrVertices = 0;
    public int m_nbrTriangles = 0;
    public Vector3 m_center;
    public Vector3 m_min;
    public Vector3 m_max;

    public void LogMesh()
    {
        Debug.Log("### Found Mesh: " + m_meshName + " ###");
        Debug.Log("nbr vertices: " + m_nbrVertices);
        Debug.Log("nbr triangles: " + m_nbrTriangles);
        Debug.Log("bounds: [" + m_min + " | " + m_center + " | " + m_max + "]");
    }

}

public class SceneTestData
{
    public SceneTestData() { }

    public SceneTestData(string sceneName, int nbrMeshes)
    {
        m_sceneName = sceneName;
        m_nbrMeshes = nbrMeshes;
        m_meshes = new List<MeshTestData>(nbrMeshes);
    }

    public void AddMeshData(MeshFilter mesh, bool log = false)
    {
        MeshTestData meshData = new MeshTestData();
        meshData.m_meshName = mesh.name;
        meshData.m_nbrVertices = mesh.mesh.vertexCount;
        meshData.m_nbrTriangles = mesh.mesh.triangles.Length / 3;
        meshData.m_center = mesh.mesh.bounds.center;
        meshData.m_min = mesh.mesh.bounds.min;
        meshData.m_max = mesh.mesh.bounds.max;

        m_meshes.Add(meshData);
        Debug.Log("nbr meshes: " + m_meshes.Count);
        if (log)
            meshData.LogMesh();
    }

    public void WriteData(string path)
    {
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("SceneName: " + m_sceneName);
        writer.WriteLine("NbrMeshes: " + m_nbrMeshes);

        foreach (MeshTestData mesh in m_meshes)
        {
            writer.WriteLine("Mesh: " + mesh.m_meshName);
            writer.WriteLine("NbrVertices: " + mesh.m_nbrVertices);
            writer.WriteLine("NbrTriangles: " + mesh.m_nbrTriangles);
            writer.WriteLine("Center: " + mesh.m_center);
            writer.WriteLine("Min: " + mesh.m_min);
            writer.WriteLine("Max: " + mesh.m_max);
        }

        writer.Close();
    }

    public void ReadData(string path)
    {
        //    string path = "Assets/Resources/test.txt";

        //    //Read the text from directly from the test.txt file

        //    StreamReader reader = new StreamReader(path);

        //    Debug.Log(reader.ReadToEnd());

        //    reader.Close();
    }


    private readonly string m_sceneName;
    private readonly int m_nbrMeshes;
    private List<MeshTestData> m_meshes = null;

}

