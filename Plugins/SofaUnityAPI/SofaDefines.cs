using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class SofaDefines
{
    static public Dictionary<int, string> msg_error = new Dictionary<int, string>
    {
        {0, "success"},
        {1, "success (bool)"},
        {2, "SofaAdvancePhysicsAPI deletion completed with success."},
        {-1, "Invalid SofaAdvancePhysicsAPI pointer (API not found or null)."},
        {-2, "Invalid Sofa3DObject pointer (Object not found or null)."},
        {-3, "Invalid Sofa3DObjectManager pointer (Object not found or null)."},
        {-4, "Method called is deprecated."},
        {-5, "Method called has not yet been implemented."},
        {-6, "Glew init error."},
        {-10, "Sofa simulation scene creation failed."},
        {-11, "Sofa simulation scene loading failed."},

        {-21, "Sofa object creation failed due to incorrect name."},
        {-22, "Sofa object creation failed because simulation can't be reached."},
        {-23, "Sofa object creation failed due to duplicated name or bad object type."},
        {-24, "Sofa object creation failed because this type is not handled."},
        {-25, "Sofa object creation failed because no mechanicalObject has been found."},
        {-26, "Sofa object creation failed because no collision model has been found."},

        {-30, "Invalid SofaAPI impl object (Object not found or null)."},
        {-31, "Invalid SofaAPI object pointer access (Object not found or null)."},
        {-32, "Invalid SofaAPI parent object access (Object not found or null)."},
        {-33, "Invalid SofaAPI component listener pointer access (Object not found or null)."},

        {-40, "Invalid Sofa object access (Object not found or null)."},
        {-41, "Invalid Sofa Data access (Data not found or null). "},
        {-42, "Invalid Data type (type doesn't exist or cast failed)."},
        {-43, "Invalid Data name (name not found)."},
        {-44, "Data is empty."},
        {-45, "Data size is incorrect."},

        {-51, "Invalid MechanicalObject (Object not found or null)."},
        {-52, "Invalid Mesh (Visual mesh or topology not found or null)."},
        {-53, "Multiple Sofa object of requested type found in the same Node."},
        {-54, "Invalid case scenario reached (good luck)."},

        {-9999, "License initialization failed at step 1."},
        {-9998, "License initialization failed at step 2."},
        {-9997, "licence limit exceeded."},
        {-9996, "licence limit reached."},
        {-9995, "License initialization failed at step 3."},

        {-100, "SofaAdvancePhysicsAPI illegal access."},
        {-101, "SofaUnity init value."},

        {-400, "Ray casting manager not found."},
        {-401, "Ray casting tool not found."},
        {-402, "Ray casting invalid tool type."},
        {-403, "Ray casting no target found."},
        {-404, "Ray casting invalid target found."},
        {-405, "Ray casting, no valid topology found."},
        {-406, "Ray casting, no valid collision model found."},
        {-407, "Ray casting, no valid mechanical mapper found."},
        {-408, "Ray casting, no valid mechanical state found."},
        {-409, "Ray casting, attribute name given is invalid."},
        {400, "Ray casting, already in contact."},

        {-500, "Garsping plugin not found."},
        {-501, "Grasping manager not found."},
        {-502, "Grasping tool not found."},
        {-503, "Grasping interaction error."},

        {-600, "Entact plugin not found."},
        {-601, "Entact manager not found."},
        {-602, "Entact tool not found."},

        {-700, "Geomagic plugin not found."},
        {-701, "Geomagic manager not found."}
    };
}

