using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    public class SofaCollisionModel : SofaBaseComponent
    {
        public bool m_drawCollision = true;

        protected bool m_collisionVisuInit = false;

        private SofaMesh m_sofaMesh = null;
        private bool m_oldStatus = false;

        [SerializeField]
        private List<GameObject> m_collisionElement;

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
                Debug.Log("CreateCollisionElements");

                if (m_collisionElement != null)
                {
                    if (m_collisionElement.Count != m_sofaMesh.NbVertices())
                        Debug.LogError("SphereCollisionModel, not the same number of spheres as the mesh vertices count.");
                    else
                        m_collisionVisuInit = true;
                    return;
                }
                    Debug.Log("m_collisionElement exists");

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

                for (int i=0; i<nbrSpheres; i++)
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
        }

        protected void ShowCollisionElements()
        {
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
                    sphere.transform.position = new Vector3(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2]);
                }
            }
        }
    }

} // namespace SofaUnity
