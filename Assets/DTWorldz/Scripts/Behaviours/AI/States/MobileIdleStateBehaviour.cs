using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileIdleStateBehaviour : BaseMobileStateBehaviour
    {
        public float WanderChance = 0.5f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Idle";
            base.OnStateEnter(animator, stateInfo, layerIndex);
            MovementBehaviour.GoIdle();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var randomDecisionDelay = Random.Next(MinDecisionDelay, MaxDecisionDelay);

            if (DecisionTime <= 0)
            {
                if (Random.NextDouble() < WanderChance)
                {
                    animator.SetTrigger("Wander");
                }

                DecisionTime = randomDecisionDelay;
            }
            else
            {
                DecisionTime -= Time.deltaTime;
            }

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
