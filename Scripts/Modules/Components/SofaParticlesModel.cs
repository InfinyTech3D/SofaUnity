using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Gameobject to transform a list of particle position into a unity sprite mesh.
/// </summary>
public class SofaParticlesModel : MonoBehaviour
{
    ////////////////////////////////////////////
    //////   SofaParticlesModel members    /////
    ////////////////////////////////////////////

    /// Pointer to the SofaMesh this ParticlesModel is related to.
    public SofaMesh m_sofaMesh = null;

    /// Size of each particle
    public float m_particleSize = 0.5f;

    /// Material to use for this particleSystem
    public Material m_particleMaterial = null;


    /// Pointer to the particle System pointer
    public ParticleSystem m_pSystem = null;

    /// Max number of particles
    public int m_nbrMax = 1000;

    /// Vector of particles inside the ParticleSystem \sa m_pSystem
    protected ParticleSystem.Particle[] m_particles = null;


    ////////////////////////////////////////////
    //////      SofaParticlesModel API     /////
    ////////////////////////////////////////////

    /// Method call by Unity animation loop when object is created
    public void Awake()
    {
        // Create a Particle System.
        if (m_pSystem == null)
        {
            m_pSystem = this.gameObject.AddComponent<ParticleSystem>();
            
            // set default settings for particle system
            var em = m_pSystem.emission;
            em.enabled = false;
            var sh = m_pSystem.shape;
            sh.enabled = false;
            var main = m_pSystem.main;
            main.playOnAwake = false;
            main.loop = false;
            main.maxParticles = m_nbrMax;

            // A simple particle material with no texture.
            if (m_particleMaterial == null)
                m_particleMaterial = new Material(Shader.Find("Particles/Standard Unlit"));

            ParticleSystemRenderer pRenderer = m_pSystem.gameObject.GetComponent<ParticleSystemRenderer>();
            pRenderer.material = m_particleMaterial;
            pRenderer.sortMode = ParticleSystemSortMode.Distance;
        }
    }


    /// Start is called by Unity animation loop before the first frame update
    void Start()
    {       
        if (m_particles == null) // first time
        {
            InitParticleSystem();
        }

        if (m_pSystem)
        {
            //Debug.Log("init m_pSystem");
            var main = m_pSystem.main;
            main.playOnAwake = false;
            main.loop = false;
            main.maxParticles = m_nbrMax;
        }
    }


    /// Update is called by Unity animation loop once per frame
    void Update()
    {
        if (m_sofaMesh == null)
            return;

        int nbrV = m_sofaMesh.NbVertices();

        //Debug.Log(Time.fixedTime + " - nbrV: " + nbrV);

        // 1. Resize particle system if needed
        if (m_pSystem.particleCount != 0)
        {
            m_pSystem.GetParticles(m_particles);

            if (nbrV > m_nbrMax && nbrV > m_pSystem.particleCount)
            {
                //Debug.Log(Time.fixedTime + " - Resize m_pSystem.particleCount: " + m_pSystem.particleCount + " -> " + nbrV);
                m_particles = new ParticleSystem.Particle[nbrV];
                var main = m_pSystem.main;
                main.maxParticles = nbrV;                
            }
        }

        //Debug.Log(Time.fixedTime + " - m_pSystem.particleCount: " + m_pSystem.particleCount + " | m_particles: " + m_particles.Length);
        
        // 2. Update particles
        float[] sofaVertices = m_sofaMesh.SofaMeshTopology.m_vertexBuffer;
        for (int i = 0; i < nbrV; ++i)
        {
            //ParticleSystem.Particle part = m_particles[i];
            m_particles[i].position = new Vector3(sofaVertices[i * 3], sofaVertices[i * 3 + 1], sofaVertices[i * 3 + 2]);
            m_particles[i].startSize = m_particleSize;
            //m_particles[i].remainingLifetime = 1000.0f;
            //part.remainingLifetime = 10000.0f;
            //part.color = Color.red;
            //part.velocity = Vector3.zero;
            //part.angularVelocity = 0.0f;
            //part.rotation = 0.0f;
            //part.size = 0.1f;
            //part.lifetime = 1.0f;
            //part.randomValue = 0.0f;
        }
        m_pSystem.SetParticles(m_particles, nbrV);
        
        //Debug.Log("m_particles: " + m_particles.Length);
        //Debug.Log("m_pSystem.particleCount: " + m_pSystem.particleCount);
        
    }


    ////////////////////////////////////////////
    ////// SofaParticlesModel internal API /////
    ////////////////////////////////////////////

    protected void InitParticleSystem()
    {
        //Debug.Log("init m_particles");
        //    m_pSystem.GetParticles(m_particles);
        m_particles = new ParticleSystem.Particle[m_nbrMax];
        var emitParams = new ParticleSystem.EmitParams();
        //emitParams.startColor = Color.red;
        emitParams.startSize = m_particleSize;
        m_pSystem.Emit(emitParams, m_nbrMax);

        m_sofaMesh.AddListener();
    }
}
