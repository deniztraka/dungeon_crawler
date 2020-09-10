using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States
{
    public class MobilAttackStateBehaviour : BaseMobileStateBehaviour
    {
        private bool attackingTrigger = false;
        
        [SerializeField]
        private float attackTime = 0;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Attack";
            
            Debug.Log("attack state enter");
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);            
            Debug.Log("attack state update");
            Attack();            

            attackTime -= Time.deltaTime;
            attackTime = attackTime <= 0 ? 0 : attackTime;
        }

        private void TriggerAttack()
        {
            AttackBehaviour.Attack();
            attackTime = AttackBehaviour.AttackingFrequency;
        }

        public void Attack()
        {
            if (attackTime <= 0)
            {
                TriggerAttack();
                attackingTrigger = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}