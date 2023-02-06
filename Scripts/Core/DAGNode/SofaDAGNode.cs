using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnityAPI;

namespace SofaUnity
{
    /// <summary>
    /// Class representing a DAGNode inside SOFA simulation scene.
    /// Animation loop process: 
    /// init() is called by nodeMgr with sofaContext and names
    /// - Set sofaContext and name and call InitImpl()
    /// - create connection with the good api
    /// - create Object if needed
    /// - get all data + types
    /// - get data values and fill data with default values or loaded values
    /// 
    /// When play
    /// - Awake should be done delayed (loop) while SofaContext is init
    /// - GraphNodeMgr should only init itself with empty vectors
    /// - Awake is called on each DAGNode which should retrieve SofaContext pointer
    /// - Using saved Name, connect to API
    /// - Register to GraphNodeMgr
    /// - get all data + types
    /// - Set value modified by editor!!!!!
    /// ---> Start()
    /// </summary>
    public class SofaDAGNode : SofaBase
    {
        ////////////////////////////////////////////
        //////       SofaDAGNode members       /////
        ////////////////////////////////////////////

        /// Pointer to the Sofa Context API.
        private SofaDAGNodeAPI m_impl = null;

        /// Name of this parent DAGNode
        [SerializeField]
        protected string m_parentNodeName = "None";

        /// List of SofaBaseComponent in this DAGNode
        public List<SofaBaseComponent> m_sofaComponents = null;

        /// Pointer to the SofaMesh component (if one) in this DAGNode
        protected SofaMesh m_nodeMesh = null;


        /// Bool parameter to check if transformation can be applied to this DAGNode
        [SerializeField]
        protected bool m_hasTransformEngine = false;

        /// Current Translation of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_translation;
        /// Bool parameter to store the fact that translation has been changed manually in unity.
        [SerializeField]
        protected bool m_isTranslationCustom = false;

        /// Current Rotation of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_rotation;
        /// Bool parameter to store the fact that rotation has been changed manually in unity.
        [SerializeField]
        protected bool m_isRotationCustom = false;

        /// Current Scale of this object (same as in Unity Editor and Sofa object)
        [SerializeField]
        protected Vector3 m_scale = new Vector3(1.0f, 1.0f, 1.0f);
        /// Bool parameter to store the fact that scale has been changed manually in unity.
        [SerializeField]
        protected bool m_isScaleCustom = false;


        ////////////////////////////////////////////
        //////      SofaDAGNode accessors      /////
        ////////////////////////////////////////////

        /// Getter/setter to \sa m_parentNodeName
        public string ParentNodeName
        {
            set { m_parentNodeName = value; }
            get { return m_parentNodeName; }
        }

        /// Getter to \sa m_nodeMesh
        public SofaMesh GetSofaMesh()
        {
            return m_nodeMesh;
        }

        /// Getter to \sa m_nodeMesh if pointer is null will look for it in the DAGnode component list and return one if found.
        public SofaMesh FindSofaMesh()
        {
            if (m_nodeMesh != null)
                return m_nodeMesh;

            GameObject DAGNode = this.gameObject;

            foreach (Transform child in DAGNode.transform)
            {
                SofaMesh sofaMesh = child.GetComponent<SofaMesh>();
                if (sofaMesh != null)
                {
                    m_nodeMesh = sofaMesh;
                    break;
                }
            }

            return m_nodeMesh;
        }



        public Vector3 Translation
        {
            get { return m_translation; }
            set
            {
                if (m_translation != value)
                {
                    m_translation = value;
                    m_isTranslationCustom = true;

                    if (m_impl != null)
                    {
                        m_impl.SetTransformation("translation", m_translation);
                        //PropagateSetDirty(true);
                    }
                }
            }
        }


        public Vector3 Rotation
        {
            get { return m_rotation; }
            set
            {
                if (m_rotation != value)
                {
                    m_rotation = value;
                    m_isRotationCustom = true;

                    if (m_impl != null)
                        m_impl.SetTransformation("rotation", m_rotation);
                }
            }
        }


        public Vector3 Scale
        {
            get { return m_scale; }
            set
            {
                if (m_scale != value)
                {
                    m_scale = value;
                    m_isScaleCustom = true;

                    if (m_impl != null)
                        m_impl.SetTransformation("scale3d", m_scale);
                }
            }
        }

        public bool HasTransform() { return m_hasTransformEngine; }


        ////////////////////////////////////////////
        /////      SofaDAGNode public API      /////
        ////////////////////////////////////////////

        /// Method to set all components of this Node to dirty. Does not propagate to child DAGNode, is it done by DAGNodeMgr
        public void PropagateSetDirty(bool value)
        {
            foreach (SofaBaseComponent scompo in m_sofaComponents)
            {
                scompo.SetDirty(value);
            }
        }

        /// Method to destroy this component DAGNode and all component beneath
        public void DestroyDAGNode(bool killGameObject = false)
        {
            // first clear all components
            foreach (SofaBaseComponent scompo in m_sofaComponents)
            {
                scompo.DestroyComponent(killGameObject);
            }
            m_sofaComponents.Clear();

            if (killGameObject && UniqueNameId != "root")
            {
                DestroyImmediate(this.gameObject);
            }

            DestroyImmediate(this);
        }


        /// Method to refresh the list of components child of this Node. Will ask the new list of component on the sofa side and compare to what is stored
        public void RefreshNodeChildren(bool recursive = false)
        {
            Debug.Log("RefreshNodeChildren " + UniqueNameId);
            if (m_impl == null)
            {
                SofaLog("SofaDAGNode " + UniqueNameId + " can't be refreshed, SofaDAGNodeAPI is null.", 2);
                return;
            }

            string componentsS = m_impl.RecomputeDAGNodeComponents();
            SofaLog("####### SofaDAGNode::RefreshNodeChildren: CreateSofaAPI " + UniqueNameId + " -> " + componentsS, 0, false);
            if (componentsS.Length == 0)
                return;

            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                bool found = false;
                foreach(SofaBaseComponent compo in m_sofaComponents)
                {
                    if (compo.UniqueNameId == compoName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) // new component to be added
                {
                    string baseType = m_impl.GetBaseComponentType(compoName);
                    if (baseType.Contains("Error"))
                    {
                        SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);
                    }
                    else
                    {
                        SofaLog("############## CREATE SofaBaseComponent - " + compoName + " " + baseType);
                        SofaBaseComponent compo = SofaComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
                        if (compo != null)
                        {
                            if (baseType == "SofaMesh")
                            {
                                m_nodeMesh = compo as SofaMesh;
                            }
                            m_sofaComponents.Add(compo);
                        }
                    }
                }
            }
        }
        

        ////////////////////////////////////////////
        /////     SofaDAGNode internal API     /////
        ////////////////////////////////////////////

        /// Method called by @sa SofaBase::Create() when creating objects. Will create all Sofa components.
        protected override void Create_impl()
        {
            if (m_impl != null)
            {
                if (UniqueNameId != "root")
                    SofaLog("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.", 1);
                m_impl = null;
            }
            
            m_impl = new SofaDAGNodeAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_parentNodeName, m_isCustom);

            m_sofaComponents = new List<SofaBaseComponent>();

            string componentsS = m_impl.GetDAGNodeComponents();
            if (componentsS.Length == 0)
                return;

            SofaLog("####### SofaDAGNode::Create_impl: CreateSofaAPI " + UniqueNameId + " -> " + componentsS);

            List<string> compoNames = ConvertStringToList(componentsS);
            foreach (string compoName in compoNames)
            {
                string baseType = m_impl.GetBaseComponentType(compoName);

                if (baseType.Contains("Error"))
                {
                    SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);
                }
                else
                {
                    SofaLog("############## CREATE SofaBaseComponent - " + compoName + " " + baseType);
                    SofaBaseComponent compo = SofaComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
                    if (compo != null)
                    {
                        if (baseType == "SofaMesh")
                        {
                            m_nodeMesh = compo as SofaMesh;
                        }
                        m_sofaComponents.Add(compo);
                    }
                    else
                    {
                        SofaLog("############## CREATE SofaBaseComponent Failed ");
                    }
                }
            }

            m_parentNodeName = m_impl.GetParentNodeName();
            if (m_parentNodeName.Contains("Error"))
            {
                SofaLog("Node Parent Name return error: " + m_parentNodeName + ", will use None.");
                m_parentNodeName = "None";
            }

            m_isReady = true;
        }


        /// Method called by @sa SofaBase::Reconnect() when reloading objects. Will reconnect all Sofa objects
        protected override void Reconnect_impl()
        {
            if (m_impl != null)
            {
                SofaLog("SofaDAGNode " + UniqueNameId + " already has a SofaDAGNodeAPI.", 2);
                return;
            }
            
            m_impl = new SofaDAGNodeAPI(m_sofaContext.GetSimuContext(), UniqueNameId, m_parentNodeName, m_isCustom);

            if (m_parentNodeName == "None" || m_parentNodeName.Contains("Error"))
                m_parentNodeName = m_impl.GetParentNodeName();

            string componentsS = m_impl.GetDAGNodeComponents();
            SofaLog("####### SofaDAGNode::Reconnect_impl " + UniqueNameId + " -> " + componentsS, 0, false);
            if (componentsS.Length == 0)
                return;
            
            List<string> compoNames = ConvertStringToList(componentsS);
            m_sofaComponents = new List<SofaBaseComponent>();

            foreach (string compoName in compoNames)
            {
                bool found = false;
                foreach (Transform child in this.gameObject.transform)
                {
                    SofaBaseComponent[] components = child.GetComponents<SofaBaseComponent>();
                    foreach (SofaBaseComponent component in components)
                    {
                        if (component != null && component.UniqueNameId == compoName)
                        {
                            component.Reconnect(this.m_sofaContext);
                            m_sofaComponents.Add(component);
                            found = true;
                            break;
                        }
                    }
                }


                if (!found)
                {
                    // Component has not be found in the current unity scene. Will try to add it if it was added after scene loading
                    string baseType = m_impl.GetBaseComponentType(compoName);
                    if (baseType.Contains("Error"))
                    {
                        SofaLog("Component " + compoName + " returns baseType: " + baseType, 2);
                    }
                    else
                    {
                        SofaLog("############## CREATE SofaBaseComponent - " + compoName + " " + baseType);
                        // Use the Sofa component factory to create it. Warning will be fired only if factory failed to create object. Note that Some component are ignored.
                        SofaBaseComponent compo = SofaComponentFactory.CreateSofaComponent(compoName, baseType, this, this.gameObject);
                        if (compo != null)
                        {
                            if (baseType == "SofaMesh")
                            {
                                m_nodeMesh = compo as SofaMesh;
                            }
                            m_sofaComponents.Add(compo);
                        }
                    }
                }
                    
            }

            // check not reconnected components under this nodeGameObject
            foreach (Transform child in this.gameObject.transform)
            {
                SofaBaseComponent[] components = child.GetComponents<SofaBaseComponent>();
                foreach (SofaBaseComponent component in components)
                {
                    if (!component.IsReady()) // not reconnected, need to remove old component
                    {
                        Debug.LogWarning("Component: '" + component.DisplayName + "' under node: '" + child.name + "' not reconnected to a SOFA component. GameObject will be removed.");
                        DestroyImmediate(component.gameObject);
                    }
                }
                
            }

            m_isReady = true;
        }


        /// Method called by @sa Start() method. Will check transformation values
        protected override void Init_impl()
        {
            if (m_impl == null)
                return;

            m_hasTransformEngine = m_impl.GetTransformationTest("translation");
            if (m_hasTransformEngine == false)
                return;

            if (m_isTranslationCustom)
                m_impl.SetTransformation("translation", m_translation);
            else
                m_translation = m_impl.GetTransformation("translation");

            if (m_isRotationCustom)
                m_impl.SetTransformation("rotation", m_rotation);
            else
                m_rotation = m_impl.GetTransformation("rotation");

            if (m_isScaleCustom)
                m_impl.SetTransformation("scale3d", m_scale);
            else
                m_scale = m_impl.GetTransformation("scale3d");
        }
    }

} // namespace SofaUnity
