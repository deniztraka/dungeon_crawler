using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileIdleStateBehaviour : BaseMobileStateBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Idle";
            base.OnStateEnter(animator, stateInfo, layerIndex);
            MovementBehaviour.GoIdle();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var randomDecisionDelay = UnityEngine.Random.Range(MobileStateBehaviour.MinDecisionDelay, MobileStateBehaviour.MaxDecisionDelay);

            if (DecisionTime <= 0)
            {
                CheckHostility();

                if (AudioManager != null && UnityEngine.Random.value < 0.25f)
                {
                    AudioManager.Play("Idle");
                }

                if (Random.NextDouble() < MobileStateBehaviour.WanderChance)
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
