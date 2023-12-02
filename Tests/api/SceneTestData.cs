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
    public Vector3 m_size;


    public bool CompareMesh(MeshTestData logMesh)
    {
        if (m_meshName != logMesh.m_meshName)
        {
            Debug.LogError("MeshName differ between ref: " + m_meshName + " and new log: " + logMesh.m_meshName);
            return false;
        }

        if (m_nbrVertices != logMesh.m_nbrVertices)
        {
            Debug.LogError("In mesh: " + m_meshName + ". Number of vertices differ between ref: " + m_nbrVertices + " and new log: " + logMesh.m_nbrVertices);
            return false;
        }

        if (m_nbrTriangles != logMesh.m_nbrTriangles)
        {
            Debug.LogError("In mesh: " + m_meshName + ". Number of triangles differ between ref: " + m_nbrTriangles + " and new log: " + logMesh.m_nbrTriangles);
            return false;
        }

        if (m_center != logMesh.m_center)
        {
            Debug.LogError("In mesh: " + m_meshName + ". Mesh center differ between ref: " + m_center + " and new log: " + logMesh.m_center);
            return false;
        }

        if (m_size != logMesh.m_size)
        {
            Debug.LogError("In mesh: " + m_meshName + ". Mesh size differ between ref: " + m_size + " and new log: " + logMesh.m_size);
            return false;
        }

        return true;
    }

    public void LogMesh()
    {
        Debug.Log("### Found Mesh: " + m_meshName + " ###");
        Debug.Log("nbr vertices: " + m_nbrVertices);
        Debug.Log("nbr triangles: " + m_nbrTriangles);
        Debug.Log("bounds: [" + m_center.ToString("F6") + " | " + m_size.ToString("F6") + "]");
    }

}

public class SceneTestData
{
    public SceneTestData(string sceneName) { }

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
        meshData.m_size = mesh.mesh.bounds.size;

        m_meshes.Add(meshData);
        if (log)
            meshData.LogMesh();
    }


    public void WriteData(string path)
    {
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("NbrMeshes: " + m_nbrMeshes);

        foreach (MeshTestData mesh in m_meshes)
        {
            writer.WriteLine("Mesh: " + mesh.m_meshName);
            writer.WriteLine("NbrVertices: " + mesh.m_nbrVertices);
            writer.WriteLine("NbrTriangles: " + mesh.m_nbrTriangles);
            writer.WriteLine("Center: " + mesh.m_center.x + ";" + mesh.m_center.y + ";" + mesh.m_center.z);
            writer.WriteLine("Min: " + mesh.m_size.x + ";" + mesh.m_size.y + ";" + mesh.m_size.z);
        }

        writer.Close();
    }


    public void ReadData(string path, bool log = false)
    {
        StreamReader reader = new StreamReader(path);
        
        // get number of meshes
        string meshLine = reader.ReadLine();
        int nbrMesh = int.Parse(meshLine.Split(" ")[1]);
        
        for (int cptMesh = 0; cptMesh< nbrMesh; cptMesh++)
        {
            MeshTestData meshData = new MeshTestData();

            string nameLine = reader.ReadLine();
            meshData.m_meshName = nameLine.Split(" ")[1];

            string verticesLine = reader.ReadLine();
            meshData.m_nbrVertices = int.Parse(verticesLine.Split(" ")[1]);

            string trianglesLine = reader.ReadLine();
            meshData.m_nbrTriangles = int.Parse(trianglesLine.Split(" ")[1]);

            string centerLine = reader.ReadLine();
            centerLine = centerLine.Split(" ")[1];
            string[] centerString = centerLine.Split(";");
            meshData.m_center = new Vector3(float.Parse(centerString[0]), float.Parse(centerString[1]), float.Parse(centerString[2]));

            string sizeLine = reader.ReadLine();
            sizeLine = sizeLine.Split(" ")[1];
            string[] sizeString = sizeLine.Split(";");
            meshData.m_size = new Vector3(float.Parse(sizeString[0]), float.Parse(sizeString[1]), float.Parse(sizeString[2]));

            if (log)
                meshData.LogMesh();
        }

        reader.Close();
    }

    public bool CompareScene(SceneTestData otherScene)
    {
        if (otherScene.m_nbrMeshes != m_nbrMeshes)
        {
            Debug.LogError("In scene: " + m_sceneName + ". Number of mesh differ between ref: " + m_nbrMeshes + " and new log: " + otherScene.m_nbrMeshes);
            return false;
        }

        for (int i=0; i<m_nbrMeshes; ++i)
        {
            bool res = m_meshes[i].CompareMesh(otherScene.m_meshes[i]);
            if (!res)
                return false;
        }

        return true;
    }


    private readonly string m_sceneName;
    private readonly int m_nbrMeshes;
    private List<MeshTestData> m_meshes = null;

}

