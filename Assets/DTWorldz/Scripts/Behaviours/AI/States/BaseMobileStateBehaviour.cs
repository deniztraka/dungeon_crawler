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
        protected AttackBehaviour AttackBehaviour;
        private float randomDecisionDelay;        

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            SetComponents(animator);

            MobileStateBehaviour.SetState(StateName);            
            Random = new Random(DateTime.Now.Millisecond);
            randomDecisionDelay = UnityEngine.Random.Range(MobileStateBehaviour.MinDecisionDelay, MobileStateBehaviour.MaxDecisionDelay);

            DecisionTime = 0;
        }



        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            AttackBehaviour.SetDirection(MovementBehaviour.GetDirection());
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

        protected GameObject GetClosestEnemy()
        {
            //todo:use an enemy matrix to get closest enemy type
            //currently only enemy is player
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            var playerDistance = Vector2.Distance(this.MovementBehaviour.transform.position, playerGameObject.transform.position);
            if (playerDistance < MovementBehaviour.AwareDistance)
            {
                return playerGameObject;
            }

            return null;
        }

        protected void CheckAttack()
        {

        }

        protected void CheckHostility()
        {
            if (MovementBehaviour == null && MobileStateBehaviour == null)
            {
                return;
            }

            if (MobileStateBehaviour.IsPeaceful)
            {
                //peacefull
                return;
            }

            var closestEnemy = GetClosestEnemy();
            if (closestEnemy == null)
            {
                //no enemy in aware distance
                return;
            }

            var followingTarget = MovementBehaviour.GetFollowingTarget();
            if (followingTarget != null && followingTarget.Equals(closestEnemy))
            {
                //already targeted closest enemy
                //do nothing
                return;
            }


            //if we still here, lets chase it
            MovementBehaviour.SetFollowingTarget(closestEnemy);
            MovementBehaviour.SetIsRunning(true);
        }

        private void SetComponents(Animator animator)
        {
            if (MobileStateBehaviour == null)
            {
                MobileStateBehaviour = animator.gameObject.GetComponent<MobileStateBehaviour>();
            }
            if (MovementBehaviour == null)
            {
                MovementBehaviour = animator.gameObject.GetComponent<MovementBehaviour>();
            }
            if (AttackBehaviour == null)
            {
                AttackBehaviour = animator.gameObject.GetComponentInChildren<AttackBehaviour>();
            }
            if (AudioManager == null)
            {
                AudioManager = animator.gameObject.GetComponent<AudioManager>();
            }
            if (MobileHealth == null)
            {
                MobileHealth = animator.gameObject.GetComponent<HealthBehaviour>();
            }
        }
    }
}