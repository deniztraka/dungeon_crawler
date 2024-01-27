using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Scripts.Managers;
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
            if (AttackBehaviour != null)
            {
                AttackBehaviour.SetDirection(MovementBehaviour.GetDirection());
            }

            if (StateName != "Attack")
            {
                CheckAttack(animator);
            }

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
            var playerGameObject = GameManager.Instance.PlayerBehaviour.gameObject;
            var playerHealth = playerGameObject.GetComponent<HealthBehaviour>();
            if (playerHealth.CurrentHealth < 0)
            {
                return null;
            }
            var playerDistance = Vector2.Distance(this.MovementBehaviour.transform.position, playerGameObject.transform.position);
            if (playerDistance < MovementBehaviour.AwareDistance)
            {
                return playerGameObject;
            }

            return null;
        }

        protected void CheckAttack(Animator animator)
        {
            if (MobileStateBehaviour != null && !MobileStateBehaviour.IsAngry)
            {
                //Debug.Log("not angry");
                return;
            }

            if (AttackBehaviour == null)
            {
                return;
            }

            if (MovementBehaviour != null && MovementBehaviour.FollowingTarget == null)
            {
                //Debug.Log("no following target");
                return;
            }

            if (MovementBehaviour.FollowingTarget != null)
            {
                var targetHealth = MovementBehaviour.FollowingTarget.gameObject.GetComponent<HealthBehaviour>();
                if (targetHealth.CurrentHealth < 0)
                {
                    return;
                }
            }

            var distance = Vector2.Distance(MovementBehaviour.FollowingTarget.transform.position, this.MovementBehaviour.transform.position);
            //Debug.Log(distance);
            if (distance > AttackBehaviour.AttackRange)
            {
                //Debug.Log("distance not enough");
                //GoIdle(animator);
                return;
            }

            if (StateName != "Attack")
            {
                animator.SetTrigger("Attack");
            }
        }

        protected void CheckHostility(Animator animator)
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
            MobileStateBehaviour.IsAngry = true;
            animator.SetTrigger("Follow");
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

        public void GoIdle(Animator animator)
        {
            animator.SetTrigger("Idle");
        }
    }
}