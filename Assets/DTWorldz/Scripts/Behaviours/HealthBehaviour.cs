﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Behaviours
{
    public class HealthBehaviour : MonoBehaviour
    {
        private float currentHealth;

        public float MaxHealth = 100;

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public delegate void HealthChanged(float currentHealth, float maxHealth);
        public event HealthChanged OnHealthChanged;
        public event HealthChanged OnDeath;

        void Start(){
            currentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentHealth, MaxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (OnDeath != null)
                {
                    OnDeath(currentHealth, MaxHealth);
                }
            }


        }
    }
}