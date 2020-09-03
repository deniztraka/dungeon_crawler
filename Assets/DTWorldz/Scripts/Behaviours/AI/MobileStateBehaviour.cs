using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.AI.States;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;

namespace DTWorldz.Behaviours.AI
{

    public class MobileStateBehaviour : MonoBehaviour
    {
        public bool DrawGizmos = false;
        public float IdleChance = 0.5f;
        public float WanderChance = 0.5f;
        public int MinDecisionDelay = 1;
        public int MaxDecisionDelay = 5;
        public float FollowRefreshFrequency = 1;

        private string state;
        private HealthBehaviour healthBehaviour;

        void Start()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();
            if (healthBehaviour != null)
            {
                healthBehaviour.OnHealthChanged += new HealthBehaviour.HealthChanged(HealthChanged);
                healthBehaviour.OnDeath += new HealthBehaviour.HealthChanged(OnDeath);
            }
        }

        private void HealthChanged(float currentHealth, float maxHealth)
        {
        }

        private void OnDeath(float currentHealth, float maxHealth)
        {
            Destroy(gameObject, 1);
        }

        public void SetState(string state)
        {
            this.state = state;
        }

        public string GetState()
        {
            return state;
        }



    }
}