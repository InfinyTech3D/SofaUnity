using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SofaUnity;
using SofaUnityAPI;
using System;

namespace SofaUnity
{
    [ExecuteInEditMode]
    public class SGrid : SDeformableMesh
    {
        protected bool m_useTex = true;

        protected override void awakePostProcess()
        {
            base.awakePostProcess();

            //to see it, we have to add a renderer
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();

            mr.material = new Material(Shader.Find("Diffuse"));

            if (this.m_useTex)
                mr.material = Resources.Load("Materials/BoxSofa") as Material;            
        }

        protected override void initMesh(bool toUpdate)
        {
            if (m_impl == null)
                return;

            base.initMesh(false);

            m_mesh.name = "SofaGrid";          
            m_impl.setGridResolution(m_gridSize);
            m_impl.recomputeTexCoords(m_mesh);

            if (toUpdate)
                m_impl.updateMesh(m_mesh);
        }


        protected override void updateImpl()
        {
            if (m_log)
                Debug.Log("SGrid::updateImpl called.");

            if (m_impl != null) {
                m_impl.updateMesh(m_mesh);
                //m_mesh.RecalculateNormals();
            }
        }
        
        public Vector3 m_gridSize = new Vector3(5, 5, 5);
        public virtual Vector3 gridSize
        {
            get { return m_gridSize; }
            set
            {
                if (value != m_gridSize)
                {
                    m_gridSize = value;
                    if (m_impl != null)
                        m_impl.setGridResolution(m_gridSize);
                }
            }
        }

    }    
}
