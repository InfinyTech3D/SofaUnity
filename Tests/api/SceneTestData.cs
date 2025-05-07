using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SofaScripts
{
    public class MeshTestData
    {
        public string m_meshName = "None";
        public int m_nbrVertices = 0;
        public int m_nbrTriangles = 0;
        public Vector3 m_center;
        public Vector3 m_size;


        public bool CompareMesh(MeshTestData logMesh, bool log = false)
        {
            // Check MeshName
            if (log)
                Debug.Log("Log MeshName: " + logMesh.m_meshName + " | Ref MeshName: " + m_meshName);

            if (m_meshName != logMesh.m_meshName)
            {
                Debug.LogError("MeshName differ between ref: " + m_meshName + " and new log: " + logMesh.m_meshName);
                return false;
            }


            // Check Number of vertices
            if (log)
                Debug.Log("Log Nbr Vertices: " + logMesh.m_nbrVertices + " | Ref Nbr Vertices: " + m_nbrVertices);

            if (m_nbrVertices != logMesh.m_nbrVertices)
            {
                Debug.LogError("In mesh: " + m_meshName + ". Number of vertices differ between ref: " + m_nbrVertices + " and new log: " + logMesh.m_nbrVertices);
                return false;
            }


            // Check Number of triangles
            if (log)
                Debug.Log("Log Nbr Triangles: " + logMesh.m_nbrTriangles + " | Ref Nbr Triangles: " + m_nbrTriangles);

            if (m_nbrTriangles != logMesh.m_nbrTriangles)
            {
                Debug.LogError("In mesh: " + m_meshName + ". Number of triangles differ between ref: " + m_nbrTriangles + " and new log: " + logMesh.m_nbrTriangles);
                return false;
            }


            // Check mesh BB center
            float diffCenter = (m_center - logMesh.m_center).magnitude;
            if (log)
                Debug.Log("Log BB center: " + logMesh.m_center + " | Ref BB center: " + m_center + " | Diff: " + diffCenter);

            if (diffCenter > 1.0f)
            {
                Debug.LogError("In mesh: " + m_meshName + ". Mesh BB center differ: Ref: " + m_center + " | Log: " + logMesh.m_center + " | diff: " + diffCenter);
                return false;
            }

            // Check mesh BB size
            float diffBB = (m_center - logMesh.m_center).magnitude;
            
            if (log)
                Debug.Log("Log BB size: " + logMesh.m_size + " | Ref BB size: " + m_size + " | Diff: " + diffBB);

            if (diffBB > 1.0f)
            {
                Debug.LogError("In mesh: " + m_meshName + ". Mesh BB size differ: Ref: " + m_size + " | Log: " + logMesh.m_size + " | diff: " + diffBB);
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
        public SceneTestData(string sceneName) 
        {
            m_sceneName = sceneName;
        }

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


        public bool ReadData(string path, bool log = false)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            StreamReader reader = new StreamReader(path);

            // get number of meshes
            string meshLine = reader.ReadLine();
            int nbrMesh = int.Parse(meshLine.Split(' ')[1]);

            if (log)
            {
                Debug.Log("Number of mesh found in file: " + nbrMesh);
            }

            m_nbrMeshes = nbrMesh;
            m_meshes = new List<MeshTestData>(nbrMesh);

            for (int cptMesh = 0; cptMesh < nbrMesh; cptMesh++)
            {
                MeshTestData meshData = new MeshTestData();

                string nameLine = reader.ReadLine();
                meshData.m_meshName = nameLine.Split(' ')[1];

                string verticesLine = reader.ReadLine();
                meshData.m_nbrVertices = int.Parse(verticesLine.Split(' ')[1]);

                string trianglesLine = reader.ReadLine();
                meshData.m_nbrTriangles = int.Parse(trianglesLine.Split(' ')[1]);

                string centerLine = reader.ReadLine();
                centerLine = centerLine.Split(' ')[1];
                string[] centerString = centerLine.Split(';');
                meshData.m_center = new Vector3(float.Parse(centerString[0]), float.Parse(centerString[1]), float.Parse(centerString[2]));

                string sizeLine = reader.ReadLine();
                sizeLine = sizeLine.Split(' ')[1];
                string[] sizeString = sizeLine.Split(';');
                meshData.m_size = new Vector3(float.Parse(sizeString[0]), float.Parse(sizeString[1]), float.Parse(sizeString[2]));

                m_meshes.Add(meshData);
                if (log)
                    meshData.LogMesh();
            }

            reader.Close();
            return true;
        }

        public bool CompareScene(SceneTestData otherScene, bool log = false)
        {
            if (log)
            {
                Debug.Log("## Start Mesh comparison for scene: " + m_sceneName + ". Number of Log mesh: " + otherScene.m_nbrMeshes + " | Number of Log mesh: " + m_nbrMeshes);
            }

            if (otherScene.m_nbrMeshes != m_nbrMeshes)
            {
                Debug.LogError("In scene: " + m_sceneName + ". Number of mesh differ between ref: " + m_nbrMeshes + " and new log: " + otherScene.m_nbrMeshes);
                return false;
            }

            for (int i = 0; i < m_nbrMeshes; ++i)
            {
                bool res = m_meshes[i].CompareMesh(otherScene.m_meshes[i], log);
                if (!res)
                    return false;
            }

            return true;
        }


        private readonly string m_sceneName;
        private int m_nbrMeshes;
        private List<MeshTestData> m_meshes = null;

    }
}
