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
        static Dictionary<string, Func<GameObject, SofaBaseComponent>> s_componentFactory = null;        

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
            sofaCompo.Create(sofaNodeOwner.m_sofaContext, nameId, "unset");
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


        public static void InitBaseFactoryType()
        {
            if (s_componentFactory != null)
                s_componentFactory = null;

            s_componentFactory = new Dictionary<string, Func<GameObject, SofaBaseComponent>>();

            Func<GameObject, SofaBaseComponent> solverMethod = (gameO) => gameO.AddComponent<SofaSolver>();
            s_componentFactory.Add("SofaOdeSolver", solverMethod);

            Func<GameObject, SofaBaseComponent> linearSolver = (gameO) => gameO.AddComponent<SofaSolver>();
            s_componentFactory.Add("SofaLinearSolver", linearSolver);

            Func<GameObject, SofaBaseComponent> loaderMethod = (gameO) => gameO.AddComponent<SofaLoader>();
            s_componentFactory.Add("SofaLoader", loaderMethod);

            Func<GameObject, SofaBaseComponent> meshMethod = (gameO) => gameO.AddComponent<SofaMesh>();
            s_componentFactory.Add("SofaMesh", meshMethod);

            Func<GameObject, SofaBaseComponent> massMethod = (gameO) => gameO.AddComponent<SofaMass>();
            s_componentFactory.Add("SofaMass", massMethod);

            Func<GameObject, SofaBaseComponent> femMethod = (gameO) => gameO.AddComponent<SofaFEMForceField>();
            s_componentFactory.Add("SofaFEMForceField", femMethod);

            Func<GameObject, SofaBaseComponent> mechaMappingMethod = (gameO) => gameO.AddComponent<SofaMechanicalMapping>();
            s_componentFactory.Add("SofaMechanicalMapping", mechaMappingMethod);

            Func<GameObject, SofaBaseComponent> collisionMethod = (gameO) => gameO.AddComponent<SofaCollisionModel>();
            s_componentFactory.Add("SofaCollisionModel", collisionMethod);

            Func<GameObject, SofaBaseComponent> constraintMethod = (gameO) => gameO.AddComponent<SofaConstraint>();
            s_componentFactory.Add("SofaConstraint", constraintMethod);

            Func<GameObject, SofaBaseComponent> visualMethod = (gameO) => gameO.AddComponent<SofaVisualModel>();
            s_componentFactory.Add("SofaVisualModel", visualMethod);

            Func<GameObject, SofaBaseComponent> pluginMethod = (gameO) => null;
            s_componentFactory.Add("SofaRequiredPlugin", pluginMethod);

            Func<GameObject, SofaBaseComponent> visualCompoMethod = (gameO) => gameO.AddComponent<SofaComponent>();
            s_componentFactory.Add("SofaVisualComponent", visualCompoMethod);

            Func<GameObject, SofaBaseComponent> DefaultVisualManagerLoop = (gameO) => null;
            s_componentFactory.Add("DefaultVisualManagerLoop", DefaultVisualManagerLoop);

            Func<GameObject, SofaBaseComponent> animLoopMethod = (gameO) => gameO.AddComponent<SofaAnimationLoop>();
            s_componentFactory.Add("SofaAnimationLoop", animLoopMethod);

            Func<GameObject, SofaBaseComponent> engineMethod = (gameO) => gameO.AddComponent<SofaComponent>();
            s_componentFactory.Add("SofaEngine", engineMethod);
        }


        static public SofaBaseComponent CreateSofaComponent(string nameId, string componentType, SofaDAGNode sofaNodeOwner, GameObject parent)
        {
            
            if (componentType != "SofaCollisionModel" && componentType.IndexOf("SofaCollision") != -1)
                return CreateSofaCollisionPipeline(nameId, componentType, sofaNodeOwner, parent);                    


            GameObject compoGO = new GameObject("SofaComponent - " + nameId);
            SofaBaseComponent sofaCompo = null;

            try
            {
                Func<GameObject, SofaBaseComponent> method = s_componentFactory[componentType];
                sofaCompo = method(compoGO);
            }
            catch (KeyNotFoundException)
            {
                sofaCompo = compoGO.AddComponent<SofaComponent>();
            }

            if (sofaCompo)
            {
                sofaCompo.SetDAGNode(sofaNodeOwner);
                sofaCompo.Create(sofaNodeOwner.m_sofaContext, nameId, "unset");
                sofaCompo.m_baseComponentType = sofaCompo.BaseTypeFromString(componentType);
                compoGO.transform.parent = parent.gameObject.transform;
            }
            else
            {
                //Debug.LogWarning("DestroyImmediate: " + compoGO.name + " Of type: " + componentType);
                GameObject.DestroyImmediate(compoGO);
            }
            
            return sofaCompo;
        }
    }

}
