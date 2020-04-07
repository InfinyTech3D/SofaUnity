using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Collision pipeline components 
    /// </summary>
    public class SofaCollisionPipeline : SofaBaseObject
    {
        ////////////////////////////////////////////
        //////  SofaCollisionPipeline members  /////
        ////////////////////////////////////////////

        protected SofaComponent m_broadPhase = null;
        protected SofaComponent m_narrowPhase = null;
        protected SofaComponent m_collisionPipeline = null;
        protected SofaComponent m_collisionresponse = null;

        protected SofaCollisionPipelineAPI m_impl = null;

        ////////////////////////////////////////////
        ////// SofaCollisionPipeline accessors /////
        ////////////////////////////////////////////

        public SofaComponent BroadPhase
        {
            get { return m_broadPhase; }
            set { m_broadPhase = value; }
        }

        public SofaComponent NarrowPhase
        {
            get { return m_narrowPhase; }
            set { m_narrowPhase = value; }
        }

        public SofaComponent CollisionPipeline
        {
            get { return m_collisionPipeline; }
            set { m_collisionPipeline = value; }
        }

        public SofaComponent Collisionresponse
        {
            get { return m_collisionresponse; }
            set { m_collisionresponse = value; }
        }


        /////////////////////////////////////////////
        ////// SofaCollisionPipeline Object API /////
        /////////////////////////////////////////////

        protected override void Create_impl()
        {
            SofaLog("####### SofaCollisionPipeline::Create_impl: " + UniqueNameId);
            if (m_impl == null)
            {
                m_impl = new SofaCollisionPipelineAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaCollisionPipeline:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                    return;
                }

                // create objects
                CreateCollisionObjects();
                m_isCreated = true;
            }
            else
                SofaLog("SofaCollisionPipeline::Create_impl, SofaCollisionPipeline already created: " + UniqueNameId, 1);
        }

        protected override void Reconnect_impl()
        {            
            SofaLog("####### SofaCollisionPipeline::Reconnect_impl: " + UniqueNameId);

            if (m_impl == null)
            {
                m_impl = new SofaCollisionPipelineAPI(m_sofaContext.GetSimuContext(), m_uniqueNameId);
                if (m_impl == null || !m_impl.m_isCreated)
                {
                    SofaLog("SofaCollisionPipeline:: Object creation failed: " + m_uniqueNameId, 2);
                    this.enabled = false;
                    return;
                }

                // create objects
                m_isCreated = true;
            }
            else
                SofaLog("SofaCollisionPipeline::Create_impl, SofaCollisionPipeline already created: " + UniqueNameId, 1);
        }


        protected override void Init_impl()
        {
            if (!m_isCreated)
                return;

            SofaComponent[] components = this.gameObject.GetComponents<SofaComponent>();
            foreach (SofaComponent component in components)
            {
                string baseType = component.BaseTypeToString(component.m_baseComponentType);
                if (baseType == "SofaCollisionPipeline")
                {
                    m_collisionPipeline = component;
                }
                else if (baseType == "SofaCollisionAlgorithm")
                {
                    m_collisionresponse = component;
                }
                else if (baseType == "SofaCollisionDetection")
                {
                    m_broadPhase = component;
                }
                else if (baseType == "SofaCollisionIntersection")
                {
                    m_narrowPhase = component;
                }
            }
        }

        protected void CreateCollisionObjects()
        {
            SofaDAGNode rootNode = m_sofaContext.gameObject.GetComponent<SofaDAGNode>();
            if (rootNode == null)
            {
                SofaLog("SofaCollisionPipeline:: root Node not found in GameObject: " + m_sofaContext.gameObject.name, 2);
                return;
            }

            rootNode.RefreshNodeChildren();
        }
    }

} // namespace SofaUnity