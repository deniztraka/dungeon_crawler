using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobilAttackStateBehaviour : BaseMobileStateBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Attack";
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (MobileStateBehaviour != null && !MobileStateBehaviour.IsAngry)
            {
                GoIdle(animator);
                return;
            }

            if (AttackBehaviour == null)
            {
                GoIdle(animator);
                return;
            }

            if (MovementBehaviour != null && MovementBehaviour.FollowingTarget == null)
            {
                GoIdle(animator);
                return;
            }

            if (MovementBehaviour.FollowingTarget != null)
            {
                var targetHealth = MovementBehaviour.FollowingTarget.gameObject.GetComponent<HealthBehaviour>();
                if (targetHealth.CurrentHealth < 0)
                {
                    GoIdle(animator);
                    return;
                }

                var distance = Vector2.Distance(MovementBehaviour.FollowingTarget.transform.position, this.MovementBehaviour.transform.position);
                //Debug.Log(distance);
                if (distance > AttackBehaviour.AttackRange)
                {
                    GoIdle(animator);
                    return;
                }
            }

            Attack();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }

        void Attack()
        {
            var attacked = AttackBehaviour.Attack();
        }
    }
}