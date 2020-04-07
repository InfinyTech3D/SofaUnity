using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SofaUnityAPI;

namespace SofaUnity
{
    /// manage specific component class implementation and creation 
    /// TODO: how to create a real factory in C# ??
    static public class SofaComponentFactory
    {
        static public SofaBaseComponent CreateSofaCollisionPipeline(string nameId, string componentType, SofaDAGNode sofaNodeOwner, GameObject parent)
        {
            GameObject UnityNode = sofaNodeOwner.gameObject;
            GameObject collisionUnityObject = null;
            foreach (Transform child in UnityNode.transform)
            {
                if (child.gameObject.name == "SofaCollisionPipeline")
                {
                    collisionUnityObject = child.gameObject;
                    break;
                }
            }


            SofaCollisionPipeline collisionPipe = null;
            if (collisionUnityObject == null)
            {
                collisionUnityObject = new GameObject("SofaCollisionPipeline");
                collisionPipe = collisionUnityObject.AddComponent<SofaCollisionPipeline>();
                collisionUnityObject.transform.parent = parent.gameObject.transform;
            }
            else
            {
                collisionPipe = collisionUnityObject.GetComponent<SofaCollisionPipeline>();
            }


            SofaComponent sofaCompo = collisionUnityObject.AddComponent<SofaComponent>();
            sofaCompo.SetDAGNode(sofaNodeOwner);
            sofaCompo.SetPropagateName(false);
            sofaCompo.ShowData = false;
            sofaCompo.Create(sofaNodeOwner.m_sofaContext, nameId);
            sofaCompo.m_baseComponentType = sofaCompo.BaseTypeFromString(componentType);

            if (componentType == "SofaCollisionPipeline")
            {
                collisionPipe.CollisionPipeline = sofaCompo;
            }
            else if (componentType == "SofaCollisionAlgorithm")
            {
                collisionPipe.Collisionresponse = sofaCompo;
            }
            else if (componentType == "SofaCollisionDetection")
            {
                collisionPipe.BroadPhase = sofaCompo;
            }
            else if (componentType == "SofaCollisionIntersection")
            {
                collisionPipe.NarrowPhase = sofaCompo;
            }

            return sofaCompo;
        }


        static public SofaBaseComponent CreateSofaComponent(string nameId, string componentType, SofaDAGNode sofaNodeOwner, GameObject parent)
        {
            
            if (componentType != "SofaCollisionModel" && componentType.IndexOf("SofaCollision") != -1)
                return CreateSofaCollisionPipeline(nameId, componentType, sofaNodeOwner, parent);                    


            GameObject compoGO = new GameObject("SofaComponent - " + nameId);
            SofaBaseComponent sofaCompo = null;
            if (componentType == "SofaSolver")
            {
                sofaCompo = compoGO.AddComponent<SofaSolver>();
            }
            else if (componentType == "SofaLoader")
            {
                sofaCompo = compoGO.AddComponent<SofaLoader>();
            }
            else if (componentType == "SofaMesh")
            {
                sofaCompo = compoGO.AddComponent<SofaMesh>();
            }
            else if (componentType == "SofaMass")
            {
                sofaCompo = compoGO.AddComponent<SofaMass>();
            }
            else if (componentType == "SofaFEMForceField")
            {
                sofaCompo = compoGO.AddComponent<SofaFEMForceField>();
            }
            else if (componentType == "SofaMechanicalMapping")
            {
                sofaCompo = compoGO.AddComponent<SofaMechanicalMapping>();
            }
            else if (componentType == "SofaCollisionModel")
            {
                sofaCompo = compoGO.AddComponent<SofaCollisionModel>();
            }
            else if (componentType == "SofaConstraint")
            {
                sofaCompo = compoGO.AddComponent<SofaConstraint>();
            }
            else if (componentType == "SofaVisualModel")
            {
                sofaCompo = compoGO.AddComponent<SofaVisualModel>();
            }
            else if (componentType == "SofaRequiredPlugin")
            {
                // not handled here. TODO: do that better
                //compoGO.destr
                compoGO = null;
                //Destroy(compoGO);
                return null;
            }
            else if (componentType == "SofaAnimationLoop")
            {
                sofaCompo = compoGO.AddComponent<SofaAnimationLoop>();
            }
            else
            {
                compoGO = null;
                Debug.LogWarning("Component type not handled: " + componentType);
                return null;
            }

            // set generic parameters
            sofaCompo.SetDAGNode(sofaNodeOwner);
            sofaCompo.Create(sofaNodeOwner.m_sofaContext, nameId);
            sofaCompo.m_baseComponentType = sofaCompo.BaseTypeFromString(componentType);
            compoGO.transform.parent = parent.gameObject.transform;

            return sofaCompo;
        }
    }

}
