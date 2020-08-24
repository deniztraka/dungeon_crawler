using System;
using System.Collections;
using System.Collections.Generic;
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
        private float randomDecisionDelay;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            MobileStateBehaviour = animator.gameObject.GetComponent<MobileStateBehaviour>();
            MovementBehaviour = animator.gameObject.GetComponent<MovementBehaviour>();

            if (MobileHealth == null)
            {
                MobileHealth = animator.gameObject.GetComponent<HealthBehaviour>();
            }

            MobileStateBehaviour.SetState(StateName);
            Random = new Random(DateTime.Now.Millisecond);
            randomDecisionDelay = Random.Next(MobileStateBehaviour.MinDecisionDelay, MobileStateBehaviour.MaxDecisionDelay);

            DecisionTime = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            DecisionTime = 0;
        }

        // public Boolean CheckState(Animator animator, String stateName, float chance){
        //     if (DecisionTime <= 0)
        //     {
        //         var nextDouble = Random.NextDouble();
        //         //Debug.Log("NextDouble:" + nextDouble);
        //         if (nextDouble < chance)
        //         {
        //             //return true;
        //         }

        //         DecisionTime = randomDecisionDelay;
        //         return true;
        //         //Debug.Log("DecisitonTimeAfter:" + DecisionTime);
        //     }
        //     else
        //     {
        //         DecisionTime -= Time.deltaTime;
        //         return false;
        //     }
        // }
    }
}