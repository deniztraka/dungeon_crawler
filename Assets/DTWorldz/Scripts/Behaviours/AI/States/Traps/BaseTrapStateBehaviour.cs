using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Items.Deco;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States.Traps
{
    public class BaseTrapStateBehaviour : StateMachineBehaviour
    {
        protected string StateName;
        protected TrapBehaviour TrapBehaviour;
        protected Collider2D Collider;
        protected SpriteRenderer SpriteRenderer;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (TrapBehaviour == null)
            {
                TrapBehaviour = animator.gameObject.GetComponent<TrapBehaviour>();
            }

            if (Collider == null)
            {
                Collider = animator.gameObject.GetComponent<Collider2D>();
            }

            if (SpriteRenderer == null)
            {
                SpriteRenderer = animator.gameObject.GetComponent<SpriteRenderer>();
            }

            TrapBehaviour.State = StateName;

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}