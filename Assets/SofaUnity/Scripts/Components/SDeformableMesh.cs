using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SDeformableMesh : SBaseMesh
    {
        protected int nbTetra = 0;
        protected int[] m_tetra;

        protected override void awakePostProcess()
        {
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();
        }

        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            m_mesh.name = "SofaMesh";
            m_mesh.vertices = new Vector3[0];
            m_impl.updateMesh(m_mesh);
            //m_mesh.triangles = m_impl.createTriangulation();
            //m_impl.updateMesh(m_mesh);

            // Special part for tetra
            if (nbTetra == 0)
            {
                nbTetra = m_impl.getNbTetrahedra();
                if (nbTetra > 0)
                {
                    Debug.Log("Tetra: " + nbTetra);
                    m_tetra = new int[nbTetra * 4];

                    m_impl.getTetrahedra(m_tetra);
                    Debug.Log("tetra found start: " + m_tetra[0] + " " + m_tetra[1] + " " + m_tetra[2] + " " + m_tetra[3]);
                    m_mesh.triangles = this.computeForceField();
                }
                else
                    m_mesh.triangles = m_impl.createTriangulation();
            }

            //m_impl.recomputeTriangles(m_mesh);
            
            m_impl.mass = m_mass;
            m_impl.youngModulus = m_young;
            m_impl.poissonRatio = m_poisson;

            base.initMesh(false);

            if (toUpdate)
            {
                if (nbTetra > 0)
                    updateTetraMesh();
                else
                    m_impl.updateMesh(m_mesh);
            }
        }

        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                m_impl = new SofaMesh(_simu, m_nameId, false);
                m_impl.loadObject();

                m_poisson = m_impl.poissonRatio;
                m_mass = m_impl.mass;
                m_young = m_impl.youngModulus;

                // Set init value loaded from the scene.
                base.createObject();
            }

            if (m_impl == null)
                Debug.LogError("SDeformableMesh:: Object not created");
        }


        protected override void updateImpl()
        {
            if (m_log)
                Debug.Log("SDeformableMesh::updateImpl called.");

            if (m_impl != null)
            {
                if (nbTetra > 0)
                    updateTetraMesh();
                else
                    m_impl.updateMesh(m_mesh);
                //m_mesh.RecalculateNormals();
            }

           // if (m_drawFF)
            //    drawForceField();
        }
               

        public bool m_drawFF = false;
        public bool drawFF
        {
            get { return m_drawFF; }
            set { m_drawFF = value;
                Debug.Log("set ff!!");
            }
        }

        //public Material mat;
        //void OnPostRender()
        //{
        //    if (m_drawFF)
        //        drawForceField();
        //}

        public void drawForceField()
        {
            if (m_mesh.vertices.Length == 0)
                return;

            GL.Begin(GL.TRIANGLES);

            for (int i=0; i<nbTetra; ++i)
            {
                Vector3 v0 = m_mesh.vertices[m_tetra[i * 4 + 0]];
                Vector3 v1 = m_mesh.vertices[m_tetra[i * 4 + 1]];
                Vector3 v2 = m_mesh.vertices[m_tetra[i * 4 + 2]];
                Vector3 v3 = m_mesh.vertices[m_tetra[i * 4 + 3]];

             //   Debug.Log("v0: " + v0 + " v1: " + v1 );

                GL.Vertex3(v0.x, v0.y, v0.z);
                GL.Vertex3(v1.x, v1.y, v1.z);
                GL.Vertex3(v2.x, v2.y, v2.z);

                GL.Vertex3(v1.x, v1.y, v1.z);
                GL.Vertex3(v2.x, v2.y, v2.z);
                GL.Vertex3(v3.x, v3.y, v3.z);

                GL.Vertex3(v2.x, v2.y, v2.z);
                GL.Vertex3(v3.x, v3.y, v3.z);
                GL.Vertex3(v0.x, v0.y, v0.z);

                GL.Vertex3(v3.x, v3.y, v3.z);
                GL.Vertex3(v0.x, v0.y, v0.z);
                GL.Vertex3(v1.x, v1.y, v1.z);
            }

            GL.End();            
        }


        protected Dictionary<int, int> mappingVertices;
        public int[] computeForceField()
        {
            int[] tris = new int[nbTetra * 12];
            Vector3[] verts = new Vector3[nbTetra * 4];//m_mesh.vertices;
            Vector3[] norms = new Vector3[nbTetra * 4];//m_mesh.normals;
            Vector2[] uv = new Vector2[nbTetra * 4];
            mappingVertices = new Dictionary<int, int>();

            for (int i = 0; i < nbTetra; ++i)
            {
                int[] id = new int[4];
                int[] old_id = new int[4];
                Vector3[] vert = new Vector3[4];

                for (int j=0; j<4; ++j)
                {
                    id[j] = i * 4 + j;
                    old_id[j] = m_tetra[i * 4 + j];
                    vert[j] = m_mesh.vertices[old_id[j]];
                }

                Vector3 bary = (vert[0] + vert[1] + vert[2] + vert[3]) / 4;

                // vert of new tetra reduce to the center of the tetra
                for (int j = 0; j < 4; ++j)
                {
                    verts[id[j]] = vert[j];
                    norms[id[j]] = m_mesh.normals[old_id[j]];
                    mappingVertices.Add(id[j], old_id[j]);
                    // update tetra to store new ids
                    m_tetra[i * 4 + j] = id[j];
                    uv[i * 4 + j].x = j / 4;
                    uv[i * 4 + j].y = uv[i * 4 + j].x;
                }

                tris[i * 12 + 0] = id[0];
                tris[i * 12 + 1] = id[2];
                tris[i * 12 + 2] = id[1];

                tris[i * 12 + 3] = id[1];
                tris[i * 12 + 4] = id[2];
                tris[i * 12 + 5] = id[3];

                tris[i * 12 + 6] = id[2];
                tris[i * 12 + 7] = id[0];
                tris[i * 12 + 8] = id[3];

                tris[i * 12 + 9] = id[3];
                tris[i * 12 + 10] = id[0];
                tris[i * 12 + 11] = id[1];
            }

            m_mesh.vertices = verts;
            m_mesh.normals = norms;
            m_mesh.uv = uv;

            return tris;
        }

        public void updateTetraMesh()
        {
            // first update the vertices dissociated
            m_impl.updateMeshTetra(m_mesh, mappingVertices);

            // Compute the barycenters of each tetra and update the vertices
            Vector3[] verts = m_mesh.vertices;
            for (int i = 0; i < nbTetra; ++i)
            {
                Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);
                int idI = i * 4;
                // compute tetra barycenter
                for (int j = 0; j < 4; ++j)
                    bary += verts[m_tetra[idI + j]];
                bary /= 4;

                // reduce the tetra size according to the barycenter
                for (int j = 0; j < 4; ++j)
                    verts[m_tetra[idI + j]] = bary + (verts[m_tetra[idI + j]] - bary) * 0.7f;
            }

            m_mesh.vertices = verts;
        }

        public float m_mass = 10.0f;
        public float mass
        {
            get { return m_mass; }
            set
            {
                if (value != m_mass)
                {
                    m_mass = value;
                    if (m_impl != null)
                        m_impl.mass  = m_mass;
                }
            }
        }


        public float m_young = 1400.0f;
        public float young
        {
            get { return m_young; }
            set
            {
                if (value != m_young)
                {
                    m_young = value;
                    if (m_impl != null)
                        m_impl.youngModulus = m_young;
                }
            }
        }

        public float m_poisson = 0.45f;
        public float poisson
        {
            get { return m_poisson; }
            set
            {
                if (value != m_poisson)
                {
                    m_poisson = value;
                    if (m_impl != null)
                        m_impl.poissonRatio = m_poisson;
                }
            }
        }
    }
}



