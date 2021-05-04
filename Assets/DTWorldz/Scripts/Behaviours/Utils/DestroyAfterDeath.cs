using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    [RequireComponent(typeof(HealthBehaviour))]
    public class DestroyAfterDeath : MonoBehaviour
    {
        public float AfterSeconds = 0.1f;
        private HealthBehaviour health;
        void Start()
        {
            health = GetComponent<HealthBehaviour>();
            health.OnDeath += new Interfaces.HealthChanged(OnDead);
        }

        private void OnDead(float currentHealth, float maxHealth)
        {
            Destroy(gameObject, AfterSeconds);
        }
    }
}