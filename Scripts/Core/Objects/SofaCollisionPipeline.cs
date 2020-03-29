using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    /// <summary>
    /// Specific class describing a Sofa Collision pipeline components 
    /// </summary>
    public class SofaCollisionPipeline : MonoBehaviour
    {
        ////////////////////////////////////////////
        //////  SofaCollisionPipeline members  /////
        ////////////////////////////////////////////

        protected SofaComponent m_broadPhase = null;
        protected SofaComponent m_narrowPhase = null;
        protected SofaComponent m_collisionPipeline = null;
        protected SofaComponent m_collisionresponse = null;


        ////////////////////////////////////////////
        //////  SofaCollisionPipeline members  /////
        ////////////////////////////////////////////

        public SofaComponent BroadPhase
        {
            get { return m_broadPhase; }
        }

        public SofaComponent NarrowPhase
        {
            get { return m_narrowPhase; }
        }

        public SofaComponent CollisionPipeline
        {
            get { return m_collisionPipeline; }
        }

        public SofaComponent Collisionresponse
        {
            get { return m_collisionresponse; }
        }


        ////////////////////////////////////////////
        ////// SofaCollisionPipeline accessors /////
        ////////////////////////////////////////////

        public void SetBroadPhaseComponent(SofaComponent compo)
        {
            m_broadPhase = compo;
        }

        public void SetNarrowPhaseComponent(SofaComponent compo)
        {
            m_narrowPhase = compo;
        }

        public void SetCollisionPipelineComponent(SofaComponent compo)
        {
            m_collisionPipeline = compo;
        }

        public void SetCollisionResponseComponent(SofaComponent compo)
        {
            m_collisionresponse = compo;
        }

    }

} // namespace SofaUnity