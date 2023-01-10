/*****************************************************************************
 *                 - Copyright (C) - 2022 - InfinyTech3D -                   *
 *                                                                           *
 * This file is part of the SofaUnity-Renderer asset from InfinyTech3D       *
 *                                                                           *
 * GNU General Public License Usage:                                         *
 * This file may be used under the terms of the GNU General                  *
 * Public License version 3. The licenses are as published by the Free       *
 * Software Foundation and appearing in the file LICENSE.GPL3 included in    *
 * the packaging of this file. Please review the following information to    *
 * ensure the GNU General Public License requirements will be met:           *
 * https://www.gnu.org/licenses/gpl-3.0.html.                                *
 *                                                                           *
 * Commercial License Usage:                                                 *
 * Licensees holding valid commercial license from InfinyTech3D may use this *
 * file in accordance with the commercial license agreement provided with    *
 * the Software or, alternatively, in accordance with the terms contained in *
 * a written agreement between you and InfinyTech3D. For further information *
 * on the licensing terms and conditions, contact: contact@infinytech3d.com  *
 *                                                                           *
 * Authors: see Authors.txt                                                  *
 * Further information: https://infinytech3d.com                             *
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using SofaUnity;
using System;
using System.Runtime.InteropServices;

[ExecuteInEditMode]
public class SofaVisualModel : MonoBehaviour
{
    /// Member: Unity Mesh object of this GameObject
    protected Mesh m_mesh;
    public string m_uniqName = "None";

    /// pointer to the SofaContext root object
    private SofaContext m_sofaContext = null;

    public bool m_invertNormals = false;

    /// Parameter to activate logging of this SOFA GameObject
    public bool displayLog = false;

    /// bool to store the status of this GameObject. Used to update the mesh if is dirty.
    protected bool m_isDirty = true;



    public void setSofaContext(SofaContext context)
    {
        m_sofaContext = context;
    } 


    /// Setter for \sa m_isDirty value   
    public void SetDirty(bool value) { m_isDirty = value; }


    void Start()
    {
        Debug.Log("Start: '" + m_uniqName + "'");
        // Add a MeshFilter to the GameObject
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        //to see it, we have to add a renderer
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();

        if (mr.sharedMaterial == null)
        {
            if (GraphicsSettings.defaultRenderPipeline)
            {
                mr.sharedMaterial = GraphicsSettings.defaultRenderPipeline.defaultMaterial;
            }
            else
            {
                mr.sharedMaterial = new Material(Shader.Find("Diffuse"));
            }
        }

#if UNITY_EDITOR
        m_mesh = mf.mesh = new Mesh();
#else
        //do this in play mode
        m_mesh = GetComponent<MeshFilter>().mesh;
        if (m_log)
            Debug.Log("SofaBox::Start play mode.");
#endif

        m_mesh.name = m_uniqName;
        m_mesh.vertices = new Vector3[0];
        UpdateMesh();
        UpdateTexCoords();
        CreateTriangulation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateInEditor();
            return;
        }

        // Call internal method that can be overwritten. Only if dirty
        if (m_isDirty)
        {
            UpdateMesh();
            m_isDirty = false;
        }
    }

    protected void UpdateInEditor()
    {

    }


    protected void UpdateMesh()
    {
        SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();

        int nbrV = sofaVisualModel_getNbVertices(sofaAPI.getSimuContext(), m_uniqName);
        if (nbrV < 0)
            return;

        // get access to sofa buffers
        float[] vertices = new float[nbrV * 3];
        int resV = sofaVisualModel_getVertices(sofaAPI.getSimuContext(), m_uniqName, vertices);
        float[] normals = new float[nbrV * 3];
        int resN = sofaVisualModel_getNormals(sofaAPI.getSimuContext(), m_uniqName, normals);

        if (displayLog)
            Debug.Log(m_uniqName + " | Number of vertices: " + nbrV + " with resV: " + resV + " | resN: " + resN);

        Vector3[] verts = m_mesh.vertices;
        Vector3[] norms = m_mesh.normals;
        bool first = false;
        if (verts.Length == 0 || verts.Length != nbrV) // need to resize vectors
        {
            verts = new Vector3[nbrV];
            norms = new Vector3[nbrV];
            first = true;
        }

        if (displayLog)
            Debug.Log(m_uniqName + " | Number of vertices: " + nbrV + " | verts.Length: " + verts.Length + " | vertices.Lengt: " + vertices.Length);

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

        m_mesh.vertices = verts;
        m_mesh.normals = norms;

        vertices = null;
        normals = null;
    }


    protected void UpdateTexCoords()
    {
        SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();

        Vector3[] verts = m_mesh.vertices;
        int nbrV = verts.Length;

        float[] texCoords = new float[nbrV * 2];
        Vector2[] uv = new Vector2[nbrV];

        int res = sofaVisualModel_getTexCoords(sofaAPI.getSimuContext(), m_uniqName, texCoords);
        if (displayLog)
            Debug.Log("res get Texcoords: " + res);

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
        m_mesh.uv = uv;
    }


    protected void CreateTriangulation()
    {
        SofaUnityAPI.SofaContextAPI sofaAPI = m_sofaContext.GetSofaAPI();

        int nbrTris = sofaVisualModel_getNbTriangles(sofaAPI.getSimuContext(), m_uniqName);
        int nbrQuads = sofaVisualModel_getNbQuads(sofaAPI.getSimuContext(), m_uniqName);

        if (displayLog)
            Debug.Log("createTriangulation: " + m_uniqName + " | nbrTris: " + nbrTris + " | nbQuads: " + nbrQuads);

        if (nbrTris < 0)
            nbrTris = 0;

        if (nbrQuads < 0)
            nbrQuads = 0;

        // get buffers
        int[] quads = new int[nbrQuads * 4];
        sofaVisualModel_getQuads(sofaAPI.getSimuContext(), m_uniqName, quads);

        int[] tris = new int[nbrTris * 3];
        sofaVisualModel_getTriangles(sofaAPI.getSimuContext(), m_uniqName, tris);

        // Create and fill unity triangles buffer
        int[] trisOut = new int[nbrTris * 3 + nbrQuads * 6];

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

        m_mesh.triangles = trisOut;
    }


    ///////////////////////////////////////////////////////////
    //////////          API Communication         /////////////
    ///////////////////////////////////////////////////////////

    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getNbVertices(IntPtr obj, string name);

    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getVertices(IntPtr obj, string name, float[] buffer);

    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getNormals(IntPtr obj, string name, float[] buffer);

    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getTexCoords(IntPtr obj, string name, float[] buffer);


    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getNbTriangles(IntPtr obj, string name);
    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getTriangles(IntPtr obj, string name, int[] buffer);

    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getNbQuads(IntPtr obj, string name);
    [DllImport("SofaPhysicsAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int sofaVisualModel_getQuads(IntPtr obj, string name, int[] buffer);


}
