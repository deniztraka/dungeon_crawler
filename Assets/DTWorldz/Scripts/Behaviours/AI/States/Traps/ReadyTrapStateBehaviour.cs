using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.AI.States.Traps
{
    public class ReadyTrapStateBehaviour : BaseTrapStateBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateName = "Ready";
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = TrapBehaviour.ReadySprite;
            }

            if (Collider != null)
            {
                Collider.enabled = true;
            }
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
