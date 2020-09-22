using System.Collections;
using System.Collections.Generic;
using DTWorlds.Behaviours.Effects;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Interfaces;
using UnityEngine;

namespace DTWorldz.Behaviours.Mobiles
{
    public class HealthBehaviour : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float currentHealth;

        [SerializeField]
        private float maxHealth = 100;
        public BodyType BodyType;

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }       

        public delegate void DamageTaken(float damageAmount, DamageType type);
        public event DamageTaken OnDamageTaken;
        public event HealthChanged OnHealthChanged;
        public event HealthChanged OnDeath;

        private AudioManager audioManager;

        void Start()
        {
            currentHealth = MaxHealth;
            audioManager = GetComponent<AudioManager>();
            if (OnHealthChanged != null)
            {
                OnHealthChanged(MaxHealth, MaxHealth);
            }
        }




        public void TakeDamage(float damage, DamageType type)
        {
            if (currentHealth == -1)
            {
                return;
            }

            currentHealth -= damage;

            if (audioManager != null && UnityEngine.Random.value > 0.5f)
            {
                audioManager.Play("Hit");
            }



            if (OnDamageTaken != null)
            {
                OnDamageTaken(damage, type);
            }

            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentHealth, MaxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = -1;
                if (OnDeath != null)
                {
                    if (audioManager != null)
                    {
                        audioManager.Play("Dead");
                    }
                    OnDeath(currentHealth, MaxHealth);
                }
            }
        }
    }
}