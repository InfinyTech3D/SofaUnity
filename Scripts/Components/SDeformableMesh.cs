using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Base class for a Deformable Mesh, inherite from SBaseMesh 
    /// This class will add a meshRenderer and create a SofaMesh API object to load the topology from Sofa Object.
    /// It will also try to create triangles mesh from tetra.
    /// </summary>
    [ExecuteInEditMode]
    public class SDeformableMesh : SBaseMesh
    {
        ////////////////////////////////////////////
        /////          Object members          /////
        ////////////////////////////////////////////

        /// Parameter to define the Mass of this deformable Object
        public float m_mass = 10.0f;
        /// Parameter to define the Young Modulus of this deformable Object
        public float m_young = 1400.0f;
        /// Parameter to define the Poisson Ratio of this deformable Object
        public float m_poisson = 0.45f;
        /// Parameter to define the uniform stiffness for all the springs of this deformable Object
        public float m_stiffness = 1000.0f;
        /// Parameter to define the uniform damping for all the springs of this deformable Object
        public float m_damping = 1.0f;

        /// Parameter to define the Mass of this deformable Object
        public float m_init_mass = 10.0f;
        /// Parameter to define the Young Modulus of this deformable Object
        public float m_init_young = 1400.0f;
        /// Parameter to define the Poisson Ratio of this deformable Object
        public float m_init_poisson = 0.45f;
        /// Parameter to define the uniform stiffness for all the springs of this deformable Object
        public float m_init_stiffness = 1000.0f;
        /// Parameter to define the uniform damping for all the springs of this deformable Object
        public float m_init_damping = 1.0f;


        /// Member: if tetrahedron is detected, will gather the number of element
        protected int nbTetra = 0;
        /// Member: if tetrahedron is detected, will store the tetrahedron topology
        protected int[] m_tetra;
        /// Member: if tetrahedron is detected, will store the vertex mapping between triangulation and tetrahedron topology
        protected Dictionary<int, int> mappingVertices;

        
        /// Initial number of vertices
        int nbVert = 0;

        /// Bool to store the last MeshFilter status. Allow to know if mesh is renderer or not.
        protected bool m_previousMRDisplay = true;




        ////////////////////////////////////////////
        /////       Object creation API        /////
        ////////////////////////////////////////////

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get access to the sofaContext
            IntPtr _simu = m_context.getSimuContext();
            if (_simu != IntPtr.Zero)
            {
                // Create the API object for SofaMesh
                m_impl = new SofaMesh(_simu, m_nameId, false);

                // TODO: check if this is still needed (and why not in children)
                m_impl.loadObject();

                // Init the FEM and spring parameter from scene.
                if (!Application.isPlaying)
                {
                    m_poisson = m_impl.poissonRatio;
                    m_mass = m_impl.mass;
                    m_young = m_impl.youngModulus;
                    m_stiffness = m_impl.stiffness;
                    m_damping = m_impl.damping;
                }

                // Call SBaseMesh.createObject() to init value loaded from the scene.
                base.createObject();
            }

            if (m_impl == null)
                Debug.LogError("SDeformableMesh:: Object creation failed.");
        }


        /// Method to check which deformable parameters coubld be set in the GUI
        protected override void initParameters()
        {
            m_init_poisson = m_impl.poissonRatio;
            m_init_mass = m_impl.mass;
            m_init_young = m_impl.youngModulus;
            m_init_stiffness = m_impl.stiffness;
            m_init_damping = m_impl.damping;

            if (!Application.isPlaying)
            {
                m_poisson = m_impl.poissonRatio;
                m_mass = m_impl.mass;
                m_young = m_impl.youngModulus;
                m_stiffness = m_impl.stiffness;
                m_damping = m_impl.damping;
            }
        }


        /// Method called by @sa Awake() method. As post process method after creation.
        protected override void awakePostProcess()
        {
            // Call SBaseMesh.awakePostProcess()
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();
            mr.enabled = false;
            m_previousMRDisplay = false;
        }




        ////////////////////////////////////////////
        /////        Object members API        /////
        ////////////////////////////////////////////

        /// Getter/Setter of current mass @see m_mass
        public float mass
        {
            get { return m_mass; }
            set
            {
                if (value != m_mass)
                {
                    m_mass = value;
                    if (m_impl != null)
                        m_impl.mass = m_mass;
                }
            }
        }

        /// Getter/Setter of current young @see m_young
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

        /// Getter/Setter of current poisson @see m_poisson
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

        /// Getter/Setter of current poisson @see m_poisson
        public float stiffness
        {
            get { return m_stiffness; }
            set
            {
                if (value != m_stiffness)
                {
                    m_stiffness = value;
                    if (m_impl != null)
                        m_impl.stiffness = m_stiffness;
                }
            }
        }

        /// Getter/Setter of current poisson @see m_poisson
        public float damping
        {
            get { return m_damping; }
            set
            {
                if (value != m_damping)
                {
                    m_damping = value;
                    if (m_impl != null)
                        m_impl.damping = m_damping;
                }
            }
        }

        /// public method that return the number of vertices, override base method by returning potentially the number of vertices from tetra topology.
        public override int nbVertices()
        {
            return nbVert;
        }

        /// public method that return the number of elements, override base method by returning potentially the number of tetrahedra.
        public override int nbTriangles()
        {
            return nbTetra;
        }




        ////////////////////////////////////////////
        /////       Object behavior API        /////
        ////////////////////////////////////////////

        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
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
                    if (m_log)
                        Debug.Log("Tetra found! Number: " + nbTetra);
                    m_tetra = new int[nbTetra * 4];

                    m_impl.getTetrahedra(m_tetra);
                    m_mesh.triangles = this.computeForceField();
                }
                else
                    m_mesh.triangles = m_impl.createTriangulation();
            }

            //m_impl.recomputeTriangles(m_mesh);

            // Set the FEM and spring parameters.
            if (m_mass != float.MinValue) // Otherwise means it has been unactivated from scene parsing
                m_impl.mass = m_mass;
            if (m_young != float.MinValue)
                m_impl.youngModulus = m_young;
            if (m_poisson != float.MinValue)
                m_impl.poissonRatio = m_poisson;
            if (m_stiffness != float.MinValue)
                m_impl.stiffness = m_stiffness;
            if (m_damping != float.MinValue)
                m_impl.damping = m_damping;

            base.initMesh(false);

            if (toUpdate)
            {
                if (nbTetra > 0)
                    updateTetraMesh();
                else
                    m_impl.updateMesh(m_mesh);
            }
        }


        /// Method called by @sa Update() method.
        public override void updateImpl()
        {
            if (m_log)
                Debug.Log("SDeformableMesh::updateImpl called.");

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();            

            if (m_impl != null && mr.enabled)
            {
                // TODO: for the moment the recompute of tetra is too expensive. Only update the number of vertices and tetra
                // Need to find another solution.
                if (m_impl.hasTopologyChanged() )
                {                    
                    m_impl.setTopologyChange(false);

                    if (nbTetra > 0)
                        updateTetraMesh();
                    else
                        m_impl.updateMesh(m_mesh);                    
                }

                if (nbTetra > 0)
                    updateTetraMesh();
                else if (mr.enabled == m_previousMRDisplay) // which is true
                    m_impl.updateMeshVelocity(m_mesh, m_context.timeStep);
                else // pass from false to true.
                {
                    m_impl.updateMesh(m_mesh);
                }
            }

            m_previousMRDisplay = mr.enabled;
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
                Vector3[] vert = new Vector3[4];

                for (int j=0; j<4; ++j)
                {
                    id[j] = i * 4 + j;
                    old_id[j] = m_tetra[i * 4 + j];
                    vert[j] = m_mesh.vertices[old_id[j]];
                }

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


        /// Method to update the TetrahedronFEM topology using the vertex mapping.
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
        
    }
}



