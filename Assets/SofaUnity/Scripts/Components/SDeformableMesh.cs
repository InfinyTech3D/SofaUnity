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

            m_impl.updateMesh(m_mesh);

            m_impl.recomputeTriangles(m_mesh);

            m_impl.setMass(m_mass);
            m_impl.setYoungModulus(m_young);
            m_impl.setPoissonRatio(m_poisson);

            base.initMesh(false);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);            
        }

        protected override void createObject()
        {
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                m_impl = new SofaMesh(_simu, m_nameId, false);
                m_impl.loadObject();
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
                m_impl.updateMesh(m_mesh);
                m_mesh.RecalculateNormals();
            }

            if (m_drawFF)
                drawForceField();
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

        public int[] computeForceField()
        {
            int[] tris = new int[nbTetra * 12];

            for (int i = 0; i < nbTetra; ++i)
            {
                int id0 = m_tetra[i * 4 + 0];
                int id1 = m_tetra[i * 4 + 1];
                int id2 = m_tetra[i * 4 + 2];
                int id3 = m_tetra[i * 4 + 3];

                tris[i * 12 + 0] = id0;
                tris[i * 12 + 1] = id2;
                tris[i * 12 + 2] = id1;

                tris[i * 12 + 3] = id1;
                tris[i * 12 + 4] = id2;
                tris[i * 12 + 5] = id3;

                tris[i * 12 + 6] = id2;
                tris[i * 12 + 7] = id0;
                tris[i * 12 + 8] = id3;

                tris[i * 12 + 9] = id3;
                tris[i * 12 + 10] = id0;
                tris[i * 12 + 11] = id1;
            }

            return tris;
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
                        m_impl.setMass(m_mass);
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
                        m_impl.setYoungModulus(m_young);
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
                        m_impl.setPoissonRatio(m_poisson);
                }
            }
        }
    }
}



