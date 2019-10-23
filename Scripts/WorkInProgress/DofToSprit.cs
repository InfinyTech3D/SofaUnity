using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class DofToSprit : MonoBehaviour
{
    /// Pointer to the particle System pointer
    public ParticleSystem m_pSystem = null;

    /// Pointer to Sofa deformable Mesh
    public SDeformableMesh m_sofaObject = null;

    /// Vector of particles
    private ParticleSystem.Particle[] m_particles = null;

    /// Size of each particle
    public float m_particleSize = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Particle System.
        if (m_pSystem == null)
        {
            m_pSystem = this.gameObject.GetComponent<ParticleSystem>();
            // need to better setup ParticleSystem here.

            // A simple particle material with no texture.
            //Material particleMaterial = new Material(Shader.Find("Particles/Standard Unlit"));

            //m_pSystem.gameObject.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
        }

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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_sofaObject == null)
            return;

        Mesh mesh = m_sofaObject.getMesh();
        int nbrV = mesh.vertices.Length;
        
        if (m_particles == null) // first time
        {
            m_particles = new ParticleSystem.Particle[nbrV];
            var emitParams = new ParticleSystem.EmitParams();
            //emitParams.startColor = Color.red;
            emitParams.startSize = m_particleSize;
            m_pSystem.Emit(emitParams, nbrV);
        }

        if (m_pSystem.particleCount != 0)
        {
            m_pSystem.GetParticles(m_particles);
        }
        
        // Update particles
        for (int i=0; i< nbrV; ++i)
        {
            //ParticleSystem.Particle part = m_particles[i];
            m_particles[i].position = mesh.vertices[i];
            m_particles[i].size = m_particleSize;
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
    }   
}
