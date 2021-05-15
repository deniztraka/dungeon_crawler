using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Items.Deco;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Utils;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    [RequireComponent(typeof(HealthBehaviour))]
    [RequireComponent(typeof(FlickeringLightBehaviour))]
    public class CampFire : MonoBehaviour
    {
        private const float maxEmissionRate = 50;
        private FlickeringLightBehaviour flickeringLight;
        private HealthBehaviour health;
        private AudioManager audioManager;
        [SerializeField]
        private ParticleSystemHandler fireParticles;
        public bool IsBurning;
        private float healthBefore;

        void Awake()
        {
            flickeringLight = GetComponent<FlickeringLightBehaviour>();
            fireParticles = GetComponentInChildren<ParticleSystemHandler>();
            health = GetComponent<HealthBehaviour>();
            audioManager = GetComponent<AudioManager>();
            health.OnHealthChanged += new Interfaces.HealthChanged(HealthChanged);
        }

        void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     AddFuel(10);
            // }

            // if (Input.GetMouseButtonDown(1))
            // {
            //     Extinguish();
            // }
        }

        private void AddFuel(float count)
        {
            if (health.CurrentHealth <= 0)
            {
                Ignite(count);
            }
            else
            {
                if (audioManager != null)
                {
                    audioManager.Play("Burning");
                }
                health.CurrentHealth += count;
            }
        }

        public void HealthChanged(float currentHealth, float maxHealth)
        {
            if (healthBefore > 0 && currentHealth <= 0)
            {
                if (audioManager != null)
                {
                    audioManager.Stop("Burning");
                    audioManager.Play("Extinguish");
                }
            }

            if (currentHealth <= 0)
            {
                if (audioManager != null)
                {
                    audioManager.SetVolume(1f);
                }
            }

            StartCoroutine(SetVolume(currentHealth / maxHealth));

            if (flickeringLight != null)
            {
                flickeringLight.SetIntensity(currentHealth, maxHealth);
            }
            if (fireParticles != null)
            {
                fireParticles.SetEmission(currentHealth, maxHealth);
            }

            IsBurning = currentHealth > 0;
            healthBefore = currentHealth;
        }

        public void Ignite()
        {
            Ignite(0);
        }

        public void Ignite(float count)
        {
            if (audioManager != null)
            {
                audioManager.Play("Ignite");
            }
            StartCoroutine(StartFire(count - health.CurrentHealth));
        }

        IEnumerator StartFire(float count)
        {
            yield return new WaitForSeconds(0.5f);
            health.CurrentHealth += count;
            if (audioManager != null)
            {
                audioManager.Play("Burning");
            }
        }

        IEnumerator SetVolume(float v)
        {
            yield return new WaitForSeconds(1f);
            if (audioManager != null)
            {
                audioManager.SetVolume(v);
            }
        }



        public void Extinguish()
        {
            if (health.CurrentHealth > 0)
            {
                health.CurrentHealth = 0;
            }
        }
    }
}