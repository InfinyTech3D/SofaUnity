using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SofaUnity;
using System;

//[InitializeOnLoad]
public class GeomagicInitPlugin
{
    protected SofaContext m_sofaContext = null;

    static GeomagicInitPlugin()
    {
        Debug.Log("#####GeomagicInitPlugin ");
        Func<GameObject, SofaBaseComponent> constraintMethod = (gameO) => gameO.AddComponent<SofaConstraint>();
        //SofaComponentFactory.RegisterComponentType("SofaController", constraintMethod);
    }

    //void Awake()
    //{
    //    if (m_sofaContext == null)
    //    {
    //        GameObject _contextObject = GameObject.FindGameObjectWithTag("GameController");
    //        if (_contextObject != null)
    //        {
    //            // Get Sofa context
    //            m_sofaContext = _contextObject.GetComponent<SofaUnity.SofaContext>();
    //        }
    //        else
    //        {
    //            Debug.LogError("RayCaster::loadContext - No SofaContext found.");
    //            return;
    //        }
    //    }

    //    //m_sofaContext.PluginManager.

    //    //SofaComponentFactory.RegisterComponentType()
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
