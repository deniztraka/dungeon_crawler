using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.AI.States;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
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

        public bool IsPeaceful = true;

        private string state;
        private HealthBehaviour healthBehaviour;

        [SerializeField]
        private bool isAngry;
        public bool IsAngry
        {
            get { return isAngry; }
            set { isAngry = value; }
        }

        void Start()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();
            if (healthBehaviour != null)
            {
                healthBehaviour.OnHealthChanged += new HealthChanged(HealthChanged);
                healthBehaviour.OnDeath += new HealthChanged(OnDeath);
            }
        }

        private void HealthChanged(float currentHealth, float maxHealth)
        {
        }

        private void OnDeath(float currentHealth, float maxHealth)
        {

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