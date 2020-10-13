using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class Barrel : MonoBehaviour
    {
        public ParticleSystem BrakeEffect;
        private HealthBehaviour healthBehaviour;
        private SpriteRenderer spriteRenderer;
        private AudioManager audioManager;
        private Collider2D coll;

        void Start()
        {
            if (BrakeEffect == null)
            {
                BrakeEffect = GetComponentInChildren<ParticleSystem>();
            }
            audioManager = GetComponent<AudioManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            coll = GetComponent<Collider2D>();

            healthBehaviour = GetComponent<HealthBehaviour>();
            if (healthBehaviour != null)
            {
                healthBehaviour.OnHealthChanged += new HealthChanged(HealthChanged);
                healthBehaviour.OnDeath += new HealthChanged(OnDeath);
            }
        }

        private void HealthChanged(float currentHealth, float maxHealth)
        {
            //Debug.Log("health is changed");
        }

        private void OnDeath(float currentHealth, float maxHealth)
        {
            if(audioManager != null){
                audioManager.Play("Breake");
            }
            spriteRenderer.enabled = false;
            coll.enabled = false;

            if (BrakeEffect != null)
            {
                BrakeEffect.Play();
            }
            Destroy(gameObject, 3);

        }
    }
}
