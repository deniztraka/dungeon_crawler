using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Items.Deco;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Items.Behaviours.Weapons
{

    [RequireComponent(typeof(HealthBehaviour))]
    [RequireComponent(typeof(FlickeringLightBehaviour))]
    public class TorchBehaviour : MonoBehaviour
    {
        private const float maxEmissionRate = 50;
        private FlickeringLightBehaviour flickeringLight;
        private HealthBehaviour health;
        private AudioManager audioManager;
        [SerializeField]
        public bool IsBurning;
        private float healthBefore;

        void Awake()
        {
            flickeringLight = GetComponent<FlickeringLightBehaviour>();
            health = GetComponent<HealthBehaviour>();
            audioManager = GetComponent<AudioManager>();
            health.OnHealthChanged += new Interfaces.HealthChanged(HealthChanged);
        }

        public void Start()
        {
            // Ignite(health.MaxHealth);
        }

        public void HealthChanged(float currentHealth, float maxHealth)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

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