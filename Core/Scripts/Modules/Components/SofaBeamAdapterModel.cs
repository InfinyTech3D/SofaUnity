using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SofaUnity;




namespace SofaUnity
{

    [ExecuteInEditMode]
    public class SofaBeamAdapterModel : SofaBeamModel
    {
        public GameObject m_childCameraScript;

        protected Transform tipTransform = null;
        protected Vector3 m_tipPosition = Vector3.zero;
        protected Vector3 m_tipDirection = Vector3.forward;

        protected override void CreateBeamMesh()
        {
            if (m_sofaMesh == null || m_sofaMesh.SofaMeshTopology == null)
                return;

            int nbrV = m_sofaMesh.NbVertices();
            Debug.Log("SofaBeamAdapterModel: " + nbrV);


            // nothing to do
            if (nbrV < 2)
                return;

            m_vertCenter = new Vector3[nbrV];
            int nbrPointPerCircle = 4 * m_beamDiscretisation + 1; // +1 to close cylinder UV
            int nbrP = nbrV * nbrPointPerCircle + 2; // +2 for the centers
            m_vertices = new Vector3[nbrP];
            m_normals = new Vector3[nbrP];

            UpdateBeamMesh();
            
            // 5. create triangles here
            CreateMeshTriangulation(nbrV - 1);

            // 6. create fake uv
            Vector2[] uv = new Vector2[nbrP];
            int nbrCircles = nbrV;
            uv[0] = Vector2.zero;
            uv[nbrP - 1] = Vector2.zero;

            float incrementU = (float)(1.0f / (nbrPointPerCircle - 1));
            float incrementV = (float)(1.0f / (nbrCircles - 1));

            for (int i = 0; i < nbrCircles; i++)
            {
                int incr = i * nbrPointPerCircle + 1;
                float vvalue = i;// * incrementV;
                for (int j = 0; j < nbrPointPerCircle; j++)
                {
                    uv[incr + j].x = j * incrementU;
                    uv[incr + j].y = vvalue;
                }
            }
            m_mesh.uv = uv;

            m_doComputeMesh = false;

            // 7. Add SofaMesh listener to ensure update of the mesh positions
            m_sofaMesh.AddListener();


            if (m_childCameraScript)
            {
                m_childCameraScript.transform.parent = this.gameObject.transform;
            }
        }



        /// Main Method to update the beam mesh position from SofaMesh center positions. Will call \sa UpdateLine() around each center point
        protected override void UpdateBeamMesh()
        {
            if (m_sofaMesh == null || m_sofaMesh.SofaMeshTopology == null)
                return;

            int nbrV = m_vertCenter.Length;

            // nothing to do
            if (nbrV < 2)
                return;

            //Debug.Log("BeamModel::UpdateLinearMesh");            
            float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;
            for (int i = 0; i < nbrV; i++)
            {
                m_vertCenter[nbrV - i - 1] = new Vector3(sofaVertices[i * 3], sofaVertices[i * 3 + 1], sofaVertices[i * 3 + 2]);
            }

            // update position vectors for camera
            Vector3 sofaScale = m_sofaMesh.m_sofaContext.GetScaleSofaToUnity();
            
            m_tipPosition[0] = m_vertCenter[nbrV - 1][0]* sofaScale[0];
            m_tipPosition[1] = m_vertCenter[nbrV - 1][1] * sofaScale[1];
            m_tipPosition[2] = m_vertCenter[nbrV - 1][2] * sofaScale[2];

            // TODO: to be removed by rigid handling
            m_tipDirection = Vector3.forward;
            for (int i = 1; i < nbrV; i++)
            {
                Vector3 tmpDirection = m_vertCenter[i] - m_vertCenter[i - 1];
                if (tmpDirection.magnitude < 0.001f)
                    break;

                m_tipDirection = tmpDirection;
            }

            m_tipDirection[0] *= sofaScale[0];
            m_tipDirection[1] *= sofaScale[1];
            m_tipDirection[2] *= sofaScale[2];


            // update borders first
            int idLast = m_vertices.Length - 1;
            m_vertices[0] = m_vertCenter[0];
            m_vertices[idLast] = m_vertCenter[nbrV - 1];
            m_normals[0] = m_vertCenter[0] - m_vertCenter[1];
            m_normals[idLast] = m_vertCenter[nbrV - 1] - m_vertCenter[nbrV - 2];

            // update first:
            UpdateLine(m_vertCenter[0], m_vertCenter[1], 0, true);
            // update intermediate points
            for (int i = 1; i < nbrV; i++)
            {
                UpdateLine(m_vertCenter[i-1], m_vertCenter[i], i);
            }

            //update last
            m_mesh.vertices = m_vertices;
            m_mesh.normals = m_normals;
            m_mesh.RecalculateBounds();

            UpdateCamera();
        }



        void UpdateCamera()
        {
            if (m_childCameraScript == null)
                return;

            
            m_childCameraScript.transform.position = m_tipPosition;
            m_childCameraScript.transform.forward = m_tipDirection;
        }


        /// Method to draw debug information like the vertex being grabed
        void OnDrawGizmosSelected()
        {
            if (m_vertCenter == null)
                return;

            //Gizmos.color = Color.yellow;
            //////float factor = m_sofaContext.GetFactorSofaToUnity();
            
            
            //foreach (Vector3 vert in m_vertCenter)
            //{
            //    Gizmos.DrawWireSphere(this.transform.TransformPoint(vert), 0.2f);
            //}

            //Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(this.transform.TransformPoint(m_vertCenter[0]), 0.5f);

            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(this.transform.TransformPoint(m_vertCenter[1]), 0.6f);


            //Gizmos.color = Color.grey;
            //Gizmos.DrawWireSphere(this.transform.TransformPoint(m_vertCenter[m_vertCenter.Length-2]), 0.7f);

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(this.transform.TransformPoint(m_vertCenter[m_vertCenter.Length - 1]), 0.8f);

            //Gizmos.color = Color.black;
            //foreach (Vector3 vert in m_vertices)
            //{
            //    Gizmos.DrawWireSphere(this.transform.TransformPoint(vert), 0.1f);
            //}
        }
    }

}
