using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class SofaDefines
{
    public enum SRayInteraction
    {
        None,
        CuttingTool,
        AttachTool,
        FixTool
    };

    static public Dictionary<int, string> msg_error = new Dictionary<int, string>
    {
        {0, "success"},
        {1, "success (bool)"},
        {2, "SofaAdvancePhysicsAPI deletion completed with success."},
        {-1, "-1: Invalid SofaAdvancePhysicsAPI pointer (API not found or null)."},
        {-2, "-2: Invalid Sofa3DObject pointer (Object not found or null)."},
        {-3, "-3: Invalid Sofa3DObjectManager pointer (Object not found or null)."},
        {-4, "-4: Method called is deprecated."},
        {-5, "-5: Method called has not yet been implemented."},
        {-6, "-6: Glew init error."},

        {-10, "-10: Sofa simulation scene creation failed."},
        {-11, "-11: Sofa plugin loading failed."},
        {-12, "-12: Sofa plugin loading failed. Cause: invalid loading."},
        {-13, "-13: Sofa plugin loading failed. Cause: missing symbol."},
        {-14, "-14: Sofa plugin loading failed. Cause: file not found."},
        {-15, "-15: Invalid SofaDAGNodeManager pointer (Object not found or null)."},
        {-16, "-16: Invalid SofaComponentManager pointer (Object not found or null)."},
        {-17, "-17: Invalid SofaSimulationManager pointer (Object not found or null)."},

        {-18, "-18: Simulation scene file (.scn) not found."},
        {-19, "-19: Simulation scene file (.scn) is empty."},

        {-21, "-21: Sofa object creation failed due to incorrect name."},
        {-22, "-22: Sofa object creation failed because simulation can't be reached."},
        {-23, "-23: Sofa object creation failed due to duplicated name or bad object type."},
        {-24, "-24: Sofa object creation failed because this type is not handled."},
        {-25, "-25: Sofa object creation failed because no mechanicalObject has been found."},
        {-26, "-26: Sofa object creation failed because no collision model has been found."},
        {-27, "-27: Sofa object duplicated objects found."},

        {-30, "-30: Invalid SofaAPI impl object (Object not found or null)."},
        {-31, "-31: Invalid SofaAPI object pointer access (Object not found or null)."},
        {-32, "-32: Invalid SofaAPI parent object access (Object not found or null)."},
        {-33, "-33: Invalid SofaAPI component listener pointer access (Object not found or null)."},

        {-40, "-40: Invalid Sofa object access (Object not found or null)."},
        {-41, "-41: Invalid Sofa Data access (Data not found or null). "},
        {-42, "-42: Invalid Data type (type doesn't exist or cast failed)."},
        {-43, "-43: Invalid Data name (name not found)."},
        {-44, "-44: Data is empty."},
        {-45, "-45: Data size is incorrect."},

        {-51, "-51: Invalid MechanicalObject (Object not found or null)."},
        {-52, "-52: Invalid Mesh (Visual mesh or topology not found or null)."},
        {-53, "-53: Multiple Sofa object of requested type found in the same Node."},
        {-54, "-54: Invalid case scenario reached (good luck)."},
        {-55, "-55: Duplicated Sofa object found in simulation graph."},

        {-60, "-60: Invalid DAGNode access (DAGNode not found or null)."},
        {-61, "-61: Invalid embedded Sofa Node access (DAGNode not found or null)."},
        {-62, "-62: Invalid DAGNode root access (DAGNode not found or null)."},
        {-63, "-63: Invalid DAGNode name, DAGNode not found in API."},
        {-64, "-64: Invalid DAGNode access. DAGNode not found in API."},
        {-65, "-65: Invalid DAGNode parent access (DAGNode not found or null)."},
        {-66, "-66: Invalid DAGNode ID. Out of bounds."},

        {-70, "-70: Invalid Component access (Component not found or null)."},
        {-71, "-71: Invalid Component name. Component not found in API."},
        {-72, "-72: Invalid Component base type. (Base type not found in API)."},
        {-73, "-73: Invalid Component type, Type not found in component available types."},
        {-74, "-74: Error, component creation fails during the process."},

        {-80, "-80: Invalid Mesh Interface implementation. MeshIO specialisation is needed."},
        {-81, "-81: Invalid Mesh Interface type. Request can't be process for this type of MeshIO"},
        {-82, "-82: Invalid Mesh Interface access (MeshIO not found or null)."},
        {-83, "-83: Invalid Mesh ID, out of bounds."},

        {-9999, "License initialization failed at step 1."},
        {-9998, "License initialization failed at step 2."},
        {-9997, "licence limit exceeded."},
        {-9996, "licence limit reached."},
        {-9995, "License initialization failed at step 3."},

        {-9994, "License file not found."},
        {-9993, "License file content error."},
        {-9992, "licence installation limit exceeded."},
        {-9991, "licence MAC address error."},

        {-100, "-100: SofaAdvancePhysicsAPI illegal access."},
        {-101, "-101: SofaUnity init value."},

        {-400, "-400: Ray casting manager not found."},
        {-401, "-401: Ray casting tool not found."},
        {-402, "-402: Ray casting invalid tool type."},
        {-403, "-403: Ray casting no target found."},
        {-404, "-404: Ray casting invalid target found."},
        {-405, "-405: Ray casting, no valid topology found."},
        {-406, "-406: Ray casting, no valid collision model found."},
        {-407, "-407: Ray casting, no valid mechanical mapper found."},
        {-408, "-408: Ray casting, no valid mechanical state found."},
        {-409, "-409: Ray casting, attribute name given is invalid."},
        {-410, "-410: Ray casting, already in contact."},
        {-411, "-411: Ray casting method not implemented."},
        {400, "400: Ray casting, already in contact."},

        {-500, "-500: Garsping plugin not found."},
        {-501, "-501: Grasping manager not found."},
        {-502, "-502: Grasping tool not found."},
        {-503, "-503: Grasping interaction error."},

        {-600, "-600: Entact plugin not found."},
        {-601, "-601: Entact manager not found."},
        {-602, "-602: Entact tool not found."},

        {-700, "-700: Geomagic plugin not found."},
        {-701, "-701: Geomagic manager not found."},

        {-800, "-800: SofaCUDA is disable."},
        {-666, "SofaAPAPI test id."}
    };
}



