﻿using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileFollowStateBehaviour : BaseMobileStateBehaviour
    {
        public float RefreshTime;

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
                RefreshTime = MobileStateBehaviour.FollowRefreshFrequency;
                CheckHostility(animator);
                MovementBehaviour.SetTargetPaths();
            }
            else
            {
                RefreshTime -= Time.deltaTime;
            }

            if (MovementBehaviour.FollowingTarget == null)
            {
                GoIdle(animator);
                return;
            }
            else
            {
                var targetHealth = MovementBehaviour.FollowingTarget.gameObject.GetComponent<HealthBehaviour>();
                if (targetHealth.CurrentHealth < 0)
                {
                    GoIdle(animator);
                    return;
                }
            }

            //follow
            var distanceFromTarget = GetDistanceFrom(MovementBehaviour.FollowingTarget.transform.position);
            MovementBehaviour.SetIsRunning(distanceFromTarget > MovementBehaviour.CloseDistance && distanceFromTarget < MovementBehaviour.AwareDistance);

            //go idle if not in aware distance
            if (distanceFromTarget > MovementBehaviour.AwareDistance)
            {
                GoIdle(animator);
                return;
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