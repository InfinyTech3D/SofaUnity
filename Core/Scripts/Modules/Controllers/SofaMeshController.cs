using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

namespace SofaUnity
{
    public class SofaMeshController : MonoBehaviour
    {
        public SofaMesh m_sofaMesh = null;
        public MeshFilter m_unityMesh = null;
        public string m_sofaMeshName = "";

        /// Parameter bool to store information if vec3 or rigid are parsed.
        private bool m_ready = false;
        private Vector3 unityToSofa;
        private Vector3 sofaToUnity;

        private List<int> map;

        //    private Vector3 objectOri = Vector3.zero;
        private Vector3[] newPosition;
        private int nbrSofaV = 0;

        // Start is called before the first frame update
        void Start()
        {
            if (m_sofaMeshName.Length > 0)
            {
                SofaMesh[] meshes = GameObject.FindObjectsOfType<SofaMesh>();
                Debug.Log("Nbr Mesh: " + meshes.Length);
                foreach (SofaMesh mesh in meshes)
                {
                    if (mesh.UniqueNameId.Contains(m_sofaMeshName))
                        m_sofaMesh = mesh;
                }
            }


            if (m_sofaMesh == null)
            {
                Debug.LogError("m_sofaMesh is not set.");
                m_ready = false;
                return;
            }

            if (m_unityMesh == null)
            {
                Debug.LogError("m_unityMesh is not set.");
                m_ready = false;
                return;
            }

            nbrSofaV = m_sofaMesh.NbVertices();
            int nbrUnityV = m_unityMesh.mesh.vertexCount;

            List<Vector3> tmp = new List<Vector3>();
            map = new List<int>();


            for (int i = 0; i < m_unityMesh.mesh.vertices.Length; ++i)
            {
                Vector3 vert = m_unityMesh.mesh.vertices[i];
                bool found = false;
                for (int j = 0; j < tmp.Count; ++j)
                {
                    if ((vert - tmp[j]).magnitude < 0.0001) // found duplicate
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    tmp.Add(vert);
                    map.Add(i);
                }
            }

            if (nbrSofaV == 0 || nbrSofaV != tmp.Count)
            {
                Debug.LogError("This controller can only act on a valid mesh similar to the same as the SOFA one.");
                Debug.LogError("Found: Nbr Sofa vertices: " + nbrSofaV + " vs Nbr Unity vertices: " + nbrUnityV + " | not duplicated: " + tmp.Count);
                m_ready = false;
                return;
            }

            sofaToUnity = m_sofaMesh.m_sofaContext.GetScaleSofaToUnity();
            unityToSofa = m_sofaMesh.m_sofaContext.GetScaleUnityToSofa();

            newPosition = new Vector3[nbrSofaV];

            m_ready = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!m_ready)
                return;

            for (int i = 0; i < nbrSofaV; ++i)
            {
                newPosition[i] = transform.TransformPoint(m_unityMesh.mesh.vertices[map[i]]);
            }
            m_sofaMesh.SetPositions(newPosition);

            UpdateFromSofa();
        }

        protected void UpdateFromSofa()
        {
            //int nbrV = m_sofaMesh.NbVertices();
            //float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;

            //for (int i = 0; i < 3; i++)
            //{
            //    objectOri[i] = sofaVertices[i] * sofaToUnity[i];
            //}

            //this.transform.position = objectOri;
        }


        protected void UpdateToSofa(Vector3 my_newPosition)
        {
            //if (isRigidMesh)
            //{
            //    newPositionRigid[0] = my_newPosition[0];
            //    newPositionRigid[1] = my_newPosition[1];
            //    newPositionRigid[2] = my_newPosition[2];
            //    m_sofaMesh.SetVelocities(stopVelocityRigid);
            //    m_sofaMesh.SetPositions(newPositionRigid);
            //}
            //else
            //{
            //    m_sofaMesh.SetVelocities(stopVelocity);
            //    m_sofaMesh.SetVertices(newPosition);
            //}

            //this.transform.position = my_newPosition;
            //objectOri = my_newPosition;
        }
    }
}
