using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Behaviours.Mobiles
{
    public class HealthBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float currentHealth;

        public float MaxHealth = 100;

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public delegate void HealthChanged(float currentHealth, float maxHealth);
        public delegate void DamageTaken(float damageAmount, DamageType type);
        public event DamageTaken OnDamageTaken;
        public event HealthChanged OnHealthChanged;
        public event HealthChanged OnDeath;

        void Start()
        {
            currentHealth = MaxHealth;
        }

        public void TakeDamage(float damage, DamageType type)
        {
            currentHealth -= damage;
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
                currentHealth = 0;
                if (OnDeath != null)
                {
                    //Debug.Log(gameObject.name + " is dead.");
                    OnDeath(currentHealth, MaxHealth);
                }
            }
        }
    }
}