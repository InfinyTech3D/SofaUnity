using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SofaBaseMesh : SofaBaseObject
{
    public SofaBaseMesh(IntPtr simu, string nameID, bool isRigid)
        : base (simu, nameID, isRigid)
    {

    }

    public override void loadObject()
    {
        //if (m_native != IntPtr.Zero)        
        if (m_native == IntPtr.Zero) // first time create object only
        {
//            m_name = sofaPhysicsAPI_get3DObjectName(m_simu, m_idObject);
//            m_type = sofaPhysicsAPI_get3DObjectType(m_simu, m_idObject); // sofa3DObject_getObjectType(m_native);
            m_native = sofaPhysicsAPI_get3DObject(m_simu, m_name);

            if (m_native == IntPtr.Zero)
                Debug.LogError("Error Mesh can't be found!");
            else
            {
                Debug.Log("Load Node Name: " + m_name);
                //Debug.Log("Load Node Type: " + m_type);
            }
        }
    }

    public void setMass(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "totalMass", value);
            if(log)
                Debug.Log("Change Mass res: " + res);
        }
    }

    public void setYoungModulus(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "youngModulus", value);
            if (log)
                Debug.Log("Change youngModulus res: " + res);
        }
    }

    public void setPoissonRatio(float value)
    {
        if (m_native != IntPtr.Zero)
        {
            int res = sofaPhysics3DObject_setFloatValue(m_simu, m_name, "poissonRatio", value);
            if (log)
                Debug.Log("Change poissonRatio res: " + res);
        }
    }


    public Vector3 getTranslation()
    {
        Vector3 values = new Vector3(0.0f, 0.0f, 0.0f);
        if (m_native != IntPtr.Zero)
        {
            float[] trans = new float[3];
            int res = sofaPhysics3DObject_getVec3fValue(m_simu, m_name, "translation", trans);

            
            for (int i = 0; i < 3; ++i)
                values[i] = trans[i];
            
           // if (log)
                Debug.Log("Change translation res: " + res + " value: " + values);

        }
        return values;
    }

    public void setTranslation(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] trans = new float[3];
            for (int i = 0; i < 3; ++i)
                trans[i] = values[i];
            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, "translation", trans);
            if (log)
                Debug.Log("Change translation res: " + res);
        }
    }

    public void setRotation(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] trans = new float[3];
            for (int i = 0; i < 3; ++i)
                trans[i] = values[i];
            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, "rotation", trans);
            if (log)
                Debug.Log("Change rotation res: " + res);
        }
    }

    public void setScale(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            float[] trans = new float[3];
            for (int i = 0; i < 3; ++i)
                trans[i] = values[i];
            int res = sofaPhysics3DObject_setVec3fValue(m_simu, m_name, "scale", trans);
            if (log)
                Debug.Log("Change scale res: " + res);
        }
    }


    public void setGridResolution(Vector3 values)
    {
        if (m_native != IntPtr.Zero)
        {
            int[] gridSizes = new int[3];
            for (int i = 0; i < 3; ++i)
                gridSizes[i] = (int)values[i];
            int res = sofaPhysics3DObject_setVec3iValue(m_simu, m_name, "gridSize", gridSizes);
            if (log)
                Debug.Log("Change gridSize res: " + res);
        }
    }

    public virtual int[] createTriangulation()
    {
        
        int nbrTris = sofaPhysics3DObject_getNbTriangles(m_simu, m_name);
        int nbrQuads = sofaPhysics3DObject_getNbQuads(m_simu, m_name);

       // if (log)
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
            trisOut[nbrIntTri + i * 6 + 1] = quads[i * 4 + 2];
            trisOut[nbrIntTri + i * 6 + 2] = quads[i * 4 + 1]; 

            trisOut[nbrIntTri + i * 6 + 3] = quads[i * 4];
            trisOut[nbrIntTri + i * 6 + 4] = quads[i * 4 + 3]; 
            trisOut[nbrIntTri + i * 6 + 5] = quads[i * 4 + 2];
        }

        return trisOut;
    }

    public virtual void updateMesh(Mesh mesh)
    {
        if (m_native != IntPtr.Zero)
        {
            int nbrV = sofaPhysicsAPI_getNbVertices(m_simu, m_name);
            //Debug.Log("vertices: " + nbrV);
            //Debug.Log("vert: " + mesh.vertices.Length);
            //Debug.Log("normals: " + normals.Length);
            Debug.Log(nbrV);

            float[] vertices = new float[nbrV * 3];
            sofaPhysics3DObject_getVertices(m_simu, m_name, vertices);
            float[] normals = new float[nbrV * 3];
            sofaPhysics3DObject_getNormals(m_simu, m_name, normals);

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


            if (vertices.Length != 0 && normals.Length != 0)
            {

                for (int i = 0; i < verts.Length; ++i)
                {
                    // Debug.Log(i + " -> " + verts[i]);
                    //Debug.Log(i + " vert -> " + vertices[i]);
                    if (first)
                    {
                        verts[i] = new Vector3(0, 0, 0);
                        norms[i] = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        verts[i].x = vertices[i * 3];
                        verts[i].y = vertices[i * 3 + 1];
                        verts[i].z = vertices[i * 3 + 2];

                        if (norms[i][0] == 0 && norms[i][1] == 0 && norms[i][2] == 0)
                        {
                            Vector3 vec = Vector3.Normalize(verts[i]);
                            norms[i].x = vec.x;// normals[i * 3];
                            norms[i].y = vec.y; //normals[i * 3 + 1];
                            norms[i].z = vec.z; //normals[i * 3 + 2];
                        }
                        //else
                        //{
                        //    norms[i].x = normals[i * 3];
                        //    norms[i].y = normals[i * 3 + 1];
                        //    norms[i].z = normals[i * 3 + 2];
                        //}
                    }
                }
            }

            mesh.vertices = verts;
            mesh.normals = norms;
        }
    }

    public virtual void recomputeTriangles(Mesh mesh)
    {
       
    }

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

    public void getTetrahedra(int[] tetra)
    {
        if (m_native != IntPtr.Zero)
        {
            sofaPhysics3DObject_getTetrahedra(m_simu, m_name, tetra);
            Debug.Log("tetra found getTetrahedra: " + tetra[0] + " " + tetra[1] + " " + tetra[2] + " " + tetra[3]);
        }
    }

    public virtual void recomputeTexCoords(Mesh mesh)
    {
        Vector3[] verts = mesh.vertices;
        int nbrV = verts.Length;

        float[] texCoords = new float[nbrV * 2];
        Vector2[]  uv = new Vector2[nbrV];

        sofaPhysics3DObject_getTexCoords(m_simu, m_name, texCoords);

        for (int i = 0; i < nbrV; i++)
        {
            uv[i].x = texCoords[i * 2];
            uv[i].y = texCoords[i * 2 + 1];

            if(uv[i].x == 0.0 && uv[i].y == 0.0)
                uv[i] = new Vector2(verts[i].x + verts[i].y, verts[i].z + verts[i].y);
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
    //}
}
