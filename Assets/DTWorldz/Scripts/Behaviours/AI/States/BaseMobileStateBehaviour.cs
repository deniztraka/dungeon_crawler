using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
using UnityEngine.Animations;
using Random = System.Random;

namespace DTWorldz.Behaviours.AI.States
{
    public class BaseMobileStateBehaviour : StateMachineBehaviour
    {
        protected String StateName;
        protected HealthBehaviour MobileHealth;
        protected Random Random;
        protected float DecisionTime;
        protected MobileStateBehaviour MobileStateBehaviour;
        protected MovementBehaviour MovementBehaviour;
        protected AudioManager AudioManager;
        private float randomDecisionDelay;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            MobileStateBehaviour = animator.gameObject.GetComponent<MobileStateBehaviour>();
            MovementBehaviour = animator.gameObject.GetComponent<MovementBehaviour>();
            AudioManager = animator.gameObject.GetComponent<AudioManager>();
            if (MobileHealth == null)
            {
                MobileHealth = animator.gameObject.GetComponent<HealthBehaviour>();
            }

            MobileStateBehaviour.SetState(StateName);
            Random = new Random(DateTime.Now.Millisecond);
            randomDecisionDelay = UnityEngine.Random.Range(MobileStateBehaviour.MinDecisionDelay, MobileStateBehaviour.MaxDecisionDelay);

            DecisionTime = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (MobileHealth.CurrentHealth <= 0)
            {
                animator.SetTrigger("Dead");
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            DecisionTime = 0;
        }
    }
}