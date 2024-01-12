using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Main class of the Sofa plugin. This is the base class representing Sofa 3D Object, handling all bindings to Sofa 3D Object.
/// It will connect to the SofaPhysicsAPI. 
/// </summary>
public class SofaBaseObjectAPI : IDisposable
{
    /// <summary> Default constructor, will call impl method: @see createObject() </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaBaseObjectAPI(IntPtr simu, string nameID, string parentName, bool isRigid)
    {
        m_simu = simu;
        m_name = nameID;
        m_isRigid = isRigid;
        m_parentName = parentName;

        m_isCreated = createObject();
    }

    /// Destructor
    ~SofaBaseObjectAPI()
    {
        Dispose(false);
    }


    /// Pointer to the SofaPhysicsAPI 
    protected IntPtr m_simu = IntPtr.Zero;
    /// Pointer to the Sofa 3D object. TODO: check why this pointer messed up in use.
    //protected IntPtr m_native = IntPtr.Zero;
    protected bool m_hasObject = false;

    // TODO: check if needed
    bool m_isDisposed;
    /// Parameter to activate internal logging
    public bool displayLog = false;

    /// Name of the Sofa 3D Object mapped to this Object.
    protected string m_name;
    /// Type of Sofa 3D Object mapped to this Object.
    protected string m_type;
    /// Sofa 3D Object parent name in Sofa simulation Tree.
    protected string m_parentName;
    /// Parameter to store the information if the object is rigid or deformable.
    protected bool m_isRigid = false;

    /// Parameter to store the info if creation succeed. Will store the value return by @sa createObject
    public bool m_isCreated = false;


    /// Memory free method
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// Memory free method
    protected virtual void Dispose(bool disposing)
    {
        if (!m_isDisposed)
        {
            m_isDisposed = true;
        }
    }


    /// Implicit method to really create object and link to Sofa object. To be overwritten by child.
    protected virtual bool createObject()
    {
        return false;
    }


    /// Implicit method load the object from the Sofa side. TODO: check if still needed
    public virtual void loadObject()
    {

    }


    /// Getter of the sofa 3D Object parent name.
    public string parent
    {
        get { return m_parentName; }
    }


    /// BoundingBox min Value in 3D
    protected Vector3 m_min = new Vector3(100000, 100000, 100000);
    /// BoundingBox max Value in 3D
    protected Vector3 m_max = new Vector3(-100000, -100000, -100000);

    /// Method to compute the Mesh BoundingBox
    public virtual void computeBoundingBox(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;

        // Get min and max of the mesh
        for (int i = 0; i < nbrV; i++)
        {
            if (verts[i].x > m_max.x)
                m_max.x = verts[i].x;
            if (verts[i].y > m_max.y)
                m_max.y = verts[i].y;
            if (verts[i].z > m_max.z)
                m_max.z = verts[i].z;

            if (verts[i].x < m_min.x)
                m_min.x = verts[i].x;
            if (verts[i].y < m_min.y)
                m_min.y = verts[i].y;
            if (verts[i].z < m_min.z)
                m_min.z = verts[i].z;
        }
    }


    /// Booleen to warn mesh normals have to be inverted
    public bool m_invertNormals = false;

    public virtual int[] createTriangulation()
    {
        return new int[0];
    }

    public virtual void updateMesh(Mesh mesh)
    {

    }

    public virtual void recomputeTexCoords(Mesh mesh)
    {

    }

    public virtual void recomputeTopology(Mesh mesh)
    {

    }


    /// <summary> Generic method to get value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Float value of the Data field, return float.MinValue if field is not found. </returns>
    public float getFloatValue_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return float.MinValue;
    }


    /// <summary> Generic method to set value of a Data<float> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New float value of the Data. </param>
    public void setFloatValue_deprecated(string dataName, float value)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
    }


    /// <summary> Generic method to get value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Int value of the Data field, return int.MinValue if field is not found. </returns>
    public int getIntValue_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return int.MinValue;
    }


    /// <summary> Generic method to set value of a Data<int> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New int value of the Data. </param>
    public void setIntValue_deprecated(string dataName, int value)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
    }


    /// <summary> Generic method to get value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Bool value of the Data field, return false if field is not found. </returns>
    public bool getBoolValue_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return false;
    }


    /// <summary> Generic method to set value of a Data<bool> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New bool value of the Data. </param>
    public void setBoolValue_deprecated(string dataName, bool value)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
    }


    /// <summary> Generic method to get value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> String value of the Data field, return "None" if field is not found. </returns>
    public string getStringValue_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return "None";
    }


    /// <summary> Generic method to set value of a Data<string> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="value"> New string value of the Data. </param>
    public void setStringValue_deprecated(string dataName, string value)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
    }


    /// <summary> Generic method to get value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> Vector3 of the Data field, return Vector3 of float.MinValue if field is not found. </returns>
    public Vector3 getVector3fValue_deprecated(string dataName)
    {
        Vector3 values = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return values;
    }



    /// <summary> Generic method to set value of a Data<Vec3f> field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="values"> New Vector3 values of the Data. </param>
    public void setVector3fValue_deprecated(string dataName, Vector3 values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVecfSize_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int getVectorfValue_deprecated(string dataName, int size, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVectorfValue_deprecated(string dataName, int size, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<int> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVeciSize_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    /// <summary> Generic method to get values of a Data< vector<int> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int getVeciValue_deprecated(string dataName, int size, int[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVeciValue_deprecated(string dataName, int size, int[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }



    /// <summary> Generic method to get size of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> size of the Data vector. Return negative value if field not found or error encountered. </returns>
    public int getVecofVec3fSize_deprecated(string dataName)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }


    /// <summary> Generic method to get values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> Values of the Data vector field returned. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int getVecofVec3fValue_deprecated(string dataName, int size, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }


    /// <summary> Generic method to set values of a Data< vector<float> > field. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <param name="size"> Size of the Data vector. </param>
    /// <param name="values"> New values to set to the Data vector field. </param>
    /// <returns> Int error code. Negative value if method failed, 0 otherwise. </returns>
    public int setVecofVec3fValue_deprecated(string dataName, int size, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }






    public int getVecfValue_deprecated(string dataName, string dataType, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    public int setVecfValue_deprecated(string dataName, string dataType, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    public int getRigidfValue_deprecated(string dataName, string dataType, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    public int setRigidfValue_deprecated(string dataName, string dataType, float[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    public int getRigiddValue_deprecated(string dataName, string dataType, double[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }

    public int setRigiddValue_deprecated(string dataName, string dataType, double[] values)
    {
        Debug.LogError("Method is deprecated and should not be used anymore.");
        return -1;
    }



    /// <summary> Method to check pointer of the Sofa object. </summary>
    /// <param name="dataName"> Name of the Data field requested. </param>
    /// <returns> True if pointer is valid otherwise return false. </returns>
    protected bool checkNativePointerForGetData(string dataName)
    {
        if (!m_hasObject)
        {
            Debug.LogError("Error getting parameter: " + dataName + " of object: " + m_name + " . Can't access Object Pointer m_native.");
            return false;
        }
        else
            return true;
    }


    /// <summary> Method to check pointer of the Sofa object. </summary>
    /// <param name="dataName"> Name of the Data field to set. </param>
    /// <returns> True if pointer is valid otherwise return false. </returns>
    protected bool checkNativePointerForSetData(string dataName)
    {
        if (!m_hasObject)
        {
            Debug.LogError("Error setting parameter: " + dataName + " in object: " + m_name + " . Can't access Object Pointer m_native.");
            return false;
        }
        else
            return true;
    }



    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////            API to Communication with Sofa objects            ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////
    
}
