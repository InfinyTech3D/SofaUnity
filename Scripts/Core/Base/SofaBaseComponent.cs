using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SofaUnity
{
    // TODO find a way to interactively add more type if plugin are loaded
    public enum SBaseComponentType
    {
        SofaSolver,
        SofaLoader,
        SofaMesh,
        SofaMass,
        SofaFEMForceField,
        SofaMechanicalMapping,
        SofaCollisionModel,
        SofaVisualModel
    };

    
    public class SofaBaseComponent : SofaBase
    {
        // do generic stuff for baseComponent here
        protected SofaDAGNode m_ownerNode = null;

        public string m_baseComponentType = "Not set";
        public string m_componentType = "Not set";
        public SBaseComponentType myType;
    }
}