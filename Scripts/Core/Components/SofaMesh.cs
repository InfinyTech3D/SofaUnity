using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SofaUnity
{
    public class SofaMesh : SofaBaseComponent
    {
        /// Member: Unity Mesh object of this GameObject
        protected Mesh m_mesh;
        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMeshAPI m_sofaMeshAPI = null;

        /// Member: if tetrahedron is detected, will gather the number of element
        protected int nbTetra = 0;
        /// Member: if tetrahedron is detected, will store the tetrahedron topology
        protected int[] m_tetra;
        /// Member: if tetrahedron is detected, will store the vertex mapping between triangulation and tetrahedron topology
        protected Dictionary<int, int> mappingVertices;
        /// Initial number of vertices
        int nbVert = 0;

        protected override void CreateSofaAPI()
        {
            if (m_impl != null)
            {
                Debug.LogError("SofaBaseComponent " + UniqueNameId + " already has a SofaBaseComponentAPI.");
                return;
            }

            if (m_sofaContext == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext is null", 1);
                return;
            }

            if (m_sofaContext.GetSimuContext() == null)
            {
                SofaLog("CreateSofaAPI: " + UniqueNameId + " m_sofaContext.GetSimuContext() is null", 1);
                return;
            }

            SofaLog("SofaVisualModel::CreateSofaAPI: " + UniqueNameId + " | m_sofaContext: " + m_sofaContext + " | m_sofaContext.GetSimuContext(): " + m_sofaContext.GetSimuContext());
            m_impl = new SofaVisualModelAPI(m_sofaContext.GetSimuContext(), UniqueNameId);


            InitBaseMeshAPI();
        }



        ///// public method that return the number of vertices, override base method by returning potentially the number of vertices from tetra topology.
        //public override int nbVertices()
        //{
        //    return nbVert;
        //}

        ///// public method that return the number of elements, override base method by returning potentially the number of tetrahedra.
        //public override int nbTriangles()
        //{
        //    return nbTetra;
        //}



        protected void InitBaseMeshAPI()
        {
            if (m_sofaMeshAPI == null)
            {
                // Get access to the sofaContext
                IntPtr _simu = m_sofaContext.GetSimuContext();

                if (_simu == IntPtr.Zero)
                    return;

                // Create the API object for SofaMesh
                m_sofaMeshAPI = new SofaBaseMeshAPI(m_sofaContext.GetSimuContext(), UniqueNameId, false);
                SofaLog("SofaVisualModel::InitBaseMeshAPI object created");

                m_sofaMeshAPI.loadObject();

                // Add a MeshFilter to the GameObject
                MeshFilter mf = gameObject.GetComponent<MeshFilter>();
                if (mf == null)
                    gameObject.AddComponent<MeshFilter>();

                //to see it, we have to add a renderer
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr == null)
                {
                    mr = gameObject.AddComponent<MeshRenderer>();
                    mr.enabled = false;
                }

                if (mr.sharedMaterial == null)
                {
                    mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
                }

                //MeshCollider collid = gameObject.GetComponent<MeshCollider>();
                //if (collid == null)
                //    gameObject.AddComponent<MeshCollider>();

                initMesh(true);
            }
        }


        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected void initMesh(bool toUpdate)
        {
            if (m_sofaMeshAPI == null)
                return;

#if UNITY_EDITOR
            //Only do this in the editor
            MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
            //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
            Mesh meshCopy = new Mesh();
            m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes

#else
            //do this in play mode
            m_mesh = GetComponent<MeshFilter>().mesh;
            if (m_log)
                Debug.Log("SofaBox::Start play mode.");
#endif

            m_mesh.name = "SofaMesh";
            m_mesh.vertices = new Vector3[0];
            m_sofaMeshAPI.updateMesh(m_mesh);


            // Special part for tetra
            if (nbTetra == 0)
            {
                nbTetra = m_sofaMeshAPI.getNbTetrahedra();
                if (nbTetra > 0)
                {
                    SofaLog("Tetra found! Number: " + nbTetra, 1, true);
                    m_tetra = new int[nbTetra * 4];

                    m_sofaMeshAPI.getTetrahedra(m_tetra);
                    m_mesh.triangles = this.computeForceField();
                }
                else
                    m_mesh.triangles = m_sofaMeshAPI.createTriangulation();
            }

            SofaLog("SofaVisualModel::initMesh ok: " + m_mesh.vertices.Length);
            //base.initMesh(false);

            if (toUpdate)
            {
                if (nbTetra > 0)
                    updateTetraMesh();
                else
                    m_sofaMeshAPI.updateMesh(m_mesh);
            }
                
        }

        public bool m_forceUpdate = false;
        /// Method called by @sa Update() method.
        protected override void UpdateImpl()
        {
            SofaLog("SofaVisualMesh::updateImpl called.");

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

            if (m_impl != null && (m_forceUpdate || mr.enabled))
            {
                // TODO: for the moment the recompute of tetra is too expensive. Only update the number of vertices and tetra
                // Need to find another solution.
                //if (m_impl.hasTopologyChanged())
                //{
                //    m_impl.setTopologyChange(false);

                //    if (nbTetra > 0)
                //        updateTetraMesh();
                //    else
                //        m_impl.updateMesh(m_mesh);
                //}

                if (nbTetra > 0)
                    updateTetraMesh();
                else if (mr.enabled == true) // which is true
                    m_sofaMeshAPI.updateMeshVelocity(m_mesh, m_sofaContext.TimeStep);
                else // pass from false to true.
                {
                    m_sofaMeshAPI.updateMesh(m_mesh);
                }
            }
        }


        /// Method to compute the TetrahedronFEM topology and store it as triangle in Unity Mesh, will store the vertex mapping into @see mappingVertices
        public int[] computeForceField()
        {
            int[] tris = new int[nbTetra * 12];
            Vector3[] verts = new Vector3[nbTetra * 4];//m_mesh.vertices;
            Vector3[] norms = new Vector3[nbTetra * 4];//m_mesh.normals;
            Vector2[] uv = new Vector2[nbTetra * 4];
            mappingVertices = new Dictionary<int, int>();
            nbVert = m_mesh.vertices.Length;

            for (int i = 0; i < nbTetra; ++i)
            {
                int[] id = new int[4];
                int[] old_id = new int[4];

                int idTet = i * 4;
                for (int j = 0; j < 4; ++j)
                {
                    id[j] = idTet + j;
                    old_id[j] = m_tetra[idTet + j];

                    verts[id[j]] = m_mesh.vertices[old_id[j]];
                    norms[id[j]] = m_mesh.normals[old_id[j]];
                    mappingVertices.Add(id[j], old_id[j]);

                    m_tetra[idTet + j] = id[j];
                    uv[idTet + j].x = j / 4;
                    uv[idTet + j].y = uv[i * 4 + j].x;
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

                tris[i * 12 + 9] = id[0];
                tris[i * 12 + 10] = id[1];
                tris[i * 12 + 11] = id[3];
            }

            m_mesh.vertices = verts;
            m_mesh.normals = norms;
            m_mesh.uv = uv;

            return tris;
        }


        /// Method to update the TetrahedronFEM topology using the vertex mapping.
        public void updateTetraMesh()
        {
            // first update the vertices dissociated
            m_sofaMeshAPI.updateMeshTetra(m_mesh, mappingVertices);

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
                    verts[m_tetra[idI + j]] = bary + (verts[m_tetra[idI + j]] - bary) * 0.5f;
            }

            m_mesh.vertices = verts;
        }
    }

} // namespace SofaUnity
