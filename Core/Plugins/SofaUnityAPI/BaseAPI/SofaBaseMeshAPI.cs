using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class representing Sofa Mesh Object, handling all bindings to Sofa 3D Mesh Object.
/// It will connect to the SofaPhysicsAPI and prepar the link to specific mesh objects. 
/// </summary>
public class SofaBaseMeshAPI : SofaBaseAPI
{
    /// <summary> Default constructor to create a Sofa 3D Mesh. </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaBaseMeshAPI(IntPtr simu, string nameID, bool isCustom)
        : base(simu, nameID, isCustom)
    {

    }

    /// Uniq Id of this mesh. Warning need to be retrieve each time a node/component is destroy
    protected int m_uniqID = -2002;

    /// Booleen to warn mesh normals have to be inverted
    public bool m_invertNormals = false;

    protected int m_meshDimension = 0;


    /// Internal method to check if API can be used
    override protected bool Init()
    {
        // first time create object only
        UpdateMeshId();

        m_meshDimension = sofaMeshAPI_getMeshDimension(m_simu, m_name);
        //Debug.Log("Mesh: " + m_name + " | m_meshDimension: " + m_meshDimension);

        if (m_uniqID < 0)
            return false;
        else
            return true;
    }


    public void UpdateMeshId()
    {
        if (checkNativePointer() == false)
        {
            m_uniqID = -2002;
            return;
        }

        m_uniqID = sofaMeshAPI_getId(m_simu, m_name);

        if (m_uniqID < 0)
            m_isReady = false;
    }


    public int GetMeshDimension()
    {
        return m_meshDimension;
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

    
    /// Method to get the number of vertices in the current SOFA object
    public int getNbVertices()
    {
        if (m_isReady)
        {
            int nbrV = sofaMeshAPI_getNbVertices(m_simu, m_name);
            if (nbrV < 0)
            {
                Debug.LogError("SofaBaseMeshAPI::getNbVertices method returns error: " + nbrV + ": " + SofaDefines.msg_error[nbrV] + " for object: " + m_name);
                return 0;
            }

            return nbrV;
        }
        else
            return 0;
    }


    public int GetNumberOfFaces()
    {
        int nbrTris = sofaMeshAPI_getNbTriangles(m_simu, m_name);
        if (nbrTris < 0)
        {
            //Debug.LogError("SofaBaseMeshAPI::GetNumberOfFaces method returns: " + SofaDefines.msg_error[nbrTris] + " for object: " + m_name);
            nbrTris = 0;
        }

        int nbrQuads = sofaMeshAPI_getNbQuads(m_simu, m_name);
        if (nbrQuads < 0)
        {
            //Debug.LogError("SofaBaseMeshAPI::GetNumberOfFaces method returns: " + SofaDefines.msg_error[nbrQuads] + " for object: " + m_name);
            nbrQuads = 0;
        }

        return nbrTris + nbrQuads;
    }


    /// Method to get the number of hexahedron in the current SOFA object
    public int GetNbHexahedra()
    {
        if (m_isReady)
        {
            int nbrElem = sofaMeshAPI_getNbHexahedra(m_simu, m_name);
            if (nbrElem < 0)
            {
                //Debug.LogError("SofaBaseMeshAPI::GetNbHexahedra method returns: " + SofaDefines.msg_error[nbrElem] + " for object: " + m_name);
                nbrElem = 0;
            }
            return nbrElem;
        }
        else
            return 0;
    }


    /// Method to get the number of tetrahedron in the current SOFA object
    public int GetNbTetrahedra()
    {
        if (m_isReady)
        {
            int nbrElem = sofaMeshAPI_getNbTetrahedra(m_simu, m_name);
            if (nbrElem < 0)
            {
                //Debug.LogError("SofaBaseMeshAPI::GetNbTetrahedra method returns: " + SofaDefines.msg_error[nbrElem] + " for object: " + m_name);
                nbrElem = 0;
            }
            return nbrElem;
        }
        else
            return 0;
    }


    /// Method to get the number of quads in the current SOFA object
    public int GetNbQuads()
    {
        if (m_isReady)
        {
            int nbrElem = sofaMeshAPI_getNbQuads(m_simu, m_name);
            if (nbrElem < 0)
            {
                //Debug.LogError("SofaBaseMeshAPI::GetNbQuads method returns: " + SofaDefines.msg_error[nbrElem] + " for object: " + m_name);
                nbrElem = 0;
            }
            return nbrElem;
        }
        else
            return 0;
    }


    /// Method to get the number of triangles in the current SOFA object
    public int GetNbTriangles()
    {
        if (m_isReady)
        {
            int nbrElem = sofaMeshAPI_getNbTriangles(m_simu, m_name);
            if (nbrElem < 0)
            {
                //Debug.LogError("SofaBaseMeshAPI::GetNbTriangles method returns: " + SofaDefines.msg_error[nbrElem] + " for object: " + m_name);
                nbrElem = 0;
            }
            return nbrElem;
        }
        else
            return 0;
    }


    /// Method to get the number of triangles in the current SOFA object
    public int GetNbEdges()
    {
        if (m_isReady)
        {
            int nbrElem = sofaMeshAPI_getNbEdges(m_simu, m_name);
            if (nbrElem < 0)
            {
                //Debug.LogError("SofaBaseMeshAPI::GetNbEdges method returns: " + SofaDefines.msg_error[nbrElem] + " for object: " + m_name);
                nbrElem = 0;
            }
            return nbrElem;
        }
        else
            return 0;
    }


    public virtual int[] GetHexahedraArray(int nbElem)
    {
        if (m_isReady)
        {
            int[] elems = new int[nbElem * 8];
            sofaMeshAPI_getHexahedra(m_simu, m_name, elems);
            return elems;
        }
        else
            return null;

    }


    public virtual int[] GetTetrahedraArray(int nbElem)
    {
        if (m_isReady)
        {
            int[] elems = new int[nbElem * 4];
            sofaMeshAPI_getTetrahedra(m_simu, m_name, elems);
            return elems;
        }
        else
            return null;

    }


    public virtual int[] GetQuadsArray(int nbElem)
    {
        if (m_isReady)
        {
            int[] elems = new int[nbElem * 4];
            sofaMeshAPI_getQuads(m_simu, m_name, elems);
            return elems;
        }
        else
            return null;

    }


    public virtual int[] GetTrianglesArray(int nbElem)
    {
        if (m_isReady)
        {
            int[] elems = new int[nbElem * 3];
            sofaMeshAPI_getTriangles(m_simu, m_name, elems);
            return elems;
        }
        else
            return null;

    }


    public virtual int[] GetEdgesArray(int nbElem)
    {
        if (m_isReady)
        {
            int[] elems = new int[nbElem * 2];
            sofaMeshAPI_getEdges(m_simu, m_name, elems);
            return elems;
        }
        else
            return null;

    }


    public int GetVertices(float[] vertices)
    {
        if (m_isReady)
        {
            int resV = sofaMeshAPI_getVertices(m_simu, m_name, vertices);
            return resV;
        }
        else
        {
            Debug.LogError("GetVertices: Native Pointer to Sofa3DObject is null");
            return -1;
        }
    }

    public int GetVelocities(float[] velocities)
    {
        if (m_isReady)
        {
            int resV = sofaMeshAPI_getVelocities(m_simu, m_name, velocities);
            return resV;
        }
        else
        {
            Debug.LogError("GetVelocities: Native Pointer to Sofa3DObject is null");
            return -1;
        }
    }

    public int GetForces(float[] forces)
    {
        if (m_isReady)
        {
            int resV = sofaMeshAPI_getForces(m_simu, m_name, forces);
            return resV;
        }
        else
        {
            Debug.LogError("GetVelocities: Native Pointer to Sofa3DObject is null");
            return -1;
        }
    }


    public virtual void updateVertices(Vector3[] unityVertices)
    {
        if (m_isReady)
        {
            int nbrV = sofaMeshAPI_getNbVertices(m_simu, m_name);

            if (nbrV < 0)
                return;

            if (nbrV != unityVertices.Length)
            {
                Debug.LogError("updateVertices, not the same number of vertices: " + nbrV + "compare to unity buffer: " + unityVertices.Length);
                return;
            }

            // get access to sofa buffers
            float[] vertices = new float[nbrV * 3];
            int resV = sofaMeshAPI_getVertices(m_simu, m_name, vertices);

            if (displayLog)
                Debug.Log(m_name + " | Number of vertices: " + nbrV + " with resV: " + SofaDefines.msg_error[resV]);

            for (int i = 0; i < nbrV; ++i)
            {
                unityVertices[i].x = vertices[i * 3];
                unityVertices[i].y = vertices[i * 3 + 1];
                unityVertices[i].z = vertices[i * 3 + 2];
            }
        }
    }

    /// Method to create the triangulation from Sofa topology to Unity buffers
    public virtual int[] createTriangulation()
    {
        int nbrTris = sofaMeshAPI_getNbTriangles(m_simu, m_name);
        int nbrQuads = sofaMeshAPI_getNbQuads(m_simu, m_name);

        if (displayLog)
            Debug.Log("createTriangulation: " + m_name + " | nbrTris: " + nbrTris + " | nbQuads: " + nbrQuads);
        
        if (nbrTris < 0)
            nbrTris = 0;

        if (nbrQuads < 0)
            nbrQuads = 0;

        // get buffers
        int[] quads = new int[nbrQuads*4];
        sofaMeshAPI_getQuads(m_simu, m_name, quads);

        int[] tris = new int[nbrTris * 3];
        sofaMeshAPI_getTriangles(m_simu, m_name, tris);

        // Create and fill unity triangles buffer
        int[] trisOut = new int[nbrTris*3 + nbrQuads*6];

        // fill triangles first
        int nbrIntTri = nbrTris * 3;
        for (int i = 0; i < nbrIntTri; ++i)
            trisOut[i] = tris[i];

        // Add quads splited as triangles
        for (int i = 0; i < nbrQuads; ++i)
        {
            trisOut[nbrIntTri + i * 6] = quads[i * 4];
            trisOut[nbrIntTri + i * 6 + 1] = quads[i * 4 + 1];
            trisOut[nbrIntTri + i * 6 + 2] = quads[i * 4 + 2]; 

            trisOut[nbrIntTri + i * 6 + 3] = quads[i * 4];
            trisOut[nbrIntTri + i * 6 + 4] = quads[i * 4 + 2]; 
            trisOut[nbrIntTri + i * 6 + 5] = quads[i * 4 + 3];
        }

        // Force future deletion
        quads = null;
        tris = null;

        return trisOut;
    }


    /// Method to update the Unity mesh buffers (vertices and normals) from sofa object side. Assume topology hadn't changed by default.
    /// If topology has change, createTriangulation method need to be called before this method.
    public virtual void updateMesh(Mesh mesh)
    {
        if (m_isReady)
        {
            int nbrV = sofaMeshAPI_getNbVertices(m_simu, m_name);

            if (nbrV < 0)
                return; 

            // get access to sofa buffers
            float[] vertices = new float[nbrV * 3];
            int resV = sofaMeshAPI_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            int resN = sofaMeshAPI_getNormals(m_simu, m_name, normals);

            if (displayLog)
                Debug.Log(m_name + " | Number of vertices: " + nbrV + " with resV: " + SofaDefines.msg_error[resV] + " | resN: " + SofaDefines.msg_error[resN]);


            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            bool first = false;
            if (verts.Length == 0 || verts.Length != nbrV) // need to resize vectors
            {
                verts = new Vector3[nbrV];
                norms = new Vector3[nbrV];
                first = true;
            }

            if (displayLog)
                Debug.Log(m_name + " | Number of vertices: " + nbrV + " | verts.Length: " + verts.Length + " | vertices.Lengt: " + vertices.Length);

            if (vertices.Length != 0)
            {
                int factor = 1;
                if (m_invertNormals)
                    factor = -1;

                for (int i = 0; i < nbrV; ++i)
                {
                    if (first)
                    {
                        verts[i] = new Vector3(0, 0, 0);
                        norms[i] = new Vector3(0, 0, 0);
                    }
                    
                    verts[i].x = vertices[i * 3];
                    verts[i].y = vertices[i * 3 + 1];
                    verts[i].z = vertices[i * 3 + 2];

                    if (resN < 0) // no normals
                    {
                        Vector3 vec = Vector3.Normalize(verts[i]);
                        norms[i].x = vec.x * factor;
                        norms[i].y = vec.y * factor;
                        norms[i].z = vec.z * factor;
                    }
                    else
                    {
                        norms[i].x = normals[i * 3] * factor;
                        norms[i].y = normals[i * 3 + 1] * factor;
                        norms[i].z = normals[i * 3 + 2] * factor;
                    }
                }
            }            

            mesh.vertices = verts;
            mesh.normals = norms;

            vertices = null;
            normals = null;
        }
    }


    /// Method to update the Unity mesh vertex buffer only from sofa object side. Assume topology hadn't changed by default.
    /// If topology has change, createTriangulation method need to be called before this method.
    public virtual int updateMeshVelocity(Mesh mesh, float timestep)
    {
        if (!m_isReady)
            return 0;

        bool highMov = false;
        float threshold = 5000;

        int nbrV = sofaMeshAPI_getNbVertices(m_simu, m_name);
        if (nbrV < 0)
            return 0;

        float[] velocities = new float[nbrV * 4];
        int resV = sofaMeshAPI_getVelocities(m_simu, m_name, velocities);

        if (displayLog)
            Debug.Log(m_name + " | Number of vertices: " + nbrV + " | resV: " + resV);

        if (resV < 0 || nbrV <= 0)
            return 0;

        Vector3[] verts = mesh.vertices;
        if (verts.Length == 0)// first time
            verts = new Vector3[nbrV];

        if (displayLog)
            Debug.Log(m_name + " | Number of vertices: " + nbrV + " | verts.Length: " + verts.Length + " | velocities.Lengt: " + velocities.Length);


        if (velocities.Length != 0)
        {
            int cpt = nbrV;
            for (int i = 0; i < nbrV; ++i)
            {
                int id = (int)velocities[i * 4];
                if (id == -1){ // Means no more data to copy in the buffer
                    cpt = i;
                    break;
                }
                
                verts[id].x = verts[id].x + timestep * velocities[i * 4 + 1] ;
                verts[id].y = verts[id].y + timestep * velocities[i * 4 + 2] ;
                verts[id].z = verts[id].z + timestep * velocities[i * 4 + 3] ;

                if(velocities[i * 4 + 1] > threshold || velocities[i * 4 + 2] > threshold || velocities[i * 4 + 3] > threshold)
                {
                    if (displayLog)
                        Debug.Log("BIG MOVE");
                    highMov = true;
                }
            }

            if (displayLog)
                Debug.Log(m_name + " update stop at: " + cpt + " of: " + nbrV);

        }

        mesh.vertices = verts;

        // Force future deletion
        velocities = null;
        if (highMov)
            return -1;
        else
            return 1;
    }


    /// Post processing method to recompute topology internally if needed. Should be overwritten by child if needed only.
    public virtual void recomputeTopology(Mesh mesh)
    {

    }

    private int m_topologyRevision = 0;

    /// Method to check if the topology of this mesh has changed since last update.
    public bool HasTopologyChanged()
    {
        if (m_isReady)
        {
            int value = sofaMeshAPI_getTopologyRevision(m_simu, m_name);
            if (value < 0)
            {
                if (value < -2) // no topology...
                    Debug.LogError("getTopologyRevision: " + m_name + " method returns error: " + SofaDefines.msg_error[value]);
                return false;
            }
            else
            {
                if (value != m_topologyRevision)
                {
                    m_topologyRevision = value;
                    return true;
                }
                else
                    return false;
            }
        }
        else
            return false;
    }

    /// Method to directly get the revision of the topology
    public int GetTopologyRevision()
    {
        return m_topologyRevision;
    }

    /// Method to set that the change of topology of this mesh has well be handle in this update.
    public int setTopologyChange(bool value)
    {
        if (m_isReady)
        {
            return sofaMeshAPI_setTopologyChanged(m_simu, m_name, value);
        }
        else
            return -5;
    }

    
    /// Method to get the buffer of tetrahedra from the current SOFA object
    public void getTetrahedra(int[] tetra)
    {
        if (m_isReady)
        {
            sofaMeshAPI_getTetrahedra(m_simu, m_name, tetra);
        }
    }


    /// Method to update the Unity mesh buffers (vertices and normals) from a tetrahedron topology object. Assume no topology change here.
    public virtual void updateVolumeMesh(Mesh mesh, Dictionary<int, int> mapping)
    {
        if (m_isReady)
        {
            int nbrV = sofaMeshAPI_getNbVertices(m_simu, m_name);

            //if (displayLog)
            //Debug.Log("vertices: " + nbrV);

            float[] vertices = new float[nbrV * 3];
            int resV = sofaMeshAPI_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            int resN = sofaMeshAPI_getNormals(m_simu, m_name, normals);
            
            if (resV < 0) {
                //Debug.LogError("SofaBaseMesh::updateVolumeMesh: No vertices found in '" + m_name + "'. Error return: " + SofaDefines.msg_error[resV]);
                return;
            }
            else if (displayLog)
                Debug.Log(m_name + " | Number of vertices: " + nbrV + " | verts.Length: " + vertices.Length);

            Vector3[] verts = new Vector3[nbrV];
            Vector3[] norms = new Vector3[nbrV];

            Vector3[] vertsNew = mesh.vertices;
            Vector3[] normsNew = mesh.normals;

            if (vertices.Length != 0)
            {
                for (int i = 0; i < nbrV; ++i)
                {
                    verts[i].x = vertices[i * 3];
                    verts[i].y = vertices[i * 3 + 1];
                    verts[i].z = vertices[i * 3 + 2];

                    if (resN < 0) // no normals
                    {
                        Vector3 vec = Vector3.Normalize(verts[i]);
                        norms[i].x = vec.x;// normals[i * 3];
                        norms[i].y = vec.y; //normals[i * 3 + 1];
                        norms[i].z = vec.z; //normals[i * 3 + 2];
                    }
                    else
                    {
                        norms[i].x = normals[i * 3];
                        norms[i].y = normals[i * 3 + 1];
                        norms[i].z = normals[i * 3 + 2];
                    }
                }

                foreach (KeyValuePair<int, int> id in mapping)
                {
                    vertsNew[id.Key] = verts[id.Value];
                    normsNew[id.Key] = norms[id.Value];
                }
            }

            mesh.vertices = vertsNew;
            mesh.normals = normsNew;
            vertices = null;
            normals = null;
        }
    }


    /// test method to compute UV with sphere projection, id = 0 for normal on X, id = 1 for Y and = 2 for normal on Z. Default is Y
    public void ComputeStereographicsUV(Mesh mesh, int id)
    {
        this.computeBoundingBox(mesh);

        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;
        Vector3[] vertsSphere = new Vector3[nbrV];
        Vector2[] uv = new Vector2[nbrV];

        // Compute max radius
        Vector3 bbSizes = m_max - m_min;
        float radius = bbSizes[0];
        for (int i = 1; i < 3; i++)
            if (bbSizes[i] > radius)
                radius = bbSizes[i];
        radius *= 0.5f; 

        // compute center
        Vector3 center = (m_max + m_min) * 0.5f;

        // Project all point on a sphere
        for (int i = 0; i < nbrV; i++)
        {
            Vector3 direction = verts[i] - center;
            vertsSphere[i] = center + direction;
        }

        float rangeX = 1 / (m_max.x - m_min.x);
        float rangeY = 1 / (m_max.y - m_min.y);
        float rangeZ = 1 / (m_max.z - m_min.z);

        // Project on UV map
        if (id == 0) // normal on X
        {
            for (int i = 0; i < nbrV; i++)
                uv[i] = new Vector2((vertsSphere[i].y - m_min.y) * rangeY, (vertsSphere[i].z - m_min.z) * rangeZ);
        }
        else if (id == 2) // normal on Z
        {
            for (int i = 0; i < nbrV; i++)
                uv[i] = new Vector2((vertsSphere[i].x - m_min.x) * rangeX, (vertsSphere[i].y - m_min.y) * rangeY);
        }
        else //normal on Y
        {
            for (int i = 0; i < nbrV; i++)
                uv[i] = new Vector2((vertsSphere[i].x - m_min.x) * rangeX, (vertsSphere[i].z - m_min.z) * rangeZ);
        }

        mesh.uv = uv;
    }


    public void ComputeCubeProjectionUV(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;
        this.computeBoundingBox(mesh);
        Vector3[] normals = mesh.normals;
        Vector2[] uv = new Vector2[nbrV];

        // test the orientation of the mesh
        int test = 40;
        if (test > nbrV)
            test = nbrV;

        float dist = 0.0f;
        Vector3 meanNorm = Vector3.zero;
        for (int i = 0; i < test; i++)
        {
            int id = UnityEngine.Random.Range(1, nbrV);
            dist = dist + (verts[id] - verts[id - 1]).magnitude;
            meanNorm += normals[id];
        }

        meanNorm /= test;
        dist /= test;
        dist *= 0.25f; //arbitraty scale

        int id0, id1, id3;
        if (Mathf.Abs(meanNorm.x) > 0.8)
        {
            id0 = 1; id1 = 2; id3 = 0;
        }
        else if (Mathf.Abs(meanNorm.z) > 0.8)
        {
            id0 = 0; id1 = 1; id3 = 2;
        }
        else
        {
            id0 = 0; id1 = 2; id3 = 1;
        }
        id0 = 0;
        id1 = 1;
        id3 = 2;

        float range0 = 1 / (m_max[id0] - m_min[id0]);
        float range1 = 1 / (m_max[id1] - m_min[id1]);

        for (int i = 0; i < nbrV; i++)
        {
            Vector3 norm = normals[i].normalized;
            Vector3 vert = verts[i];

            if (norm[id3] > 0.8)
                vert = vert - dist * Vector3.one;

            uv[i] = new Vector2((vert[id0] - m_min[id0]) * range0,
                (vert[id1] - m_min[id1]) * range1);
        }
        mesh.uv = uv;
    }


    /// Method to recompute the Tex coords according to mesh position and geometry.
    public virtual void UpdateTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;

        float[] texCoords = new float[nbrV * 2];
        Vector2[]  uv = new Vector2[nbrV];

        int res = sofaMeshAPI_getTexCoords(m_simu, m_name, texCoords);
        if (displayLog)
            Debug.Log("res get Texcoords: " + SofaDefines.msg_error[res]);

        if (res < 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < nbrV; i++)
            {
                uv[i].x = texCoords[i * 2];
                uv[i].y = texCoords[i * 2 + 1];
            }
        }
        mesh.uv = uv;
    }


    /// Method to set new vertices position to this mesh
    public void SetVertices(Vector3[] vertices)
    {
        if (!m_isReady)
            return;

        int nbrV = vertices.Length;
        float[] val = new float[(nbrV) * 3];

        for (int i = 0; i < nbrV; i++)
        {
            val[i * 3] = vertices[i].x;
            val[i * 3 + 1] = vertices[i].y;
            val[i * 3 + 2] = vertices[i].z;
        }

        int resUpdate = sofaMeshAPI_setVertices(m_simu, m_name, val);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }

    public void SetPositions(float[] vertices)
    {
        if (!m_isReady)
            return;

        int resUpdate = sofaMeshAPI_setVertices(m_simu, m_name, vertices);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }

    public void SetVelocities(float[] values)
    {
        if (!m_isReady)
            return;

        int resUpdate = sofaMeshAPI_setVelocities(m_simu, m_name, values);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }


    /// Method to set new vertices position to this mesh
    public void SetRestPositions(float[] vertices)
    {
        if (!m_isReady)
            return;

        //int nbrV = vertices.Length;
        //float[] val = new float[(nbrV) * 3];

        //for (int i = 0; i < nbrV; i++)
        //{
        //    val[i * 3] = vertices[i].x;
        //    val[i * 3 + 1] = vertices[i].y;
        //    val[i * 3 + 2] = vertices[i].z;
        //}

        int resUpdate = sofaMeshAPI_setRestPositions(m_simu, m_name, vertices);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }


    /// Method to set new vertices position to this mesh
    public void SetVelocities(Vector3[] vels)
    {
        if (!m_isReady)
            return;

        int nbrV = vels.Length;
        float[] val = new float[(nbrV) * 3];

        for (int i = 0; i < nbrV; i++)
        {
            val[i * 3] = vels[i].x;
            val[i * 3 + 1] = vels[i].y;
            val[i * 3 + 2] = vels[i].z;
        }

        int resUpdate = sofaMeshAPI_setVelocities(m_simu, m_name, val);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }


    public void SetNewPosition(Vector3 value)
    {
        if (!m_isReady)
            return;

        float[] val = new float[3];
        val[0] = value[0];
        val[1] = value[1];
        val[2] = value[2];
        int resUpdate = sofaMeshAPI_setVertices(m_simu, m_name, val);
        if (resUpdate < 0)
            Debug.LogError("SofaBaseMeshAPI updateMesh: " + m_name + " return error: " + SofaDefines.msg_error[resUpdate]);

    }

    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////            API to Communication with mesh geometry           ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getId(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setActive(IntPtr obj, string name, bool active);


    /// Get number of vertices in the object
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbVertices(IntPtr obj, string name);

    /// Binding to access to the position buffer
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getVertices(IntPtr obj, string name, float[] arr);

    /// Binding to access to the velocity buffer
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getVelocities(IntPtr obj, string name, float[] arr);

    /// Binding to access to the velocity buffer
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getForces(IntPtr obj, string name, float[] arr);

    /// Binding to access to the normal buffer
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNormals(IntPtr obj, string name, float[] arr);

    /// Binding to access to the texcoord buffer
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getTexCoords(IntPtr obj, string name, float[] arr);



    /////////////////////////////////////////////////////////////////////////////////////////
    ///////////            API to Communication with mesh topology           ////////////////
    /////////////////////////////////////////////////////////////////////////////////////////

    /// Binding to Edges API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbEdges(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getEdges(IntPtr obj, string name, int[] arr);
    

    /// Binding to Triangles API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbTriangles(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getTriangles(IntPtr obj, string name, int[] arr);


    /// Binding to Quads API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbQuads(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getQuads(IntPtr obj, string name, int[] arr);


    /// Binding to Tetra API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbTetrahedra(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getTetrahedra(IntPtr obj, string name, int[] arr);


    /// Binding to Hexa API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getNbHexahedra(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getHexahedra(IntPtr obj, string name, int[] arr);
    

    /// Binding to Topology change API
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getTopologyRevision(IntPtr obj, string name);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setTopologyChanged(IntPtr obj, string name, bool value);

    /// Binding to set a new mesh
    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setNbVertices(IntPtr obj, string name, int nbrV);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setVertices(IntPtr obj, string name, float[] arr);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setVelocities(IntPtr obj, string name, float[] arr);

    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_setRestPositions(IntPtr obj, string name, float[] arr);


    [DllImport("SAPAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaMeshAPI_getMeshDimension(IntPtr obj, string name);

}
