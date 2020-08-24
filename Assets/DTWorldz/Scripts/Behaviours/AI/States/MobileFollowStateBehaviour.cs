using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileFollowStateBehaviour : BaseMobileStateBehaviour
    {
        public float RefreshTime;
        public float RefreshFrequency = 1;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Following";
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (RefreshTime <= 0)
            {
                RefreshTime = RefreshFrequency;
                MovementBehaviour.SetTargetPaths();
            }
            else
            {
                RefreshTime -= Time.deltaTime;
            }

            var goIdle = false;
            if (MovementBehaviour.FollowingTarget != null)
            {
                var distanceFromTarget = GetDistanceFrom(MovementBehaviour.FollowingTarget.transform.position);

                MovementBehaviour.SetIsRunning(distanceFromTarget > MovementBehaviour.CloseDistance && distanceFromTarget < MovementBehaviour.AwareDistance);                
                if (distanceFromTarget > MovementBehaviour.AwareDistance)
                {
                    goIdle = true;
                }
            }
            else
            {
                goIdle = true;
            }

            if (goIdle)
            {
                MovementBehaviour.FollowingTarget = null;
                animator.SetTrigger("Idle");
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }

        private float GetDistanceFrom(Vector2 position)
        {
            return Vector2.Distance(position, MovementBehaviour.transform.position);
        }
    }
}