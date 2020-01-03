using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class DofToSprit : MonoBehaviour
{
    /// Pointer to the particle System pointer
    public ParticleSystem m_pSystem = null;

    /// Pointer to Sofa deformable Mesh
    public SofaDeformableMesh m_sofaObject = null;

    /// Vector of particles
    private ParticleSystem.Particle[] m_particles = null;

    /// Size of each particle
    public float m_particleSize = 0.5f;

    public Material m_particleMaterial = null;

    protected int m_nbrMax = 1000;

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


    // Start is called before the first frame update
    void Start()
    {       
        if (m_sofaObject != null)
        {
            Mesh mesh = m_sofaObject.getMesh();
            if (mesh != null)
            {
                int nbr = mesh.vertices.Length;
                Debug.Log("awk mesh.vertices: " + nbr);
            }
            m_sofaObject.m_forceUpdate = true;
        }

        if (m_particles == null) // first time
        {
            Debug.Log("init m_particles");
        //    m_pSystem.GetParticles(m_particles);
            m_particles = new ParticleSystem.Particle[m_nbrMax];
            var emitParams = new ParticleSystem.EmitParams();
            //emitParams.startColor = Color.red;
            emitParams.startSize = m_particleSize;
            m_pSystem.Emit(emitParams, m_nbrMax);
        }

        if (m_pSystem)
        {
            Debug.Log("init m_pSystem");
            var main = m_pSystem.main;
            main.playOnAwake = false;
            main.loop = false;
            main.maxParticles = m_nbrMax;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_sofaObject == null)
            return;

        Mesh mesh = m_sofaObject.getMesh();
        int nbrV = mesh.vertices.Length;

        //Debug.Log(Time.fixedTime + " - nbrV: " + nbrV);

        if (m_pSystem.particleCount != 0)
        {
            m_pSystem.GetParticles(m_particles);
            
            if (nbrV > m_nbrMax && nbrV > m_pSystem.particleCount)
            {
                //Debug.Log(Time.fixedTime + " - Resize m_pSystem.particleCount: " + m_pSystem.particleCount);
                m_particles = new ParticleSystem.Particle[nbrV];
                var main = m_pSystem.main;
                main.maxParticles = nbrV;

                  // m_pSystem.Emit(5000);
            }
        }

        //Debug.Log(Time.fixedTime + " - m_pSystem.particleCount: " + m_pSystem.particleCount + " | m_particles: " + m_particles.Length);
        // Update particles
        for (int i = 0; i < nbrV; ++i)
        {
            //ParticleSystem.Particle part = m_particles[i];
            m_particles[i].position = mesh.vertices[i];
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
        //Debug.Log("m_particles: " + m_particles.Length);
        m_pSystem.SetParticles(m_particles, nbrV);
    }   
}
