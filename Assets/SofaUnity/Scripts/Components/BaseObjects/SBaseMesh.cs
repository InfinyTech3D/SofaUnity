using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    /// <summary>
    /// Base class that design a Mesh GameObject mapped to a Sofa 3DObject.
    /// This class inherite from @see SBaseObject and add the creation of Mesh and handle transformation
    /// </summary>
    [ExecuteInEditMode]
    public class SBaseMesh : SBaseObject
    {
        /// Member: Unity Mesh object of this GameObject
		protected Mesh m_mesh;

        /// Pointer to the corresponding SOFA API object
        protected SofaBaseMesh m_impl = null;

        /// Initial Translation from Sofa Object at init
        protected Vector3 m_initTranslation;
        /// Initial Rotation from Sofa Object at init
        protected Vector3 m_initRotation;
        /// Initial Scale from Sofa Object at init
        protected Vector3 m_initScale = new Vector3(1.0f, 1.0f, 1.0f);

        /// Current Translation of this object (same as in Unity Editor and Sofa object)
        public Vector3 m_translation;
        /// Current Rotation of this object (same as in Unity Editor and Sofa object)
        public Vector3 m_rotation;
        /// Current Scale of this object (same as in Unity Editor and Sofa object)
        public Vector3 m_scale = new Vector3(1.0f, 1.0f, 1.0f);

        /// Booleen to warn mesh normals have to be inverted
        public bool m_invertNormals = false;

        /// Method called by @sa loadContext() method. To create the object when Sofa context has been found.
        protected override void createObject()
        {
            // Get initial transformation
            m_initTranslation = m_impl.translation;
            m_initRotation = m_impl.rotation;
            m_initScale = m_impl.scale;

            // Copy the initial transformation as the current one for init
            m_translation = m_initTranslation;
            m_rotation = m_initRotation;
            m_scale = m_initScale;
        }

        /// Method called by \sa Awake after the loadcontext method.
        protected override void awakePostProcess()
        {
            // Add a MeshFilter to the GameObject
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf == null)
                gameObject.AddComponent<MeshFilter>();
        }

        /// Method called at GameObject init (after creation or when starting play).
        private void Start()
        {
            if (m_log)
                Debug.Log("UNITY_EDITOR - SBaseMesh::start - " + m_nameId);

            if (m_impl != null)
            {
#if UNITY_EDITOR
                //Only do this in the editor
                MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
                //Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
                Mesh meshCopy = new Mesh();
                m_mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes

                if (m_log)
                    Debug.Log("SMesh::Start editor mode.");
#else
                //do this in play mode
                m_mesh = GetComponent<MeshFilter>().mesh;
                if (m_log)
                    Debug.Log("SBox::Start play mode.");
#endif

                initMesh(true);
            }
        }


        /// Method called by \sa Start() method to init the current object and impl. @param toUpdate indicate if updateMesh has to be called.
        protected virtual void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            // Only apply transformation if different from init one (change from editor)
            if (m_translation != m_initTranslation)
                m_impl.translation = m_translation;

            if (m_rotation != m_initRotation)
                m_impl.rotation = m_rotation;

            if (m_scale != m_initScale)
                m_impl.scale = m_scale;

            if (m_invertNormals)
            {
                m_impl.m_invertNormals = m_invertNormals;
                invertMeshNormals();
            }

            // Update the Sofa Object
            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }


        /// Getter of parentName of this Sofa Object.
        public override string parentName()
        {
            if (m_impl == null)
                return "No impl";
            else
                return m_impl.parent;
        }

        /// Getter/Setter to @see m_invertNormals that order to change or not the mesh normals at load.
        public bool invertNormals
        {
            get { return m_invertNormals; }
            set
            {
                m_invertNormals = value;
                if (m_invertNormals)
                {
                    if (m_impl != null)
                        m_impl.m_invertNormals = m_invertNormals;

                    invertMeshNormals();
                }
            }
        }

        /// Method to invert normal by changing triangles orientation
        public void invertMeshNormals()
        {
            int[] triangles = m_mesh.GetTriangles(0);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            m_mesh.SetTriangles(triangles, 0);
        }

        /// Getter/Setter of current translation @see m_translation
        public Vector3 translation
        {
            get { return m_translation; }
            set
            {
                if (value != m_translation)
                {
                    Vector3 diffTrans = value - m_translation;
                    m_translation = value;
                    if (m_impl != null)
                    {
                        m_impl.translation = diffTrans;
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

        /// Getter/Setter of current rotation @see m_rotation
        public Vector3 rotation
        {
            get { return m_rotation; }
            set
            {
                if (value != m_rotation)
                {
                    Vector3 diffRot = value - m_rotation;
                    m_rotation = value;
                    if (m_impl != null)
                    {
                        m_impl.rotation = diffRot;
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

        /// Getter/Setter of current scale @see m_scale        
        public Vector3 scale
        {
            get { return m_scale; }
            set
            {
                if (value != m_scale)
                {
                    Vector3 diffScale = value - m_scale;
                    m_scale = value;
                    if (m_impl != null)
                    {
                        m_impl.scale = diffScale;
                        m_impl.updateMesh(m_mesh);
                    }
                }
            }
        }

        /// Public method to get the number of vertices in then embedded mesh
        public virtual int nbVertices()
        {
            if (m_mesh)
                return m_mesh.vertexCount;
            else
                return -1;
        }

        /// Public method to get the number of triangles in then embedded mesh
        public virtual int nbTriangles()
        {
            if (m_mesh)
                return m_mesh.triangles.Length/3;
            else
                return -1;
        }
    }
}
