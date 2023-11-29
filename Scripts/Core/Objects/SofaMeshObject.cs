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
    /// This class inherite from @see SofaBaseObject and add the creation of Mesh and handle transformation
    /// </summary>
    [ExecuteInEditMode]
    public class SofaMeshObject : SofaBaseObject
    {
        /////////////////////////////////////////////
        //////    SofaMeshObject API members    /////
        /////////////////////////////////////////////

        /// Pointer to the corresponding SOFA API object
        //public SofaBaseObjectAPI m_impl = null;


        ///// Parameter to define the Mass of this deformable Object
        //public float m_mass = float.MinValue;
        ///// Parameter to define the Young Modulus of this deformable Object
        //public float m_young = float.MinValue;
        ///// Parameter to define the Poisson Ratio of this deformable Object
        //public float m_poisson = float.MinValue;
        ///// Parameter to define the uniform stiffness for all the springs of this deformable Object
        //public float m_stiffness = float.MinValue;
        ///// Parameter to define the uniform damping for all the springs of this deformable Object
        //public float m_damping = float.MinValue;

        /// Current Translation of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_translation;
        
        /// Current Rotation of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_rotation;
        
        /// Current Scale of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_scale = new Vector3(1.0f, 1.0f, 1.0f);


        /// Booleen to warn mesh normals have to be inverted
        public bool m_invertNormals = false;

        /////////////////////////////////////////////
        //////   SofaMeshObject API accessors   /////
        /////////////////////////////////////////////

        /// Getter/Setter of current translation @see m_translation
        public Vector3 translation
        {
            get { return m_translation; }
            set
            {
                if (value != m_translation)
                {
                    //Vector3 diffTrans = value - m_translation;
                    m_translation = value;
                //    if (m_impl != null)
                //    {
                //        //m_impl.translation = diffTrans;
                //        m_impl.updateMesh(m_mesh);
                //    }
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
                    //Vector3 diffRot = value - m_rotation;
                    m_rotation = value;
                    //if (m_impl != null)
                    //{
                    //    //m_impl.rotation = diffRot;
                    //    m_impl.updateMesh(m_mesh);
                    //}
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
                    //Vector3 diffScale = value - m_scale;
                    m_scale = value;
                    //if (m_impl != null)
                    //{
                    //    //m_impl.scale = diffScale;
                    //    m_impl.updateMesh(m_mesh);
                    //}
                }
            }
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
                    //if (m_impl != null)
                    //    m_impl.m_invertNormals = m_invertNormals;

                    invertMeshNormals();
                }
            }
        }

        /// Method to invert normal by changing triangles orientation
        public void invertMeshNormals()
        {
            //int[] triangles = m_mesh.GetTriangles(0);
            //for (int i = 0; i < triangles.Length; i += 3)
            //{
            //    int temp = triangles[i + 0];
            //    triangles[i + 0] = triangles[i + 1];
            //    triangles[i + 1] = temp;
            //}
            //m_mesh.SetTriangles(triangles, 0);
        }

        protected override void Init_impl()
        {

        }


        // TODO: restore that
        public int nbVertices()
        {
            return 0;
        }
        // TODO: restore that
        public int nbTriangles()
        {
            return 0;
        }

        /////////////////////////////////////////////
        //////   SofaMeshObject internal API    /////
        /////////////////////////////////////////////



    }
}
