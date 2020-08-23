using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobileWanderStateBehaviour : BaseMobileStateBehaviour
    {
        public float IdleChance = 0.5f;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Wander";
            base.OnStateEnter(animator, stateInfo, layerIndex);

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var randomDecisionDelay = Random.Next(MinDecisionDelay, MaxDecisionDelay);

            if (DecisionTime <= 0)
            {
                var nextDouble = Random.NextDouble();
                //Debug.Log("NextDouble:" + nextDouble);
                if (nextDouble < IdleChance)
                {
                    animator.SetTrigger("Idle");
                }
                else
                {
                    SetNewTarget(animator);
                }

                DecisionTime = randomDecisionDelay;
                //Debug.Log("DecisitonTimeAfter:" + DecisionTime);
            }
            else
            {
                DecisionTime -= Time.deltaTime;
            }

        }

        private void SetNewTarget(Animator animator)
        {

            var newMovementTarget = animator.transform.position + new Vector3(Random.Next(-2, 3), Random.Next(-2, 3), 0);
            
            var wallMap = MovementBehaviour.GetMovementMap();
            if (wallMap != null)
            {
                var cellPos = wallMap.WorldToCell(newMovementTarget);
                var tile = wallMap.GetTile(cellPos);
                if (tile != null)
                {
                    return;
                }
            }

            MovementBehaviour.SetTargetPoint(newMovementTarget);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

        }
    }
}
