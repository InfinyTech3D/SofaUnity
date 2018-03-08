using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the base class representing Sofa Mesh Object, handling all bindings to Sofa 3D Object.
/// It will connect to the SofaPhysicsAPI and prepar the link to specific mesh objects. 
/// </summary>
public class SofaBaseMesh : SofaBaseObject
{
    /// <summary>
    /// Default constructor to create a Sofa Mesh
    /// </summary>
    /// <param name="simu">Pointer to the SofaPhysicsAPI</param>
    /// <param name="nameID">Name of this Object</param>
    /// <param name="isRigid">Type rigid or deformable</param>
    public SofaBaseMesh(IntPtr simu, string nameID, bool isRigid)
        : base(simu, nameID, isRigid)
    {

    }

    /// Booleen to warn mesh normals have to be inverted
    public bool m_invertNormals = false;


    /// Implicit method load the object from the Sofa side.
    public override void loadObject()
    {
        //if (m_native != IntPtr.Zero)        
        if (m_native == IntPtr.Zero) // first time create object only
        {
            m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Mesh can't be found: " + m_name);
            else
            {
                // Debug.Log("Load Node Name: " + m_name);
                m_parent = sofaPhysicsAPI_getParentNodeName(m_simu, m_name);
            }
        }
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


    /// Getter/Setter of the translation field
    public Vector3 translation
    {
        get { return getVector3fValue("translation"); }
        set { setVector3fValue("translation", value); }
    }

    /// Getter/Setter of the rotation field
    public Vector3 rotation
    {
        get { return getVector3fValue("rotation"); }
        set { setVector3fValue("rotation", value); }
    }

    /// Getter/Setter of the scale field
    public Vector3 scale
    {
        get { return getVector3fValue("scale"); }
        set { setVector3fValue("scale", value); }
    }

    /// Setter of the grid resolution
    public void setGridResolution(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            int[] gridSizes = new int[3];
            for (int i = 0; i < 3; ++i)
                gridSizes[i] = (int)values[i];
            int res = sofaPhysics3DObject_setVec3iValue(m_simu, m_name, "gridSize", gridSizes);
            gridSizes = null;
            if (log)
                Debug.Log("Change gridSize res: " + res);
        }
    }

    /// Getter/Setter of the mass field
    public float mass
    {
        get { return getFloatValue("totalMass"); }
        set { setFloatValue("totalMass", value); }
    }

    /// Getter/Setter of the young Modulus field
    public float youngModulus
    {
        get { return getFloatValue("youngModulus"); }
        set { setFloatValue("youngModulus", value); }
    }

    /// Getter/Setter of the poisson Ratio field
    public float poissonRatio
    {
        get { return getFloatValue("poissonRatio"); }
        set { setFloatValue("poissonRatio", value); }
    }

    /// Getter/Setter of the stiffness for all the springs of this mesh
    public float stiffness
    {
        get { return getFloatValue("stiffness"); }
        set { setFloatValue("stiffness", value); }
    }

    /// Getter/Setter of the damping for all the springs of this mesh
    public float damping
    {
        get { return getFloatValue("damping"); }
        set { setFloatValue("damping", value); }
    }


    /// Method to create the triangulation from Sofa topology to Unity buffers
    public virtual int[] createTriangulation()
    {
        
        int nbrTris = sofaPhysics3DObject_getNbTriangles(m_simu, m_name);
        int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, m_name);

        if (log)
        {
            Debug.Log("createTriangulation: " + m_name);
            Debug.Log("nbrTris: " + nbrTris);
            Debug.Log("nbQuads: " + nbrQuads);
        }
        
        if (nbrTris < 0)
            nbrTris = 0;

        if (nbrQuads < 0)
            nbrQuads = 0;

        // get buffers
        int[] quads = new int[nbrQuads*4];
        sofaPhysics3DObject_getQuads(m_simu, m_name, quads);

        int[] tris = new int[nbrTris * 3];
        sofaPhysics3DObject_getTriangles(m_simu, m_name, tris);

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

        quads = null;
        tris = null;

        return trisOut;
    }


    /// Method to update the Unity mesh buffers (vertices and normals) from sofa object side. Assume no topology change here.
    public virtual void updateMesh(Mesh mesh)
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);

            if (log)
                Debug.Log("vertices: " + nbrV);

            float[] vertices = new float[nbrV * 3];
            int resV = sofaPhysics3DObject_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            int resN = sofaPhysics3DObject_getNormals(m_simu, m_name, normals);
            if (log)
            {
                Debug.Log(m_name + " - resV: " + resV);
                Debug.Log(m_name + " - resN: " + resN);
                if (resN < 0 )
                    Debug.Log("No normals found!");
            }

            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            bool first = false;
            if (verts.Length == 0)// first time
            {
                //Debug.Log("init");
                verts = new Vector3[nbrV];
                norms = new Vector3[nbrV];
                first = true;
            }

            if (log)
            {
                Debug.Log("   - verts size: " + verts.Length);
                Debug.Log("   - vertices.Length: " + vertices.Length);
            }

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
                        norms[i].x = vec.x * factor;// normals[i * 3];
                        norms[i].y = vec.y * factor; //normals[i * 3 + 1];
                        norms[i].z = vec.z * factor; //normals[i * 3 + 2];
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


    public virtual void updateMeshVelocity(Mesh mesh, float timestep)
    {
        if (m_native == IntPtr.Zero)
            return;

        int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);

        if (log)
            Debug.Log("vertices: " + nbrV);

        float[] velocities = new float[nbrV * 4];
        int resV = sofaPhysics3DObject_getVelocities(m_simu, m_name, velocities);

        if (resV < 0)
        {
            //Debug.Log(m_name + " - resV: " + resV);
            return;
        }

        Vector3[] verts = mesh.vertices;
        if (verts.Length == 0)// first time
            verts = new Vector3[nbrV];

        if (log)
        {
            Debug.Log("   - verts size: " + verts.Length);
            Debug.Log("   - vertices.Length: " + velocities.Length);
        }

        if (velocities.Length != 0)
        {
            for (int i = 0; i < nbrV; ++i)
            {
                int id = (int)velocities[i * 4];
                if (id == -1)
                {
                    //Debug.Log("Stop at: " + i);
                    break;
                }

                verts[id].x = verts[id].x + timestep * velocities[i * 4 + 1] ;
                verts[id].y = verts[id].y + timestep * velocities[i * 4 + 2] ;
                verts[id].z = verts[id].z + timestep * velocities[i * 4 + 3] ;
            }
        }

        mesh.vertices = verts;

        velocities = null;
    }


    /// Post processing method to recompute topology if needed. Should be overwritten by child if needed only.
    public virtual void recomputeTopology(Mesh mesh)
    {

    }

    /// Method to check if the topology of the mesh has changed.
    public bool hasTopologyChanged()
    {
        if (m_native != IntPtr.Zero)
        {
            bool value = sofaPhysicsAPI_hasTopologyChanged(m_simu, m_name);
            return value;
        }
        else
            return false;
    }

    /// Method to set that the change of topology has well be handle.
    public int setTopologyChange(bool value)
    {
        if (m_native != IntPtr.Zero)
        {
            return sofaPhysicsAPI_setTopologyChanged(m_simu, m_name, value);
        }
        else
            return -5;
    }


    /// Method to get the number of tetrahedron in the current SOFA object
    public int getNbTetrahedra()
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrTetra = sofaPhysics3DObject_getNbTetrahedra(m_simu, m_name);
            return nbrTetra;
        }
        else
            return 0;
    }

    /// Method to get the number of vertices in the current SOFA object
    public int getNbVertices()
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrTetra = sofaPhysicsAPI_getNbVertices(m_simu, m_name);
            return nbrTetra;
        }
        else
            return 0;
    }

    /// Method to get the buffer of tetrahedra from the current SOFA object
    public void getTetrahedra(int[] tetra)
    {
        if (m_native != IntPtr.Zero)
        {
            sofaPhysics3DObject_getTetrahedra(m_simu, m_name, tetra);
        }
    }

    /// Method to update the Unity mesh buffers (vertices and normals) from a tetrahedron topology object. Assume no topology change here.
    public virtual void updateMeshTetra(Mesh mesh, Dictionary<int, int> mapping, Vector3 scaleSofaToUnity)
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);

            //if (log)
            //Debug.Log("vertices: " + nbrV);

            float[] vertices = new float[nbrV * 3];
            int resV = sofaPhysics3DObject_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            int resN = sofaPhysics3DObject_getNormals(m_simu, m_name, normals);

            if (resV < 0) {
                Debug.LogError("SofaBaseMesh::updateMeshTetra: No vertices found, Error return: " + resV);
                return;
            }

            Vector3[] verts = new Vector3[nbrV];
            Vector3[] norms = new Vector3[nbrV];

            Vector3[] vertsNew = mesh.vertices;
            Vector3[] normsNew = mesh.normals;

            if (vertices.Length != 0)
            {
                for (int i = 0; i < nbrV; ++i)
                {
                    verts[i].x = vertices[i * 3] * scaleSofaToUnity.x;
                    verts[i].y = vertices[i * 3 + 1] * scaleSofaToUnity.y;
                    verts[i].z = vertices[i * 3 + 2] * scaleSofaToUnity.z;

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
    

    /// test method to compute UV with sphere projection
    private void computeStereographicsUV(Mesh mesh)
    {
        this.computeBoundingBox(mesh);

        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;
        Vector3[] vertsSphere = new Vector3[nbrV];

        float[] texCoords = new float[nbrV * 2];
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
            direction.Normalize();
            vertsSphere[i] = center + direction * radius;
        }

        float rangeX = 1 / (m_max.x - m_min.x);
        float rangeZ = 1 / (m_max.z - m_min.z);

        // Project on UV map
        
        for (int i = 0; i < nbrV; i++)
        {
            uv[i] = new Vector2( (vertsSphere[i].x - m_min.x) * rangeX,
                (vertsSphere[i].z - m_min.z) * rangeZ);
        }
        

        mesh.uv = uv;
    }    


    /// Method to recompute the Tex coords according to mesh position and geometry.
    public virtual void recomputeTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;

        float[] texCoords = new float[nbrV * 2];
        Vector2[]  uv = new Vector2[nbrV];

        int res = sofaPhysics3DObject_getTexCoords(m_simu, m_name, texCoords);
        if (log)
            Debug.Log("res get Texcoords: " + res);        

        if (res < 0)
        {
            this.computeBoundingBox(mesh);
            Vector3[] normals = mesh.normals;

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


    // API to update Mesh
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_getNbVertices(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVertices(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNormals(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTexCoords(IntPtr obj, string name, float[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getVelocities(IntPtr obj, string name, float[] arr);
    //}

    // API to access Topology
    //{
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbTriangles(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getQuads(IntPtr obj, string name, int[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbQuads(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTriangles(IntPtr obj, string name, int[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getTetrahedra(IntPtr obj, string name, int[] arr);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysics3DObject_getNbTetrahedra(IntPtr obj, string name);

    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern bool sofaPhysicsAPI_hasTopologyChanged(IntPtr obj, string name);
    [DllImport("SofaAdvancePhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaPhysicsAPI_setTopologyChanged(IntPtr obj, string name, bool value);

    //}
}
