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

        /// Pointer to the Sofa Broad phase component
        protected SofaComponent m_broadPhase = null;
        /// Pointer to the Sofa Narrow phase component
        protected SofaComponent m_narrowPhase = null;
        /// Pointer to the Sofa collision pipeline component
        protected SofaComponent m_collisionPipeline = null;
        /// Pointer to the Sofa collision response component
        protected SofaComponent m_collisionresponse = null;

        /// Pointer to the internal API to create the Collision pipeline object
        protected SofaCollisionPipelineAPI m_impl = null;

        ////////////////////////////////////////////
        ////// SofaCollisionPipeline accessors /////
        ////////////////////////////////////////////

        /// Getter/Setter on the @sa m_broadPhase component
        public SofaComponent BroadPhase
        {
            get { return m_broadPhase; }
            set { m_broadPhase = value; }
        }

        /// Getter/Setter on the @sa m_narrowPhase component
        public SofaComponent NarrowPhase
        {
            get { return m_narrowPhase; }
            set { m_narrowPhase = value; }
        }

        /// Getter/Setter on the @sa m_collisionPipeline component
        public SofaComponent CollisionPipeline
        {
            get { return m_collisionPipeline; }
            set { m_collisionPipeline = value; }
        }

        /// Getter/Setter on the @sa m_collisionresponse component
        public SofaComponent Collisionresponse
        {
            get { return m_collisionresponse; }
            set { m_collisionresponse = value; }
        }


        /////////////////////////////////////////////
        ////// SofaCollisionPipeline Object API /////
        /////////////////////////////////////////////

        /// Method called by @sa CreateObject method to really create the CollisionPipeline on SOFA side
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

        /// Method called by @sa Reconnect method to recreate the CollisionPipeline on SOFA side but not add Unity objects
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


        /// Method called by @sa Start() method. Will reconnect pointers to other objects
        protected override void Start_impl()
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


        /// Internal method to ask current owner node to refresh its list of components
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