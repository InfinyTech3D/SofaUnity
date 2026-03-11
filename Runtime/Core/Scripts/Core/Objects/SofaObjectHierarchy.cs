using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SofaUnity
{    
    public class SofaObjectHierarchy
    {
        /// List of SofaBaseObject in this hierarchy
        public List<SofaBaseObject> m_objects = null;

        /// Dictionary storing the hierarchy of Sofa objects. Key = parent name, value = List of children names.
        protected Dictionary<string, List<string> > hierarchy;

        /// pointer to the SofaContext root object
        private SofaContext m_sofaContext = null;

        /// Parameter: Int number of objects created in this context. Used to add count in object name created from Unity.
        public int m_objectCpt = 0;

        /// Parameter: Int number of object to be loaded from a Sofa scene.
        public int m_nbrObject = 0;

        /// Parameter: Internal counter to the number of object created from a Sofa scene.
        public int cptCreated = 0;

        // default constructor of the SObjectHiearchy
        public SofaObjectHierarchy(SofaContext context)
        {
            // set the sofa Context
            m_sofaContext = context;
            if (m_sofaContext == null)
                Debug.LogError("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " >> SofaContext is null");

            // create the list of SofaBaseObject
            m_objects = new List<SofaBaseObject>();
        }

        //~SofaObjectHierarchy()
        //{
        //    //Debug.Log("## SofaObjectHierarchy::IsPlaying: >> ~SofaObjectHierarchy()");
        //}


        public void registerSObject(SofaBaseObject obj)
        {
            if (m_objects == null)
                Debug.LogError("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " >> m_objects is null");

            m_objects.Add(obj);
        }



        /// Method to compute the hierarchy of SofaObject using the parent name. Will move all children to recreate the Sofa hierarchy.
        public void recreateHiearchy()
        {
            if (m_sofaContext == null)
                return;

            hierarchy = new Dictionary<string, List<string>>();

            foreach (Transform child in m_sofaContext.transform)
            {
                SofaBaseObject obj = child.GetComponent<SofaBaseObject>();

                if (m_sofaContext.m_log)
                    Debug.Log("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " parent: " + obj.ParentName());
                if (hierarchy.ContainsKey(obj.ParentName()))
                    hierarchy[obj.ParentName()].Add(child.name);
                else
                {
                    List<string> children = new List<string>();
                    children.Add(child.name);
                    hierarchy.Add(obj.ParentName(), children);
                }
            }

            foreach (KeyValuePair<string, List<string>> entry in hierarchy)
            {
                List<string> children = entry.Value;

                if (m_sofaContext.m_log)
                {
                    foreach (string childName in children)
                        Debug.Log("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " parent: " + entry.Key + " - child: " + childName);
                }

                if (entry.Key != "root" && entry.Key != "No impl")
                    moveChildren(entry.Key);
            }
            hierarchy.Clear();
        }


        /// Method to move each children according to its parent name
        protected void moveChildren(string parentName)
        {
            List<string> children = hierarchy[parentName];

            // get parent
            Transform parent = m_sofaContext.transform;
            bool found = false;
            foreach (Transform child in m_sofaContext.transform)
                if (child.name.Contains(parentName))
                {
                    parent = child.transform;
                    found = true;
                    break;
                }

            if (!found)
                Debug.LogError("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " moveChildren parent node not found: " + parentName);


            foreach (string childName in children)
            {
                foreach (Transform child in m_sofaContext.transform)
                {
                    if (m_sofaContext.m_log)
                        Debug.Log("## SofaObjectHierarchy::IsPlaying: " + Application.isPlaying + " name found: " + child.name + " current parent: " + child.transform.parent.name);

                    if (child.name.Contains(childName))
                    {
                        child.transform.parent = parent.transform;
                        break;
                    }
                }
            }
        }


        public void clearHierarchy()
        {
            
        }

    }
}
