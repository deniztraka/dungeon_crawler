using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemHandler : MonoBehaviour
    {
        public float MaxEmissionRate = 50;
        private ParticleSystem particles;
        private ParticleSystem.EmissionModule emissionModule;
        void Awake()
        {
            particles = GetComponent<ParticleSystem>();
            emissionModule = particles.emission;
        }

        internal void SetEmission(float currentHealth, float maxHealth)
        {
            var rate = MaxEmissionRate / maxHealth * currentHealth;
            
            if (rate > 0)
            {
                particles.Play();
            }
            else
            {
                rate = 0;
                particles.Stop();
            }

            emissionModule.rateOverTime = rate;
        }
    }
}