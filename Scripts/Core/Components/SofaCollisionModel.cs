using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaCollisionModel : SofaBaseComponent
    {
        [SerializeField]
        protected bool m_drawCollision = false;
        protected bool m_collisionVisuInit = false;

        private SofaMesh m_sofaMesh = null;
        private bool m_oldStatus = false;

        [SerializeField]
        private List<GameObject> m_collisionElement = null;

        protected override void Init_impl()
        {
            if (m_sofaMesh == null)
            {
                m_sofaMesh = m_ownerNode.GetSofaMesh();
                if (m_sofaMesh == null)
                    m_sofaMesh = m_ownerNode.FindSofaMesh();

                if (m_sofaMesh && m_drawCollision)
                {
                    CreateCollisionElements();
                }
            }
        }

        protected override void FillPossibleTypes()
        {
            //SofaLog("FillPossibleTypes SofaCollisionModel");
        }


        /// Method called by @sa Update() method.
        protected override void Update_impl()
        {
            if (m_drawCollision && m_collisionVisuInit == false)
            {
                CreateCollisionElements();
            }

            if (!m_collisionVisuInit)
                return;


            if (m_drawCollision != m_oldStatus)
            {
                m_oldStatus = m_drawCollision;
                if (m_drawCollision)
                {
                    m_sofaMesh.AddListener();
                    UpdateCollisionElements();
                    ShowCollisionElements();
                    return;
                }
                else
                {
                    m_sofaMesh.RemoveListener();
                    HideCollisionElements();
                }
            }

            if (m_drawCollision)
            {
                UpdateCollisionElements();
            }
        }


        protected void CreateCollisionElements()
        {
            if (this.m_componentType == "SphereCollisionModel")
            {
                CreateSphereCollisionModel();
            }
            else if (this.m_componentType == "TriangleCollisionModel")
            {
                CreateTriangleCollisionModel();
            }
        }


        protected void CreateSphereCollisionModel()
        {
            if (m_collisionElement != null)
            {
                if (m_collisionElement.Count != m_sofaMesh.NbVertices())
                {
                    //Debug.LogError("SphereCollisionModel, not the same number of spheres as the mesh vertices count: " + m_collisionElement.Count + " vs " + m_sofaMesh.NbVertices());
                    m_collisionElement = null;
                }
                else
                {
                    m_collisionVisuInit = true;
                    return;
                }
            }

            int nbrSpheres = m_sofaMesh.NbVertices();
            m_collisionElement = new List<GameObject>();

            float[] vertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;

            // get sphere radius
            float radius = 1.0f;
            SofaDoubleData datad = this.m_dataArchiver.GetSofaDoubleData("radius");
            if (datad != null)
                radius = datad.Value;
            else
            {
                SofaFloatData dataf = this.m_dataArchiver.GetSofaFloatData("radius");
                radius = dataf.Value;
            }

            Vector3 scaleRadius = new Vector3(radius * 2, radius * 2, radius * 2);

            for (int i = 0; i < nbrSpheres; i++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.name = "SphereCollision_" + i;
                sphere.transform.position = new Vector3(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2]);
                sphere.transform.localScale = scaleRadius;
                sphere.transform.parent = this.transform;
                m_collisionElement.Add(sphere);
            }

            m_collisionVisuInit = true;
        }


        protected void CreateTriangleCollisionModel()
        {
            if (m_collisionElement != null)
            {
                if (m_collisionElement.Count == 0)
                {
                    Debug.Log("exist but is empty, why??");
                    m_collisionElement = null;
                }
                else
                {
                    m_collisionVisuInit = true;
                    return;
                }
            }

            GameObject triangulation = new GameObject();
            triangulation.name = "TriangleCollision";
            triangulation.transform.parent = this.transform;

            MeshRenderer renderer = triangulation.AddComponent<MeshRenderer>();
            renderer.enabled = true;
            renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));

            MeshFilter mf = triangulation.AddComponent<MeshFilter>();
            mf.mesh = m_sofaMesh.SofaMeshTopology.m_mesh;

            m_collisionElement = new List<GameObject>();
            m_collisionElement.Add(triangulation);
        }
        

        public bool DrawCollision
        {
            get { return m_drawCollision; }
            set
            {
                if (m_drawCollision != value)
                {
                    m_drawCollision = value;
                    if (m_drawCollision)
                        ShowCollisionElements();
                    else
                        HideCollisionElements();
                }
            }
        }
            

        protected void ShowCollisionElements()
        {
            if (m_collisionElement == null || m_collisionElement.Count == 0)
                CreateCollisionElements();

            foreach(GameObject elem in m_collisionElement)
            {
                elem.SetActive(true);
            }
        }

        protected void HideCollisionElements()
        {
            foreach (GameObject elem in m_collisionElement)
            {
                elem.SetActive(false);
            }
        }

        protected void UpdateCollisionElements()
        {
            if (this.m_componentType == "SphereCollisionModel")
            {
                int nbrSpheres = m_collisionElement.Count;
                float[] vertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;
                for (int i = 0; i < nbrSpheres; i++)
                {
                    GameObject sphere = m_collisionElement[i];
                    sphere.transform.localPosition = new Vector3(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2]);
                }
            }
            else if (this.m_componentType == "TriangleCollisionModel")
            {
                GameObject triangulation = m_collisionElement[0];
                MeshFilter mf = triangulation.GetComponent<MeshFilter>();
                mf.mesh.vertices = m_sofaMesh.SofaMeshTopology.m_mesh.vertices;
                mf.mesh.normals = m_sofaMesh.SofaMeshTopology.m_mesh.normals;
            }
        }
    }

} // namespace SofaUnity
